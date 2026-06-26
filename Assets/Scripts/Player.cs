using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector3 startingPosition;
    private Quaternion startingRotation;

    private CameraFollow cameraFollow;

    public bool playing { get; private set; }
    private InputManager inputManager;
    
    private void Awake()
    {
        playing = false;
        cameraFollow = Camera.main.GetComponent<CameraFollow>();
        rb = GetComponent<Rigidbody2D>();
        startingPosition = transform.position;
        startingRotation = transform.rotation;
        inputManager = InputManager.Instance;
    }

    private void OnEnable()
    {
        inputManager.OnPressPlay += StartGame;
    }

    private void OnDisable()
    {
        inputManager.OnPressPlay -= StartGame;
    }

    private void StartGame()
    {
        playing = !playing;
        if (playing)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.interpolation = RigidbodyInterpolation2D.Interpolate;
            cameraFollow.enabled = true;
        }
        else
        {
            rb.bodyType = RigidbodyType2D.Static;
            transform.position = startingPosition;
            transform.rotation = startingRotation;
            cameraFollow.CenterOnTarget();
            cameraFollow.enabled = false;
        }
    }

}
