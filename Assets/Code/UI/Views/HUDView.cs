using System.Collections.Generic;
using UnityEngine;

namespace Code.UI.Views
{
    public class HUDView : MonoBehaviour
    {
        [SerializeField] private Transform _content;
        [SerializeField] private LifeItemUI _lifePrefab;

        private LinkedList<LifeItemUI> _lives = new LinkedList<LifeItemUI>();

        public void Show(int lives)
        {
            for (var i = 0; i < lives; i++)
            {
                AddLife();
            }
        }

        public void AddLife()
        {
            _lives.AddLast(Instantiate(_lifePrefab, _content));
        }

        public void RemoveLife()
        {
            var life = _lives.Last.Value;
            Destroy(life.gameObject);
            _lives.RemoveLast();
        }
    }
}