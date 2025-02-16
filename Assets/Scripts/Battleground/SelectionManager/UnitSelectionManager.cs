using Helpers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitSelectionManager : MonoBehaviour
{
    private ICollection<GameObject> _allUnits = new List<GameObject>();
    private ICollection<GameObject> _selectedUnits = new List<GameObject>();

    private Camera _camera;

    [Header("Layers")]
    [SerializeField] private LayerMask _clickable;
    [SerializeField] private LayerMask _ground;
    [SerializeField] private LayerMask _attackable;

    [Header("Visual Effects")]
    [SerializeField] private GameObject _groundMarker;

    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            var ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, _clickable))
            {
                if(Input.GetKey(KeyCode.LeftShift))
                {
                    MultiSelect(hit.collider.gameObject);
                }
                else
                {
                    SelectByClicking(hit.collider.gameObject);
                }
            }
            else
            {
                DeselectAll();
            } 
        }
        
        if (Input.GetMouseButtonDown(1) && _selectedUnits.Count > 0)
        {
            RaycastHit hit;
            var ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, _ground))
            {
                _groundMarker.transform.position = hit.point;

                _groundMarker.SetActive(false);
                _groundMarker.SetActive(true);
            }
        }

        CursorSelector();
    }

    private void CursorSelector()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, Mathf.Infinity, _clickable))
        {
            CursorManager.Instance.SetMarkerType(CursorManager.CursorType.Selectable);
        } 
        else if(Physics.Raycast(ray, out hit, Mathf.Infinity, _attackable) && _selectedUnits.Count > 0 && AtLeastOneOffensiveUnit(_selectedUnits))
        {
            CursorManager.Instance.SetMarkerType(CursorManager.CursorType.Attackable);
        }
        else if (Physics.Raycast(ray, out hit, Mathf.Infinity, _ground) && _selectedUnits.Count > 0)
        {
            CursorManager.Instance.SetMarkerType(CursorManager.CursorType.Walkable);
        }
        else
        {
            CursorManager.Instance.SetMarkerType(CursorManager.CursorType.None);
        }
    }

    private bool AtLeastOneOffensiveUnit(ICollection<GameObject> selectedUnits)
    {
        foreach(var unit in selectedUnits)
        {
            if (unit != null && unit.GetComponent<AttackController>())
            {
                return true;
            }
        }

        return false;
    }

    private void MultiSelect(GameObject unit)
    {
        if (_selectedUnits.Contains(unit))
        {
            _selectedUnits.Remove(unit);
            SelectUnit(unit, false);
        }
        else
        {
            _selectedUnits.Add(unit);
            SelectUnit(unit, true);
        }
    }

    private void SelectByClicking(GameObject unit)
    {
        DeselectAll();

        _selectedUnits.Add(unit);

        SelectUnit(unit, true);
    }

    public void DragSelect(GameObject unit)
    {
        if (!_selectedUnits.Contains(unit))
        {
            _selectedUnits.Add(unit);
            SelectUnit(unit, true);
        }
    }

    private void SelectUnit(GameObject unit, bool isSelected)
    {
        TriggerSelectionIndicator(unit, isSelected);
        unit.GetComponent<Unit>().IsSelected = isSelected;
    }

    public void DeselectAll()
    {
        foreach (var unit in _selectedUnits)
        {
            SelectUnit(unit, false);
        }

        _selectedUnits.Clear();
    }

    private void TriggerSelectionIndicator(GameObject unit, bool isVisible)
    {
        unit.transform.Find("Indicator").gameObject.SetActive(isVisible);
    }

    public void AddUnit(GameObject unit)
    {
        _allUnits.Add(unit);
    }

    public void RemoveUnit(GameObject unit)
    {
        _allUnits.Remove(unit);
    }

    public IEnumerable<GameObject> GetSelectableUnits()
    {
        return _allUnits.Where(unit => LayerMaskHelper.IsInLayerMask(unit.layer, _clickable));
    }
}
