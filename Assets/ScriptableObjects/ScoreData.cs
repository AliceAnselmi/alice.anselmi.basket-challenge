using UnityEngine;

[CreateAssetMenu(fileName = "ScoreData", menuName = "Game Data/Score Data")]
public class ScoreData : ScriptableObject
{
    public int perfectShotScore = 3;
    public int normalShotScore = 2;
}