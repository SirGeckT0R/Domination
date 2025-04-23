using Assets.Scripts.MainMenu.UI;
using UnityEngine;

namespace Assets.Scripts.Map.PlayerInput
{
    public class PlayerInputManager : MonoBehaviour
    {
        private Camera _camera;
        private CountyUI _currentlyActiveCountyUI;
        private PauseMenu _pauseMenu;

        [SerializeField] private LayerMask _countyMask;
        [SerializeField] private LayerMask _uiMask;

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            //if (Input.GetKeyDown(KeyCode.Escape))
            //{
            //    if (GameIsPaused)
            //    {
            //        Resume();
            //    }
            //    else
            //    {
            //        Pause();
            //    }

            //    return;
            //}

            if (Input.GetMouseButtonDown(0))
            {
                HandleCountyUIInteraction();
            }
        }

        private void HandleCountyUIInteraction()
        {
            var ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var hit, Mathf.Infinity, _countyMask | _uiMask))
            {
                if (1 << hit.transform.gameObject.layer == _uiMask)
                {
                    return;
                }

                ActivateCountyUI(hit);

                return;
            }

            if (_currentlyActiveCountyUI != null)
            {
                _currentlyActiveCountyUI.Activate(false);
            }
        }

        private void ActivateCountyUI(RaycastHit hit)
        {   
            var detectedCountyUI = hit.collider.GetComponent<CountyUI>();
            if (_currentlyActiveCountyUI != null && _currentlyActiveCountyUI != detectedCountyUI)
            {
                _currentlyActiveCountyUI.Activate(false);
            }

            _currentlyActiveCountyUI = detectedCountyUI;
            _currentlyActiveCountyUI?.Activate(true);
        }
    }
}
