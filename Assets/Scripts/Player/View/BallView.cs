using System;
using Level.Model;
using Level.View;
using Player.Model;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

namespace Player.View
{
    [RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
    public class BallView : MonoBehaviour
    {
        private Transform _transform;
        private Collider2D _collider2D;
        private Rigidbody2D _rigidbody2D;
        private BallModel _ballModel;

        [SerializeField] private SkinView _skinView;

        [SerializeField] private float _impulse = 8f;
        private float _minImpulse = 1f;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip[] _audioClips;
        [SerializeField] private TrailRenderer _trailRenderer;

        private void Awake()
        {
            _transform = transform;
            _collider2D = GetComponent<Collider2D>();
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _rigidbody2D.isKinematic = true;
        }

        public void Initialize(BallModel ballModel)
        {
            _ballModel = ballModel;
            _skinView.SetSkin((int)ballModel.ToolType);
            SetExtraAbility();
            
            Material trailMaterial = Resources.Load<Material>("Materials/Tool/tool_" + (int)_ballModel.ToolType);
            if (trailMaterial)
            {
                _trailRenderer.enabled = true;
                _trailRenderer.material = trailMaterial;
            }
        }

        private void Start()
        {
            _rigidbody2D.AddForce(Vector2.right * _impulse, ForceMode2D.Impulse);
        }

        public void Shoot(Vector2 direction)
        {
            _rigidbody2D.isKinematic = false;
            _rigidbody2D.AddForce(direction * _impulse, ForceMode2D.Impulse);
        }

        public int GetDamage(MoneyType moneyType)
        {
            if (_ballModel.CompareMoneyType(moneyType))
            {
                _audioSource.clip = _audioClips[0];
                _audioSource.Play();
                return _ballModel.Strength;
            }
            _audioSource.clip = _audioClips[1];
            _audioSource.Play();
            return 1;
        }

        public void LossDurability()
        {
            if (_ballModel.LossDurability())
            {
                _skinView.DeathAnimation();
            }
        }
        
        private void OnCollisionEnter2D(Collision2D col)
        {
            BorderView borderView = col.gameObject.GetComponent<BorderView>();
            Vector2 contactNormal = col.contacts[Random.Range(0, col.contacts.Length)].normal;
            
            _skinView.SetAnimationSpeed(_rigidbody2D.velocity.magnitude/6f);
            
            if (borderView)
            {
                switch (_ballModel.ToolType)
                {
                    case ToolType.Arrow:
                        if (Random.Range(0, 10) > 4)
                        {
                            Vector2 newVelocity = _rigidbody2D.velocity;
                            newVelocity.x = 0;
                            _rigidbody2D.velocity = newVelocity;
                            if (newVelocity.y < 0)
                            {
                                _rigidbody2D.AddForce(Vector2.down * Random.Range(_minImpulse, _impulse), ForceMode2D.Impulse);
                            }
                            else
                            {
                                _rigidbody2D.AddForce(Vector2.up * Random.Range(_minImpulse, _impulse), ForceMode2D.Impulse);
                            }
                            
                        }
                        else
                        {
                            _rigidbody2D.AddForce(contactNormal * Random.Range(_minImpulse, _impulse), ForceMode2D.Impulse);
                            if (_rigidbody2D.velocity.x > 0)
                            {
                                _rigidbody2D.AddForce(Vector2.right * Random.Range(_minImpulse, _impulse), ForceMode2D.Impulse);
                            }
                            else
                            {
                                _rigidbody2D.AddForce(Vector2.left * Random.Range(_minImpulse, _impulse), ForceMode2D.Impulse);
                            }
                        }
                        break;
                    case ToolType.Bone:
                        _rigidbody2D.AddForce(contactNormal * Random.Range(_minImpulse, _impulse), ForceMode2D.Impulse);
                        if (_rigidbody2D.velocity.x > 0)
                        {
                            _rigidbody2D.AddForce(Vector2.right * Random.Range(_minImpulse, _impulse), ForceMode2D.Impulse);
                        }
                        else
                        {
                            _rigidbody2D.AddForce(Vector2.left * Random.Range(_minImpulse, _impulse), ForceMode2D.Impulse);
                        }
                        break;
                    case ToolType.Tnt:
                        _rigidbody2D.AddForce(contactNormal * Random.Range(_minImpulse, _impulse), ForceMode2D.Impulse);
                        foreach (var raycastHit2D in Physics2D.CircleCastAll(_transform.position, 2f, Vector2.zero, 0))
                        {
                            DestructibleBorderView destructibleBorderView =
                                raycastHit2D.collider.GetComponent<DestructibleBorderView>();
                            if (destructibleBorderView)
                            {
                                destructibleBorderView.Damage(1);
                            }
                        }
                        
                        break;
                    default:
                        _rigidbody2D.AddForce(contactNormal * Random.Range(_minImpulse, _impulse), ForceMode2D.Impulse);
                        if (Mathf.Abs(_rigidbody2D.velocity.x) < 5f && Random.Range(0f, _impulse) > _impulse/3f)
                        {
                            if (_rigidbody2D.velocity.x > 0)
                            {
                                _rigidbody2D.AddForce(Vector2.right * Random.Range(_impulse/3f, _impulse), ForceMode2D.Impulse);
                            }
                            else
                            {
                                _rigidbody2D.AddForce(Vector2.left * Random.Range(_impulse/3f, _impulse), ForceMode2D.Impulse);
                            }
                        }
                        break;
                }
                
            }
        }

        private void SetExtraAbility()
        {
            switch (_ballModel.ToolType)
            {
                case ToolType.Slime:
                    _minImpulse = _impulse;
                    _impulse *= 2;
                    break;
                case ToolType.Arrow:
                    _minImpulse = _impulse/3f;
                    break;
                case ToolType.Bread:
                    _rigidbody2D.gravityScale = -1;
                    break;
                case ToolType.Bone:
                    _rigidbody2D.gravityScale = 0.6f;
                    _impulse -= _impulse/4f;
                    _minImpulse = _impulse/4f;
                    break;
                case ToolType.Tnt:
                    _minImpulse = _impulse/2f;
                    break;
                default:
                    break;
            }
        }
    }
}