using System;
using System.Collections.Generic;
using System.Linq;
using Configs;
using Shared;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Root
{
    public class EnterPoint : SerializedMonoBehaviour
    {
        [Header("Configs")] [SerializeField] private PlayersConfigs _playersConfigs;
        [SerializeField] private LevelConfigs _levelConfigs;
        [SerializeField] private ResourcesConfigs _resourcesConfigs;
        [SerializeField] private CameraConfigs _cameraConfigs;
        [Space, Header("UI")] [SerializeField] private RectTransform _uiRoot;
        [SerializeField] private List<Roadline> _roadlinePoints;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private Camera _camera;

        private GameRoot _root;

        private void Start()
        {
            InitializeGameRoot();
        }

        private void InitializeGameRoot()
        {
            var roadlineNode = GetListNode(_roadlinePoints);
            
            var rootCtx = new GameRoot.Ctx
            {
                playersConfigs = _playersConfigs,
                levelConfigs = _levelConfigs,
                resourcesConfigs = _resourcesConfigs,
                cameraConfigs = _cameraConfigs,
                uiRoot = _uiRoot,
                spawnRoadlineNode = roadlineNode,
                camera = _camera
            };

            _root = new GameRoot(rootCtx);
        }

        /// <summary>
        /// Конвертирует линию в ListNode для упрощенного оперирования через поля Next и Previous
        /// </summary>
        private LinkedListNode<Roadline> GetListNode(List<Roadline> list)
        {
            var roadline = _roadlinePoints.FirstOrDefault(r => r.transform == _spawnPoint);

            if (roadline == null)
            {
                Debug.LogException(new NullReferenceException("The spawn point must be one of roadlines"));
            }

            var roadlines = new LinkedList<Roadline>(_roadlinePoints);
            var roadlineNode = roadlines.Find(roadline);

            return roadlineNode;
        }

        private void OnDestroy()
        {
            _root.Dispose();
        }
    }
}