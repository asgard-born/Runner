using System;
using UnityEngine;

namespace Code.ObjectsPool
{
    public class PoolObject : MonoBehaviour
    {
        [NonSerialized] public Transform poolParent;

        public void ReturnToPool()
        {
            gameObject.SetActive(false);
            transform.position = poolParent.position;
            transform.parent = poolParent;
        }

        public virtual void Init()
        {
            
        }
    }
}