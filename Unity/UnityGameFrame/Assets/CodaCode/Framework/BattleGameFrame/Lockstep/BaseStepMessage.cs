
namespace Coda.Battle
{
    public abstract class BaseStepMessage
    {
        public BaseStepMessage(string code) { ProtocalParse(code); }

        public abstract int frame { get; }

        public abstract void ProtocalParse(string code);
    }
}
