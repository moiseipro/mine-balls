using System;
using UnityEngine;

namespace Camera.View
{
    public class CameraView : MonoBehaviour
    {
        private Transform _transform;

        private Vector3 _position;
        [SerializeField] private float _cameraSpeed = 3f;
        [SerializeField] private Transform _paralax;

        private void Awake()
        {
            _transform = transform;
            _position = _transform.position;
        }

        private void Update()
        {
            _transform.position = Vector3.Lerp(_transform.position, _position, Time.deltaTime * _cameraSpeed);
            _paralax.localPosition =  Vector3.Lerp(_paralax.localPosition, _position / -3f, Time.deltaTime * _cameraSpeed);
        }

        public void AddYPosition(float value)
        {
            _position.y -= value;
        }

        public void ResetPosition()
        {
            _position = new Vector3(0,0, -10);
        }
    }
}