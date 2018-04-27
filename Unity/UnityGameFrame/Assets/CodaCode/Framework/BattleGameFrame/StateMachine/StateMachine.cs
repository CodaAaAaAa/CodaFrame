 
using System.Collections.Generic;

namespace Coda.Battle
{
    public abstract class StateMachine
    {
        /// <summary>
        /// Main update for battle.
        /// </summary>
        public virtual void Update(float deltaTime)
        {
            if (stateAction.ContainsKey(mCurrentState))
                stateAction[mCurrentState].Update(deltaTime);
        }


        /// <summary>
        /// Update action for Mono's update.
        /// </summary>
        public virtual void FrameUpdate()
        {
            if (stateAction.ContainsKey(mCurrentState))
                stateAction[mCurrentState].FrameUpdate();
        }


        /// <summary>
        /// Can't change state.
        /// </summary>
        public bool banStateChange;

        
        /// <summary>
        /// Use this function to initialize "stateAction".
        /// </summary>
        public abstract void StateActionInit();


        protected Dictionary<int, StateAction> stateAction = new Dictionary<int, StateAction>();
        protected int mCurrentState { get { return _currentState; } private set { _currentState = value; } }
        protected int mLastState { get; private set; }

        protected void UpdateState(int newState, params object[] param)
        {
            if (banStateChange) return;

            if (stateAction.ContainsKey(newState))
            {
                if (stateAction.ContainsKey(mCurrentState))
                    stateAction[mCurrentState].Terminate();
                mLastState = mCurrentState;

                stateAction[newState].Reset(param);
                mCurrentState = newState;
            }
        }

        private int _currentState = -99;
    }
}