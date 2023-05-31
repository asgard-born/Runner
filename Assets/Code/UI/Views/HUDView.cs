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
            AddLives(lives);
        }

        public void AddLives(int value)
        {
            for (var i = 0; i < value; i++)
            {
                _lives.AddLast(Instantiate(_lifePrefab, _content));
            }
        }

        public void RemoveLives(int value)
        {
            value = Mathf.Abs(value);
            
            for (var i = 0; i < value; i++)
            {
                if (_lives.Count == 0) return;

                var life = _lives.Last.Value;
                Destroy(life.gameObject);
                _lives.RemoveLast();
            }
        }
    }
}