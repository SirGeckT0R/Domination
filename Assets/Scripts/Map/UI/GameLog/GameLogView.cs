using Assets.Scripts.Map.UI.GameLog;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Map.UI
{
    public class GameLogView : MonoBehaviour
    {
        [SerializeField] private GameLogMessage _messagePrefab;
        [SerializeField] private ScrollRect _scrollRect;


        public void AddLogMessage(MessageDto message)
        {
            var messageObject = Instantiate(_messagePrefab, transform);
            messageObject.SetText(message);
            
            StartCoroutine(ScrollToBottom());
        }

        IEnumerator ScrollToBottom()
        {
            yield return new WaitForEndOfFrame();
            _scrollRect.gameObject.SetActive(true);
            _scrollRect.verticalNormalizedPosition =0f;
        }
    }
}
