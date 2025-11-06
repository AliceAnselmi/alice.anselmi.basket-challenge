using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SwipeFeedback : MonoBehaviour
{
    [SerializeField] private TrailRenderer trail;
    [SerializeField] private Camera camera;
    [SerializeField] private float distanceFromCamera = 1f; // added for adjustments if needed
    private bool m_IsActive = false;

    private void Awake()
    {
        if (camera == null)
            camera = Camera.main;
        trail.emitting = false;
    }

    private void Update()
    {
        if ((Input.GetMouseButton(0) || Input.touchCount > 0 ) && GameManager.Instance.player.canShoot)
        {
            trail.emitting = true;        
            Vector2 screenPos = InputManager.Instance.inputPosition;
            transform.position = camera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, distanceFromCamera));
        }
        else
        {
            trail.emitting = false;
        }
    }

}