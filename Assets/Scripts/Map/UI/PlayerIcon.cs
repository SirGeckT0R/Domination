using Assets.Scripts.Map.Players;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerIcon : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Button _createPactButton;
    [SerializeField] private LayerMask _playerIcon;

    public Player Player { get; set; }

    private void Awake()
    { 
        _createPactButton.onClick.AddListener(() => HandleHUDInteraction(Player));
    }

    private void HandleHUDInteraction(Player player)
    {
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _createPactButton.gameObject.SetActive(true);
    }
}
