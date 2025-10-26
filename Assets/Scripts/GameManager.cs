using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int score = 0;
    public static GameManager Instance { get; private set; }
    
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private GameObject player;
    public Transform ringAimTransform;
    public Transform backboardAimTransform;
    public MeshCollider ringCollider;
    
    [SerializeField] private ScoreData scoreData;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RespawnBall()
    {
        GameObject oldBall = GameObject.FindGameObjectWithTag("Ball");
        if (oldBall != null)
        {
            Destroy(oldBall);
        }
        MovePlayerToNextPosition();
        Transform spawnPoint = GameObject.FindGameObjectWithTag("BallSpawnPoint").transform;
        Instantiate(ballPrefab, spawnPoint.position, Quaternion.identity);
    }
    
    public void ScorePerfectShot()
    {
        score += scoreData.perfectShotScore;
        Debug.Log("Score now is " + score);
    }

    public void ScoreShot()
    {
        score += scoreData.normalShotScore;
        Debug.Log("Score now is " + score);
    }

    public void ScoreBackboardShot()
    {
        Debug.Log("Score now is " + score);
        //TODO later
    }
    
    public void MovePlayerToNextPosition()
    {
        Transform nextPos = ShootingPositionsManager.Instance.GetNextPosition();
        if (nextPos != null)
        {
            player.transform.position = nextPos.position;
            player.transform.rotation = nextPos.rotation;
            
            // rotate player towards ring
            Vector3 playerToRing = ringAimTransform.position - player.transform.position;
            playerToRing.y = 0;
            player.transform.rotation = Quaternion.LookRotation(playerToRing);
        }

    }


}
