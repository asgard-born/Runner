using System;
using System.Collections.Generic;
using System.Linq;
using Configs;
using Shared;
using Sirenix.Utilities;
using UnityEngine;

namespace Root
{
    /// <summary>
    /// Мост между входными данными и началом жизненного цикла игрового процесса
    /// Отвечает за валидацию контента для передачи его в корневой объект
    /// </summary>
    public class EnterPoint : MonoBehaviour
    {
        [Header("Configs")] [SerializeField] private PlayersConfigs _playersConfigs;
        [SerializeField] private LevelConfigs _levelConfigs;
        [SerializeField] private ResourcesConfigs _resourcesConfigs;
        [SerializeField] private CameraConfigs _cameraConfigs;
        [SerializeField] private List<RoadlinePoint> _roadlinePoints;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private Camera _camera;
        [Space, Header("UI")] [SerializeField] private RectTransform _uiRoot;

        private GameRoot _root;

        private void Start()
        {
            ValidateData();

            InitializeGameRoot();
        }

        private void InitializeGameRoot()
        {
            var rootCtx = new GameRoot.Ctx
            {
                playersConfigs = _playersConfigs,
                levelConfigs = _levelConfigs,
                resourcesConfigs = _resourcesConfigs,
                cameraConfigs = _cameraConfigs,
                uiRoot = _uiRoot,
                roadlinePoints = _roadlinePoints,
                spawnPoint = _roadlinePoints.First(r => r.transform == _spawnPoint),
                camera = _camera
            };

            _root = new GameRoot(rootCtx);
        }

        private void ValidateData()
        {
            ValidateConfigs(_playersConfigs);
            ValidateConfigs(_levelConfigs);
            ValidateConfigs(_resourcesConfigs);
            ValidateConfigs(_cameraConfigs);
            ValidateRoadlines();

            if (_spawnPoint == null)
            {
                Debug.LogException(new NullReferenceException("Spawn point cannot be null"));
            }

            if (_camera == null)
            {
                Debug.LogException(new NullReferenceException("Camera cannot be null"));
            }

            if (_uiRoot == null)
            {
                Debug.LogException(new NullReferenceException("UI Root cannot be null"));
            }
        }

        private void ValidateConfigs(ScriptableObject config)
        {
            if (config == null)
            {
                Debug.LogException(new NullReferenceException($"{config.name} cannot be null"));
            }
        }

        private void ValidateRoadlines()
        {
            if (_roadlinePoints.IsNullOrEmpty())
            {
                Debug.LogException(new NullReferenceException("RoadlinePoint points cannot be null or empty"));
            }

            var roadline = _roadlinePoints.FirstOrDefault(r => r.transform == _spawnPoint);

            if (roadline == null)
            {
                Debug.LogException(new NullReferenceException("The spawn point must be one of roadlines"));
            }
        }

        private void OnDestroy()
        {
            _root.Dispose();
        }
    }
}