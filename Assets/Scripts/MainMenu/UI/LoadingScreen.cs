using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.MainMenu.UI
{
    public class LoadingScreen : MonoBehaviour
    {
        [SerializeField] private GameObject _loadingScreen;
        [SerializeField] private Image _loadingBar;

        public void LoadScene(int buildIndex)
        {
            StartCoroutine(StartLoading(buildIndex));
        }

        private IEnumerator StartLoading(int buildIndex)
        {
            _loadingScreen.SetActive(true);
            yield return new WaitForSeconds(0.1f);

            var asyncLoad = SceneManager.LoadSceneAsync(buildIndex); 
            asyncLoad.allowSceneActivation = false;

            while (!asyncLoad.isDone)
            {
                var clampedValue = Mathf.Clamp01(asyncLoad.progress / 0.9f);

                _loadingBar.fillAmount = clampedValue;

                if (asyncLoad.progress >= 0.9f)
                {
                    asyncLoad.allowSceneActivation = true;
                }

                yield return null;
            }
        }
    }
}
