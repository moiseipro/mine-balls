using System;
using UnityEngine;

namespace Player.View
{
    [RequireComponent(typeof(Animator))]
    public class SkinView : MonoBehaviour
    {
        private Animator _animator;
        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void SetSkin(int id)
        {
            _spriteRenderer.sprite = Resources.Load<Sprite>("Tools/tool_"+id);;
        }
        
        public void SetAnimationSpeed(float speed)
        {
            _animator.SetFloat("speed", speed);
        }

        public void DeathAnimation()
        {
            _animator.SetTrigger("destroy");
        }
        
        public void DestroyBall()
        {
            gameObject.transform.parent.gameObject.SetActive(false);
        }
    }
}