using UnityEngine;
using UnityEngine.UI;

public class SwipeFeedback : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private Image feedbackImage;
    [SerializeField] private float followSpeed = 15f;
    [SerializeField] private float fadeSpeed = 5f;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private bool isVisible = false;

    private void Awake()
    {
        rectTransform = feedbackImage.GetComponent<RectTransform>();
        canvasGroup = feedbackImage.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
    }

    private void Update()
    {
        if ((Input.GetMouseButton(0) || Input.touchCount > 0 ) && GameManager.Instance.player.canShoot)
        {
            UIManager.Instance.DrawSwipeLine(rectTransform, followSpeed);
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 1f, fadeSpeed * Time.deltaTime); // Fade in
            isVisible = true;
        } else if (isVisible)
        {
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 0f, fadeSpeed * Time.deltaTime); // Fade out

            if (canvasGroup.alpha < 0.01f)
                isVisible = false;
        }
    }
}