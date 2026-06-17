using System;
using UnityEngine;

public class CaudaIK : MonoBehaviour
{
    [SerializeField] Transform corpo; 
    [SerializeField] Transform caudaTarget;  
    [SerializeField] Transform caudaPosPadrao;
    [SerializeField] float distanciaMax;    // distancia máxima do corpo que pode estar
    [SerializeField] float distanciaMinPosPadrao;    // distancia minima do corpo que pode estar
    [SerializeField] float velMovTarget = 1f;

    float distancia;
    Vector3 direcao;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direcaoPadraoRelativa = caudaPosPadrao.position - corpo.position;
        Vector3 direcaoTargetRelativa = caudaTarget.position - corpo.position;

        float distanciaTarget = Vector3.Distance(caudaTarget.position, caudaPosPadrao.position);

        // Interpola esfericamente a direção atual em direção à direção padrão
        Vector3 novaDirecaoRelativa = Vector3.Slerp(
            direcaoTargetRelativa, 
            direcaoPadraoRelativa, 
            velMovTarget * Time.deltaTime
        );

        if (distanciaTarget > distanciaMinPosPadrao)
        {
            caudaTarget.position = corpo.position + novaDirecaoRelativa;
        }

        distancia = Vector3.Distance(caudaTarget.position, corpo.position);
        direcao = caudaTarget.position - corpo.position; // Direção do corpo PARA a cauda

        if (distancia > distanciaMax)
        {
            caudaTarget.position = corpo.position + direcao.normalized * distanciaMax;
        }

    }


}
