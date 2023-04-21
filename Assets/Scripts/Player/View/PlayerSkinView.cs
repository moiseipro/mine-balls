using UnityEngine;

namespace Player.View
{
    public class PlayerSkinView : MonoBehaviour
    {
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void ShootBall()
        {
            _animator.SetTrigger("Shoot");
        }

        public void DragBall()
        {
            _animator.SetTrigger("Drag");
        }
    }
}