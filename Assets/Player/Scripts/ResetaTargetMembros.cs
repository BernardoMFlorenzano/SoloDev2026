using System.Collections;
using UnityEngine;

public class ResetaTargetMembros : MonoBehaviour
{
    [SerializeField] Transform raizMembro;
    [SerializeField] Transform posicaoPadrao; // Transform no membro que move junto (torso ou quadril)
    [SerializeField] float distanciaMax = 0.5f;
    [SerializeField] float tempoResetaTarget = 0.5f;
    float distancia;
    Vector3 direcao;
    Coroutine resetaPosCor;
    bool resetando = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    void Update()
    {
        distancia = Vector2.Distance(transform.position, raizMembro.position);

        direcao = posicaoPadrao.position - transform.position;

        if (distancia > distanciaMax)
        {
            if (!resetando)
            {
                if (resetaPosCor != null)
                {
                    StopCoroutine(resetaPosCor);
                    resetaPosCor = null;
                }
                resetando = true;
                resetaPosCor = StartCoroutine(ResetaPosicaoTimer(direcao));
            }
        }
    }

    void ResetaPosicao(Vector3 direcaoLoc)
    {
        transform.position = posicaoPadrao.position;
    }

    IEnumerator ResetaPosicaoTimer(Vector3 direcaoLoc)
    {
        yield return new WaitForSeconds(tempoResetaTarget);
        ResetaPosicao(direcaoLoc);
        resetando = false;
    }


}
