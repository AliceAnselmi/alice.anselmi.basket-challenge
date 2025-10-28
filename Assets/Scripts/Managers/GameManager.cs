using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    [Header("Game Settings")]
    [SerializeField] private float matchDuration = 120f; // two minutes in seconds
    [SerializeField] private float gravity = -9.81f;

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
    private float m_RollInterval = 1f;
    private float m_RollTimer = 0f;

    [Header("Aim Transforms")]
    public Transform ringAimTransform;
    public Transform backboardAimTransform;
    private Transform m_BallTransform;
    private BonusRarity m_CurrentBonusRarity;
    
    [HideInInspector] public float shootForce = 0f;
    [HideInInspector] public bool isBackboardBonusActive = false;
    [HideInInspector] public bool gameEnded = false;
    [HideInInspector] public bool canShoot = false;
    private int m_Score = 0;
    private float m_Timer = 120f;
    
    private enum BonusRarity
    {
        Common,
        Rare,
        VeryRare
    }

    private void Awake()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
        Physics.gravity = new Vector3(0, gravity, 0);
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
        m_BallTransform = GameObject.FindGameObjectWithTag("Ball").transform;
        StartCoroutine(TimerRoutine());
    }
    
    private IEnumerator TimerRoutine()
    {
        WaitForSeconds delay = new WaitForSeconds(1);
        while (m_Timer > 0)
        {
            m_Timer -= 1f;
            UIManager.Instance.UpdateTimer(m_Timer);
            yield return delay;
        }
        EndGame();
    }
    
    private void Update()
    {
        // Roll for backboard bonus activation
        m_RollTimer += Time.deltaTime;
        if (m_RollTimer >= m_RollInterval)
        {
            m_RollTimer = 0f;
            RollBackboardBonus();
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

    public void RefreshMatch()
    {
        InputManager.Instance.hasInputEnded = false;
        UIManager.Instance.ResetInputBar();
        shootForce = 0f;
        MovePlayerToNextPosition();
        RespawnBall();
        SetupCamera(false);
        canShoot = true;
    }

    private void RespawnBall()
    {
        GameObject oldBall = GameObject.FindGameObjectWithTag("Ball");
        if (oldBall != null)
        {
            Destroy(oldBall);
        }
        Transform spawnPoint = GameObject.FindGameObjectWithTag("BallSpawnPoint").transform;
        GameObject newBall = Instantiate(ballPrefab, spawnPoint.position, Quaternion.identity);
        m_BallTransform = newBall.transform;
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
        if (isBackboardBonusActive)
            return;
        float rolledValue = Random.value;
        if (rolledValue < backboardBonusChance)
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
    
    private void UpdateScore(int scoreToAdd)
    {
        m_Score += scoreToAdd;
        UIManager.Instance.UpdateScore(m_Score);
        UIManager.Instance.ShowScoreFlyer(scoreToAdd, m_BallTransform.transform.position);
    }

    public void ScorePerfectShot()
    {
        UpdateScore(scoreData.perfectShotScore);
    }

    public void ScoreShot()
    {
        UpdateScore(scoreData.normalShotScore);
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
        UpdateScore(bonusScore);
    }

    private void EndGame()
    {
        gameEnded = true;
        SceneManager.LoadScene(sceneData.rewardIndex);
    }
}
