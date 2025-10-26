using UnityEngine;

[CreateAssetMenu(fileName = "ScoreData", menuName = "Game Data/Score Data")]
public class ScoreData : ScriptableObject
{
    public int perfectShotScore = 3;
    public int normalShotScore = 2;
    public int commonBonusScore = 4;
    public int rareBonusScore = 6;
    public int veryRareBonusScore = 8;
}