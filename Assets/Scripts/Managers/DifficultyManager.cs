using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager Instance { get; private set; }
    public enum DifficultyLevel { Easy, Medium, Hard };
    [SerializeField] private DifficultyData difficultyData;
    
    [HideInInspector] public float accuracy; 
    [HideInInspector] public float aimError;
    [HideInInspector] public float shootIntervalMin;
    [HideInInspector] public float shootIntervalMax;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public void SetDifficulty(DifficultyLevel difficulty)
    {
        switch (difficulty)
        {
            case DifficultyLevel.Easy:
                accuracy = difficultyData.easyAccuracy;
                aimError = difficultyData.easyAimError;
                shootIntervalMin = difficultyData.easyShootIntervalMin;
                shootIntervalMax = difficultyData.easyShootIntervalMax;
                break;
            
            case DifficultyLevel.Medium:
                accuracy = difficultyData.mediumAccuracy;
                aimError = difficultyData.mediumAimError;
                shootIntervalMin = difficultyData.mediumShootIntervalMin;
                shootIntervalMax = difficultyData.mediumShootIntervalMax;
                break;
            case DifficultyLevel.Hard:
                accuracy = difficultyData.hardAccuracy;
                aimError = difficultyData.hardAimError;
                shootIntervalMin = difficultyData.hardShootIntervalMin;
                shootIntervalMax = difficultyData.hardShootIntervalMax;
                break;
        }
        
    }
}
