using UnityEngine;

public class SomPasso : MonoBehaviour
{
    MovimentoPlayer movimentoPlayer;
    InimigoTipo1 inimigo;

    int tipo;

    Som somPasso;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (gameObject.CompareTag("Player"))
        {
            tipo = 0;   // Player
            movimentoPlayer = GetComponent<MovimentoPlayer>();
        }
        else
        {
            tipo = 1;   // Inimigo
            inimigo = GetComponent<InimigoTipo1>();
        }
            
    }

    public void TocaSomPasso(Transform perna)
    {
        float range;
        if (tipo == 0)
        {
            if (movimentoPlayer.GetSneaking())
                range = movimentoPlayer.playerData.rangeSomSneak;
            else if (movimentoPlayer.GetCorrendo())
                range = movimentoPlayer.playerData.rangeSomRun;
            else 
                range = movimentoPlayer.playerData.rangeSomWalk;

            somPasso = new Som(perna.position, range);

            somPasso.tipoSom = Som.TipoSom.Desconhecido;
        }

        else    // Fazer logica dos passos do inimigo depois
        {
            range = 5f;

            somPasso = new Som(perna.position, range);

            somPasso.tipoSom = Som.TipoSom.Aliado;
        }

        GameSonsManager.PropagaSom(somPasso);

        //Debug.Log("Passo com som de range " + range);
        
    }

    
}
