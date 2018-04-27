
namespace Coda.Tools
{
    public class TrigerInUpdate
    {
        public delegate bool TrigerChecker();
        public delegate void OnTriger();

        public TrigerInUpdate(CodaUtility.BoolDelegate checker, CodaUtility.VoidDelegate callBack)
        {
            ResetData(checker, callBack);
        }

        public void ResetData(CodaUtility.BoolDelegate checker, CodaUtility.VoidDelegate callBack)
        {
            _checker = checker;
            _callBack = callBack;
            ResetData();
        }

        public void ResetData()
        {
            _isTriged = false;
        }

        public bool Update()
        {
            if (!_isTriged && _checker())
            {
                _isTriged = true;
                _callBack();
            }

            return _isTriged;
        }

        private CodaUtility.BoolDelegate _checker;
        private CodaUtility.VoidDelegate _callBack;
        private bool _isTriged;
    }
}