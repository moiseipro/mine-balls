using System;
using Level.Model;
using Player.ScriptableObjects;
using Player.View;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Level.View
{
    [Serializable]
    public class ToolDropDictionary : SerializableDictionary<float, ToolObject> {}
    public class LuckBorderView : LootBorderView
    {
        [SerializeField] private string _name = "base";
        [SerializeField] private int _max = 5;
        [SerializeField] private int _length = 10;
        [SerializeField] private ToolObject[] _toolObjects;

        protected override void Awake()
        {
            base.Awake();
            _max = _toolObjects.Length;
        }

        protected override void OnCollisionEnter2D(Collision2D col)
        {
            BallView ballView = col.gameObject.GetComponent<BallView>();
            if (ballView)
            {
                ballView.LossDurability();

                Damage(1);
            }
        }

        public override void Damage(int damage)
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

        protected override void DeathBlock()
        {
            DeactivateCollider();
            ClearDestroySprite();
            LootAnimation();
            _playerViewTarget = FindObjectOfType<PlayerView>();
            _position = _playerViewTarget.Transform.position;
            int goods = PlayerPrefs.GetInt(_name + "_goods");
            int tries = PlayerPrefs.GetInt(_name + "_tries");
            ToolObject dropTool = null;


            foreach (var toolObject in _toolObjects)
            {
                
            }
            float generateChance = Random.Range(0f, 1f);
            float currentChance = (float)(_max - goods) / (_length - tries);
            Debug.Log("CH: " + generateChance + " - " + currentChance);
            if (currentChance > generateChance)
            {
                goods = Mathf.Clamp(goods+1,0, _max);
                dropTool = _toolObjects[Random.Range(0, goods)];
            }
            
            _spriteRenderer.color = Color.white;
            if (dropTool)
            {
                _spriteRenderer.sprite = Resources.Load<Sprite>("Tools/tool_"+(int)dropTool.ToolType);
                GameStatsView.Singleton.AddNewTool(dropTool);
            }
            else
            {
                _spriteRenderer.sprite = Resources.Load<Sprite>("Blocks/resource_"+(int)_moneyType);
                GameStatsView.Singleton.AddMoney(_moneyType, Random.Range(1, _moneyAmount+1));
            }
            tries = Mathf.Clamp(tries+1,0, _length);
            if (goods == _max && tries == _length)
            {
                PlayerPrefs.SetInt(_name + "_goods", 0);
                PlayerPrefs.SetInt(_name + "_tries", 0);
            }
            else
            {
                PlayerPrefs.SetInt(_name + "_goods", goods);
                PlayerPrefs.SetInt(_name + "_tries", tries);
            }
            
            Debug.Log("goods: "+ goods +" tries: "+tries);
        }
    }
}