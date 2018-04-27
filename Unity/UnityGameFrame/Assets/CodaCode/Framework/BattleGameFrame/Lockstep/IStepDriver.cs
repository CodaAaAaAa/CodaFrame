
namespace Coda.Battle
{
    public interface IStepDriver
    {
        void StepUpdate(float deltaTime);
        void DoOperation(BaseStepMessage message);
    }
}
