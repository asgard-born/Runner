using Character.Behaviour;
using Shared;
using UnityEngine;

namespace Items
{
    public abstract class Item<T> : MonoBehaviour where T : CharacterBehaviour
    {
        [SerializeField] protected BehaviourContainer<T> _behaviourType;

        public BehaviourContainer<T> behaviourType => _behaviourType;
    }
}