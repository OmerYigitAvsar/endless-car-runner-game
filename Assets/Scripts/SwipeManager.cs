using UnityEngine;

[DefaultExecutionOrder(-100)]
public class SwipeManager : MonoBehaviour
{
    public static bool tap
    {
        get;
        private set;
    }

    public static bool swipeLeft
    {
        get;
        private set;
    }

    public static bool swipeRight
    {
        get;
        private set;
    }

    public static bool swipeUp
    {
        get;
        private set;
    }

    public static bool swipeDown
    {
        get;
        private set;
    }

    [SerializeField, Range(0.02f, 0.25f)]
    private float swipeThresholdPercent = 0.08f;

    private bool isDragging;

    private Vector2 startTouch;
    private Vector2 swipeDelta;

    private float SwipeThreshold
    {
        get
        {
            float screenReference =
                Mathf.Min(
                    Screen.width,
                    Screen.height
                );

            return Mathf.Max(
                50f,
                screenReference *
                swipeThresholdPercent
            );
        }
    }

    private void Update()
    {
        ResetFrameFlags();

#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
        HandleMouseInput();
#else
        HandleTouchInput();
#endif

        if (!isDragging)
            return;

        if (swipeDelta.magnitude >= SwipeThreshold)
        {
            EvaluateSwipe(swipeDelta);
            EndDrag();
        }
    }

    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            BeginDrag(
                Input.mousePosition
            );
        }
        else if (
            Input.GetMouseButton(0) &&
            isDragging
        )
        {
            swipeDelta =
                (Vector2)Input.mousePosition -
                startTouch;
        }
        else if (
            Input.GetMouseButtonUp(0) &&
            isDragging
        )
        {
            FinishDrag(
                Input.mousePosition
            );
        }
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount <= 0)
            return;

        Touch touch = Input.GetTouch(0);

        switch (touch.phase)
        {
            case TouchPhase.Began:

                BeginDrag(touch.position);

                break;

            case TouchPhase.Moved:
            case TouchPhase.Stationary:

                if (isDragging)
                {
                    swipeDelta =
                        touch.position -
                        startTouch;
                }

                break;

            case TouchPhase.Ended:
            case TouchPhase.Canceled:

                if (isDragging)
                {
                    FinishDrag(
                        touch.position
                    );
                }

                break;
        }
    }

    private void BeginDrag(Vector2 position)
    {
        tap = true;
        isDragging = true;

        startTouch = position;
        swipeDelta = Vector2.zero;
    }

    private void FinishDrag(Vector2 endPosition)
    {
        swipeDelta =
            endPosition -
            startTouch;

        if (swipeDelta.magnitude >= SwipeThreshold)
        {
            EvaluateSwipe(
                swipeDelta
            );
        }

        EndDrag();
    }

    private void EvaluateSwipe(Vector2 delta)
    {
        float x = delta.x;
        float y = delta.y;

        if (Mathf.Abs(x) > Mathf.Abs(y))
        {
            if (x < 0f)
                swipeLeft = true;
            else
                swipeRight = true;
        }
        else
        {
            if (y < 0f)
                swipeDown = true;
            else
                swipeUp = true;
        }
    }

    private void EndDrag()
    {
        startTouch = Vector2.zero;
        swipeDelta = Vector2.zero;

        isDragging = false;
    }

    private static void ResetFrameFlags()
    {
        tap = false;

        swipeLeft = false;
        swipeRight = false;
        swipeUp = false;
        swipeDown = false;
    }

    public Vector2 SwipeDelta =>
        swipeDelta;

    public bool SwipeLeft =>
        swipeLeft;

    public bool SwipeRight =>
        swipeRight;

    public bool SwipeUp =>
        swipeUp;

    public bool SwipeDown =>
        swipeDown;
}