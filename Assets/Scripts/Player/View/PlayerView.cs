using Level.View;
using Player.Model;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Player.View
{
    public class PlayerView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private Transform _transform;
        public Transform Transform => _transform;
        private GameInput _gameInput;
        private UnityEngine.Camera _camera;
        private PlayerSkinView _playerSkinView;

        [SerializeField] private LevelChangerView _levelChangerView;
        [SerializeField] private BallView _ballViewPrefab;
        [SerializeField] private BallView _ballView;
        [SerializeField] private float _maxTension = 2;

        private BallModel _craftedBallModel;

        private int _shootCount = 3;
        public int ShootCount => _shootCount;
        
        private Vector3 _shootDirection;

        private void Awake()
        {
            _camera = UnityEngine.Camera.main;
            _gameInput = new GameInput();
            _transform = transform;

            _playerSkinView = GetComponentInChildren<PlayerSkinView>();
            //_gameInput.Player.ShootBall.performed += ShootBall;
        }

        private void Start()
        {
            GameStatsView.Singleton.OnCraftedTool += AddNewTool;
            _levelChangerView.OnLevelChanged += ActivateShoot;
            _levelChangerView.OnRestartLevel += ResetShoot;
        }

        private void AddNewTool(BallModel ballModel)
        {
            _craftedBallModel = ballModel;
            PrepareTool();
        }

        private void PrepareTool()
        {
            if(_ballView != null) Destroy(_ballView.gameObject);
            if (_shootCount == 0 || _craftedBallModel == null) return;
            BallView ballView = Instantiate(_ballViewPrefab, _transform.position, Quaternion.identity, _levelChangerView.LevelView.transform);
            ballView.Initialize(_craftedBallModel);
            _ballView = ballView;
        }

        private void ShootBall()
        {
            if (_ballView==null) return;
            if (_shootCount == 0) return;
            _shootCount--;
            _ballView.Shoot(_shootDirection);
            _ballView = null;
            _playerSkinView.ShootBall();
            if (_shootCount > 0)
            {
                _craftedBallModel.CraftItem();
            }
        }

        private void ActivateShoot()
        {
            _shootCount += 3;
        }

        private void ResetShoot()
        {
            _shootCount = 3;
        }
        
        private void OnEnable()
        {
            _gameInput.Player.Enable();
        }
        
        private void OnDisable()
        {
            _gameInput.Player.Disable();
            GameStatsView.Singleton.OnCraftedTool -= AddNewTool;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (_ballView != null)
            {
                _playerSkinView.DragBall();
            }
            Debug.Log("Player Drag Begin");
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_ballView != null && _shootCount != 0)
            {
                _shootDirection = Vector3.ClampMagnitude(_camera.ScreenToWorldPoint(eventData.position) - _transform.position, _maxTension);
                _ballView.transform.position = -_shootDirection + _transform.position;
                _playerSkinView.transform.position = -_shootDirection + _transform.position;
            }
            
            Debug.Log("Player Drag");
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (_ballView != null && _shootCount != 0)
            {
                _playerSkinView.transform.position = _transform.position;
                ShootBall();
            }
            Debug.Log("Player Drag End");
        }
    }
}