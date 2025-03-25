using Assets.Scripts.Map.Managers;
using Assets.Scripts.Map.Players;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public class PlayerIcon : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Button _createPactButton;
    [SerializeField] private LayerMask _playerIcon;

    public Player Player { get; set; }

    private UIManager _uiManager;

    [Inject]
    public void Construct(UIManager uiManager)
    {
        _uiManager = uiManager;
    }

    private void Awake()
    {
        _createPactButton.onClick.AddListener(HandleHUDInteraction);
    }

    private void HandleHUDInteraction() => _uiManager.HandleSendPactInteraction(Player);

    public void OnPointerClick(PointerEventData eventData)
    {
        _createPactButton.gameObject.SetActive(true);
    }
}
