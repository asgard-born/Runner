using System.Collections.Generic;
using Shared;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Items
{
    public class Item : SerializedMonoBehaviour
    {
        [SerializeField] private List<BehaviourInfo> _behaviours;

        public List<BehaviourInfo> behaviours => _behaviours;
    }
}