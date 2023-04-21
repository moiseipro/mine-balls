using System;
using System.Collections.Generic;
using UnityEngine;

namespace Source.TutorialSystem.Views
{
    public class TutorialStarter : MonoBehaviour
    {
        //[SerializeField] private TutorialPanel[] _tutorialPanelPrefab;
        private List<TutorialPanel> _tutorialList = new List<TutorialPanel>();
        private TutorialPanel _currentTutorialPanel;

        public void AddTutorial(TutorialTarget tutorialTarget)
        {
            TutorialPanel newTutorialPanel = Instantiate(tutorialTarget.tutorialPanelPrefab, transform);
            tutorialTarget.OnPlayTutorial += newTutorialPanel.StartTutorial;
            if (tutorialTarget.nextTutorialTarget)
            {
                newTutorialPanel.OnCloseTutorial += tutorialTarget.nextTutorialTarget.StartTutorial;
            }
            _tutorialList.Add(newTutorialPanel);
        }
    }
}