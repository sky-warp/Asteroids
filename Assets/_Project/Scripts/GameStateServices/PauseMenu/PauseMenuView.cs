using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.GameStateServices.PauseMenu
{
    public class PauseMenuView : MonoBehaviour
    {
        [field: SerializeField] public Button ResumeButton { get; private set; }
        [field: SerializeField] public Button ExitButton { get; private set; }

        [SerializeField] private GameObject _pauseMenu;

        private void Start()
        {
            HidePauseMenu();
        }

        public void ShowPauseMenu()
        {
            _pauseMenu.SetActive(true);
        }

        public void HidePauseMenu()
        {
            _pauseMenu.SetActive(false);
        }
    }
}