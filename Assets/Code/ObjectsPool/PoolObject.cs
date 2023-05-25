using UnityEngine;

namespace Code.ObjectsPool
{
    public class PoolObject : MonoBehaviour
    {
        public void ReturnToPool()
        {
            gameObject.SetActive(false);
            transform.position = Vector3.zero;
        }
    }
}