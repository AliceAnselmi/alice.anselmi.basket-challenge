using System.Collections;
using UnityEngine;

public class Backboard : MonoBehaviour
{
    [SerializeField] private float blinkInterval = 0.8f;

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
            m_Renderer.enabled = !m_Renderer.enabled;
            yield return new WaitForSeconds(blinkInterval);
        }
        m_Renderer.enabled = true;
        m_Renderer.material.color = m_BaseColor;
    }
}