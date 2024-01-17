using System;
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
    public class BehaviourConfigs : SerializedScriptableObject
    {
        [SerializeField] private BehaviourKey _key;
        [SerializeField] private Type _behaviourType;
        [SerializeField] private GameObject[] _effects;
        [SerializeField] private bool _isChangingSpeed;
        // для разных поведений нужна разная скорость, лаконично будет скомпоновать в Vector3
        [SerializeField, ShowIf("_isChangingSpeed")] private Vector3 _speed;
        [SerializeField] private float _height;

        public BehaviourKey key => _key;
        public Type type => _behaviourType;
        public GameObject[] effects => _effects;
        public Vector3 speed => _speed;
        public float height => _height;
    }
}