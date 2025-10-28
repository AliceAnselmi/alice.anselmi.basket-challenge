using System.Collections;
using UnityEngine;

public class Backboard : MonoBehaviour
{
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