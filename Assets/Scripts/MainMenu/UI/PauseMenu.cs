using UnityEngine.SceneManagement;
using UnityEngine;

namespace Assets.Scripts.MainMenu.UI
{
    public class PauseMenu : MonoBehaviour
    {
        private static bool GameIsPaused = false;

        [SerializeField] private GameObject pauseMenu;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (GameIsPaused)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
            }
        }

        public void Pause()
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
            GameIsPaused = true;
        }

        public void Resume()
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1;
            GameIsPaused = false;
        }

        public void LoadMenu()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(0);
        }
    }
}
