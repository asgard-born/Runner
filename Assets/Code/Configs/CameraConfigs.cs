using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(menuName = "Configs/Camera Configs", fileName = "Camera_Configs")]
    public class CameraConfigs : ScriptableObject
    {
        [SerializeField] private float _smooth = 1.5f;
        [SerializeField] private Vector3 _positionOffset = new(0, 4, -6);
        [SerializeField] private Vector3 _rotationOffset = new(15, 0, 0);
        
        public float smooth => _smooth;
        public Vector3 positionOffset => _positionOffset;
        public Vector3 rotationOffset => _rotationOffset;
    }
}