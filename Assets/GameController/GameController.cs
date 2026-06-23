using MenuSystem;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public bool jogoPausado = false;
    public bool inputAtivo = true;
    public bool playerVivo = true;

    [SerializeField] PauseMenuController pauseMenuController;

    

    void OnEnable()
    {
        SistemaVida.OnDeathPlayer += PlayerMorto;
        InputPlayerManager.OnPauseInput += PausaJogo;
    }

    void OnDisable()
    {
        SistemaVida.OnDeathPlayer -= PlayerMorto;
        InputPlayerManager.OnPauseInput -= PausaJogo;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PlayerMorto()
    {
        inputAtivo = false;
        playerVivo = false;
    }

    void PausaJogo()
    {
        if (pauseMenuController)
        {
            if (!jogoPausado)
            {
                pauseMenuController.TogglePause(true);
                jogoPausado = true;
                inputAtivo = false;
            }
            else if (jogoPausado)
            {
                pauseMenuController.TogglePause(false);
                jogoPausado = false;
                inputAtivo = true;
            }
        }
    }
}
