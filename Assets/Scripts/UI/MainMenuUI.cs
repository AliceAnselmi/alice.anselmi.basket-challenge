using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{

    [SerializeField] private Button playButton;
    [SerializeField] private Button easyButton;
    [SerializeField] private Button mediumButton;
    [SerializeField] private Button hardButton;
    
    
    public void OnPlayButtonPressed()
    {
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
        playButton.gameObject.SetActive(true);
        easyButton.gameObject.SetActive(false);
        mediumButton.gameObject.SetActive(false);
        hardButton.gameObject.SetActive(false);
        GameManager.Instance.ResetMatch();
        GameManager.Instance.StartGame();
    }
    
}
