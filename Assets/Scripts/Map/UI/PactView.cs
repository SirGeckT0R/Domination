using Assets.Scripts.Map.AI.Events;
using Assets.Scripts.Map.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PactView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textElement;
    [SerializeField] private Button _acceptButton;
    [SerializeField] private Button _declineButton;

    private CreatePactEvent _pactEvent;

    private UIManager _uiManager;

    [Inject]
    public void Construct(UIManager uiManager)
    {
        _uiManager = uiManager;
    }

    private void Awake()
    {
        _acceptButton.onClick.AddListener(() => HandleButtonClick(true));
        _declineButton.onClick.AddListener(() => HandleButtonClick(false));
    }

    public void UpdateCurrentPact()
    {
        _pactEvent = _uiManager.CurrentPact;
        _textElement.text = $"{_pactEvent.SenderId} have sent you a pact. Do you accept?";
    }

    private void HandleButtonClick(bool isAccepted) => _uiManager.HandlePactResolved(isAccepted);
}
