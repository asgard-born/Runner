using Shared;
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
        [SerializeField] private float _speed;
        [SerializeField, Range(0, 30)] private float _sideSpeed;
        [SerializeField] private float _jumpForce;

        public BehaviourType type => _type;
        public BehaviourName name => _name;
        public GameObject[] effects => _effects;
        public float speed => _speed;
        public float sideSpeed => _sideSpeed;
        public float jumpForce => _jumpForce;
    }
}