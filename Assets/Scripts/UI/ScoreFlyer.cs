using System.Collections;
using UnityEngine;
using TMPro;

public class ScoreFlyer : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 50f;
    [SerializeField] private float fadeDuration = 1.2f;

    private TextMeshProUGUI text;
    private RectTransform rect;
    private Color startColor;
    private float timer = 0f;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        rect = GetComponent<RectTransform>();
        startColor = text.color;
    }

    public void Initialize(int points)
    {
        text.text = $"+{points}";
        text.color = startColor;
        timer = 0f;
    }

    private void Update()
    {
       StartCoroutine(ShowScore());
    }

    private IEnumerator ShowScore()
    {
        timer += Time.deltaTime;
        rect.anchoredPosition += Vector2.up * moveSpeed * Time.deltaTime;
        float fadeOutFactor = timer / fadeDuration;
        // Wait for a short delay before starting to fade out
        yield return new WaitForSeconds(1f);
        text.color = new Color(startColor.r, startColor.g, startColor.b, 1f - fadeOutFactor);

        if (fadeDuration - timer <= 0f)
            Destroy(gameObject);
    }
}
