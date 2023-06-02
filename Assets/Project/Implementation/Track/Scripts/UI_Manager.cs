using SOGameEvents;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DrivingSimulation
{
    public class UI_Manager : MonoBehaviour
    {

        [Header("UI References")]
        [SerializeField]
        private GameObject _gamePanel;
        [SerializeField]
        private GameObject _gameWinPanel;
        [SerializeField]
        private TMP_Text _pointText;
        [SerializeField]
        private UI_MistakeItem _mistakeItemPrefab;
        [SerializeField]
        private Transform _mistakeItemParent;
        [Scene]
        public string mainMenuScene;

        [Header("Events")]
        [SerializeField]
        private GameEventNoParam _initializeGame = null;
        [SerializeField]
        private GameEventNoParam _gameWinCallback = null;
        [SerializeField]
        private GameEventNoParam _gotoLevelCallback = null;
        [SerializeField]
        private GameEventInt _onPointChanged = null;

        private int _currentPoint = 0;


        private void Awake()
        {
            _initializeGame.AddListener(Initialize);
            _gameWinCallback.AddListener(GameWin);
            _onPointChanged.AddListener(OnPointChanged);
        }

        private void OnDestroy()
        {
            _initializeGame.RemoveListener(Initialize);
            _gameWinCallback.RemoveListener(GameWin);
            _onPointChanged.RemoveListener(OnPointChanged);
        }

        private void Initialize()
        {
            _gamePanel.SetActive(true);
            _gameWinPanel.SetActive(false);
        }

        private void GameWin()
        {
            _pointText.text = _currentPoint.ToString();
            _gamePanel.SetActive(false);
            _gameWinPanel.SetActive(true);

            // instantiate all mistake
            InstantiatePersonaList();
        }

        private void InstantiatePersonaList()
        {
            if (PersonaDataTracker.Instance == null)
                return;

            PersonaData persona = PersonaDataTracker.Instance.PersonaData;

            if (persona.WrongLane > 0)
            {
                UI_MistakeItem wrongLaneUI = Instantiate(_mistakeItemPrefab, _mistakeItemParent);
                wrongLaneUI.SetText("Wrong Lane", (int)persona.WrongLane);
            }

            if (persona.Crash > 0)
            {
                UI_MistakeItem crashUI = Instantiate(_mistakeItemPrefab, _mistakeItemParent);
                crashUI.SetText("Crash", (int)persona.Crash);
            }

            if (persona.WrongIndicator > 0)
            {
                UI_MistakeItem indicatorUI = Instantiate(_mistakeItemPrefab, _mistakeItemParent);
                indicatorUI.SetText("Wrong Indicator", (int)persona.WrongIndicator);
            }

            if (persona.ViolateSpeedLimit > 0)
            {
                UI_MistakeItem speedLimitUI = Instantiate(_mistakeItemPrefab, _mistakeItemParent);
                speedLimitUI.SetText("Violate Speed Limit", (int)persona.ViolateSpeedLimit);
            }
        }

        public void NextLevel()
        {
            _gotoLevelCallback?.Invoke();
        }

        public void RetryLevel()
        {
            SceneManager.LoadScene(gameObject.scene.name);
        }

        public void BackMenu()
        {
            SceneManager.LoadScene(mainMenuScene);
        }

        private void OnPointChanged(int point)
        {
            _currentPoint = point;
        }
    }
}
