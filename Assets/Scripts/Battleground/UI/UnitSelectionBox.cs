using UnityEngine;
using Zenject;

public class UnitSelectionBox : MonoBehaviour
{
    private Camera _camera;
    private UnitSelectionManager _selectionManager;

    [SerializeField] private RectTransform _boxVisual;

    private Rect _selectionBox;

    private Vector2 _startPosition;
    private Vector2 _endPosition;

    [Inject]
    public void Construct(UnitSelectionManager unitSelectionManager)
    {
        _selectionManager = unitSelectionManager;
    }

    private void Start()
    {
        _camera = Camera.main;
        _startPosition = Vector2.zero;
        _endPosition = Vector2.zero;
        DrawVisual();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))        
        {
            _startPosition = Input.mousePosition;

            _selectionBox = new Rect();
        }

        if (Input.GetMouseButton(0))
        {
            if(_boxVisual.rect.width > 0 || _boxVisual.rect.height > 0)
            {
                _selectionManager.DeselectAll();
                SelectUnits();
            }

            _endPosition = Input.mousePosition;
            DrawVisual();
            DrawSelection();
        }

        if (Input.GetMouseButtonUp(0))
        {
            SelectUnits();

            _startPosition = Vector2.zero;
            _endPosition = Vector2.zero;
            DrawVisual();
        }
    }

    void DrawVisual()
    {
        Vector2 boxStart = _startPosition;
        Vector2 boxEnd = _endPosition;

        Vector2 boxCenter = (boxStart + boxEnd) / 2;

        _boxVisual.position = boxCenter;

        Vector2 boxSize = new Vector2(Mathf.Abs(boxStart.x - boxEnd.x), Mathf.Abs(boxStart.y - boxEnd.y));

        _boxVisual.sizeDelta = boxSize;
    }

    void DrawSelection()
    {
        if (Input.mousePosition.x < _startPosition.x)
        {
            _selectionBox.xMin = Input.mousePosition.x;
            _selectionBox.xMax = _startPosition.x;
        }
        else
        {
            _selectionBox.xMin = _startPosition.x;
            _selectionBox.xMax = Input.mousePosition.x;
        }


        if (Input.mousePosition.y < _startPosition.y)
        {
            _selectionBox.yMin = Input.mousePosition.y;
            _selectionBox.yMax = _startPosition.y;
        }
        else
        {
            _selectionBox.yMin = _startPosition.y;
            _selectionBox.yMax = Input.mousePosition.y;
        }
    }

    void SelectUnits()
    {
        var selectableUnits = _selectionManager.GetSelectableUnits();

        foreach (var unit in selectableUnits)
        {
            if (_selectionBox.Contains(_camera.WorldToScreenPoint(unit.transform.position)))
            {
                _selectionManager.DragSelect(unit);
            }
        }
    }
}
