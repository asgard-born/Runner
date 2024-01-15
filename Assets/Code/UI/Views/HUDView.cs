using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace UI.Views
{
    public class HUDView : MonoBehaviour
    {
        [SerializeField] private GameObject _lifePrefabView;
        [SerializeField] private Transform _livesContainer;

        private List<GameObject> _spawnedLives = new();

        public struct Ctx
        {
            public ReactiveProperty<int> lives;
        }

        public void SetContext(Ctx ctx)
        {
            ctx.lives.Subscribe(OnLivesChanges).AddTo(this);
        }


        private void OnLivesChanges(int newValue)
        {
        }
    }
}