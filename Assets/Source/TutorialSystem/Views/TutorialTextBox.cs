using System;
using UnityEngine;
using UnityEngine.UI;

namespace Source.TutorialSystem.Views
{
    public class TutorialTextBox : MonoBehaviour
    {
        private RectTransform _rectTransform;
        [SerializeField] private Text _tutorialText;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        public Vector3 GetPosition()
        {
            return _rectTransform.position;
        }
    }
}