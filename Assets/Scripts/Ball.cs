using System;
using System.Collections;
using System.Numerics;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;
    using Vector2 = UnityEngine.Vector2;
[RequireComponent(typeof(Rigidbody))]
public class Ball : MonoBehaviour
{
    [Header("Testing Parameters")]
    [SerializeField] private float shootAngle = 45f;
    [SerializeField] private bool aimForBackboard = false;
    
    [Header("Randomness")]
    [SerializeField] private float randomForcePercentage = 0.1f;

    private Rigidbody m_RigidBody;
    private Vector3 m_AppliedVelocity;
    private bool m_HitRing = false;
    private bool m_BallShot = false;
    private bool m_Scored = false;
    void Awake()
    {
        m_RigidBody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        // Preventing ball from falling down
        m_RigidBody.useGravity = false;
        
    }
    void Update()
    {
        if (InputManager.Instance.isInputDown && !m_BallShot)
        {
            m_RigidBody.useGravity = true;
            Vector3 ringAimTransform = GameManager.Instance.ringAimTransform.position;
            Vector3 backboardAimTransform = GameManager.Instance.backboardAimTransform.position;
            Vector3 targetPos = aimForBackboard ? backboardAimTransform : ringAimTransform;
            ShootBall(targetPos);
        }
    }
    
    void FixedUpdate()
    {
        if (!m_BallShot || m_Scored) return;

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
        
        // Only score if ball is falling
        if (m_RigidBody.velocity.y >= 0) return;

        if (horizontalDistance < ringRadius && gameObject.transform.position.y <= ringCenter.y)
        {
            if (m_HitRing)
            {
                m_Scored = true;
                GameManager.Instance.ScoreShot();
            }
            else if (!m_HitRing && transform.position.y <= ringCenter.y)
            {
                m_Scored = true;
                GameManager.Instance.ScorePerfectShot();
            }
        }
    }

    private void ShootBall(Vector3 targetPos)
    {
        m_BallShot = true;
        m_Scored = false;
        CalculateShootVelocity(targetPos);
        GameManager.Instance.SetupCamera(true);
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
        
        // Adding a small randomness to make the shots more fun
        float randomFactor = Random.Range(0, randomForcePercentage);
        velocity *= (1f + randomFactor);
        
        m_RigidBody.velocity = velocity;
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.collider.CompareTag("Floor"))
        {
            m_Scored = false;
            GameManager.Instance.RespawnBall();
            GameManager.Instance.SetupCamera(false);
        } else if (other.collider.CompareTag("Ring"))
        {
            m_HitRing = true;
        }
    }

}
