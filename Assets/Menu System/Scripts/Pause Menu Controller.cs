using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MenuSystem
{
    [RequireComponent(typeof(Canvas))]
    public class PauseMenuController : MonoBehaviour
    {
        public static PauseMenuController Instance;

        [Header("Sections")]
        [SerializeField] private PauseSection pauseSection;
        [SerializeField] private ConfigSection configSection;
        [SerializeField] private ControlsSection controlsSection;

        [Header("Elements")]
        [SerializeField] private Button configButton;
        [SerializeField] private Button controlsButton;

        private Canvas canvas;

        void OnDisable()
        {
            
        }

        private void Awake()
        {

            canvas = GetComponent<Canvas>();
            canvas.enabled = false;
            Debug.Log(canvas);
        }

        void Start()
        {
    
        }

        public void TogglePause(bool activate)
        {
            if (activate)
            {
                Time.timeScale = 0;

                canvas.enabled = true;

                OpenPauseSection();
            }
            else
            {
                Time.timeScale = 1;

                canvas.enabled = false;

                CloseConfigSection();
                CloseControlsSection();
                ClosePauseSection();
            }
        }

        public void ReturnToMenu()
        {
            SceneManager.LoadScene(0);

            TogglePause(false);
        }

        public void OpenPauseSection() { pauseSection.Activate(null); }
        public void ClosePauseSection() { pauseSection.Deactivate(); }

        public void OpenConfigSection() { configSection.Activate(configButton.gameObject); }
        public void CloseConfigSection() { configSection.Deactivate(); }

        public void OpenControlsSection() { controlsSection.Activate(controlsButton.gameObject); }
        public void CloseControlsSection() { controlsSection.Deactivate(); }
    }
}