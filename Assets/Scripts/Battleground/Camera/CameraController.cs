using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;

    [Header("General")]
    [SerializeField] private Transform _cameraTransform;
    public Transform followTransform;
    private Vector3 _newPosition;
    private Vector3 _dragStartPosition;
    private Vector3 _dragCurrentPosition;

    [Header("Optional Functionality")]
    [SerializeField] private bool _moveWithKeyboard;
    [SerializeField] private bool _moveWithEdgeScrolling;
    [SerializeField] private bool _moveWithMouseDrag;

    [Header("Keyboard Movement")]
    [SerializeField] private float _fastSpeed = 0.05f;
    [SerializeField] private float _normalSpeed = 0.01f;
    [SerializeField] private float _movementSensitivity = 1f;
    private float movementSpeed;

    [Header("Edge Scrolling Movement")]
    [SerializeField] private float _edgeSize = 50f;
    [SerializeField] private bool _isCursorSet = false;
    [SerializeField] private Texture2D _cursorArrowUp;
    [SerializeField] private Texture2D _cursorArrowDown;
    [SerializeField] private Texture2D _cursorArrowLeft;
    [SerializeField] private Texture2D _cursorArrowRight;

    private CursorArrow currentCursor = CursorArrow.DEFAULT;
    enum CursorArrow
    {
        UP,
        DOWN,
        LEFT,
        RIGHT,
        DEFAULT
    }

    private void Start()
    {
        Instance = this;

        _newPosition = transform.position;

        movementSpeed = _normalSpeed;
    }

    private void Update()
    {
        if (followTransform != null)
        {
            transform.position = followTransform.position;
        }

        else
        {
            HandleCameraMovement();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            followTransform = null;
        }

        if (Input.GetMouseButton(2))
        {
            transform.Rotate(new Vector3(Input.GetAxis("Mouse Y") * _fastSpeed, -Input.GetAxis("Mouse X") * _fastSpeed, 0));
            var X = transform.rotation.eulerAngles.x;
            var Y = transform.rotation.eulerAngles.y;
            transform.rotation = Quaternion.Euler(X, Y, 0);
        }
    }

    private void HandleCameraMovement()
    {
        if (_moveWithMouseDrag)
        {
            HandleMouseDragInput();
        }

        if (_moveWithKeyboard)
        {
            if (Input.GetKey(KeyCode.LeftCommand))
            {
                movementSpeed = _fastSpeed;
            }
            else
            {
                movementSpeed = _normalSpeed;
            }

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                _newPosition += (transform.forward * movementSpeed);
            }
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                _newPosition += (transform.forward * -movementSpeed);
            }
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                _newPosition += (transform.right * movementSpeed);
            }
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                _newPosition += (transform.right * -movementSpeed);
            }
        }

        if (_moveWithEdgeScrolling)
        {
            if (Input.mousePosition.x > Screen.width - _edgeSize)
            {
                _newPosition += (transform.right * movementSpeed);
                ChangeCursor(CursorArrow.RIGHT);
                _isCursorSet = true;
            }

            else if (Input.mousePosition.x < _edgeSize)
            {
                _newPosition += (transform.right * -movementSpeed);
                ChangeCursor(CursorArrow.LEFT);
                _isCursorSet = true;
            }

            else if (Input.mousePosition.y > Screen.height - _edgeSize)
            {
                _newPosition += (transform.forward * movementSpeed);
                ChangeCursor(CursorArrow.UP);
                _isCursorSet = true;
            }

            else if (Input.mousePosition.y < _edgeSize)
            {
                _newPosition += (transform.forward * -movementSpeed);
                ChangeCursor(CursorArrow.DOWN);
                _isCursorSet = true;
            }
            else
            {
                if (_isCursorSet)
                {
                    ChangeCursor(CursorArrow.DEFAULT);
                    _isCursorSet = false;
                }
            }
        }

        transform.position = Vector3.Lerp(transform.position, _newPosition, Time.deltaTime * _movementSensitivity);

        Cursor.lockState = CursorLockMode.Confined;
    }

    private void ChangeCursor(CursorArrow newCursor)
    {
        if (currentCursor != newCursor)
        {
            switch (newCursor)
            {
                case CursorArrow.UP:
                    Cursor.SetCursor(_cursorArrowUp, Vector2.zero, CursorMode.Auto);
                    break;
                case CursorArrow.DOWN:
                    Cursor.SetCursor(_cursorArrowDown, new Vector2(_cursorArrowDown.width, _cursorArrowDown.height), CursorMode.Auto); // So the Cursor will stay inside view
                    break;
                case CursorArrow.LEFT:
                    Cursor.SetCursor(_cursorArrowLeft, Vector2.zero, CursorMode.Auto);
                    break;
                case CursorArrow.RIGHT:
                    Cursor.SetCursor(_cursorArrowRight, new Vector2(_cursorArrowRight.width, _cursorArrowRight.height), CursorMode.Auto); // So the Cursor will stay inside view
                    break;
                case CursorArrow.DEFAULT:
                    Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                    break;
            }

            currentCursor = newCursor;
        }
    }

    private void HandleMouseDragInput()
    {
        if (Input.GetMouseButtonDown(2) && EventSystem.current.IsPointerOverGameObject() == false)
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (plane.Raycast(ray, out float entry))
            {
                _dragStartPosition = ray.GetPoint(entry);
            }
        }
        if (Input.GetMouseButton(2) && EventSystem.current.IsPointerOverGameObject() == false)
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float entry;

            if (plane.Raycast(ray, out entry))
            {
                _dragCurrentPosition = ray.GetPoint(entry);

                _newPosition = transform.position + _dragStartPosition - _dragCurrentPosition;
            }
        }
    }
}
