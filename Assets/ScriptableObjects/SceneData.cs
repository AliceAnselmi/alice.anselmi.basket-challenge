using UnityEngine;

[CreateAssetMenu(fileName = "SceneData", menuName = "Game Data/Scene Data")]
public class SceneData : ScriptableObject
{
    public int mainMenuIndex = 0;
    public int gameplayIndex = 1;
    public int rewardIndex = 2;
}