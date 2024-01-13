﻿using System.Collections.Generic;
using Behaviour;
using Framework;
using Framework.Reactive;
using Shared;
using UniRx;
using UnityEngine;

namespace Character
{
    /// <summary>
    /// Управляет поведениями  персонажа
    /// </summary>
    public class CharacterPm : BaseDisposable
    {
        private Dictionary<BehaviourType, CharacterBehaviourPm> _behaviours = new();

        private Ctx _ctx;

        public struct Ctx
        {
            public Transform characterTransform;
            public RoadlinePoint spawnPoint;
            public BehaviourInfo initialBehaviourInfo;

            public ReactiveCommand<BehaviourInfo> onBehaviourTaken;
            public ReactiveTrigger<BehaviourType, CharacterBehaviourPm> onNewBehaviourProduced;
            public ReactiveCommand<BehaviourType> onBehaviourAdded;
            public ReactiveCommand<Transform> onCharacterInitialized;
        }

        public CharacterPm(Ctx ctx)
        {
            _ctx = ctx;

            AddUnsafe(ctx.onNewBehaviourProduced.Subscribe(OnNewBehaviourProduced));
            InitializeCharacter();
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
    }
}