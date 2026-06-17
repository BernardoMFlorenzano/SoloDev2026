using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class BracosHandler : MonoBehaviour
{
    [SerializeField] List<Transform> bracosTargets = new List<Transform>();
    [SerializeField] List<Transform> bracosPosPadrao = new List<Transform>();
    [SerializeField] float tempoResetaInteracao = 0.5f;
    [SerializeField] float forcaJogaObjetos = 5f;

    public bool emRepouso = true;
    public bool interagindo = false;
    public bool segurando = false;

    bool podeInteragir = true;

    Transform bracoAtivo = null;
    Transform posPadraoBracoAtivo = null;
    Transform objetoSegurado = null;
    int tipoObjetoSegurado = 1;     // tipo do objeto que esta sendo segurado

    DirecaoCorpo direcaoCorpo;





    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        direcaoCorpo = GetComponent<DirecaoCorpo>();
    }

    private void OnEnable()
    {
        PlayerInteracao.OnInteracao += EscolheBracoInteracao;
    }
    private void OnDisable()
    {
        PlayerInteracao.OnInteracao -= EscolheBracoInteracao;
    }

    // Update is called once per frame
    void Update()
    {
        if (emRepouso)
        {
            EmRepouso();
        }
        else if (interagindo)
        {
            Interagindo();
        }
        else if (segurando)
        {
            Segurando();
        }
    }


    void EmRepouso()
    {
        for (int i = 0; i < bracosTargets.Count; i++)
        {
            if (bracosTargets[i].position != bracosPosPadrao[i].position)
            {
                bracosTargets[i].position = bracosPosPadrao[i].position;
            } 
        }
        
    }

    // Igual a EmRepouso, excento quanto ao braço que esta sendo usado em uma interação
    void Interagindo()
    {
        for (int i = 0; i < bracosTargets.Count; i++)
        {
            if (bracosTargets[i].position != bracosPosPadrao[i].position && bracosTargets[i] != bracoAtivo)
            {
                bracosTargets[i].position = bracosPosPadrao[i].position;
            } 
        }
    }

    void Segurando()
    {
        for (int i = 0; i < bracosTargets.Count; i++)
        {
            if (bracosTargets[i].position != bracosPosPadrao[i].position && bracosTargets[i] != bracoAtivo)
            {
                bracosTargets[i].position = bracosPosPadrao[i].position;
            } 
            else if (bracosTargets[i].position != bracosPosPadrao[i].position && bracosTargets[i] == bracoAtivo)
            {
                bracosTargets[i].position = bracosPosPadrao[i].position;
            } 
        }
    }

    public void EscolheBracoInteracao(int tipoInteracao, Transform objeto)
    {
        if (!podeInteragir)
            return;


        if (segurando)
        {
            if (tipoObjetoSegurado == 1)
            {
                SoltaObjeto();

                return;
            }
            if (tipoObjetoSegurado == 2)
            {
                JogaObjeto();

                return;
            }
        }
        else if (tipoInteracao < 0) // Cancela interação pois não há objeto pra ser interagido
            return;



        float distancia = 100f;
        float distanciaAux;
        bracoAtivo = bracosTargets[0];
        posPadraoBracoAtivo = bracosPosPadrao[0];

        for (int i = 0; i < bracosTargets.Count; i++)
        {
            distanciaAux = Vector2.Distance(bracosTargets[i].position, objeto.position);
            if (distancia > distanciaAux)
            {
                bracoAtivo = bracosTargets[i];
                posPadraoBracoAtivo = bracosPosPadrao[i];
                distancia = distanciaAux;
            }
        }

        ResetaEstados();        
        interagindo = true;

        bracoAtivo.position = objeto.position;

        podeInteragir = false;

        if (tipoInteracao == 0)
        {
            StartCoroutine(AcabaInteracao());
        }
        else if (tipoInteracao == 1 || tipoInteracao == 2)
        {
            tipoObjetoSegurado = tipoInteracao;
            objetoSegurado = objeto;
            StartCoroutine(SeguraObjeto());
        }
    }

    IEnumerator AcabaInteracao()
    {
        yield return new WaitForSeconds(tempoResetaInteracao);

        ResetaEstados();
        emRepouso = true;
        bracoAtivo = null;
        posPadraoBracoAtivo = null;

        podeInteragir = true;
    }

    IEnumerator SeguraObjeto()
    {
        yield return new WaitForSeconds(tempoResetaInteracao);

        ResetaEstados();
        segurando = true;


        objetoSegurado.SetParent(posPadraoBracoAtivo);
        objetoSegurado.position = posPadraoBracoAtivo.position;

        podeInteragir = true;

        Debug.Log("Segurou");
    }

    void SoltaObjeto()
    {
        objetoSegurado.SetParent(null);

        ResetaObjetoSegurado();
        ResetaEstados();
        emRepouso = true;
        bracoAtivo = null;
        posPadraoBracoAtivo = null;
    }

    void JogaObjeto()
    {
        objetoSegurado.SetParent(null);

        objetoSegurado.GetComponent<IInteragivel>().Interagir();

        objetoSegurado.GetComponent<Rigidbody2D>().AddForce(direcaoCorpo.RetornaDirecaoMouse() * forcaJogaObjetos, ForceMode2D.Impulse);

        ResetaObjetoSegurado();
        ResetaEstados();
        emRepouso = true;
        bracoAtivo = null;
        posPadraoBracoAtivo = null;

        Debug.Log("Jogou");
    }





    void ResetaEstados()
    {
        emRepouso = false;
        interagindo = false;
        segurando = false;
    }

    void ResetaObjetoSegurado()
    {
        objetoSegurado = null;
        tipoObjetoSegurado = 1;
    }
}
