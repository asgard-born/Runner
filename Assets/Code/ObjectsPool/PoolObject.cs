using UnityEngine;

namespace Code.ObjectsPool
{
    public class PoolObject : MonoBehaviour
    {
        public Transform poolParent;
        public void ReturnToPool()
        {
            gameObject.SetActive(false);
            transform.position = poolParent.position;
            transform.parent = poolParent;
        }
    }
}