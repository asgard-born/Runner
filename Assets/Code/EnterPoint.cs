using Code.Configs;
using Code.Platforms;
using Code.Player;
using UnityEngine;

namespace Code
{
    public class EnterPoint : MonoBehaviour
    {
        [SerializeField] private LevelConfigs _levelConfigs;
        [SerializeField] private PlayersConfigs _playersConfigs;
        [SerializeField] private PoolsConfigs _poolsConfigs;
        [SerializeField] private Transform _playerSpawnPoint;
        [SerializeField] private Transform _platformsParent;

        private PlayerEntity _player;
        private PlayerMovingSystem _movingSystem;
        private PlatformsSpawningSystem _spawningSystem;

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            _poolsConfigs.Initialize();
            SpawnPlayer();

            var movingSystemCtx = new PlayerMovingSystem.Ctx
            {
                player = _player,
                playersConfigs = _playersConfigs
            };

            _movingSystem = new PlayerMovingSystem(movingSystemCtx);
            _spawningSystem = new PlatformsSpawningSystem(_levelConfigs);
            _spawningSystem.StartSpawning(_platformsParent);
        }

        private void SpawnPlayer()
        {
            _player = Instantiate(_playersConfigs.playerPrefab, _playerSpawnPoint);
        }
        
        
    }
}