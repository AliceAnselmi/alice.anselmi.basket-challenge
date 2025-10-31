using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("UI References")] [SerializeField]
    private GameObject mainMenuUI;

    [SerializeField] private GameObject gameplayUI;
    [SerializeField] private GameObject rewardUI;
    [SerializeField] private GameObject background;

    [Header("Match UI Elements")] [SerializeField]
    private Camera camera;

    [SerializeField] private Canvas canvas;
    [SerializeField] private TextMeshProUGUI playerScoreText;
    [SerializeField] private TextMeshProUGUI opponentScoreText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private GameObject scoreFlyerPrefab;
    [SerializeField] private Image inputBarFill;
    [SerializeField] private float inputBarFillSpeed = 1f;

    private float currentTime;
    private Coroutine timerRoutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void OnStartGame()
    {
        background.SetActive(false);
        mainMenuUI.SetActive(false);
        gameplayUI.SetActive(true);
        rewardUI.SetActive(false);
    }

    public void OnEndGame()
    {
        background.SetActive(true);
        mainMenuUI.SetActive(false);
        gameplayUI.SetActive(false);
        rewardUI.SetActive(true);
    }
    
    public void GoBackToMenu()
    {
        background.SetActive(true);
        mainMenuUI.SetActive(true);
        gameplayUI.SetActive(false);
        rewardUI.SetActive(false);
    }
}
