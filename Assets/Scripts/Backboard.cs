using System.Collections;
using UnityEngine;

public class Backboard : MonoBehaviour
{
    [Header("Bonus Settings")]
    [Range(0f, 1f)] [SerializeField] private float backboardBonusChance = 0.3f;
    [SerializeField] private float bonusDuration = 6f;
    [SerializeField] private float rollInterval = 2f;
    private float rollTimer = 0f;
    
    [Header("Blink Settings")]
    [SerializeField] private float blinkInterval = 0.8f;
    [SerializeField] private Color blinkColor = Color.yellow;
    private Renderer m_Renderer;
    private Color m_BaseColor;

    private void Awake()
    {
        m_Renderer = GetComponent<Renderer>();
        if (m_Renderer != null)
            m_BaseColor = m_Renderer.material.color;
    }

    private void Update()
    {
        rollTimer += Time.deltaTime;
        if (rollTimer >= rollInterval)
        {
            rollTimer = 0f;
            RollBonus();
        }
    }
    
    
    private void RollBonus()
    {
        if (!GameManager.Instance.gameStarted || GameManager.Instance.isBackboardBonusActive) return;

        if (Random.value < backboardBonusChance)
        {
            float rarityRoll = Random.value;
            ScoreData scoreData = GameManager.Instance.scoreData;
            switch (rarityRoll)
            {
                case < 0.7f:
                    scoreData.currentBonusScore = scoreData.commonBonusScore;
                    break;
                case < 0.9f:
                    scoreData.currentBonusScore = scoreData.rareBonusScore;
                    break;
                default:
                    scoreData.currentBonusScore  = scoreData.veryRareBonusScore;
                    break;
            }
            GameManager.Instance.EnableBackboardBonus();
            StartBlinking();
            StartCoroutine(DisableBonusAfterDelay());
        }
    }

    private IEnumerator DisableBonusAfterDelay()
    {
        yield return new WaitForSeconds(bonusDuration);
        GameManager.Instance.DisableBackboardBonus();
        StopBlinking();
    }

    public void StartBlinking()
    {
        StartCoroutine(Blink());
    }

    public void StopBlinking()
    {
        StopAllCoroutines();
    }

    private IEnumerator Blink()
    {
        while (GameManager.Instance.isBackboardBonusActive)
        {
            m_Renderer.material.color = blinkColor;
            yield return new WaitForSeconds(blinkInterval);
            m_Renderer.material.color = m_BaseColor;
            yield return new WaitForSeconds(blinkInterval);
        }
        m_Renderer.material.color = m_BaseColor;
    }
}