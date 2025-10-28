using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private Camera camera;
    [SerializeField] private Canvas canvas;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private GameObject scoreFlyerPrefab;
    [SerializeField] private Image inputBarFill;
    [SerializeField] private float inputBarFillSpeed = 1f;

    private float currentTime;
    private Coroutine timerRoutine;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        UpdateInputBarValue();
    }
    public void UpdateScore(int newScore)
    {
        scoreText.text = $"SCORE: {newScore}";
    }
    

    public void ShowScoreFlyer(int points, Vector3 worldPosition)
    {

        // Project the world position to UI screen position
        Vector3 screenPos = camera.WorldToScreenPoint(worldPosition);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            screenPos,
            camera,
            out Vector2 localPoint
        );

        GameObject flyer = Instantiate(scoreFlyerPrefab, canvas.gameObject.transform);
        transform.localPosition = localPoint;
        flyer.GetComponent<ScoreFlyer>().Initialize(points);
    }
    
    public void DrawSwipeLine(RectTransform rectTransform, float followSpeed)
    {
        Vector2 screenPos = InputManager.Instance.inputPosition;

        // Project to UI space
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            screenPos,
            camera,
            out Vector2 uiSpacePos
        );
        
        rectTransform.anchoredPosition = Vector2.Lerp(
            rectTransform.anchoredPosition, uiSpacePos, followSpeed * Time.deltaTime); // Lerp for smooth follow
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
