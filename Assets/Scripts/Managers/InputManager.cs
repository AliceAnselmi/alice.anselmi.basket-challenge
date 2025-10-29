using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    
    [HideInInspector] public float incrementalSwipeDelta = 0f;
    [HideInInspector] public bool hasInputEnded = false;
    [HideInInspector] public Vector2 inputPosition;
    private float m_PreviousTouchY = 0f;
    private Vector2 startTouch;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Update()
    {
        if(!GameManager.Instance.player.canShoot)
            return;
#if UNITY_EDITOR || UNITY_STANDALONE
        HandleMouseInput();
#else
        HandleTouchInput();
#endif
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount == 0) return;

        Touch touch = Input.GetTouch(0);

        switch (touch.phase)
        {
            case TouchPhase.Began:
                m_PreviousTouchY = touch.position.y;
                OnInputStart();
                break;
            case TouchPhase.Moved:
                incrementalSwipeDelta = (touch.position.y - m_PreviousTouchY) / Screen.height;
                inputPosition = touch.position;
                OnInputHold();
                m_PreviousTouchY = touch.position.y;
                break;

            case TouchPhase.Ended:
            case TouchPhase.Canceled:
                OnInputEnd();
                break;
        }
    }

    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            m_PreviousTouchY =  Input.mousePosition.y;
            OnInputStart();
        }

        if (Input.GetMouseButton(0))
        {
            inputPosition = Input.mousePosition;
            incrementalSwipeDelta = (Input.mousePosition.y - m_PreviousTouchY) / Screen.height;
            OnInputHold();
            m_PreviousTouchY = Input.mousePosition.y;
        }

        if (Input.GetMouseButtonUp(0))
        {
            OnInputEnd();
        }
    }

    private void OnInputStart()
    {
        GameManager.Instance.shootForce = 0f;
    }

    private void OnInputHold()
    {
        if(incrementalSwipeDelta < 0f) incrementalSwipeDelta = 0f; // Prevent negative swipe
        GameManager.Instance.shootForce += incrementalSwipeDelta;
    }
    
    private void OnInputEnd()
    {
        incrementalSwipeDelta = 0f;
        hasInputEnded = true;
    }
}