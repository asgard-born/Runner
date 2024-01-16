using UnityEngine;

namespace Data.Imported.Police_Car___Helicopter
{
    public class Rotor : MonoBehaviour
    {
        [SerializeField] private Transform rotor;
        [SerializeField] private float _speed;

        private void Update()
        {
            rotor.Rotate(Vector3.up * _speed * Time.deltaTime, Space.World);
        }
    }
}