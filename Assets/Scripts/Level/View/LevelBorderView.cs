using System;
using UnityEngine;

namespace Level.View
{
    public class LevelBorderView : MonoBehaviour
    {
        [SerializeField] private BorderView _leftBorder;
        [SerializeField] private BorderView _rightBorder;
        [SerializeField] private BorderView _topBorder;
        [SerializeField] private BorderView _bottomBorder;

        private float _screenWidth, _screenHeight; 
        private UnityEngine.Camera _camera;

        // Start is called before the first frame update
        void Start()
        {
            _camera = UnityEngine.Camera.main;
            _screenWidth = Screen.width;
            _screenHeight = Screen.height;
        }

        private void FixedUpdate()
        {
            Vector2 newPosition = _camera.ScreenToWorldPoint(new Vector2(0, _screenHeight / 2f));
            Vector2 newScale = new Vector2(0.2f, 40);
            _leftBorder.SetPosition(newPosition);
            _leftBorder.SetScale(newScale);
            newPosition = _camera.ScreenToWorldPoint(new Vector2(_screenWidth, _screenHeight/2f));
            _rightBorder.SetPosition(newPosition);
            _rightBorder.SetScale(newScale);
            newPosition = _camera.ScreenToWorldPoint(new Vector2(_screenWidth/2f, _screenHeight));
            newScale = new Vector2(60, 0.2f);
            _topBorder.SetPosition(newPosition);
            _topBorder.SetScale(newScale);
            newPosition = _camera.ScreenToWorldPoint(new Vector2(_screenWidth/2f, 0));
            _bottomBorder.SetPosition(newPosition);
            _bottomBorder.SetScale(newScale);
        }
    }
}
