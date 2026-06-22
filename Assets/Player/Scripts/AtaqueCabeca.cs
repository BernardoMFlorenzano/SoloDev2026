using UnityEngine;

public class AtaqueCabeca : MonoBehaviour
{
    MovimentoPlayer movimentoPlayer;
    InimigoTipo1 inimigoScript;

    void Start()
    {
        movimentoPlayer = GetComponentInParent<MovimentoPlayer>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Inimigo") && movimentoPlayer.movAtivo)
        {
            inimigoScript = collision.GetComponent<InimigoTipo1>();

            if (!inimigoScript.morto)
            {
                inimigoScript.RecebeDano(1);
                movimentoPlayer.AtaqueMordida();        
            }

            
        }
    }
}
