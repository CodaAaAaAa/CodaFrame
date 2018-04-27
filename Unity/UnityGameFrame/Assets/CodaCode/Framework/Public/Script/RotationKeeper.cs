
using UnityEngine;

namespace Coda.Tools
{
    [ExecuteInEditMode]
    public class RotationKeeper : MonoBehaviour
    {
        public KeepType keepType { get { return _keepType; } }
        public bool fixLocalRotation = true;

        public enum KeepType
        {
            FixedAxis,
            FixedAngle,
        }


        /// <summary>
        /// Change keeper mode.
        /// </summary>
        /// <param name="keepValue">if mode is "FixedAxis", keepValue is a axis, else keepValue is a euler angle.</param>
        public void ChangeKeepMode(KeepType keepType, Vector3 keepValue)
        {
            _keepType = keepType;
            _fixedValue = keepValue;
        }


        /// <summary>
        /// Run this function if you want to fix rotation to pre-setting.
        /// </summary>
        public void FixRotation()
        {
            if (_keepType == KeepType.FixedAngle)
                _rotation = Quaternion.Euler(_fixedValue);
            else
            {
                _rotation = Quaternion.FromToRotation(_up, _fixedValue) * _rotation;
            }
        }


        #region Private Part

        [SerializeField]private KeepType _keepType;
        [SerializeField]private Vector3 _fixedValue;

        private Quaternion _rotation
        {
            get { return fixLocalRotation ? transform.localRotation : transform.rotation; }
            set
            {
                if (fixLocalRotation)
                    transform.localRotation = value;
                else
                    transform.rotation = value;
            }
        }
        private Vector3 _up
        {
            get
            {
                if (fixLocalRotation)
                {
                    if (transform.parent != null)
                        return Quaternion.Inverse(transform.parent.rotation) * transform.up;
                    else
                        return transform.up;
                }
                else
                    return transform.up;
            }
        }

        private void Update()
        {
            FixRotation();
        }

        #endregion
    }
}