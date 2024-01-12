using System.Collections.Generic;
using Behaviour;
using Framework;
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
            public Transform spawnPoint;

            public ReactiveCommand<CharacterBehaviourPm> onBehaviourCreated;
            public ReactiveCommand<Transform> onCharacterInitialized;
        }

        public CharacterPm(Ctx ctx)
        {
            _ctx = ctx;

            AddUnsafe(ctx.onBehaviourCreated.Subscribe(OnBehaviourCreated));
            InitializeCharacter();
        }

        private void InitializeCharacter()
        {
            _ctx.characterTransform.position = _ctx.spawnPoint.position;
            _ctx.characterTransform.rotation = _ctx.spawnPoint.rotation;

            _ctx.onCharacterInitialized?.Execute(_ctx.characterTransform);
        }

        private void OnBehaviourCreated(CharacterBehaviourPm behaviour)
        {
            
        }
    }
}