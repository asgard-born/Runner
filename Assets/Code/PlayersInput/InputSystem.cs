using System;
using UnityEngine;

namespace Code.PlayersInput
{
    public class InputSystem : MonoBehaviour
    {
        public Action HasTouched;
        
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                HasTouched?.Invoke();
            }
        }
    }
}