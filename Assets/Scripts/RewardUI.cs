using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardUI : MonoBehaviour
{
    [SerializeField] private SceneData sceneData;
    public void GoBackToMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneData.mainMenuIndex);
    }
    
    public void PlayAgain()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneData.gameplayIndex);
    }
}
