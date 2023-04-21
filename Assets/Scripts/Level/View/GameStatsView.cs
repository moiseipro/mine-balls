using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Level.Model;
using Player.Model;
using Player.ScriptableObjects;
using UI.View;
using UnityEngine;
using YG;

namespace Level.View
{
    public class GameStatsView : MonoBehaviour
    {
        public static GameStatsView Singleton { get; private set; }

        private GameStatsModel _gameStatsModel;
        public List<BallModel> ToolModels => _gameStatsModel.ToolModels;
        public MoneyDictionary Money => _gameStatsModel.Money;

        [SerializeField] private LevelChangerView _levelChangerView;
        public LevelChangerView LevelChangerView => _levelChangerView;
        
        [SerializeField] private MenuManager _menuManager;
        
        public Action<BallModel> OnCraftedTool;
        public Action OnResourcesChanged;
        public Action OnToolChanged;

        

        private void Awake()
        {
            if (!Singleton)
            {
                Singleton = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(gameObject);
            }

        }

        private void CraftTool(BallModel ballModel)
        {
            //ballModel.OnBallDestroy += LevelChangerView.EndLevel;
            ballModel.SetLevelChanger(_levelChangerView);
            ballModel.OnCraftTool += CraftTool;
            ballModel.OnUpgradeTool += UpgradeTool;
            OnCraftedTool?.Invoke(ballModel);
            OnResourcesChanged?.Invoke();

            SaveToJson();
        }

        private void UpgradeTool(BallModel ballModel)
        {
            if (ballModel.PricesDictionary.Count != 0)
            {
                foreach (var prices in ballModel.PricesDictionary)
                {
                    if (!Money.ContainsKey(prices.Key) && prices.Value != 0)
                    {
                        return;
                    } 
                    if (Money.ContainsKey(prices.Key) && Money[prices.Key] < prices.Value*2)
                    {
                        return;
                    }
                }
                foreach (var prices in ballModel.PricesDictionary)
                {
                    if (Money.ContainsKey(prices.Key))
                    {
                        if (Money[prices.Key] >= prices.Value*2)
                        {
                            Money[prices.Key] -= prices.Value*2;
                        }
                    }
                }
            }
            ballModel.Strength += 2;
            ballModel.Durability += 10;
            MoneyDictionary moneyDictionary = new MoneyDictionary();
            moneyDictionary.CopyFrom(ballModel.PricesDictionary);
            foreach (var prices in moneyDictionary)
            {
                ballModel.PricesDictionary[prices.Key] = Mathf.RoundToInt(prices.Value * 1.5f);
                
            }
            OnResourcesChanged?.Invoke();
            OnToolChanged?.Invoke();
        }
        
        private void BuyTool(BallModel ballModel)
        {
            if (ballModel.PricesDictionary.Count != 0)
            {
                foreach (var prices in ballModel.PricesDictionary)
                {
                    if (!Money.ContainsKey(prices.Key) && prices.Value != 0)
                    {
                        Debug.Log("No money type");
                        return;
                    } 
                    if (Money.ContainsKey(prices.Key) && Money[prices.Key] < prices.Value)
                    {
                        Debug.Log("No money!");
                        return;
                    }
                }
                foreach (var prices in ballModel.PricesDictionary)
                {
                    if (Money.ContainsKey(prices.Key))
                    {
                        if (Money[prices.Key] >= prices.Value)
                        {
                            Money[prices.Key] -= prices.Value;
                        }
                    }
                }
            }
            ballModel.IsBuy = true;
            
            OnResourcesChanged?.Invoke();
            OnToolChanged?.Invoke();
        }

        public void AddMoney(MoneyType moneyType, int count)
        {
            _gameStatsModel.AddMoney(moneyType, count);
            OnResourcesChanged?.Invoke();
        }

        public void AddNewTool(ToolObject toolObject)
        {
            BallModel newBallModel = _gameStatsModel.AddNewTool(toolObject);
            newBallModel.OnCraftTool += CraftTool;
            newBallModel.OnUpgradeTool += UpgradeTool;
            newBallModel.OnBuyTool += BuyTool;
            newBallModel.OnSellTool += SellTool;
            OnToolChanged?.Invoke();
        }
        
        public void SellTool(BallModel ballModel)
        {
            _gameStatsModel.SellTool(ballModel);
            
            _gameStatsModel.AddMoney(MoneyType.Emerald, 1);
            
            OnResourcesChanged?.Invoke();
            OnToolChanged?.Invoke();
        }

        public void SaveToJson()
        {
            string json = JsonUtility.ToJson(_gameStatsModel);
            //Debug.Log(json);
            PlayerPrefs.SetString("game_stats", json);
            YandexGame.savesData.gameStats = json;
            YandexGame.SaveProgress();
            YandexGame.NewLeaderboardScores("score", YandexGame.savesData.maxLevel);
            File.WriteAllText(Application.persistentDataPath + "/game_stats.json", json);
        }

        public void LoadFromJson()
        {
            string path = Application.persistentDataPath + "/game_stats.json";
            //PlayerPrefs.DeleteKey("game_stats"); // Убрать в релизе
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                Debug.Log("Loaded JSON GameData from:" + path+"\n"+json);
                _gameStatsModel = JsonUtility.FromJson<GameStatsModel>(json);
                //JsonUtility.FromJsonOverwrite(json, _gameStatsModel);
            }else if (PlayerPrefs.HasKey("game_stats"))
            {
                string json = PlayerPrefs.GetString("game_stats");
                _gameStatsModel = JsonUtility.FromJson<GameStatsModel>(json);
            }
            else
            {
                List<BallModel> toolDictionary = new List<BallModel>
                {
                    new BallModel(
                        new MoneyDictionary(), 
                        new []{MoneyType.Dirt, MoneyType.Tree}, 
                        ToolType.Stick, 200, 2, true),
                    new BallModel(Resources.Load<ToolObject>("Scriptable/Tool/Tree/TreeShovel")),
                    //new BallModel(Resources.Load<ToolObject>("Scriptable/Tool/Stone/StoneArrow")),
                    //new BallModel(Resources.Load<ToolObject>("Scriptable/Tool/Stone/StoneSlime")),
                    //new BallModel(Resources.Load<ToolObject>("Scriptable/Tool/Iron/IronBread")),
                    //new BallModel(Resources.Load<ToolObject>("Scriptable/Tool/Iron/IronBone")),
                    //new BallModel(Resources.Load<ToolObject>("Scriptable/Tool/Iron/IronTnt")),
                };
                _gameStatsModel = new GameStatsModel(new MoneyDictionary(), toolDictionary, 0);
            }

            SubscribeAction();
        }

        public void LoadFromYandex()
        {
            string gs_string = YandexGame.savesData.gameStats;
            if (gs_string != null)
            {
                _gameStatsModel = JsonUtility.FromJson<GameStatsModel>(gs_string);
            }
            else
            {
                LoadFromJson();
                return;
            }
            SubscribeAction();
        }

        private void SubscribeAction()
        {
            LevelChangerView.OnEndLevel += _gameStatsModel.SetLevel;
            
            foreach (var toolModel in ToolModels)
            {
                toolModel.OnCraftTool += CraftTool;
                toolModel.OnUpgradeTool += UpgradeTool;
                toolModel.OnBuyTool += BuyTool;
                toolModel.OnSellTool += SellTool;
            }
            
            _menuManager.GenerateAllMenus();
        }

        private void GetRewarded(int id)
        {
            switch (id)
            {
                case 0:
                    AddMoney(MoneyType.Tree, 500);
                    break;
                case 1:
                    AddMoney(MoneyType.Stone, 500);
                    break;
                case 2:
                    AddMoney(MoneyType.Coal, 250);
                    break;
                case 3:
                    AddMoney(MoneyType.Iron, 250);
                    break;
            }
        }

        private void OnEnable()
        {
            YandexGame.GetDataEvent += LoadFromYandex;
            YandexGame.RewardVideoEvent += GetRewarded;
        }

        private void OnDisable()
        {
            foreach (var toolModel in ToolModels)
            {
                toolModel.OnCraftTool -= CraftTool;
                toolModel.OnUpgradeTool -= UpgradeTool;
                toolModel.OnBuyTool -= BuyTool;
                toolModel.OnSellTool -= SellTool;
            }
            
            YandexGame.GetDataEvent -= LoadFromYandex;
            YandexGame.RewardVideoEvent -= GetRewarded;
        }
    }
}