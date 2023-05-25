using Code.Configs;
using Code.Player;
using UnityEngine;

namespace Code
{
    public class EnterPoint : MonoBehaviour
    {
        [SerializeField] private GameConfigs _gameConfigs;
        [SerializeField] private PlatformsConfigs _platformsConfigs;
        [SerializeField] private PoolsConfigs _poolsConfigs;
        
        [SerializeField] private Transform _spawnPoint;
        private PlayerEntity _player;
        private PlayerMovingSystem _movingSystem;
        

        private void Awake()
        {
            
            _movingSystem = new PlayerMovingSystem();
        }

        private void Initialize()
        {
            
        }
    }
}