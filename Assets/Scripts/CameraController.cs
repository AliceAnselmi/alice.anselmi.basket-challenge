using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Reference Transforms")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform ringTransform;
    [SerializeField] private Transform ballTransform;

    [Header("Camera Settings")]
    [SerializeField] private Vector3 defaultOffset = new Vector3(0, 2f, -6f);
    [SerializeField] private Vector3 followOffset = new Vector3(0, 1.5f, -5f);
    [SerializeField] private float followSmoothness = 4f;
    [SerializeField] private float rotationSmoothness = 5f;

    private bool m_FollowingBall = false;
    private Vector3 m_Velocity;

    void LateUpdate() 
    {
        // Using late update to make sure that the camera updates after the ball has respawned to new location
        if (m_FollowingBall && ballTransform != null) // When ball is respawning, ballTransform can be null
        {
            FollowBall();
        }
        else
        {
            FocusOnRing();
        }
    }

    private void FocusOnRing()
    {
        // Before player shoots: focus on the ring 
        Vector3 targetPosition = playerTransform.position + playerTransform.TransformDirection(defaultOffset);
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref m_Velocity, 1f / followSmoothness);

        Quaternion targetRotation = Quaternion.LookRotation(ringTransform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSmoothness * Time.deltaTime);
    }

    private void FollowBall()
    {
        // After player shoots: follow the ball
        Vector3 targetPosition = ballTransform.position + followOffset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSmoothness * Time.deltaTime);

        Quaternion targetRotation = Quaternion.LookRotation(ringTransform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSmoothness * Time.deltaTime);
    }

    public void StartFollowingBall(Transform newBallTransform)
    {
        ballTransform = newBallTransform;
        StartCoroutine(SetFollowBallAfterDelay(0.1f, true));
    }

    public void StopFollowingBall()
    {
        StartCoroutine(SetFollowBallAfterDelay(0.1f, false));
    }
    
    private IEnumerator SetFollowBallAfterDelay(float delay, bool follow)
    {
        yield return new WaitForSeconds(delay);
        m_FollowingBall = follow;
    }
}
