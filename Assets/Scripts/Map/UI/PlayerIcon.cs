using Assets.Scripts.Map.Managers;
using Assets.Scripts.Map.Players;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public class PlayerIcon : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject _uiPanel;
    [SerializeField] private LayerMask _playerIcon;
    [SerializeField] private TextMeshProUGUI _warriorsText;
    [SerializeField] private TextMeshProUGUI _moneyText;

    public Player Player { get; set; }

    private UIManager _uiManager;

    [Inject]
    public void Construct(UIManager uiManager)
    {
        _uiManager = uiManager;
    }

    private void Awake()
    {
        var pactButton = _uiPanel.GetComponentInChildren<Button>(includeInactive: true);
        pactButton.onClick.AddListener(HandleHUDInteraction);
    }

    private void HandleHUDInteraction() => _uiManager.HandleSendPactInteraction(Player);

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!_uiPanel.gameObject.activeSelf)
        {
            _moneyText.text = Player.Money.ToString();
            _warriorsText.text = Player.Warriors.ToString();

            _uiPanel.gameObject.SetActive(true);
        }
        else
        {
            _uiPanel.gameObject.SetActive(false);
        }
    }
}
