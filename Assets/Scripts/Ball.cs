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
    [SerializeField] private float shootAngle = 45f;
    [SerializeField] private float velocityMultiplier = 1f;
    public bool aimForBackboard = false;
    
    private Rigidbody m_RigidBody;
    private Vector3 m_AppliedVelocity;
    private bool m_HitRing = false;
    private bool m_HitBackboard = false;
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
        if (!GameManager.Instance.gameEnded && InputManager.Instance.hasInputEnded && GameManager.Instance.canShoot)
        {
            Vector3 ringAimTransform = GameManager.Instance.ringAimTransform.position;
            Vector3 backboardAimTransform = GameManager.Instance.backboardAimTransform.position;
            Vector3 targetPos = aimForBackboard ? backboardAimTransform : ringAimTransform;
            ShootBall(targetPos);
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
        
        // Only score if ball is falling
        if (m_RigidBody.velocity.y >= 0) return;

        if (horizontalDistance < ringRadius && transform.position.y <= ringCenter.y)
        {
            if (GameManager.Instance.isBackboardBonusActive && m_HitBackboard) // Backboard bonus active and hit backboard
            {
                m_Scored = true;
                GameManager.Instance.ScoreBackboardShot();
            }
            else if (m_HitRing) // Hit the ring
            {
                m_Scored = true;
                GameManager.Instance.ScoreShot();
            }
            else // Did not hit ring: perfect shot
            {
                m_Scored = true;
                GameManager.Instance.ScorePerfectShot();
            }
        }
    }

    private void ShootBall(Vector3 targetPos)
    {
        GameManager.Instance.canShoot = false;
        m_RigidBody.useGravity = true;
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
        
        // Add initial force depending on swipe
        float launchForce = Mathf.Lerp(0f, .3f,GameManager.Instance.shootForce);
        velocity *= (1f + launchForce) * velocityMultiplier;
        
        m_RigidBody.velocity = velocity;
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.collider.CompareTag("Floor"))
        {
            GameManager.Instance.RefreshMatch();
        } else if (other.collider.CompareTag("Ring"))
        {
            m_HitRing = true;
        } else if (other.collider.CompareTag("Backboard"))
        {
            m_HitBackboard = true;
        }
    }

}
