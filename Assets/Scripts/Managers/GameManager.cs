using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    private static float s_MatchDuration = 120f; // two minutes in seconds

    [Header("References")]
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private GameObject player;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private Ball ball;
    [SerializeField] private ScoreData scoreData;
    [SerializeField] private SceneData sceneData;
    public MeshCollider ringCollider;
    
    [Header("Backboard Bonus Settings")]
    [SerializeField] private Backboard backboard;
    [SerializeField] private float backboardBonusChance = 0.3f;
    [SerializeField] private float bonusDuration = 6f;

    [Header("Aim Transforms")]
    public Transform ringAimTransform;
    public Transform backboardAimTransform;
    private Transform m_BallTransform;
    private BonusRarity m_CurrentBonusRarity;
    
    [HideInInspector] public bool isBackboardBonusActive = false;
    [HideInInspector] public bool canShoot = true;
    private int m_Score = 0;
    private float m_Timer = 0f;
    
    private enum BonusRarity
    {
        Common,
        Rare,
        VeryRare
    }

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

    private void Start()
    {
        StartCoroutine(TimerRoutine());
    }
    
    private IEnumerator TimerRoutine()
    {
        WaitForSeconds delay = new WaitForSeconds(1);
        while (m_Timer < s_MatchDuration)
        {
            Debug.Log("Timer: " + m_Timer);
            m_Timer += 1f;
            yield return delay;
        }
        EndGame();
    }
    
    private void Update()
    {
        // roll for backboard bonus activation
        RollBackboardBonus();
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
    
    private void MovePlayerToNextPosition()
    {
        Transform nextPos = ShootingPositionsManager.Instance.GetNextPosition();
        player.transform.position = nextPos.position;
        player.transform.rotation = nextPos.rotation;

        // rotate player towards ring
        Vector3 playerToRing = ringAimTransform.position - player.transform.position;
        playerToRing.y = 0;
        player.transform.rotation = Quaternion.LookRotation(playerToRing);
    }

    private void RollBackboardBonus()
    {
        if (Random.value < backboardBonusChance && !isBackboardBonusActive)
        {
            ball.aimForBackboard = true; //TODO this is for testing purposes only, reference to ball will be removed later
            isBackboardBonusActive = true;
            
            switch (Random.value)
            {
                case < 0.7f:
                    m_CurrentBonusRarity = BonusRarity.Common;
                    break;
                case < 0.9f:
                    m_CurrentBonusRarity = BonusRarity.Rare;
                    break;
                default:
                    m_CurrentBonusRarity = BonusRarity.VeryRare;
                    break;
            }
            
            backboard.StartBlinking();
            StartCoroutine(DisableBonusAfterDelay());
        }
    }
    private IEnumerator DisableBonusAfterDelay()
    {
        yield return new WaitForSeconds(bonusDuration);
        isBackboardBonusActive = false;
        backboard.StopBlinking();
    }

    public void ScorePerfectShot()
    {
        m_Score += scoreData.perfectShotScore;
        Debug.Log("Score now is " + m_Score);
    }

    public void ScoreShot()
    {
        m_Score += scoreData.normalShotScore;
        Debug.Log("Score now is " + m_Score);
    }

    public void ScoreBackboardShot()
    {
        int bonusScore = 0;
        switch (m_CurrentBonusRarity)
        {
            case BonusRarity.Common:
                bonusScore = scoreData.commonBonusScore;
                break;
            case BonusRarity.Rare:
                bonusScore = scoreData.rareBonusScore;
                break;
            case BonusRarity.VeryRare:
                bonusScore = scoreData.veryRareBonusScore;
                break;
        }

        m_Score += bonusScore;
        Debug.Log("Score now is " + m_Score);
    }

    private void EndGame()
    {
        canShoot = false;
        SceneManager.LoadScene(sceneData.rewardIndex);
    }
}
