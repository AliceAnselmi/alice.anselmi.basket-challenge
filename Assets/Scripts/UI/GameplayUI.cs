using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayUI : MonoBehaviour
{
    [SerializeField] private SceneData sceneData;
    public void EndGame()
    {
        SceneManager.LoadScene(sceneData.mainMenuIndex);
    }
}
