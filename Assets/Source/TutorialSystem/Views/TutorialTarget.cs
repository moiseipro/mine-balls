using System;
using UnityEngine;

namespace Source.TutorialSystem.Views
{
    public class TutorialTarget : MonoBehaviour
    {
        [SerializeField] private int _targetID;
        [SerializeField] private bool _isPause = false;
        public bool IsPause => _isPause;

        public TutorialPanel tutorialPanelPrefab;
        public TutorialTarget nextTutorialTarget;

        private RectTransform _rectTransform;
        
        private TutorialStarter _tutorialStarter;

        public Action<TutorialTarget> OnPlayTutorial;

        private void Awake()
        {
            //PlayerPrefs.DeleteAll();
            _rectTransform = GetComponent<RectTransform>();
            _tutorialStarter = FindObjectOfType<TutorialStarter>();
            InitializeTutorial();
        }

        public void StartTutorial()
        {
            if (!PlayerPrefs.HasKey("tutorial_" + _targetID) || PlayerPrefs.GetInt("tutorial_" + _targetID) == 0)
            {
                PlayerPrefs.SetInt("tutorial_"+_targetID, 1);
                OnPlayTutorial?.Invoke(this);
            }
        }

        private void InitializeTutorial()
        {
            if (!PlayerPrefs.HasKey("tutorial_"+_targetID) || PlayerPrefs.GetInt("tutorial_"+_targetID) == 0)
            {
                _tutorialStarter.AddTutorial(this);
            }
        }
        
        public virtual Vector3 GetPosition()
        {
            return _rectTransform.position;
        }
    }
}