using UnityEngine;

public class InputManager  : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    public Vector2 inputPosition;
    public bool isInputDown;
    public bool isInputUp;

    private void Awake()
    {
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
    void Update()
    {
        isInputDown = false;
        isInputUp = false;

#if UNITY_EDITOR || UNITY_STANDALONE
        if (Input.GetMouseButtonDown(0))
        {
            isInputDown = true;
            inputPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isInputUp = true;
            inputPosition = Input.mousePosition;
        }

#elif UNITY_IOS || UNITY_ANDROID
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            inputPosition = touch.position;

            if (touch.phase == TouchPhase.Began) {
                isInputDown = true;
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) {
                isInputUp = true;
            }
        }
#endif
    }
}