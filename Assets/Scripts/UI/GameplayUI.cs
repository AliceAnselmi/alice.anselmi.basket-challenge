using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayUI : MonoBehaviour
{
    [SerializeField] private SceneData sceneData;
    public void Quit()
    {
        SceneManager.LoadScene(sceneData.mainMenuIndex);
    }
}
