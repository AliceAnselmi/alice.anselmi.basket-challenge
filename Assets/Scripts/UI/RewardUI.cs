using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RewardUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerScoreText;
    [SerializeField] private TextMeshProUGUI opponentScoreText;
    [SerializeField] private TextMeshProUGUI resultText;
    
    private void OnEnable()
    {
        int playerScore = GameManager.Instance.player.score;
        int opponentScore = GameManager.Instance.opponent.score;
        playerScoreText.text = $"{playerScore}";
        opponentScoreText.text = $"{opponentScore}";
        if (playerScore > opponentScore)
        {
            resultText.text = "YOU WIN!";
        }
        else if (playerScore < opponentScore)
        {
            resultText.text = "YOU LOSE";
        }
        else
        {
            resultText.text = "TIE!";
        }
    }
    public void GoBackToMenu()
    {
        UIManager.Instance.GoBackToMenu();
    }
    
    public void PlayAgain()
    {
        GameManager.Instance.ResetMatch();
        UIManager.Instance.OnStartGame();
    }


}
