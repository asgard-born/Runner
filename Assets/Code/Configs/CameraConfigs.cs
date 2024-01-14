using UnityEngine;

namespace Configs
{
    /// <summary>
    /// Конфиги для работы камеры
    /// </summary>
    [CreateAssetMenu(menuName = "Configs/Camera Configs", fileName = "Camera_Configs")]
    public class CameraConfigs : ScriptableObject
    {
        [SerializeField] private float _speed = 1.5f;
        [SerializeField] private Vector3 _positionOffset = new(0, 4, -6);
        [SerializeField] private Vector3 _rotationOffset = new(15, 0, 0);
        
        public float speed => _speed;
        public Vector3 positionOffset => _positionOffset;
        public Vector3 rotationOffset => _rotationOffset;
    }
}