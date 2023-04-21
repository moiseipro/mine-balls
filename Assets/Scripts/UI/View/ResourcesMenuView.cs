using System.Collections;
using Level.View;
using Player.Model;
using Player.View;
using Source.TutorialSystem.Views;
using UnityEngine;
using UnityEngine.UIElements;
using YG;

namespace UI.View
{
    public class ResourcesMenuView : UIMainView
    {
        [SerializeField] private CraftMenuView _craftMenuView;
        [SerializeField] private UpgradeMenuView _upgradeMenuView;
        [SerializeField] private LevelChangerView _levelChangerView;
        [SerializeField] private PlayerView _playerView;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private TutorialTarget _tutorialTarget;

        private VisualElement _buttonMenuPanel;
        
        private IEnumerator _coroutine;
        private bool isBaseOpen = false;

        public override void GenerateMenu()
        {
            GenerateButtonMenu();
            OpenMenu();
            GameStatsView.Singleton.OnResourcesChanged += GenerateResourcesMenu;
            _levelChangerView.OnLevelChanged += GenerateResourcesMenu;
        }

        private void GenerateButtonMenu()
        {
            _buttonMenuPanel = _menuDocument.rootVisualElement.Q<VisualElement>("MenuButtonPanel");
            //Button craftMenuButton = root.Q<Button>("CraftMenuButton");
            //_levelChangerView.OnEndLevel += ShowNextLevelButton;
            _levelChangerView.OnLoseLevel += ShowRestartLevelButton;
            Button upgradeMenuButton = _buttonMenuPanel.Q<Button>("UpgradeMenuButton");
            Button nextLevelButton = _buttonMenuPanel.Q<Button>("NextLevelButton");
            nextLevelButton.AddToClassList("hidden");
            Button restartLevelButton = _buttonMenuPanel.Q<Button>("RestartLevelButton");
            restartLevelButton.AddToClassList("hidden");
            Button musicButton = _buttonMenuPanel.Q<Button>("MusicMenuButton");
            
            upgradeMenuButton.clicked += () =>
            {
                ButtonMusic();
                _tutorialTarget.StartTutorial();
                _upgradeMenuView.OpenMenu();
            };
            musicButton.clicked += () =>
            {
                ButtonMusic();
                int music = PlayerPrefs.GetInt("music", 1);
                music = music == 1 ? 0 : 1;
                AudioListener.volume = music;
                var newResource = Resources.Load<Sprite>("UI/music_"+music);
                musicButton.style.backgroundImage = new StyleBackground(newResource);
                PlayerPrefs.SetInt("music", music);
            };
            restartLevelButton.clicked += () =>
            {
                ButtonMusic();
                _levelChangerView.RestartLevel();
                restartLevelButton.AddToClassList("hidden");
            };
        }
        
        private void GenerateResourcesMenu()
        {
            Label depthMaxText = _buttonMenuPanel.Q<Label>("InfoMaxDepthText");
            depthMaxText.text = YandexGame.savesData.maxLevel.ToString();
            Label depthText = _buttonMenuPanel.Q<Label>("InfoDepthText");
            depthText.text = _levelChangerView.LevelNum.ToString();
            Label toolCountText = _buttonMenuPanel.Q<Label>("InfoToolText");
            toolCountText.text = _playerView.ShootCount.ToString();
            
            var root = _menuDocument.rootVisualElement;
            _mainPanel = root.Q<VisualElement>("ResourcesPanel");
            _mainPanel.Clear();
            var resourcesRow = Resources.Load<VisualTreeAsset>("UI/ResourcesMenu/ResourceRow");

            foreach (var money in GameStatsView.Singleton.Money)
            {
                var newResourcesRow = resourcesRow.CloneTree();
                var newResource = Resources.Load<Sprite>("Blocks/resource_"+(int)money.Key);
                Label resourceCount = newResourcesRow.Q<Label>("ResourceCount");
                VisualElement resourceImage = newResourcesRow.Q<VisualElement>("ResourceImage");
                resourceCount.text = (money.Value).ToString();
                resourceImage.style.backgroundImage = new StyleBackground(newResource);
                    
                _mainPanel.Add(newResourcesRow);
            }
        }

        public void ShowNextLevelButton(int level)
        {
            Button nextLevelButton = _buttonMenuPanel.Q<Button>("NextLevelButton");
            nextLevelButton.RemoveFromClassList("hidden");
        }
        
        public void ShowRestartLevelButton()
        {
            Button restartLevelButton = _buttonMenuPanel.Q<Button>("RestartLevelButton");
            restartLevelButton.RemoveFromClassList("hidden");
        }

        public override void OpenMenu()
        {
            GenerateResourcesMenu();
            base.OpenMenu();
        }
        
        private void ButtonMusic()
        {
            _audioSource.Play();
        }

        private void OnDisable()
        {
            GameStatsView.Singleton.OnResourcesChanged -= TimerOpenMenu;
            _levelChangerView.OnEndLevel -= ShowNextLevelButton;
            _levelChangerView.OnLoseLevel -= ShowRestartLevelButton;
        }
    }
}