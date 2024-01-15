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
    /// Управляет поведениями персонажа
    /// </summary>
    public class CharacterPm : BaseDisposable
    {
        private Dictionary<BehaviourType, CharacterBehaviourPm> _behaviours = new();

        private Ctx _ctx;

        public struct Ctx
        {
            public CharacterState state;
            public Transform characterTransform;
            public RoadlinePoint spawnPoint;
            public BehaviourInfo initialBehaviourInfo;

            public ReactiveCommand<BehaviourInfo> onBehaviourTaken;
            public ReactiveTrigger<BehaviourType, CharacterBehaviourPm> onNewBehaviourProduced;
            public ReactiveCommand<BehaviourType> onBehaviourAdded;
            public ReactiveCommand<Transform> onCharacterInitialized;
            public ReactiveCommand<BehaviourType> onBehaviourFinished;
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
        }

        

        private void InitializeCharacter()
        {
            _ctx.characterTransform.position = _ctx.spawnPoint.transform.position;
            _ctx.characterTransform.rotation = _ctx.spawnPoint.transform.rotation;

            _ctx.onCharacterInitialized?.Execute(_ctx.characterTransform);
            _ctx.onBehaviourTaken?.Execute(_ctx.initialBehaviourInfo);
        }

        private void OnNewBehaviourProduced(BehaviourType type, CharacterBehaviourPm newBehaviour)
        {
            if (_behaviours.TryGetValue(type, out var oldBehaviour))
            {
                oldBehaviour.Dispose();
                _behaviours[type] = newBehaviour;
            }
            else
            {
                _behaviours.Add(type, newBehaviour);
            }

            _ctx.onBehaviourAdded?.Execute(type);
        }

        private void OnBehaviourFinished(BehaviourType type)
        {
            if (_behaviours.TryGetValue(type, out var finishedBehaviour))
            {
                finishedBehaviour.Dispose();
            }
            else
            {
                Debug.LogError("The behaviour must be in dictionary");
            }

            // Если завершился жизненный цикл поведения, подменяющего наше дефолтное, мы возвращаем его обратно
            if (type == _ctx.initialBehaviourInfo.configs.type)
            {
                _ctx.onBehaviourTaken?.Execute(_ctx.initialBehaviourInfo);
            }
        }
    }
}