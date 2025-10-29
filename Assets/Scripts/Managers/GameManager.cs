using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game Settings")]
    [SerializeField] private float matchDuration = 120f; // two minutes in seconds
    [SerializeField] private float gravity = -9.81f;
    [HideInInspector] public float shootForce = 0f;
    [HideInInspector] public bool gameEnded;
    public bool isBackboardBonusActive = false;
    private float timer;

    [Header("Players")]
    public MatchPlayer player;
    public MatchPlayer opponent;

    [Header("References")]
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private SceneData sceneData;
    public ScoreData scoreData;
    public MeshCollider ringCollider;


    [Header("Aim Transforms")]
    public Transform ringAimTransform;
    public Transform backboardAimTransform;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    private void Start()
    {
        Physics.gravity = new Vector3(0, gravity, 0);
        timer = matchDuration;
        StartCoroutine(TimerRoutine());

        InitializeMatchPlayer(player);
        InitializeMatchPlayer(opponent);

        StartCoroutine(OpponentAIAutoShootRoutine());
    }
    
    private IEnumerator TimerRoutine()
    {
        WaitForSeconds delay = new WaitForSeconds(1);
        while (timer > 0)
        {
            timer -= 1f;
            UIManager.Instance.UpdateTimer(timer);
            yield return delay;
        }
        EndGame();
    }

    private void InitializeMatchPlayer(MatchPlayer matchPlayer)
    {
        matchPlayer.ball.owner = matchPlayer;
        matchPlayer.canShoot = true;
    }
    
    private IEnumerator OpponentAIAutoShootRoutine()
    {
        while (!gameEnded)
        {
            // Wait for a random time based on difficulty
            float wait = Random.Range(DifficultyManager.Instance.shootIntervalMin, DifficultyManager.Instance.shootIntervalMax);
            yield return new WaitForSeconds(wait);

            if (opponent.canShoot)
            {
                float chance = Random.value;
                if (chance <= DifficultyManager.Instance.accuracy)
                {
                    opponent.ball.aimForBackboard = GameManager.Instance.isBackboardBonusActive;
                    opponent.ball.shootAngle += Random.Range(-DifficultyManager.Instance.aimError * 10f, DifficultyManager.Instance.aimError * 10f);
                }
                else
                {
                    // if miss, add a larger random error
                    opponent.ball.shootAngle += Random.Range(-15f, 15f);
                    opponent.ball.aimForBackboard = false;
                }

                opponent.ball.ShootBall();
                opponent.canShoot = false;
            }
        }
    }

    public void RefreshMatch(MatchPlayer matchPlayer)
    {
        if (gameEnded) return;
        
        MovePlayerToNextPosition(matchPlayer);

        // Reset input and camera for player 
        if (matchPlayer.playerType == MatchPlayer.PlayerType.Player)
        {
            InputManager.Instance.hasInputEnded = false;
            UIManager.Instance.ResetInputBar();
            shootForce = 0f;
            SetupCamera(false);
        }

        RespawnBall(matchPlayer);
        matchPlayer.canShoot = true;
    }
    
    private void MovePlayerToNextPosition(MatchPlayer playerToMove)
    {
        Transform nextPos = ShootingPositionsManager.Instance.GetNextPosition();
        playerToMove.transform.position = nextPos.position;

        Vector3 playerToRing = ringAimTransform.position - playerToMove.transform.position;
        playerToRing.y = 0;
        playerToMove.transform.rotation = Quaternion.LookRotation(playerToRing);
    }
    public void ScorePerfectShot(MatchPlayer matchPlayer)
    {
        UpdateScore(matchPlayer, scoreData.perfectShotScore);
    }
    
    public void SetupCamera(bool followBall)
    {
        if (followBall)
            cameraController.StartFollowingBall(player.ballTransform);
        else
            cameraController.StopFollowingBall();
    }

    private void RespawnBall(MatchPlayer matchPlayer)
    {
        if (matchPlayer.ball != null)
            Destroy(matchPlayer.ball.gameObject);

        GameObject newBall = InstantiateBall(matchPlayer.ballSpawnPoint.position);
        matchPlayer.ball = newBall.GetComponent<Ball>();
        matchPlayer.ballTransform = matchPlayer.ball.transform;
        matchPlayer.ball.owner = matchPlayer;
    }

    private GameObject InstantiateBall(Vector3 position)
    {
        return Instantiate(ballPrefab, position, Quaternion.identity);
    }
    
    public void ScoreShot(MatchPlayer matchPlayer)
    {
        UpdateScore(matchPlayer, scoreData.normalShotScore);
    }

    public void ScoreBackboardShot(MatchPlayer matchPlayer) 
    {
        UpdateScore(matchPlayer, scoreData.currentBonusScore);
    }

    private void UpdateScore(MatchPlayer matchPlayer, int amount)
    {
        matchPlayer.score += amount;
        UIManager.Instance.UpdateScore(matchPlayer.score, matchPlayer);
        if (matchPlayer == player)
        {
            UIManager.Instance.ShowScoreFlyer(amount, matchPlayer.ballTransform.position);
        }
        
    }
    
    public void EnableBackboardBonus()
    {
        isBackboardBonusActive = true;
        player.ball.aimForBackboard = true;
        opponent.ball.aimForBackboard = true;
    }
    
    public void DisableBackboardBonus()
    {
        isBackboardBonusActive = false;
        player.ball.aimForBackboard = false;
        opponent.ball.aimForBackboard = false;
    }
    private void EndGame()
    {
        gameEnded = true;
        SceneManager.LoadScene(sceneData.rewardIndex);
    }

}
