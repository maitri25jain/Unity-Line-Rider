using UnityEngine;

public class InputManager : MonoBehaviour
{

    private static InputManager _instance;

    public static InputManager Instance
    {
        get { return _instance; }
    }
    
    #region Events
        public delegate void StartDraw();
        public event StartDraw OnStartDraw;
        public delegate void EndDraw();
        public event StartDraw OnEndDraw;
        public delegate void StartErase();
        public event StartDraw OnStartErase;
        public delegate void EndErase();
        public event StartDraw OnEndErase;
        public delegate void PressedPlay();
        public event PressedPlay OnPressPlay;
    #endregion

    private MouseControls mouseControls;
    private PlayerControls playerControls;


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        
        mouseControls = new MouseControls();
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        mouseControls.Enable();
        playerControls.Enable();
    }

    private void OnDisable()
    {
        mouseControls.Disable();
        playerControls.Disable();
    }

    void Start()
    {
        mouseControls.Mouse.Click.started += _ => { if (OnStartDraw != null) OnStartDraw(); };
        mouseControls.Mouse.Click.canceled += _ => { if (OnEndDraw != null) OnEndDraw(); };
        mouseControls.Mouse.Erase.started += _ => { if (OnStartErase != null) OnStartErase(); };
        mouseControls.Mouse.Erase.canceled += _ => { if (OnEndErase != null) OnEndErase(); };

        // Subscribe to player input
        playerControls.Player.Space.performed += _ => { if (OnPressPlay != null) OnPressPlay(); };

        // Confine the cursor to the screen
        Cursor.lockState = CursorLockMode.Confined;
    }

    public float GetZoom()
    {
        return mouseControls.Mouse.Zoom.ReadValue<float>();
    }

    public Vector2 GetMousePosition()
    {
        return mouseControls.Mouse.Position.ReadValue<Vector2>();
    }

}
