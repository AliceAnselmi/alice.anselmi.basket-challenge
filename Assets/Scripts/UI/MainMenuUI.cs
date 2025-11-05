using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{

    [SerializeField] private Button playButton;
    [SerializeField] private Button easyButton;
    [SerializeField] private Button mediumButton;
    [SerializeField] private Button hardButton;
    [SerializeField] private GameObject gameLogo;
    
    
    public void OnPlayButtonPressed()
    {
        gameLogo.SetActive(false);
        playButton.gameObject.SetActive(false);
        easyButton.gameObject.SetActive(true);
        mediumButton.gameObject.SetActive(true);
        hardButton.gameObject.SetActive(true);
    }

    public void OnEasyButtonPressed()
    {
        DifficultyManager.Instance.SetDifficulty(DifficultyManager.DifficultyLevel.Easy);
        PlayGame();
    }
    
    public void OnMediumButtonPressed()
    {
        DifficultyManager.Instance.SetDifficulty(DifficultyManager.DifficultyLevel.Medium);
        PlayGame();
    }
    
    public void OnHardButtonPressed()
    {
        DifficultyManager.Instance.SetDifficulty(DifficultyManager.DifficultyLevel.Hard);
        PlayGame();
    }
    
    private void PlayGame()
    {   
        easyButton.gameObject.SetActive(false);
        mediumButton.gameObject.SetActive(false);
        hardButton.gameObject.SetActive(false);
        StartCoroutine(PlayAfterDelay(0.5f));
    }
    
    private IEnumerator PlayAfterDelay(float delay)
    {
        // Avoiding immediate shooting after scene load because of input from button
        yield return new WaitForSeconds(delay);
        playButton.gameObject.SetActive(true);
        GameManager.Instance.ResetMatch();
        GameManager.Instance.StartGame();
    }
    
}
