using System;
using System.Collections.Generic;
using System.Linq;
using Player.Model;
using Player.ScriptableObjects;
using UnityEngine;

namespace Level.Model
{
    [Serializable]
    public class MoneyDictionary : SerializableDictionary<MoneyType, int> {}
    [Serializable]
    public class GameStatsModel
    {
        [SerializeField] private MoneyDictionary _money;
        public MoneyDictionary Money => _money;
        [SerializeField] private List<BallModel> _toolModels;
        public List<BallModel> ToolModels => _toolModels;
        [SerializeField] private int _maxLevel = 0;
        public int MaxLevel => _maxLevel;


        public GameStatsModel(MoneyDictionary money, List<BallModel> toolModels, int level)
        {
            _money = money;
            _toolModels = toolModels;
            _maxLevel = level;
        }

        public BallModel AddNewTool(ToolObject toolObject)
        {
            BallModel newBallModel = new BallModel(toolObject);
            _toolModels.Add(newBallModel);
            return newBallModel;
        }
        
        public BallModel GetTool(ToolType toolType)
        {
            if (_toolModels.Count==0)
            {
                Debug.Log("Tools Empty!");
                return null;
            }
            
            return new BallModel(_toolModels.First(ballModel => ballModel.ToolType == toolType));
        }

        public void SellTool(BallModel ballModel)
        {
            _toolModels.Remove(ballModel);
        }

        public void SetLevel(int level)
        {
            _maxLevel = level;
        }
        
        public void AddMoney(MoneyType moneyType, int count)
        {
            if (_money.ContainsKey(moneyType))
            {
                _money[moneyType]+=count;
            }
            else
            {
                _money.Add(moneyType, count);
            }
            
        }

        public void SpendMoney(MoneyType moneyType, int value)
        {
            _money[moneyType] = Mathf.Clamp(_money[moneyType] - value, 0, _money[moneyType]);
        }
    }
}