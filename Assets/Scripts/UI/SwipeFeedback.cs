using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SwipeFeedback : MonoBehaviour
{
    [SerializeField] private TrailRenderer trail;
    [SerializeField] private Camera camera;

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
            Vector2 screenPos = InputManager.Instance.inputPosition;
            transform.position = camera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, 5f));
            StartCoroutine(StartTrailAfterDelay(0.01f));
        }
        else
        {
            transform.position = Vector2.zero;
            trail.emitting = false;
        }
    }
    
    private IEnumerator StartTrailAfterDelay(float delay)
    {
        trail.Clear();
        yield return new WaitForSeconds(delay);
        trail.emitting = true;
    }

}