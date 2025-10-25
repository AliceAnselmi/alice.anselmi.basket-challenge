using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BallHandler : MonoBehaviour
{
    [SerializeField] private Transform ringAimTransform;
    [SerializeField] private Transform backboardAimTransform;
    
    [Header("Testing Parameters")]
    [SerializeField] private float shootAngle = 45f;
    [SerializeField] private bool aimForBackboard = false;
    
    [Header("Randomness")]
    [SerializeField] private float randomForcePercentage = 0.05f;

    private Rigidbody m_RigidBody;
    private Vector3 m_AppliedVelocity;
    private bool ballShot= false;
    private bool hitBackboard = false;
    private bool hitRing = false;
    private bool scored = false;
    
    private void Awake()
    {
        m_RigidBody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        // Preventing ball from falling down
        m_RigidBody.useGravity = false;
        
    }
    private void Update()
    {
        if (InputManager.Instance.isInputDown && !ballShot)
        {
            m_RigidBody.useGravity = true;
            Vector3 targetPos = aimForBackboard ? backboardAimTransform.position : ringAimTransform.position;
            ShootBall(targetPos);
        }
    }

    private void ShootBall(Vector3 targetPos)
    {
        ballShot = true;
        Vector3 velocity = CalculateShootVelocity(transform.position, targetPos, shootAngle);
        m_RigidBody.velocity = velocity;

        hitBackboard = false;
        hitRing = false;
        scored = false;
    }
    
    private Vector3 CalculateShootVelocity(Vector3 startPos, Vector3 targetPos, float shootAngle)
    {
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
        float randomFactor = Random.Range(-randomForcePercentage, randomForcePercentage);
        velocity *= (1f + randomFactor);
        
        return velocity;
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Backboard"))
            hitBackboard = true;

        if (collision.collider.CompareTag("Ring"))
            hitRing = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ring") && !scored)
        {
            if (aimForBackboard && !hitBackboard)
            {
                Debug.Log("Invalid shot, aimed for backboard but didn't hit it");
                return;
            }

            scored = true;

            if (hitBackboard)
                Debug.Log("Hit backboard");
            else if (hitRing)
                Debug.Log("Hit ring");
            else
                Debug.Log("Perfect Shot!");
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (!scored && collision.collider.CompareTag("Ring"))
            Debug.Log("Hit ring but ball bounced back");
    }
}
