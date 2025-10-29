using UnityEngine;
[CreateAssetMenu(fileName = "Difficulty Data", menuName = "Game Data/Difficulty Data")]

public class DifficultyData : ScriptableObject
{
    [Header("Easy Mode Settings")]
    [Range(0f, 1f)] public float easyAccuracy = 0.4f; 
    [Range(0f, 0.2f)] public float easyAimError = 0.15f;
    [Range(0.5f, 3f)] public float easyShootIntervalMin = 1.5f;
    [Range(0.5f, 3f)] public float easyShootIntervalMax = 3.0f;
    
    [Header("Medium Mode Settings")]
    [Range(0f, 1f)] public float mediumAccuracy = 0.6f;
    [Range(0f, 0.2f)] public float mediumAimError = 0.1f;
    [Range(0.5f, 3f)] public float mediumShootIntervalMin = 1.0f;
    [Range(0.5f, 3f)] public float mediumShootIntervalMax = 2.5f;
    
    [Header("Hard Mode Settings")]
    [Range(0f, 1f)] public float hardAccuracy = 0.8f;
    [Range(0f, 0.2f)] public float hardAimError = 0.05f;
    [Range(0.5f, 3f)] public float hardShootIntervalMin = 0.5f;
    [Range(0.5f, 3f)] public float hardShootIntervalMax = 1.0f;
}
