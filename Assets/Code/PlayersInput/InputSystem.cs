using System;
using UnityEngine;

namespace Code.PlayersInput
{
    public class InputSystem : MonoBehaviour
    {
        public Action hasTouched;
        
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                hasTouched?.Invoke();
            }
        }
    }
}