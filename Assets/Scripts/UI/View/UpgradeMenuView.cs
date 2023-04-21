using System.Globalization;
using Level.Model;
using Level.View;
using Source.TutorialSystem.Views;
using UnityEngine;
using UnityEngine.UIElements;
using YG;

namespace UI.View
{
    public class UpgradeMenuView : UIMainView
    {
        [SerializeField] private LevelChangerView _levelChangerView;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private TutorialTarget _tutorialTarget;
        
        public override void GenerateMenu()
        {
            GameStatsView.Singleton.OnToolChanged += GenerateUpgradeMenu;
            GenerateButtonMenu();
            GenerateUpgradeMenu();
            CloseMenu();
        }

        private void GenerateButtonMenu()
        {
            var root = _menuDocument.rootVisualElement;
            
            Button closeButton = root.Q<Button>("CloseButton");
            Button treeBonusButton = root.Q<Button>("AdsTreeBonus");
            Button stoneBonusButton = root.Q<Button>("AdsStoneBonus");
            Button coalBonusButton = root.Q<Button>("AdsCoalBonus");
            Button ironBonusButton = root.Q<Button>("AdsIronBonus");
            closeButton.clicked += () =>
            {
                ButtonMusic();
                YandexGame.FullscreenShow();
                _tutorialTarget.StartTutorial();
                CloseMenu();
            };
            treeBonusButton.clicked += () =>
            {
                ButtonMusic();
                YandexGame.RewVideoShow(0);
            };
            stoneBonusButton.clicked += () =>
            {
                ButtonMusic();
                YandexGame.RewVideoShow(1);
            };
            coalBonusButton.clicked += () =>
            {
                ButtonMusic();
                YandexGame.RewVideoShow(2);
            };
            ironBonusButton.clicked += () =>
            {
                ButtonMusic();
                YandexGame.RewVideoShow(3);
            };
        }
        
        private void GenerateUpgradeMenu()
        {
            var root = _menuDocument.rootVisualElement;
            
            _mainPanel = root.Q<VisualElement>("UpgradeMenuBlock");
            var upgradePanel = root.Q<VisualElement>("UpgradePanel");
            upgradePanel.Clear();
            Button treeBonusButton = root.Q<Button>("AdsTreeBonus");
            Button stoneBonusButton = root.Q<Button>("AdsStoneBonus");
            Button coalBonusButton = root.Q<Button>("AdsCoalBonus");
            Button ironBonusButton = root.Q<Button>("AdsIronBonus");
            switch (_levelChangerView.LevelNum)
            {
                case < 15:
                    treeBonusButton.RemoveFromClassList("hidden");
                    stoneBonusButton.AddToClassList("hidden");
                    coalBonusButton.AddToClassList("hidden");
                    ironBonusButton.AddToClassList("hidden");
                    break;
                case < 25:
                    treeBonusButton.RemoveFromClassList("hidden");
                    stoneBonusButton.RemoveFromClassList("hidden");
                    coalBonusButton.AddToClassList("hidden");
                    ironBonusButton.AddToClassList("hidden");
                    break;
                case < 35:
                    treeBonusButton.RemoveFromClassList("hidden");
                    stoneBonusButton.RemoveFromClassList("hidden");
                    coalBonusButton.RemoveFromClassList("hidden");
                    ironBonusButton.AddToClassList("hidden");
                    break;
                default:
                    treeBonusButton.AddToClassList("hidden");
                    stoneBonusButton.RemoveFromClassList("hidden");
                    coalBonusButton.RemoveFromClassList("hidden");
                    ironBonusButton.RemoveFromClassList("hidden");
                    break;
            }

            var craftItem = Resources.Load<VisualTreeAsset>("UI/CraftMenu/CraftItem");
            var resourcesRow = Resources.Load<VisualTreeAsset>("UI/CraftMenu/ResourcesRow");

            foreach (var toolModel in GameStatsView.Singleton.ToolModels)
            {
                var newCraftItem = craftItem.CloneTree();
                Debug.Log("Tools/tool_"+(int)toolModel.ToolType);
                
                var newBack = Resources.Load<Sprite>("Tools/tool_"+(int)toolModel.ToolType);
                var craftItemBlock = newCraftItem.Q<VisualElement>("CraftItemBlock");
                craftItemBlock.style.backgroundImage = new StyleBackground(newBack);
                
                Button upgradeButton = newCraftItem.Q<Button>("CraftButton");
                Button sellButton = newCraftItem.Q<Button>("SellButton");
                if(toolModel.ToolType == ToolType.Stick) sellButton.AddToClassList("hidden");
                else sellButton.clicked += () =>
                {
                    toolModel.SellTool();
                    ButtonMusic();
                };
                if (!toolModel.IsBuy)
                {
                    upgradeButton.AddToClassList("buy-button");
                    string upgradeText = "Craft";
                    switch (YandexGame.EnvironmentData.language)
                    {
                        case "ru":
                            upgradeText = "Создать";
                            break;
                        case "en":
                            upgradeText = "Craft";
                            break;
                        case "tr":
                            upgradeText = "Zanaat";
                            break;
                    }
                    upgradeButton.text = upgradeText;
                    upgradeButton.clicked += () =>
                    {
                        toolModel.BuyTool();
                        ButtonMusic();
                    };
                    foreach (var prices in toolModel.PricesDictionary)
                    {
                        var newResourcesRow = resourcesRow.CloneTree();
                        var newResource = Resources.Load<Sprite>("Blocks/resource_"+(int)prices.Key);
                        var craftResources = newCraftItem.Q<VisualElement>("CraftResources");
                        Label resourcePrice = newResourcesRow.Q<Label>("ResourcePrice");
                        VisualElement resourceImage = newResourcesRow.Q<VisualElement>("ResourceImage");
                        resourcePrice.text = Mathf.RoundToInt(prices.Value).ToString();
                        resourceImage.style.backgroundImage = new StyleBackground(newResource);
                    
                        craftResources.Add(newResourcesRow);
                    }
                }
                else
                {
                    upgradeButton.AddToClassList("craft-button");
                    string upgradeText = "Equip";
                    switch (YandexGame.EnvironmentData.language)
                    {
                        case "ru":
                            upgradeText = "Взять";
                            break;
                        case "en":
                            upgradeText = "Equip";
                            break;
                        case "tr":
                            upgradeText = "Donatmak";
                            break;
                    }

                    upgradeButton.text = upgradeText;
                    upgradeButton.clicked += () =>
                    {
                        toolModel.CraftItem();
                        ButtonMusic();
                    };
                }

                upgradePanel.Add(newCraftItem);
            }
        }

        private void ButtonMusic()
        {
            _audioSource.Play();
        }
        
        public override void OpenMenu()
        {
            GenerateUpgradeMenu();
            base.OpenMenu();
        }
    }
}