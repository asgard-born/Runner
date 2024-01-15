using Shared;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Configs
{
    /// <summary>
    /// Конфиг, который мы создаем при добавлении в игру нового типа поведения
    /// При создании, мы устанавливаем контракт тип-название
    /// Поведения одних и тех же типов будет взаимоисключаемым
    /// </summary>
    [CreateAssetMenu(menuName = "Configs/Behaviour Configs", fileName = "Behaviour_Configs")]
    public class BehaviourConfigs : ScriptableObject
    {
        [SerializeField] private BehaviourType _type;
        [SerializeField] private BehaviourName _name;
        [SerializeField] private GameObject[] _effects;
        [SerializeField] private bool _hasSpeed;
        // для разных поведений нужна разная скорость, лаконично будет скомпоновать в Vector3
        [SerializeField, ShowIf("_hasSpeed")] private Vector3 _speed;
        [SerializeField] private float _height;

        public BehaviourType type => _type;
        public BehaviourName name => _name;
        public GameObject[] effects => _effects;
        public Vector3 speed => _speed;
        public float height => _height;
    }
}