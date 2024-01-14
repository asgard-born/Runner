using System.Collections.Generic;
using Shared;
using UnityEngine;

namespace Items
{
    public class Item : MonoBehaviour
    {
        [SerializeField] private List<BehaviourInfo> _behaviours;

        public List<BehaviourInfo> behaviours => _behaviours;
    }
}