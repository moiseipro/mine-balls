using System;
using Source.TutorialSystem.Data;
using UnityEngine;

namespace Source.TutorialSystem.Views
{
    public class TutorialPanelPointer : TutorialPanel
    {
        [SerializeField] private TutorialPointer _tutorialPointerPrefab;

        private TutorialPointer _tutorialPointer;

        public override void StartTutorial(TutorialTarget tutorialTarget)
        {
            base.StartTutorial(tutorialTarget);
            _tutorialPointer = Instantiate(_tutorialPointerPrefab, transform);
            _tutorialPointer.SetTarget(tutorialTarget, _tutorialTextBox);
        }
        
    }
}