using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int score = 0;
    public static GameManager Instance { get; private set; }

    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private GameObject player;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private ScoreData scoreData;
    
    public Transform ringAimTransform;
    public Transform backboardAimTransform;
    public MeshCollider ringCollider;

    private Transform m_BallTransform;

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

    public void SetupCamera(bool followBall)
    {
        if (followBall)
        {
            cameraController.StartFollowingBall(m_BallTransform);
        }
        else
        {
            cameraController.StopFollowingBall();
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
        GameObject ball = Instantiate(ballPrefab, spawnPoint.position, Quaternion.identity);
        m_BallTransform = ball.transform;
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
        player.transform.position = nextPos.position;
        player.transform.rotation = nextPos.rotation;

        // rotate player towards ring
        Vector3 playerToRing = ringAimTransform.position - player.transform.position;
        playerToRing.y = 0;
        player.transform.rotation = Quaternion.LookRotation(playerToRing);
    }
}
