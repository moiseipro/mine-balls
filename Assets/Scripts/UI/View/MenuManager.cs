using System;
using Level.View;
using Source.TutorialSystem.Views;
using UnityEngine;

namespace UI.View
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] private UIMainView[] _mainViews;
        
        private GameInput _gameInput;
        
        private void Awake()
        {
            _gameInput = new GameInput();
            _gameInput.UI.Enable();
        }

        private void Start()
        {
            GetComponent<TutorialTarget>().StartTutorial();
        }

        public void GenerateAllMenus()
        {
            foreach (var mainView in _mainViews)
            {
                mainView.GenerateMenu();
            }
        }

        private void OnDisable()
        {
            _gameInput.UI.Disable();
        }
    }
}