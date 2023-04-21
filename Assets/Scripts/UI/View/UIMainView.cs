using System;
using Level.View;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.View
{
    public class UIMainView : MonoBehaviour
    {
        [SerializeField] protected UIDocument _menuDocument;
        protected VisualElement _mainPanel;

        public Action OnClose;

        public virtual void GenerateMenu()
        {
            
        }
        
        public virtual void OpenMenu()
        {
            _mainPanel.RemoveFromClassList("hidden");
        }
        
        public virtual void TimerOpenMenu()
        {
            _mainPanel.RemoveFromClassList("hidden");
        }
        
        public virtual void CloseMenu()
        {
            OnClose?.Invoke();
            _mainPanel.AddToClassList("hidden");
        }
    }
}