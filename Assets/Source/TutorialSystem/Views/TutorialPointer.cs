using System;
using UnityEngine;

namespace Source.TutorialSystem.Views
{
    public class TutorialPointer : MonoBehaviour
    {
        [SerializeField] private float _speed = 10f;
        [SerializeField] private float _maxСonvergence = 50f;
        private RectTransform _rectTransform;
        private TutorialTarget _tutorialTarget;
        private TutorialTextBox _tutorialTextBox;

        private Vector3 _movePosition;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            
        }

        public void SetTarget(TutorialTarget tutorialTarget, TutorialTextBox tutorialTextBox)
        {
            _tutorialTarget = tutorialTarget;
            _tutorialTextBox = tutorialTextBox;
            _movePosition = tutorialTextBox.GetPosition();
            _rectTransform.position = _movePosition;
        }

        private void Update()
        {
            if (_tutorialTarget && _tutorialTextBox)
            {
                _rectTransform.position = Vector3.MoveTowards(_rectTransform.position, _movePosition, Time.deltaTime * _speed);
            }
            if ((_rectTransform.position - _tutorialTarget.GetPosition()).magnitude < _maxСonvergence)
            {
                _movePosition = _tutorialTextBox.GetPosition();
            }
            if ((_rectTransform.position - _tutorialTextBox.GetPosition()).magnitude < _maxСonvergence)
            {
                _movePosition = _tutorialTarget.GetPosition();
            }
        }
    }
}