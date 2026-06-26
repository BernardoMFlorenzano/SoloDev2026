using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace MenuSystem
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] protected GameObject firstSelectedObject;
        [Header("Sections")]
        [SerializeField] private ConfigSection configSection;
        [SerializeField] private ControlsSection controlsSection;
        [SerializeField] private CreditsSection creditsSection;

        public void StartNewGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        public void ExitGame()
        {
            Application.Quit();
        }

        public void OpenConfigSection() { configSection.Activate(null); }
        public void CloseConfigSection() 
        { 
            configSection.Deactivate(); 

            EventSystem.current.SetSelectedGameObject(firstSelectedObject);
        }

        public void OpenControlsSection() { controlsSection.Activate(null); }
        public void CloseControlsSection() 
        { 
            controlsSection.Deactivate(); 

            EventSystem.current.SetSelectedGameObject(firstSelectedObject);
        }

        public void OpenCreditsSection() { creditsSection.Activate(null); }
        public void CloseCreditsSection() 
        { 
            creditsSection.Deactivate(); 

            EventSystem.current.SetSelectedGameObject(firstSelectedObject);
        }
    }
}