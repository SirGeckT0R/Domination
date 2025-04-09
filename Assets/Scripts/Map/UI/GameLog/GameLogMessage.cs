using Assets.Scripts.Map.UI.GameLog;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Map.UI
{
    public class GameLogMessage : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textElement;

        public void SetText(MessageDto message)
        {
            _textElement.text = $"{message.Player}: {message.Message}";
        }
    }
}
