using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PernasHandler : MonoBehaviour
{
    [SerializeField] List<Transform> pernasTargets = new List<Transform>();
    [SerializeField] List<Transform> pernasPosPadrao = new List<Transform>();

    [SerializeField] float distanciaMax = 0.5f;
    [SerializeField] float distanciaIdeal = 0.1f;
    [SerializeField] float tamanhoPasso = 0.25f;
    [SerializeField] float tempoResetaPernas = 0.5f;    // Tempo que as pernas voltam pra posição padrão quando parado
    [SerializeField] float delayPasso = 0.5f;   // Tempo que não pode dar passo apos um passo

    //public bool andando = true;
    public bool correndo = false;
    public bool stealth = false;

    public bool pernasAtivas = true;
    public bool podeDarPasso = true;

    SomPasso somPasso;

    private Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        somPasso = GetComponent<SomPasso>();

        // Inicia corrotina de cada perna
        for (int i = 0; i < pernasTargets.Count; i++)
        {
            StartCoroutine(HandlerPosPerna(pernasTargets[i], pernasPosPadrao[i]));
        }

        
    }

    float Distancia(Transform perna, Transform posPadrao)
    {
        return Vector3.Distance(perna.position, posPadrao.position);
    }

    void MexePerna(Transform perna, Transform posPadrao, Vector3? direcaoMod = null)
    {
        if (!podeDarPasso && pernasAtivas)
            return;

        if (rb && direcaoMod == null)
        {
            perna.position = posPadrao.position + (Vector3)rb.linearVelocity * tamanhoPasso;
        }
        else if (direcaoMod != null)
        {
            perna.position = posPadrao.position + (Vector3)direcaoMod;
        }
        else
        {
            perna.position = posPadrao.position + ((posPadrao.position-perna.position).normalized * tamanhoPasso);
        }

        if (pernasAtivas)
        {
            podeDarPasso = false;
            StartCoroutine(DelayPasso());
        }
        
        if (somPasso)
        {
            somPasso.TocaSomPasso(perna);
        }
        
        //Debug.Log((perna.position-posPadrao.position).normalized);
    }

    void ResetaPerna(Transform perna, Transform posPadrao)
    {
        perna.position = posPadrao.position;
    }

    IEnumerator HandlerPosPerna(Transform perna, Transform posPadrao)
    {
        float cronometroReset = 0f;
        bool contandoTempo = false;

        while (true)
        {
            if (!pernasAtivas)
                yield return new WaitUntil(() => pernasAtivas == true);


            if (Distancia(perna, posPadrao) > distanciaMax)
            {
                MexePerna(perna, posPadrao);
                contandoTempo = false;
                cronometroReset = 0f;
            }
            else if (Distancia(perna, posPadrao) > distanciaIdeal && rb.linearVelocity.magnitude < 0.1f)
            {
                if (!contandoTempo)
                {
                    contandoTempo = true;
                    cronometroReset = 0f;
                }

                cronometroReset += Time.deltaTime;

                if (cronometroReset >= tempoResetaPernas)
                {
                    ResetaPerna(perna, posPadrao);
                    contandoTempo = false;
                    cronometroReset = 0f;
                }
            }
            else
            {
                contandoTempo = false;
                cronometroReset = 0f;
            }

            yield return null;
        }
    }

    IEnumerator DelayPasso()
    {
        yield return new WaitForSeconds(delayPasso);
        podeDarPasso = true;
    }

    public void ForcarPasso(Vector3 direcaoMod)
    {
        for (int i = 0; i < pernasTargets.Count; i++)
        {
            MexePerna(pernasTargets[i], pernasPosPadrao[i], direcaoMod);
        }
    }
}