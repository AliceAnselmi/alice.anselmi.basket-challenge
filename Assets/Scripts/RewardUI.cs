using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardUI : MonoBehaviour
{
    public void GoBackToMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
    
    public void PlayAgain()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
}
