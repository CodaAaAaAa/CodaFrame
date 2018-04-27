
namespace Coda.Tools
{
    public abstract class BaseOperationCache
    {
        public CodaUtility.VoidDelegate onOperationComplete;

        public void Run()
        {
            DoOperation();
            if (onOperationComplete != null)
            {
                onOperationComplete();
            }
        }
        protected abstract void DoOperation();
    }
}
