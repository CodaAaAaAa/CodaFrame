
namespace Coda.Tools
{
    /// <summary>
    /// 0 parameter cache.
    /// </summary>
    public class OperationCache : BaseOperationCache
    {
        private CodaUtility.VoidDelegate _function;

        public OperationCache(CodaUtility.VoidDelegate function, CodaUtility.VoidDelegate onComplete = null)
        {
            _function = function;
            onOperationComplete = onComplete;
        }

        protected override void DoOperation()
        {
            _function();
        }
    }


    /// <summary>
    /// 1 parameter cache.
    /// </summary>
    public class OperationCache<T> : BaseOperationCache
    {
        private CodaUtility.VoidDelegate<T> _function;
        private T p1;

        public OperationCache(CodaUtility.VoidDelegate<T> function, T p, CodaUtility.VoidDelegate onComplete = null)
        {
            _function = function;
            p1 = p;
            onOperationComplete = onComplete;
        }

        protected override void DoOperation()
        {
            _function(p1);
        }
    }


    /// <summary>
    /// 2 parameters cache.
    /// </summary>
    public class OperationCache<T, U> : BaseOperationCache
    {
        private CodaUtility.VoidDelegate<T, U> _function;
        private T p1;
        private U p2;

        public OperationCache(CodaUtility.VoidDelegate<T, U> function, T p, U u, CodaUtility.VoidDelegate onComplete = null)
        {
            _function = function;
            p1 = p;
            p2 = u;
            onOperationComplete = onComplete;
        }

        protected override void DoOperation()
        {
            _function(p1, p2);
        }
    }


    /// <summary>
    /// 3 parameter cache.
    /// </summary>
    public class OperationCache<T, U, V> : BaseOperationCache
    {
        private CodaUtility.VoidDelegate<T, U, V> _function;
        private T p1;
        private U p2;
        private V p3;

        public OperationCache(CodaUtility.VoidDelegate<T, U, V> function, T p, U u, V v, CodaUtility.VoidDelegate onComplete = null)
        {
            _function = function;
            p1 = p;
            p2 = u;
            p3 = v;
            onOperationComplete = onComplete;
        }

        protected override void DoOperation()
        {
            _function(p1, p2, p3);
        }
    }
}