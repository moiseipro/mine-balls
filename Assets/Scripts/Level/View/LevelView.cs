using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Level.View
{
    public class LevelView : MonoBehaviour
    {
        [SerializeField] private float _levelHeight = 5f;
        public float LevelHeight => _levelHeight;

        private bool _levelNotComplete = true;
        public bool LevelNotComplete => _levelNotComplete;
        [SerializeField] private Transform _props;

        private BorderView[] _borderViews;
        
        public void StartLevel()
        {
            _borderViews = _props.GetComponentsInChildren<BorderView>();
        }

        public BorderView GetRandomActiveBorder()
        {
            BorderView randomBorder = null;
            for (int i = 0; i < _borderViews.Length; i++)
            {
                if (_borderViews[i].isActiveAndEnabled && Random.Range(0, _borderViews.Length) <= i)
                {
                    randomBorder = _borderViews[i];
                    break;
                }
            }
            return randomBorder;
        }

        public bool IsLevelComplete()
        {
            int enableCount = 0;
            foreach (var borderView in _borderViews)
            {
                _levelNotComplete = borderView.isActiveAndEnabled;
                if (_levelNotComplete)
                {
                    enableCount++;
                }
            }

            return enableCount<_borderViews.Length/3.3f;
        }
    }
}