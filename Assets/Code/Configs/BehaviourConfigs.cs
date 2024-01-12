using Shared;
using UnityEngine;

namespace Configs
{
    /// <summary>
    /// Конфиг, который мы создаем при добавлении в игру нового типа поведения
    /// При создании, мы устанавливаем контракт тип-название
    /// Поведения одних и тех же типов будет взаимоисключаемым
    /// </summary>
    [CreateAssetMenu(fileName = "Behaviour_Configs", menuName = "Configs/Behaviour Configs")]
    public class BehaviourConfigs : ScriptableObject
    {
        [SerializeField] private BehaviourType _type;
        [SerializeField] private BehaviourName _name;
        
        [SerializeField] private GameObject[] _effects;
        [SerializeField] private float _durationSec;
        
        public BehaviourType type => _type;
        public BehaviourName name => _name;
        public GameObject[] effects => _effects;
    }
}