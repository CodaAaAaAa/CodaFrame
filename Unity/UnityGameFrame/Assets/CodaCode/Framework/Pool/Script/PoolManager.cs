
using UnityEngine;

using System;
using System.Threading;
using System.Collections.Generic;

namespace Coda.Tools
{
    public class PoolManager : MonoBehaviour
    {
        /// <summary>
        /// Single instance of PoolManager.
        /// </summary>
        public static PoolManager instance { get; private set; }


        /// <summary>
        /// A parent for your pool object.
        /// </summary>
        public GameObject poolParent { get { if (_poolParent == null) { _poolParent = new GameObject(); _poolParent.name = "Object Pool"; } return _poolParent; } }


        /// <summary>
        /// Should always exist.
        /// </summary>
        public bool dontDestroyOnLoad;


        /// <summary>
        /// Collection frequncy (It's in milliseconds).
        /// </summary>
        public int collectFrequncyInMilliseconds = 60000;


        /// <summary>
        /// Collection if the pooled object doesn't used during this time.
        /// </summary>
        public int collectCheckInSeconds = 60;


        /// <summary>
        /// The count of obj in pool.
        /// </summary>
        public int poolCount { get { return _classPool.Count; } }


        /// <summary>
        /// Use pool manager to get a instance of T.
        /// </summary>
        /// <typeparam name="T">Type of instance</typeparam>
        public T GetOrCreateInstance<T>() where T : ISoluble, new()
        {
            return GetOrCreateInstance(() => new T());
        }


        /// <summary>
        /// Use pool manager to get a instance of T.
        /// </summary>
        /// <typeparam name="T">Type of instance</typeparam>
        /// <param name="customInitFunc">Custom your init function of instance</param>
        public T GetOrCreateInstance<T>(Func<T> customInitFunc) where T : ISoluble
        {
            Type type = typeof(T);
            T value;
            if (_classPool.ContainsKey(type) && _classPool[type].First != null)
            {
                value = (T)_classPool[type].First.Value.obj;
                _classPool[type].RemoveFirst();
                value.ResetData();
                _classSet.Remove(value);
            }
            else
            {
                value = customInitFunc();
                value.ResetData();
            }
            return value;
        }


        /// <summary>
        /// Use pool manager to get a instance of T.
        /// </summary>
        /// <typeparam name="T">Type of instance</typeparam>
        /// <typeparam name="V">Type of parameter</typeparam>
        /// <param name="param">Parameter of "ResetData" function</param>
        public T GetOrCreateInstance<T, V>(V param) where T : ISoluble<V>, new()
        {
            return GetOrCreateInstance(param, () => new T());
        }


        /// <summary>
        /// Use pool manager to get a instance of T.
        /// </summary>
        /// <typeparam name="T">Type of instance</typeparam>
        /// <typeparam name="V">Type of parameter</typeparam>
        /// <param name="param">Parameter of "ResetData" function</param>
        /// <param name="customInitFunc">Custom your init function of instance</param>
        public T GetOrCreateInstance<T, V>(V param, Func<T> customInitFunc) where T : ISoluble<V>
        {
            Type type = typeof(T);
            T value;
            if (_classPool.ContainsKey(type) && _classPool[type].First != null)
            {
                value = (T)_classPool[type].First.Value.obj;
                _classPool[type].RemoveFirst();
                value.ResetData(param);
                _classSet.Remove(value);
            }
            else
            {
                value = customInitFunc();
                value.ResetData(param);
            }
            return value;
        }


        /// <summary>
        /// Use pool manager to get a instance of T.
        /// </summary>
        /// <typeparam name="T">Type of instance</typeparam>
        /// <typeparam name="V">Type of first parameter</typeparam>
        /// <typeparam name="U">Type of second parameter</typeparam>
        /// <param name="param1">First parameter of "ResetData" function</param>
        /// <param name="param2">Second parameter of "ResetData" function</param>
        public T GetOrCreateInstance<T, V, U>(V param1, U param2) where T : ISoluble<V, U>, new()
        {
            return GetOrCreateInstance(param1, param2, () => new T());
        }


        /// <summary>
        /// Use pool manager to get a instance of T.
        /// </summary>
        /// <typeparam name="T">Type of instance</typeparam>
        /// <typeparam name="V">Type of first parameter</typeparam>
        /// <typeparam name="U">Type of second parameter</typeparam>
        /// <param name="param1">First parameter of "ResetData" function</param>
        /// <param name="param2">Second parameter of "ResetData" function</param>
        /// <param name="customInitFunc">Custom your init function of instance</param>
        public T GetOrCreateInstance<T, V, U>(V param1, U param2, Func<T> customInitFunc) where T : ISoluble<V, U>
        {
            Type type = typeof(T);
            T value;
            try
            {
                if (_classPool.ContainsKey(type) && _classPool[type].First != null)
                {
                    value = (T)_classPool[type].First.Value.obj;
                    _classPool[type].RemoveFirst();
                    value.ResetData(param1, param2);
                    _classSet.Remove(value);
                }
                else
                {
                    value = customInitFunc();
                    value.ResetData(param1, param2);
                }
            }
            catch
            {
                value = default(T);
            }
            return value;
        }


        /// <summary>
        /// Use pool manager to get a instance of T.
        /// </summary>
        /// <typeparam name="T">Type of instance</typeparam>
        /// <typeparam name="V">Type of first parameter</typeparam>
        /// <typeparam name="U">Type of second parameter</typeparam>
        /// <typeparam name="K">Type of third parameter</typeparam>
        /// <param name="param1">First parameter of "ResetData" function</param>
        /// <param name="param2">Second parameter of "ResetData" function</param>
        /// <param name="param3">Third parameter of "ResetData" function</param>
        /// <returns></returns>
        public T GetOrCreateInstance<T, V, U, K>(V param1, U param2, K param3) where T : ISoluble<V, U, K>, new()
        {
            return GetOrCreateInstance(param1, param2, param3, () => new T());
        }


        /// <summary>
        /// Use pool manager to get a instance of T.
        /// </summary>
        /// <typeparam name="T">Type of instance</typeparam>
        /// <typeparam name="V">Type of first parameter</typeparam>
        /// <typeparam name="U">Type of second parameter</typeparam>
        /// <typeparam name="K">Type of third parameter</typeparam>
        /// <param name="param1">First parameter of "ResetData" function</param>
        /// <param name="param2">Second parameter of "ResetData" function</param>
        /// <param name="param3">Third parameter of "ResetData" function</param>
        /// <returns></returns>
        public T GetOrCreateInstance<T, V, U, K>(V param1, U param2, K param3, Func<T> customInitFunc) where T : ISoluble<V, U, K>
        {
            Type type = typeof(T);
            T value;
            if (_classPool.ContainsKey(type) && _classPool[type].First != null)
            {
                value = (T)_classPool[type].First.Value.obj;
                _classPool[type].RemoveFirst();
                value.ResetData(param1, param2, param3);
                _classSet.Remove(value);
            }
            else
            {
                value = customInitFunc();
                value.ResetData(param1, param2, param3);
            }
            return value;
        }


        /// <summary>
        /// Recycle a instance of class, for reuse.
        /// </summary>
        /// <param name="obj"></param>
        public void Recycle(IBaseSoluble obj)
        {
            if (!_classSet.ContainsKey(obj))
            {
                obj.ClearData();
                Type type = obj.GetType();
                PooledObj pooledObj = new PooledObj(obj);
                if (_classPool.ContainsKey(type))
                    _classPool[type].AddFirst(pooledObj);
                else
                {
                    LinkedList<PooledObj> list = new LinkedList<PooledObj>();
                    list.AddFirst(pooledObj);
                    _classPool[type] = list;
                }
                _classSet.Add(obj, pooledObj);
            }
            //else
            //    throw new ShockException("Pool contain this obj already");
        }


        /// <summary>
        /// Clear all pool data.
        /// </summary>
        public void ClearAll()
        {
            _classPool.Clear();
            _classSet.Clear();
        }


        /// <summary>
        /// Clear all pool data of T.
        /// </summary>
        /// <typeparam name="T">The type you want clear</typeparam>
        /// <param name="isDoClearFunc">True means will run "ClearData" function</param>
        public void Clear<T>(bool isDoClearFunc = false) where T : IBaseSoluble
        {
            Type type = typeof(T);
            if (_classPool.ContainsKey(type))
            {
                LinkedList<PooledObj> list = _classPool[type];
                while (list.First != null)
                {
                    T value = (T)_classPool[type].First.Value.obj;
                    _classPool[type].RemoveFirst();
                    if (isDoClearFunc)
                        value.ClearData();
                    _classSet.Remove(value);
                }
                _classPool.Remove(type);
            }
        }


        /// <summary>
        /// Remove data from pool, this function will traversal LinkedList.
        /// </summary>
        /// <typeparam name="T">The type you want remove</typeparam>
        /// <param name="instance">The instance you want remove</param>
        /// <param name="isDoClearFunc">True means will run "ClearData" function</param>
        public void Remove<T>(T instance, bool isDoClearFunc = false) where T : IBaseSoluble
        {
            Type type = typeof(T);
            if (_classPool.ContainsKey(type) && _classSet.ContainsKey(instance))
            {
                LinkedList<PooledObj> list = _classPool[type];
                list.Remove(_classSet[instance]);
                if (isDoClearFunc)
                    instance.ClearData();
                _classSet.Remove(instance);
                if (list.Count == 0)
                    _classPool.Remove(type);
            }
        }

        #region Private Part

#if UNITY_EDITOR
        public struct ForEditorData
        {
            public Dictionary<Type, int> classPool;

            public ForEditorData(PoolManager manager)
            {
                classPool = new Dictionary<Type, int>();
                foreach (Type type in manager._classPool.Keys)
                {
                    classPool.Add(type, manager._classPool[type].Count);
                }
            }
        }

        public ForEditorData _GetEditorData()
        {
            return new ForEditorData(this);
        }

#endif

        private class PooledObj
        {
            public IBaseSoluble obj;

            public float useTime;

            public PooledObj(IBaseSoluble obj)
            {
                this.obj = obj;
                useTime = Time.time;
            }
        }

        private Dictionary<Type, LinkedList<PooledObj>> _classPool = new Dictionary<Type, LinkedList<PooledObj>>();
        private Dictionary<IBaseSoluble, PooledObj> _classSet = new Dictionary<IBaseSoluble, PooledObj>(); //Prevent the same obj in pool, and manage the memory collection.

        private static PoolManager _instance;
        private GameObject _poolParent;

        private void Awake()
        {
            if (instance != null)
                instance.ClearAll();
            instance = this;

            if (dontDestroyOnLoad)
                DontDestroyOnLoad(gameObject);

            new Thread(_PeriodicCollection).Start();
        }

#if UNITY_EDITOR

        private void Update()
        {
            //For editor refresh.
            //*****
            //Unity's editor refresh function only run when something change, so change this for view of pool num.
            transform.position = transform.position.x == 0 ? Vector3.one : Vector3.zero;
        }

#endif

        private void _PeriodicCollection()
        {
            while (true)
            {
                Thread.Sleep(collectFrequncyInMilliseconds);
                OperationAgency.instance.AddUpdateAgency(_Collection);
            }
        }

        private void _Collection()
        {
            bool isCollect = false;
            foreach (LinkedList<PooledObj> list in _classPool.Values)
            {
                LinkedListNode<PooledObj> listNode = list.First;
                while (listNode != null)
                {
                    PooledObj value = listNode.Value;
                    if (Time.time - value.useTime >= collectCheckInSeconds)
                    {
                        LinkedListNode<PooledObj> tempNode = listNode.Next;
                        list.Remove(listNode);
                        _classSet.Remove(listNode.Value.obj);
                        value.obj.DeleteData();
                        listNode = tempNode;
                    }
                    else
                        listNode = listNode.Next;
                }
            }

            if (isCollect)
                GC.Collect();
        }


        #endregion
    }
}