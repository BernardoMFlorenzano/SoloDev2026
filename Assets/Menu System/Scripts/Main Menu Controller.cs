using UnityEngine;

namespace MenuSystem
{
    public class MainMenuController : MonoBehaviour
    {
        [Header("Sections")]
        [SerializeField] private ConfigSection configSection;
        [SerializeField] private ControlsSection controlsSection;
        [SerializeField] private CreditsSection creditsSection;

        public void StartNewGame()
        {
            Debug.Log("Começando novo jogo!");
        }

        public void ExitGame()
        {
            Application.Quit();
        }

        public void OpenConfigSection() { configSection.Activate(null); }
        public void CloseConfigSection() { configSection.Deactivate(); }

        public void OpenControlsSection() { controlsSection.Activate(null); }
        public void CloseControlsSection() { controlsSection.Deactivate(); }

        public void OpenCreditsSection() { creditsSection.Activate(null); }
        public void CloseCreditsSection() { creditsSection.Deactivate(); }
    }
}