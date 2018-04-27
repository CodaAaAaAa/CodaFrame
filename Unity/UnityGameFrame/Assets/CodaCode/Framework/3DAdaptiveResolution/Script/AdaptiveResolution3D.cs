
using UnityEngine;

namespace Coda.Tools
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    public class AdaptiveResolution3D : MonoBehaviour
    {
        public float maxAspect { get { return _maxAspect; } }
        public float minAspect { get { return _minAspect; } }


        [ContextMenu("Execute")]
        public void ReAdaptResolution()
        {
#if UNITY_EDITOR
            if (!_LegalCheck()) return;
#endif

#if UNITY_EDITOR
            if (_gameViewSizeMethod == null)
            {
                System.Type type = System.Type.GetType("UnityEditor.GameView,UnityEditor");

                _gameViewSizeMethod = type.GetMethod("GetMainGameViewTargetSize",
                            System.Reflection.BindingFlags.Public |
                            System.Reflection.BindingFlags.NonPublic |
                            System.Reflection.BindingFlags.Static);
            }
            Vector2 screenSize = (Vector2)_gameViewSizeMethod.Invoke(null, null);
            float currentAspect = screenSize.x / screenSize.y;
#else
            float currentAspect = Screen.width / Screen.height;
#endif

            _adaptiveCamera.rect = new Rect(0, 0, 1, 1);

            if (currentAspect < _minAspect)
            {
                float fixRate = (1 - currentAspect / _minAspect) / 2;
                Rect viewportRect = new Rect(0, fixRate, 1, 1 - 2 * fixRate);
                _adaptiveCamera.rect = viewportRect;
            }
            else if (currentAspect > _maxAspect)
            {
                float fixRate = (1 - _maxAspect / currentAspect) / 2;
                Rect viewportRect = new Rect(fixRate, 0, 1 - 2 * fixRate, 1);
                _adaptiveCamera.rect = viewportRect;
            }

            if (_fixType != FixType.None)
            {
                if (_fixType == FixType.FixAsWeight)
                {
                    _adaptiveCamera.fieldOfView = _fieldOfView / currentAspect;
                }
                else if (_fixType == FixType.FixAsHeight)
                {
                    _adaptiveCamera.fieldOfView = _fieldOfView;
                }
            }
        }


        #region Private Part
#if UNITY_EDITOR
        [SerializeField]private Vector2 _maxResolution;
        [SerializeField]private Vector2 _minResolution;
        private System.Reflection.MethodInfo _gameViewSizeMethod;
#endif

        [SerializeField]private float _maxAspect;
        [SerializeField]private float _minAspect;
        [SerializeField]private float _fieldOfView;
        [SerializeField]private FixType _fixType;
        private Camera _adaptiveCamera;

        private enum FixType
        {
            None,
            FixAsWeight,
            FixAsHeight,
        }

        private void Awake()
        {
            _adaptiveCamera = GetComponent<Camera>();
            
            ReAdaptResolution();
        }

#if UNITY_EDITOR
        private bool _LegalCheck()
        {
            if (_adaptiveCamera.orthographic)
            {
                Debug.LogError("Only perspective camera can use this script.");
                return false;
            }

            if (_maxAspect < _minAspect)
            {
                Debug.LogError("Serialize property is not legal.");
                return false;
            }

            if (_maxResolution.y == 0 || _minResolution.y == 0 || float.IsNaN(_minAspect) || float.IsNaN(_maxAspect) || float.IsInfinity(_minAspect) || float.IsInfinity(_maxAspect))
            {
                Debug.LogError("Please input correct resolution.");
                return false;
            }
            return true;
        }
#endif
        #endregion
    }
}