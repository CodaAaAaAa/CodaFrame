
namespace Coda.Battle
{
    public class StateAction
    {
        /// <summary>
        /// Main update for battle.
        /// </summary>
        public void Update(float deltaTime)
        {
            if (!isActivate)
            {
                Activate();
                isActivate = true;
            }

            Process(deltaTime);
        }


        /// <summary>
        /// Update action for Mono's update.
        /// </summary>
        public void FrameUpdate()
        {
            UpdateProcess();
        }


        /// <summary>
        /// Exit from this action.
        /// </summary>
        public void Terminate()
        {
            if (isActivate) { End(); }
        }


        /// <summary>
        /// Start this action.
        /// </summary>
        public void Reset(params object[] param)
        {
            isActivate = false;
            ResetData(param);
        }

        protected bool isActivate = false;

        protected virtual void ResetData(object[] param) { }
        protected virtual void Activate() { }
        protected virtual void Process(float deltaTime) { }
        protected virtual void UpdateProcess() { }
        protected virtual void End() { }
    }
}