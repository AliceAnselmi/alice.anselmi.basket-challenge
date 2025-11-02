using UnityEngine;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;
using Vector2 = UnityEngine.Vector2;

[RequireComponent(typeof(Rigidbody))]
public class Ball : MonoBehaviour
{
    public float shootAngle = 45f;
    public bool aimForBackboard = false;
    public MatchPlayer owner;
    
    private Rigidbody m_RigidBody;
    private Vector3 m_AppliedVelocity;
    private bool m_HitRing = false;
    private bool m_HitBackboard = false;
    private bool m_Scored = false;

    private Transform ringTarget;
    private Transform backboardTarget;

    void Awake()
    {
        m_RigidBody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        // Preventing ball from falling down
        m_RigidBody.useGravity = false;
        ringTarget = GameManager.Instance.ringAimTransform;
        backboardTarget = GameManager.Instance.backboardAimTransform;
    }

    private void Update()
    {
        if (owner.playerType == MatchPlayer.PlayerType.Player
            && owner.canShoot
            && InputManager.Instance.hasInputEnded
            && !GameManager.Instance.gameEnded
            )
        {
            ShootBall();
        }
    }

    void FixedUpdate()
    {
        if(m_Scored)
            return;
        
        Vector3 ringCenter = GameManager.Instance.ringAimTransform.position;
        MeshCollider ringCollider = GameManager.Instance.ringCollider;
        float ringRadius = 0;
        if (ringCollider != null)
        {
            ringRadius = ringCollider.bounds.extents.x;
        }

        // Horizontal distance from ball to ring center
        Vector2 ringCenterXZ = new Vector2(ringCenter.x, ringCenter.z);
        Vector2 ballXZ = new Vector2(transform.position.x, transform.position.z);
        float horizontalDistance = Vector2.Distance(ringCenterXZ, ballXZ);
        float verticalDistance = Mathf.Abs(transform.position.y - ringCenter.y);
        
        // Only score if ball is falling
        if (m_RigidBody.velocity.y >= 0) return;

        if (horizontalDistance < ringRadius && transform.position.y <= ringCenter.y)
        {
            if (GameManager.Instance.isBackboardBonusActive && m_HitBackboard) // Backboard bonus active and hit backboard
            {
                m_Scored = true;
                GameManager.Instance.ScoreBackboardShot(owner);
            }
            else if (m_HitRing) // Hit the ring
            {
                m_Scored = true;
                GameManager.Instance.ScoreShot(owner);
            }
            else if (verticalDistance < 0.1f) // Perfect shot (additional check on vertical distance for better accuracy)
            {
                m_Scored = true;
                GameManager.Instance.ScorePerfectShot(owner);
            }
        }
    }

    public void ShootBall()
    {
        m_RigidBody.useGravity = true;
        Vector3 targetPos = aimForBackboard ? backboardTarget.position : ringTarget.position;
        CalculateShootVelocity(targetPos);
        if (!owner.isAI)
        {
            GameManager.Instance.SetupCamera(true);
            owner.canShoot = false;
        }
    }
    
    private void CalculateShootVelocity(Vector3 targetPos)
    {
        Vector3 startPos = transform.position;
        float gravity = Mathf.Abs(Physics.gravity.y);
        float shootAngleRad = shootAngle * Mathf.Deg2Rad;

        // Calculate horizontal direction and distance
        Vector3 flattenedTarget = new Vector3(targetPos.x, 0, targetPos.z);
        Vector3 flattenedPos = new Vector3(startPos.x, 0, startPos.z);
        Vector3 horizontalDir = (flattenedTarget - flattenedPos).normalized;
        
        float horizontalDistance = Vector3.Distance(flattenedTarget, flattenedPos);
        float verticalDistance = targetPos.y - startPos.y;

        // From projectile motion equations, solving for velocity
        float tanTheta = Mathf.Tan(shootAngleRad);
        float cosTheta = Mathf.Cos(shootAngleRad);
        
        float denominator = horizontalDistance * tanTheta - verticalDistance;

        // Calculate initial velocity magnitude
        float velocitySquared = (gravity * horizontalDistance * horizontalDistance) / 
                               (2 * cosTheta * cosTheta * denominator);
        
        if (velocitySquared < 0)
        {
            velocitySquared = Mathf.Abs(velocitySquared);
        }
        
        float velocityMagnitude = Mathf.Sqrt(velocitySquared);
        
        // Calculate horizontal and vertical components of velocity
        float horizontalVelocity = velocityMagnitude * cosTheta;
        float verticalVelocity = velocityMagnitude * Mathf.Sin(shootAngleRad);
        
        Vector3 velocity = horizontalDir * horizontalVelocity;
        velocity.y = verticalVelocity;
        
        // Add initial force depending on swipe
        float launchForce = Mathf.Lerp(-.1f, .2f,GameManager.Instance.shootForce);
        velocity *= 1f + launchForce;

        m_RigidBody.velocity = velocity;
    }

    private void OnCollisionEnter(Collision other)
    {
        switch(other.collider.tag)
        {
            case "Floor":
                SoundManager.PlaySound(Sound.HIT_FLOOR);
                GameManager.Instance.SetupNewShot(owner);
                break;
            case "Ring":
                m_HitRing = true;
                break;
            case "Backboard":
                SoundManager.PlaySound(Sound.HIT_BACKBOARD);
                m_HitBackboard = true;
                break;
        }
    }

}
