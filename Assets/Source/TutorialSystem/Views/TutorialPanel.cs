using System;
using System.Collections;
using Source.TutorialSystem.Data;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Source.TutorialSystem.Views
{
    public class TutorialPanel : MonoBehaviour, IPointerClickHandler
    {
        protected TutorialTarget _tutorialTarget;
        protected TutorialTextBox _tutorialTextBox;
        protected UnityEngine.Camera _camera;
        protected TutorialStage _tutorialStage = TutorialStage.Stopped;

        protected RectTransform _rectTransform;

        public Action OnCloseTutorial;

        private float _savedTimeScale = 1f;

        private void Awake()
        {
            _camera = UnityEngine.Camera.main;
            _rectTransform = GetComponent<RectTransform>();
            _tutorialTextBox = GetComponentInChildren<TutorialTextBox>();
            Hide();
        }

        public virtual void StartTutorial(TutorialTarget tutorialTarget)
        {
            _tutorialStage = TutorialStage.Started;
            _savedTimeScale = Time.timeScale;
            Show();
            if (tutorialTarget.IsPause)
            {
                StartCoroutine(TutorialPause());
            }
        }

        private IEnumerator TutorialPause()
        {
            yield return new WaitForSeconds(1f);
            Time.timeScale = 0f;
        }

        protected void Show()
        {
            gameObject.SetActive(true);
        }

        protected void Hide()
        {
            gameObject.SetActive(false);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _tutorialStage = TutorialStage.Stopped;
            Time.timeScale = _savedTimeScale;
            OnCloseTutorial?.Invoke();
            Hide();
        }
    }
}