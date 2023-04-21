using System;
using Level.Model;
using Player.View;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Level.View
{
    public class LootBorderView : DestructibleBorderView
    {
        protected PlayerView _playerViewTarget;
        protected Vector3 _position;
        [SerializeField] protected int _moneyAmount = 1;

        private void Update()
        {
            if (_playerViewTarget == null) return;

            if ((_transform.position-_position).magnitude < 0.5f)
            {
                DestroyAnimation();
            }

            _transform.position = Vector3.MoveTowards(_transform.position, _position, Time.deltaTime * 10);
            
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

        public override void Damage(int damage)
        {
            if (damage>0)
            {
                _health = Mathf.Clamp(_health - damage, 0, _maxHealth);
                int destroyId = (int)((float)_health / _maxHealth * 10);
                _spritesView.SetSprite(destroyId);
                DamageAnimation();
                if (_health==0)
                {
                    DeathBlock();
                }
            }
        }
        
        protected override void DeathBlock()
        {
            DeactivateCollider();
            ClearDestroySprite();
            LootAnimation();
            _playerViewTarget = FindObjectOfType<PlayerView>();
            _position = _playerViewTarget.Transform.position;
            GameStatsView.Singleton.AddMoney(_moneyType, Random.Range(1, _moneyAmount+1));
        }
        
        protected void LootAnimation()
        {
            _animator.SetTrigger("loot");
        }
    }
}