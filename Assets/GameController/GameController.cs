using UnityEngine;

public class GameController : MonoBehaviour
{
    public bool inputAtivo = true;
    public bool playerVivo = true;

    void OnEnable()
    {
        SistemaVida.OnDeathPlayer += PlayerMorto;
    }

    void OnDisable()
    {
        SistemaVida.OnDeathPlayer -= PlayerMorto;
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
}
