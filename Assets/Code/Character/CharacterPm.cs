using System.Collections.Generic;
using Behaviour.Behaviours.Abstract;
using Framework;
using Framework.Reactive;
using Shared;
using UniRx;
using UnityEngine;

namespace Character
{
    /// <summary>
    /// Управляет поведениями персонажа: добавляет новые, если находит по типу
    /// в словаре старое взаимоисключающее поведение, то меняет его на новое.
    /// Например, Run/Fly типа Move или Fast/Slow типа Velocity
    /// </summary>
    public class CharacterPm : BaseDisposable
    {
        private Dictionary<BehaviourKey, CharacterBehaviourPm> _behaviours = new();

        private Ctx _ctx;

        public struct Ctx
        {
            public Transform characterTransform;
            public RoadlinePoint spawnPoint;
            public BehaviourInfo initialBehaviourInfo;

            public ReactiveCommand<BehaviourInfo> onBehaviourTaken;
            public ReactiveTrigger<BehaviourKey, CharacterBehaviourPm> onNewBehaviourProduced;
            public ReactiveCommand<BehaviourKey> onBehaviourAdded;
            public ReactiveCommand<Transform> onCharacterInitialized;
            public ReactiveCommand<BehaviourKey> onBehaviourFinished;
            public ReactiveTrigger onGameRun;
            public ReactiveTrigger onGameWin;
            public ReactiveTrigger onGameOver;
        }

        public CharacterPm(Ctx ctx)
        {
            _ctx = ctx;

            InitializeRx();
            InitializeCharacter();
        }

        private void InitializeRx()
        {
            AddUnsafe(_ctx.onNewBehaviourProduced.Subscribe(OnNewBehaviourProduced));
            AddUnsafe(_ctx.onBehaviourFinished.Subscribe(OnBehaviourFinished));
            AddUnsafe(_ctx.onGameRun.Subscribe(OnGameRun));
            AddUnsafe(_ctx.onGameWin.Subscribe(DisposeAll));
            AddUnsafe(_ctx.onGameOver.Subscribe(DisposeAll));
        }

        private void OnGameRun()
        {
            _ctx.onBehaviourTaken?.Execute(_ctx.initialBehaviourInfo);
        }

        private void InitializeCharacter()
        {
            _ctx.characterTransform.position = _ctx.spawnPoint.transform.position;
            _ctx.characterTransform.rotation = _ctx.spawnPoint.transform.rotation;
            
            _ctx.onCharacterInitialized?.Execute(_ctx.characterTransform);
        }

        private void OnNewBehaviourProduced(BehaviourKey key, CharacterBehaviourPm newBehaviour)
        {
            if (_behaviours.TryGetValue(key, out var oldBehaviour))
            {
                oldBehaviour.Dispose();
                _behaviours[key] = newBehaviour;
            }
            else
            {
                _behaviours.Add(key, newBehaviour);
            }

            _ctx.onBehaviourAdded?.Execute(key);
        }

        private void OnBehaviourFinished(BehaviourKey key)
        {
            if (_behaviours.TryGetValue(key, out var finishedBehaviour))
            {
                finishedBehaviour.Dispose();
            }
            else
            {
                Debug.LogError("The behaviour must be in dictionary");
            }

            // Если завершился жизненный цикл поведения, подменяющего наше дефолтное, мы возвращаем его обратно
            if (key == _ctx.initialBehaviourInfo.configs.key)
            {
                _ctx.onBehaviourTaken?.Execute(_ctx.initialBehaviourInfo);
            }
        }

        private void DisposeAll()
        {
            foreach (var behaviour in _behaviours)
            {
                behaviour.Value.Dispose();
            }

            _behaviours.Clear();
        }
    }
}