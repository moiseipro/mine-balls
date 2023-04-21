using System;
using UnityEngine;

namespace Level.View
{
    public class SpritesView : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;
        
        [SerializeField] private Sprite[] _sprites;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void SetSprite(int id)
        {
            _spriteRenderer.sprite = _sprites[Mathf.Clamp(id, 0, _sprites.Length)];
        }

        public void ClearSprite()
        {
            _spriteRenderer.sprite = null;
        }
    }
}