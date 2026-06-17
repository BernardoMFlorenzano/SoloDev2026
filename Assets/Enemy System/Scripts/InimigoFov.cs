using UnityEngine;
using UnityEngine.Rendering.Universal;

public class InimigoFov : MonoBehaviour
{
    public InimigoData inimigoData;

    [SerializeField] LayerMask targetMask;
    [SerializeField] LayerMask obstaculoMask;

    bool vendoPlayer;
    Light2D inimigoLuz;

    void Start()
    {
        inimigoLuz = GetComponentInChildren<Light2D>();

        if (inimigoLuz != null)
        {
            inimigoLuz.pointLightOuterAngle = inimigoData.anguloVisao;
            inimigoLuz.pointLightOuterRadius = inimigoData.raioVisao;
        }
    }


    public bool ChecaVisao()
    {
        // Primeiro, checa se player esta em range do raio
        Collider2D checaRange = Physics2D.OverlapCircle(transform.position, inimigoData.raioVisao, targetMask);

        if (checaRange != null)
        {
            Transform target = checaRange.transform;
            Vector2 direcao = (target.position - transform.position).normalized;

            // Depois, checa se player esta dentro do angulo do FOV
            if (Vector2.Angle(transform.up, direcao) < inimigoData.anguloVisao / 2) // Supoe que a "frente" padrao do inimigo é pra cima
            {
                float distancia = Vector2.Distance(transform.position, target.position);

                // Por ultimo, faz Raycast para ver se tem algum obstaculo na linha de visão
                if (!Physics2D.Raycast(transform.position, direcao, distancia, obstaculoMask))
                {
                    vendoPlayer = true; // Viu player
                }
                else
                {
                    vendoPlayer = false; // Player atras de obstaculo
                }
            }
            else
            {
                vendoPlayer = false; // Player fora do angulo de visao
            }
        }
        else if (vendoPlayer)
        {
            vendoPlayer = false; // Player fora do raio de visão
        }

        return vendoPlayer;
    }

}
