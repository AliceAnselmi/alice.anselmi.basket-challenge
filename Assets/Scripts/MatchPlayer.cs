using UnityEngine;
[System.Serializable]
public class MatchPlayer : MonoBehaviour
{
    public PlayerType playerType;
    public Transform ballSpawnPoint;
    public Ball ball;
    public Transform ballTransform;
    public int score;
    public int positionIndex;
    public bool canShoot;
    public bool isAI;
    
    public enum PlayerType
    {
        Player,
        AI
    }

}
