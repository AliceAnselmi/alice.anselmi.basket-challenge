using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int score = 0;
    public static GameManager Instance { get; private set; }
    
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Transform spawnPoint;
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
}
