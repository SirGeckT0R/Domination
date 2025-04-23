using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.MainMenu.UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private LoadingScreen _loadingScreen;

        public void PlayGame()
        {
            _loadingScreen.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
