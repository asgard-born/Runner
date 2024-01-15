using UnityEngine;

namespace Items
{
    public class Coin : Item
    {
        [Space, SerializeField] private float _rotationSpeed = 50f;

        private void Update()
        {
            transform.Rotate(Vector3.up * _rotationSpeed * Time.deltaTime, Space.World);
        }
    }
}