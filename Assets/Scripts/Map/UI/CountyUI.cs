using UnityEngine;

public class CountyUI : MonoBehaviour
{
    private Canvas UICanvas;

    private void Awake()
    {
        UICanvas = GetComponentInChildren<Canvas>(includeInactive: true);
    }

    public void DeactivateUICanvas()
    {
        UICanvas.gameObject.SetActive(false);
    }

    public void ActivateUICanvas()
    {
        UICanvas.gameObject.SetActive(true);
    }
}
