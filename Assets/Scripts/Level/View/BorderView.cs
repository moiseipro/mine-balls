using System;
using Level.Model;
using Player.View;
using UnityEngine;

namespace Level.View
{
    [RequireComponent(typeof(Collider2D))]
    public class BorderView : MonoBehaviour
    {
        protected Transform _transform;
        public Transform Transform => _transform;
        protected Collider2D _collider2D;
        protected Animator _animator;
        protected SpriteRenderer _spriteRenderer;
        
        [SerializeField] protected MoneyType _moneyType = MoneyType.None;
        public MoneyType MoneyType => _moneyType;

        protected virtual void Awake()
        {
            _transform = transform;
            _collider2D = GetComponent<Collider2D>();
            _animator = GetComponent<Animator>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void SetPosition(Vector2 position)
        {
            _transform.position = position;
        }

        public void SetScale(Vector2 scale)
        {
            _transform.localScale = scale;
        }
        
        protected virtual void OnCollisionEnter2D(Collision2D col)
        {
            BallView ballView = col.gameObject.GetComponent<BallView>();
            if (ballView)
            {
                ballView.LossDurability();
            }
        }

        protected void DeactivateCollider()
        {
            _collider2D.enabled = false;
        }

        protected void ActivateCollider()
        {
            _collider2D.enabled = true;
        }
    }
}