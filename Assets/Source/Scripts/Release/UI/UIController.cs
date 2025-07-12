using Source.Scripts.Release.Stuff;
using Source.Scripts.Release.TurnManager;
using Source.Scripts.Release.UI.ControllerParts;
using TMPro;
using UnityEngine;
using YG;

namespace Source.Scripts.Release.UI
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _inventoryCanvasGroup;
        [SerializeField] private GameObject _playerMovementCanvas;
        [SerializeField] private TurnState _turnState;
        [SerializeField] private TMP_Text _counterText;

        [Header("Parts")]

        [SerializeField] private MainMenuUI _mainMenuUI;
        [SerializeField] private SoundUI _soundUI;
        [SerializeField] private LevelEndUI _levelEndUI;
        [SerializeField] private SceneNavigator _sceneNavigator;
        [SerializeField] private PlayerInteractionUI _playerInteractionUI;

        public PlayerInteractionUI PlayerInteractionUI => _playerInteractionUI;

        private void Awake()
        {
            AudioManager audioManager = FindObjectOfType<AudioManager>();

            _mainMenuUI.Init(audioManager);
            _soundUI.Init(audioManager);
            _levelEndUI.Init(audioManager);
            _sceneNavigator.Init(audioManager);
            _playerInteractionUI.Init(audioManager);
        }

        private void Start()
        {
            Time.timeScale = 1;
            _playerMovementCanvas.SetActive(YG2.envir.isMobile);
        }

        private void OnEnable()
        {
            _turnState.PlayerCanShootChanged += OnPlayerCanShootChanged;
            _turnState.CompletedTurns += UpdateTurnCounterText;
        }

        private void OnDisable()
        {
            _turnState.PlayerCanShootChanged -= OnPlayerCanShootChanged;
            _turnState.CompletedTurns -= UpdateTurnCounterText;
        }

        private void UpdateTurnCounterText(int turnCount)
        {
            _counterText.text = turnCount.ToString();
        }

        private void OnPlayerCanShootChanged(bool state)
        {
            _inventoryCanvasGroup.interactable = state;
        }
    }
}
