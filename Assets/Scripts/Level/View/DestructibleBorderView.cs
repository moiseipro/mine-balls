using System;
using Player.View;
using UnityEngine;

namespace Level.View
{
    public class DestructibleBorderView : BorderView
    {
        [SerializeField] protected int _maxHealth = 3;
        protected int _health;

        [SerializeField] protected SpritesView _spritesView;

        protected override void Awake()
        {
            _health = _maxHealth;
            base.Awake();
        }
        
        protected override void OnCollisionEnter2D(Collision2D col)
        {
            BallView ballView = col.gameObject.GetComponent<BallView>();
            if (ballView)
            {
                int damage = ballView.GetDamage(_moneyType);
                ballView.LossDurability();
                Damage(damage);
            }
        }

        public virtual void Damage(int damage)
        {
            _health = Mathf.Clamp(_health - damage - 1, 0, _maxHealth);
            int destroyId = (int)((float)_health / _maxHealth * 10);
            _spritesView.SetSprite(destroyId);
            DamageAnimation();
            if (_health==0)
            {
                DeathBlock();
            }
        }

        protected virtual void DeathBlock()
        {
            ClearDestroySprite();
            DeactivateCollider();
            DestroyAnimation();
        }
        
        protected void DamageAnimation()
        {
            _animator.SetTrigger("damage");
        }

        protected void DestroyAnimation()
        {
            _animator.SetTrigger("destroy");
        }

        protected void ClearDestroySprite()
        {
            _spritesView.ClearSprite();
        }

        protected void DestroyBlock()
        {
            gameObject.SetActive(false);
        }
    }
}