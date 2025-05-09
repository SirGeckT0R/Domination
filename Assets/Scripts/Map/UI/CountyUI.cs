using Assets.Scripts.Map.Counties;
using Assets.Scripts.Map.Managers;
using Assets.Scripts.Map.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class CountyUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _countyName;
    [SerializeField] private GameObject _ownerUI;
    [SerializeField] private GameObject _otherUI;

    [SerializeField] private Button _economicUpgradeButton;
    [SerializeField] private Button _militaryUpgradeButton;
    [SerializeField] private Button _attackButton;

    private County _county;
    private UIManager _uiManager;

    [Inject]
    public void Construct(UIManager uiManager)
    {
        _uiManager = uiManager;
    }

    private void Awake()
    {
        _county = GetComponent<County>();

        _economicUpgradeButton.onClick.AddListener(() => HandleButtonPressed(CountyInteractionType.EconomicUpgrade));
        _militaryUpgradeButton.onClick.AddListener(() => HandleButtonPressed(CountyInteractionType.MilitaryUpgrade));
        _attackButton.onClick.AddListener(() => HandleButtonPressed(CountyInteractionType.Attack));
    }

    public void HandleButtonPressed(CountyInteractionType interactionType)
    {
        var interactionInfo = new CountyInteractionInfo(_county, interactionType);
        _uiManager.HandleCountyInteraction(interactionInfo);
    }

    public void Activate(bool shouldActivate)
    {
        var isOwner = _uiManager.CurrentPlayer.Id == _county.BelongsTo;
        if (isOwner)
        {
            _otherUI.SetActive(false);
            _ownerUI.SetActive(shouldActivate);
        }
        else
        {
            _ownerUI.SetActive(false);
            _otherUI.SetActive(shouldActivate);
        }

        _countyName.text = shouldActivate ? _county.Name : string.Empty;
    }
}
