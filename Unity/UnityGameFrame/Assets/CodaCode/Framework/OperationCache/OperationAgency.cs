
using UnityEngine;

using System.Collections.Generic;

namespace Coda.Tools
{
    public class OperationAgency : MonoBehaviour
    {
        /// <summary>
        /// Single instance of OperationAgency.
        /// </summary>
        public static OperationAgency instance { get; private set; }


        /// <summary>
        /// Should always exist.
        /// </summary>
        public bool dontDestroyOnLoad;


        /// <summary>
        /// Clear all data.
        /// </summary>
        public void Clear()
        {
            _updateCache.Clear();
            _fixedUpdateCache.Clear();
            _lateUpdateCache.Clear();
        }

        #region Functions of AddUpdateAgency
        /// <summary>
        /// Operation will run in next Update.
        /// </summary>
        public void AddUpdateAgency(BaseOperationCache operation)
        {
            _updateCache.Enqueue(operation);
        }


        /// <summary>
        /// Operation will run in next Update.
        /// </summary>
        public void AddUpdateAgency(CodaUtility.VoidDelegate callBack, CodaUtility.VoidDelegate onComplete = null)
        {
            _updateCache.Enqueue(new OperationCache(callBack, onComplete));
        }


        /// <summary>
        /// Operation will run in next Update.
        /// </summary>
        public void AddUpdateAgency<T>(CodaUtility.VoidDelegate<T> callBack, T p, CodaUtility.VoidDelegate onComplete = null)
        {
            _updateCache.Enqueue(new OperationCache<T>(callBack, p, onComplete));
        }


        /// <summary>
        /// Operation will run in next Update.
        /// </summary>
        public void AddUpdateAgency<T, U>(CodaUtility.VoidDelegate<T, U> callBack, T p1, U p2, CodaUtility.VoidDelegate onComplete = null)
        {
            _updateCache.Enqueue(new OperationCache<T, U>(callBack, p1, p2, onComplete));
        }


        /// <summary>
        /// Operation will run in next Update.
        /// </summary>
        public void AddUpdateAgency<T, U, V>(CodaUtility.VoidDelegate<T, U, V> callBack, T p1, U p2, V p3, CodaUtility.VoidDelegate onComplete = null)
        {
            _updateCache.Enqueue(new OperationCache<T, U, V>(callBack, p1, p2, p3, onComplete));
        }
        #endregion
        #region Functions of AddFixedUpdateAgency
        /// <summary>
        /// Operation will run in next FixedUpdate.
        /// </summary>
        public void AddFixedUpdateAgency(BaseOperationCache operation)
        {
            _fixedUpdateCache.Enqueue(operation);
        }


        /// <summary>
        /// Operation will run in next FixedUpdate.
        /// </summary>
        public void AddFixedUpdateAgency(CodaUtility.VoidDelegate callBack, CodaUtility.VoidDelegate onComplete = null)
        {
            _fixedUpdateCache.Enqueue(new OperationCache(callBack, onComplete));
        }


        /// <summary>
        /// Operation will run in next FixedUpdate.
        /// </summary>
        public void AddFixedUpdateAgency<T>(CodaUtility.VoidDelegate<T> callBack, T p, CodaUtility.VoidDelegate onComplete = null)
        {
            _fixedUpdateCache.Enqueue(new OperationCache<T>(callBack, p, onComplete));
        }


        /// <summary>
        /// Operation will run in next FixedUpdate.
        /// </summary>
        public void AddFixedUpdateAgency<T, U>(CodaUtility.VoidDelegate<T, U> callBack, T p1, U p2, CodaUtility.VoidDelegate onComplete = null)
        {
            _fixedUpdateCache.Enqueue(new OperationCache<T, U>(callBack, p1, p2, onComplete));
        }


        /// <summary>
        /// Operation will run in next FixedUpdate.
        /// </summary>
        public void AddFixedUpdateAgency<T, U, V>(CodaUtility.VoidDelegate<T, U, V> callBack, T p1, U p2, V p3, CodaUtility.VoidDelegate onComplete = null)
        {
            _fixedUpdateCache.Enqueue(new OperationCache<T, U, V>(callBack, p1, p2, p3, onComplete));
        }
        #endregion
        #region Functions of AddLateUpdateAgency
        /// <summary>
        /// Operation will run in next LateUpdate.
        /// </summary>
        public void AddLateUpdateAgency(BaseOperationCache operation)
        {
            _lateUpdateCache.Enqueue(operation);
        }


        /// <summary>
        /// Operation will run in next LateUpdate.
        /// </summary>
        public void AddLateUpdateAgency<T>(CodaUtility.VoidDelegate<T> callBack, T p, CodaUtility.VoidDelegate onComplete = null)
        {
            _lateUpdateCache.Enqueue(new OperationCache<T>(callBack, p, onComplete));
        }


        /// <summary>
        /// Operation will run in next LateUpdate.
        /// </summary>
        public void AddLateUpdateAgency<T, U>(CodaUtility.VoidDelegate<T, U> callBack, T p1, U p2, CodaUtility.VoidDelegate onComplete = null)
        {
            _lateUpdateCache.Enqueue(new OperationCache<T, U>(callBack, p1, p2, onComplete));
        }


        /// <summary>
        /// Operation will run in next LateUpdate.
        /// </summary>
        public void AddLateUpdateAgency<T, U, V>(CodaUtility.VoidDelegate<T, U, V> callBack, T p1, U p2, V p3, CodaUtility.VoidDelegate onComplete = null)
        {
            _lateUpdateCache.Enqueue(new OperationCache<T, U, V>(callBack, p1, p2, p3, onComplete));
        }
        #endregion

        #region Private Part

        private Queue<BaseOperationCache> _updateCache = new Queue<BaseOperationCache>();
        private Queue<BaseOperationCache> _fixedUpdateCache = new Queue<BaseOperationCache>();
        private Queue<BaseOperationCache> _lateUpdateCache = new Queue<BaseOperationCache>();

        private void Awake()
        {
            instance = this;

            if (dontDestroyOnLoad)
                DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            while (_updateCache.Count > 0)
                _updateCache.Dequeue().Run();
        }

        private void FixedUpdate()
        {
            while (_fixedUpdateCache.Count > 0)
                _fixedUpdateCache.Dequeue().Run();
        }

        private void LateUpdate()
        {
            while (_lateUpdateCache.Count > 0)
                _lateUpdateCache.Dequeue().Run();
        }
        #endregion
    }
}