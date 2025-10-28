using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{

    [SerializeField] private SceneData sceneData;
    public void PlayGame()
    {
        SceneManager.LoadScene(sceneData.gameplayIndex);
    }
    
}
