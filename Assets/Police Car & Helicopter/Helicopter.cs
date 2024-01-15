using UnityEngine;

namespace Police_Car___Helicopter
{
    public class Helicopter : MonoBehaviour
    {
        [SerializeField] private Transform rotor;
        [SerializeField] private float _speed;

        private void Update()
        {
            rotor.Rotate(Vector3.up * _speed * Time.deltaTime, Space.World);
        }
    }
}