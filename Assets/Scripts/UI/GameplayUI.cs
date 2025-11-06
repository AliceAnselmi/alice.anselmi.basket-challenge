using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameplayUI : MonoBehaviour
{

    [Header("Match UI Elements")] [SerializeField]
    private Camera camera;

    [SerializeField] private Canvas canvas;
    [SerializeField] private TextMeshProUGUI playerScoreText;
    [SerializeField] private TextMeshProUGUI opponentScoreText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private GameObject scoreFlyerPrefab;
    [SerializeField] private GameObject perfectScoreFlyerPrefab;
    [SerializeField] private GameObject backboardScoreFlyerPrefab;
    [SerializeField] private Image inputBarFill;
    [SerializeField] private float inputBarFillSpeed = 1f;

    private float currentTime;
    private Coroutine timerRoutine;

    private void Update()
    {
        UpdateInputBarValue();
    }

    public void UpdateScore(int newScore, MatchPlayer matchPlayer)
    {
        if (matchPlayer.playerType == MatchPlayer.PlayerType.Player)
        {
            playerScoreText.text = $"{newScore}";
        }
        else
        {
            opponentScoreText.text = $"{newScore}";
        }
    }

    public void ShowScoreFlyer(int points, Vector3 worldPosition)
    {
        // Project the world position to UI screen position
        Vector3 localPoint = ProjectToScreen(worldPosition);
        GameObject flyer = Instantiate(scoreFlyerPrefab, canvas.gameObject.transform);
        flyer.GetComponent<RectTransform>().localPosition = localPoint;
        flyer.GetComponent<ScoreFlyer>().Initialize(points);
    }

    public void ShowPerfectScoreFlyer(Vector3 worldPosition)
    {
        worldPosition.y += 0.5f; // Move up the flyer a bit so that it doesn't overlap with the score flyer
        Vector3 localPoint = ProjectToScreen(worldPosition);
        GameObject flyer = Instantiate(perfectScoreFlyerPrefab, canvas.gameObject.transform);
        flyer.GetComponent<RectTransform>().localPosition = localPoint;
    }
    
    public void ShowBackboardScoreFlyer(Vector3 worldPosition)
    {
        worldPosition.y += 0.5f; // Move up the flyer a bit so that it doesn't overlap with the score flyer
        Vector3 localPoint = ProjectToScreen(worldPosition);
        GameObject flyer = Instantiate(backboardScoreFlyerPrefab, canvas.gameObject.transform);
        flyer.GetComponent<RectTransform>().localPosition = localPoint;
    }

    private Vector3 ProjectToScreen(Vector3 worldPosition)
    {
        // Project the world position to UI screen position
        Vector3 screenPos = camera.WorldToScreenPoint(worldPosition);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            screenPos,
            camera,
            out Vector2 localPoint
        );
        return localPoint;
    }
    public void UpdateTimer(float time)
    {
        // Time format: mm:ss
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    private void UpdateInputBarValue()
    {
        float delta = InputManager.Instance.incrementalSwipeDelta;
        inputBarFill.fillAmount = Mathf.Clamp01(
            inputBarFill.fillAmount + delta * inputBarFillSpeed);
    }

    public void ResetInputBar()
    {
        inputBarFill.fillAmount = 0f;
    }
}
