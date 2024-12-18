﻿using System.Collections.Generic;
using Shared;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Items
{
    /// <summary>
    /// Класс-представление собираемых предметов. Им может быть как монета, так и другой объект
    /// Имеет возможность содержать или не содержать в себе конфиги поведений
    /// </summary>
    public class ItemView : MonoBehaviour
    {
        [SerializeField] private bool _isRotating;
        [SerializeField] private bool _isCoin;
        [ShowIf("_isRotating"), SerializeField] private float _rotationSpeed = 50f;

        [Space, SerializeField] private List<BehaviourInfo> _behaviours;

        public List<BehaviourInfo> behaviours => _behaviours;
        public bool isCoin => _isCoin;

        private void Update()
        {
            if (_isRotating)
            {
                transform.Rotate(Vector3.up * _rotationSpeed * Time.deltaTime, Space.World);
            }
        }
    }
}