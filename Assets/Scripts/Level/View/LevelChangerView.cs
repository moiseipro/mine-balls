using System;
using Camera.View;
using UnityEngine;
using YG;
using Random = UnityEngine.Random;

namespace Level.View
{
    public class LevelChangerView : MonoBehaviour
    {
        private Transform _transform;
        
        private LevelView _levelView;
        public LevelView LevelView => _levelView;
        
        private int _levelNum = 0;
        public int LevelNum => _levelNum;

        [SerializeField] private float _timeToCheckLevel = 4f;
        private float _timeCheckLevel;
        [SerializeField] private CameraView _cameraView;
        [SerializeField] private LuckBorderView[] _luckBorderViewPrefabs;
        [SerializeField] private LevelView[] _levelStartPrefabs;
        [SerializeField] private LevelView[] _levelViewPrefabs;
        

        public Action<int> OnEndLevel;
        public Action OnLoseLevel;
        public Action OnLevelChanged;
        public Action OnRestartLevel;

        private void Awake()
        {
            _transform = transform;
            RestartLevel();
        }

        private void FixedUpdate()
        {
            if (_timeCheckLevel < Time.time)
            {
                _timeCheckLevel = Time.time + _timeToCheckLevel;
                EndLevel();
            }
        }

        public BorderView GetRandomLevelBlock()
        {
            return _levelView.GetRandomActiveBorder();
        }

        public void RestartLevel()
        {
            foreach (var levelView in GetComponentsInChildren<LevelView>())
            {
                Destroy(levelView.gameObject);
            }
            _cameraView.ResetPosition();
            _levelNum = 0;
            _levelView = null;
            GenerateLevel();
            OnRestartLevel?.Invoke();
        }

        public void NextLevel()
        {
            _levelNum++;
            if (YandexGame.savesData.maxLevel < _levelNum)
            {
                YandexGame.savesData.maxLevel = _levelNum;
            }
            
            
            GenerateLevel();
        }

        private void GenerateLevel()
        {
            Vector3 newPosition = _transform.position;
            if ( _levelView != null)
            {
                newPosition = _levelView.transform.position;
            }
            if (_levelNum == 0)
            {
                int randomLevel = Random.Range(0, _levelStartPrefabs.Length);
                newPosition.y -= _levelStartPrefabs[randomLevel].LevelHeight;
                
                _levelView = Instantiate(
                    _levelStartPrefabs[randomLevel],
                    newPosition,
                    Quaternion.identity,
                    _transform);
            }
            else
            {
                int randomLevel = Random.Range(
                    Mathf.Clamp(_levelNum/10, 0, _levelViewPrefabs.Length-1), 
                    Mathf.Clamp(1+_levelNum/2, 0, _levelViewPrefabs.Length));
                newPosition.y -= _levelViewPrefabs[randomLevel].LevelHeight;
                
                _levelView = Instantiate(
                    _levelViewPrefabs[randomLevel],
                    newPosition,
                    Quaternion.identity,
                    _transform);
                _cameraView.AddYPosition(_levelView.LevelHeight);
            }
            
            
            _levelView.StartLevel();
            for (int i = 0; i < 1+_levelNum/15; i++)
            {
                Instantiate(
                    _luckBorderViewPrefabs[Random.Range(
                        Mathf.Clamp(_levelNum/25, 0, _luckBorderViewPrefabs.Length-1), 
                        Mathf.Clamp(1+_levelNum/15, 0, _luckBorderViewPrefabs.Length))],
                    GetRandomLevelBlock().Transform.position,
                    Quaternion.identity,
                    _levelView.transform);
            }
            
            OnLevelChanged?.Invoke();
        }

        public void EndLevel()
        {
            if (_levelView.IsLevelComplete())
            {
                NextLevel();
                //if(_levelNum % 5==0) GS_Ads.ShowFullscreen();
                OnEndLevel?.Invoke(_levelNum);
            }
            else
            {
                OnLoseLevel?.Invoke();
            }
            
        }
    }
}