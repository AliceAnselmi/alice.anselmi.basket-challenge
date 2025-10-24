using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayUI : MonoBehaviour
{
    public void EndGame()
    {
        SceneManager.LoadScene(0);
    }
}
