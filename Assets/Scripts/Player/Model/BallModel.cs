using System;
using System.Collections.Generic;
using System.Linq;
using Level.Model;
using Level.View;
using Player.ScriptableObjects;
using UnityEngine;

namespace Player.Model
{
    [Serializable]
    public class BallModel
    {
        [SerializeField] private List<MoneyType> _moneyTypes;
        public List<MoneyType> MoneyTypes => _moneyTypes;
        [SerializeField] private ToolType _toolType;
        [SerializeField] private MoneyDictionary _pricesDictionary;
        public MoneyDictionary PricesDictionary => _pricesDictionary;
        public ToolType ToolType => _toolType;
        [SerializeField] private int _durability;
        public int Durability
        {
            get => _durability;
            set => _durability = value;
        }
        [SerializeField] private int _strength;
        public int Strength
        {
            get => _strength;
            set => _strength = value;
        }

        [SerializeField] private bool _isBuy = false;
        public bool IsBuy
        {
            get => _isBuy;
            set => _isBuy = value;
        }

        private LevelChangerView _levelChangerView;

        public Action<BallModel> OnCraftTool;
        public Action<BallModel> OnUpgradeTool;
        public Action<BallModel> OnBuyTool;
        public Action<BallModel> OnSellTool;
        public Action OnBallDestroy;

        public BallModel(ToolObject toolObject)
        {
            _pricesDictionary = toolObject.PricesDictionary;
            _moneyTypes = toolObject.MoneyTypes;
            _toolType = toolObject.ToolType;
            _durability = toolObject.Durability;
            _strength = toolObject.Strength;
            _isBuy = toolObject.IsBuy;
        }

        public BallModel(MoneyDictionary pricesDictionary, MoneyType[] moneyTypes, ToolType toolType, int durability, int strength, bool isBuy = false)
        {
            _pricesDictionary = pricesDictionary;
            _moneyTypes = moneyTypes.ToList();
            _toolType = toolType;
            _durability = durability;
            _strength = strength;
            _isBuy = isBuy;
        }
        
        public BallModel(BallModel ballModel)
        {
            _pricesDictionary = ballModel._pricesDictionary;
            _moneyTypes = ballModel._moneyTypes;
            _toolType = ballModel._toolType;
            _durability = ballModel._durability;
            _strength = ballModel._strength;
            _isBuy = ballModel._isBuy;
        }

        public void SetLevelChanger(LevelChangerView levelChangerView)
        {
            _levelChangerView = levelChangerView;
        }
        
        public BorderView GetRandomLevelBlock()
        {
            if (_levelChangerView == null) return null;
            return _levelChangerView.GetRandomLevelBlock();
        }

        public void CraftItem()
        {
            Debug.Log("Crafted Item! ");
            OnCraftTool?.Invoke(new BallModel(this));
        }

        public void UpgradeTool()
        {
            Debug.Log("Upgraded Item! ");
            OnUpgradeTool?.Invoke(this);
        }
        
        public void BuyTool()
        {
            Debug.Log("Buy Item! ");
            OnBuyTool?.Invoke(this);
        }

        public void SellTool()
        {
            Debug.Log("Sell Item! ");
            OnSellTool?.Invoke(this);
        }

        public bool LossDurability()
        {
            if (_durability == 0) return false;
            
            _durability = Mathf.Clamp(_durability - 1, 0, _durability);
            //Debug.Log("Loss Durability: "+_durability);
            if (_durability == 0)
            {
                OnBallDestroy?.Invoke();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CompareMoneyType(MoneyType compareMoneyType)
        {
            foreach (var moneyType in _moneyTypes)
            {
                if (moneyType == compareMoneyType)
                {
                    return true;
                }
            }

            return false;
        }

    }
}