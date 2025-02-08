using UnityEngine;

public class Unit : MonoBehaviour
{
    private void Start()
    {
        UnitSelectionManager.Instance.AddUnit(gameObject);
    }

    private void OnDestroy()
    {
        UnitSelectionManager.Instance.RemoveUnit(gameObject);
    }
}
