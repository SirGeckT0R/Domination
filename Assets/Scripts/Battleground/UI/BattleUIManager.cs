using Assets.Scripts.MainMenu.UI;
using TMPro;
using UnityEngine;

public class BattleUIManager : MonoBehaviour
{
    [SerializeField] private GameObject _endOfBattleScreen;
    [SerializeField] private LoadingScreen _loadingScreen;
    private TextMeshProUGUI _endOfBattleText;

    private void Awake()
    {
        _endOfBattleText = _endOfBattleScreen.GetComponentInChildren<TextMeshProUGUI>(includeInactive: true);
    }

    public void ShowEndOfBattleScreen(string text)
    {
        _endOfBattleText.text = text;
        _endOfBattleScreen.SetActive(true);
    }

    public void LoadScene(int buildIndex)
    {
        _loadingScreen.LoadScene(buildIndex);
    }
}
