using System;
using Level.View;
using Player.Model;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.View
{
    public class CraftMenuView : UIMainView
    {
        public override void GenerateMenu()
        {
            GenerateCraftMenu();
            CloseMenu();
        }

        private void GenerateCraftMenu()
        {
            var root = _menuDocument.rootVisualElement;
            
            _mainPanel = root.Q<VisualElement>("CraftPanel");
            var itemsPanel = root.Q<VisualElement>("ItemPanel");
            itemsPanel.Clear();
            Button closeButton = root.Q<Button>("CloseButton");
            closeButton.clicked += CloseMenu;
            
            var craftItem = Resources.Load<VisualTreeAsset>("UI/CraftMenu/CraftItem");
            var resourcesRow = Resources.Load<VisualTreeAsset>("UI/CraftMenu/ResourcesRow");

            foreach (var toolModel in GameStatsView.Singleton.ToolModels)
            {
                var newCraftItem = craftItem.CloneTree();
                Debug.Log("Tools/tool_"+(int)toolModel.ToolType);
                var newBack = Resources.Load<Sprite>("Tools/tool_"+(int)toolModel.ToolType);
                Button craftButton = newCraftItem.Q<Button>("CraftButton");
                craftButton.text = "Craft";
                craftButton.clicked += toolModel.CraftItem;
                newCraftItem.style.backgroundImage = new StyleBackground(newBack);

                foreach (var prices in toolModel.PricesDictionary)
                {
                    var newResourcesRow = resourcesRow.CloneTree();
                    var newResource = Resources.Load<Sprite>("Blocks/resource_"+(int)prices.Key);
                    var craftResources = newCraftItem.Q<VisualElement>("CraftResources");
                    Label resourcePrice = newResourcesRow.Q<Label>("ResourcePrice");
                    VisualElement resourceImage = newResourcesRow.Q<VisualElement>("ResourceImage");
                    resourcePrice.text = (prices.Value).ToString();
                    resourceImage.style.backgroundImage = new StyleBackground(newResource);
                    
                    craftResources.Add(newResourcesRow);
                    
                }
                
                itemsPanel.Add(newCraftItem);
            }
        }
        
        public override void OpenMenu()
        {
            GenerateCraftMenu();
            base.OpenMenu();
        }
    }
}