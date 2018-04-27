
using System.Diagnostics;
using System.Collections.Generic;

using UnityEngine;

namespace Coda.Battle
{
    public abstract class Lockstep : MonoBehaviour
    {
        public event CodaUtility.VoidDelegate<int> onMissingLogicFrame;

        public float frameSpeedScaleRate = 100;

        public float frameSpeedScaleSmoothRate = 5;

        public int currentFrame { get { return _currentFrame; } }

        public bool isFreeUpdate { get; set; }

        public BaseStepMessage newestMessage { get { return _logicFrame.Count == 0 ? null : _logicFrame.Last.Value; } }

        public int currentNetDelayMilliseconds { get { return _currentNetDelay; } }


        /// <summary>
        /// Run this function first.
        /// </summary>
        /// <param name="logicFrameSpace">The server's frame space</param>
        public void Init(int logicFrameSpaceMilliseconds, params IStepDriver[] objects)
        {
            _logicFrameSpaceMilliseconds = logicFrameSpaceMilliseconds;
            _logicFrameSpace = _logicFrameSpaceMilliseconds / 1000f;  //Space from server.
            _frameSpace = Time.fixedDeltaTime;
            _mainTimeCounter = 0;
            _currentFrame = -1;

            for (int i = 0; i < objects.Length; i++)
            {
                AddObject(objects[i]);
            }

#if UNITY_EDITOR
            _isInit = true;
#endif
        }


        /// <summary>
        /// Add a drivable obj.
        /// </summary>
        public void AddObject(IStepDriver obj)
        {
            _driverList.Add(obj);
        }


        /// <summary>
        /// Remove a drivable obj.
        /// </summary>
        public bool RemoveObject(IStepDriver obj)
        {
            return _driverList.Remove(obj);
        }


        /// <summary>
        /// Clear all object.
        /// </summary>
        public void ClearObject()
        {
            _driverList.Clear();
        }


        /// <summary>
        /// Call this function when recieve frame message from server.
        /// </summary>
        public void AddLogicFrame(BaseStepMessage message)
        {
            _netFixWatch.Stop();
            _currentNetDelay = Mathf.Max(0, (int)_netFixWatch.ElapsedMilliseconds - _logicFrameSpaceMilliseconds);
            _netFixWatch.Reset();
            _netFixWatch.Start();
            _addCacheFrame.Enqueue(message);
        }


        /// <summary>
        /// Use this function to fix missing part of logic frame.
        /// </summary>
        public void FixLogicFrame(BaseStepMessage message)
        {
            _fixCacheFrame.Enqueue(message);
        }
        
        
        protected abstract void OnMissingLogicFrame(int missingFrameIndex);
        protected virtual void OnFixedUpdate() { }


        #region Private Part

#if UNITY_EDITOR
        private bool _isInit;
#endif
        private float _mainTimeCounter;
        private int _currentFrame;
        private LinkedList<BaseStepMessage> _logicFrame = new LinkedList<BaseStepMessage>(); // First -- Last == Old -- New.
        private Queue<BaseStepMessage> _addCacheFrame = new Queue<BaseStepMessage>();
        private Queue<BaseStepMessage> _fixCacheFrame = new Queue<BaseStepMessage>();
        private float _logicFrameSpace;
        private int _logicFrameSpaceMilliseconds;
        private List<IStepDriver> _driverList = new List<IStepDriver>();
        private float _frameSpace;
        private HashSet<int> _missingFrameOperationSet = new HashSet<int>();
        private float _freeTimeCounter;
        private Stopwatch _netFixWatch = new Stopwatch();
        private int _currentNetDelay;

        private void FixedUpdate()
        {
            OnFixedUpdate();
#if UNITY_EDITOR
            if (!_isInit)
            {
                throw new System.Exception("Run \"Init\" function first.");
            }
#endif
            if (isFreeUpdate)
            {
                _freeTimeCounter += Time.deltaTime;
                while (_freeTimeCounter > 0)
                {
                    _freeTimeCounter -= _frameSpace;
                    for (int i = 0; i < _driverList.Count; i++)
                    {
                        _driverList[i].StepUpdate(_frameSpace);
                    }
                }
                return;
            }

            _OperateCacheQueue();

            float totalTime = _GetLocalFrameSpace() + _mainTimeCounter;

            while (true)
            {
                _mainTimeCounter += _frameSpace;

                if (_mainTimeCounter >= totalTime)
                {
                    _mainTimeCounter -= _frameSpace;
                    break;
                }

                bool isNeedDoLogicFrame = false;
                BaseStepMessage nextFrame = null;

                if (_logicFrame.Count > 0)
                {
                    nextFrame = _logicFrame.First.Value;

                    //Useless frame.
                    if (nextFrame.frame <= _currentFrame)
                    {
                        UnityEngine.Debug.LogWarning("Local frame faster than logic frame.");
                        _logicFrame.RemoveFirst();
                        _mainTimeCounter -= _frameSpace;
                        break;
                    }

                    //Miss frame.
                    if (nextFrame.frame != _currentFrame + 1)
                    {
                        _OnMissingLogicFrame();
                        _mainTimeCounter -= _frameSpace;
                        break;
                    }
                    else
                        _missingFrameOperationSet.Remove(_currentFrame + 1);

                    //Need do logic frame.
                    if (_mainTimeCounter >= nextFrame.frame * _logicFrameSpace)
                    {
                        isNeedDoLogicFrame = true;
                    }
                }

                for (int i = 0; i < _driverList.Count; i++)
                {
                    if (isNeedDoLogicFrame)
                        _driverList[i].DoOperation(nextFrame);

                    _driverList[i].StepUpdate(_frameSpace);
                }

                if (isNeedDoLogicFrame)
                {
                    _logicFrame.RemoveFirst();
                    _currentFrame = nextFrame.frame;
                }
            }
        }

        private void _OperateCacheQueue()
        {
            while (_addCacheFrame.Count > 0)
            {
                _logicFrame.AddLast(_addCacheFrame.Dequeue());
            }

            while (_fixCacheFrame.Count > 0)
            {
                _logicFrame.AddFirst(_fixCacheFrame.Dequeue());
            }
        }

        private void _OnMissingLogicFrame()
        {
            int missingFrame = _currentFrame + 1;
            if (!_missingFrameOperationSet.Contains(missingFrame))
            {
                _missingFrameOperationSet.Add(missingFrame);

                OnMissingLogicFrame(missingFrame);
                if (onMissingLogicFrame != null)
                    onMissingLogicFrame(missingFrame);
            }
        }

        private float _GetLocalFrameSpace()
        {
            float maxTime;      //The time for lock local time.
            float nextMyTime;   //Next frame local time will become this.

            if (_logicFrame.Count == 0)
            {
                maxTime = (_currentFrame + 1) * _logicFrameSpace;
            }
            else
            {
                maxTime = (_logicFrame.Last.Value.frame + 1) * _logicFrameSpace;
            }
            nextMyTime = _mainTimeCounter + _frameSpace;

            if (nextMyTime >= maxTime)
            {
                //Lock local time.
                return 0;
            }
            else
            {
                if (_logicFrame.Count == 0)
                {
                    //if all of logic frame has operated, then smooth run.
                    return _frameSpace;
                }
                else
                {
                    float targetTime;   //if logic frame hasn't operated exist, then smooth fix the local time to targetTime.
                    targetTime = Mathf.Max(maxTime - _logicFrameSpace, maxTime - ((_currentNetDelay) / 1000f));

                    if (nextMyTime == targetTime)
                    {
                        return _frameSpace;
                    }
                    else if (nextMyTime > targetTime)
                    {
                        if (_currentNetDelay == 0)
                            return 0;

                        float speedRate = (nextMyTime - maxTime) / (targetTime - maxTime);
                        return Mathf.Max(0, speedRate * _frameSpace);
                    }
                    else
                    {
                        float x = nextMyTime - targetTime;
                        float speedRate = frameSpeedScaleRate * Mathf.Pow(x, 2) + 1;
                        if (speedRate - 1 > -frameSpeedScaleSmoothRate * x)
                        {
                            speedRate = 1 - frameSpeedScaleSmoothRate * x;
                        }
                        return Mathf.Min(targetTime - _mainTimeCounter, speedRate * _frameSpace);
                    }
                }
            }
        }
        #endregion
    }
}
