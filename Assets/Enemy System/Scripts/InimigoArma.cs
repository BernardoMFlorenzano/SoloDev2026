using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InimigoArma : MonoBehaviour
{
    public static event Action<float> OnHitPlayer;

    public Vector3 posPadraoRepouso;
    public Vector3 rotPadraoRepouso;
    public Vector3 posPadraoMirando;
    public Vector3 rotPadraoMirando;

    public List<Transform> maosPos;   // Locais que targets das mãos vão
    public Transform pontaArma;
    public ArmaData armaData;
    public LayerMask targetLayerMask;

    public GameObject bulletTrail;
    BulletTrail trailScript;
    

    float modo = 0; // Repouso 
    bool atirando = false;

    bool delayTiro = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.localPosition = posPadraoRepouso;
        transform.localEulerAngles = rotPadraoRepouso;
    }

    // Update is called once per frame
    void Update()
    {
        if (atirando && !delayTiro)
        {
            Atira();
                
            delayTiro = true;

            StartCoroutine(DelayTiro());
        }
    }

    public void TrocaModo(int novoModo)
    {   
        if (novoModo == modo)
            return;

        if (novoModo == 0)
        {
            modo = 0;

            transform.localPosition = posPadraoRepouso;
            transform.localEulerAngles = rotPadraoRepouso;
        }
        else if (novoModo == 1)
        {
            modo = 1;

            transform.localPosition = posPadraoMirando;
            transform.localEulerAngles = rotPadraoMirando;
        }
        else 
        {
            // novoModo é invalido
        }
    }

    public void SetaAtirando(bool novo)
    {
        atirando = novo;
    }

    IEnumerator DelayTiro()
    {
        yield return new WaitForSeconds(armaData.delayTiro);
        delayTiro = false;
    }

    void Atira()
    {
        var hit = Physics2D.Raycast(pontaArma.position, SpreadArma(transform.up, armaData.spreadMaxTiro), 20f, targetLayerMask);

        var trail = Instantiate(bulletTrail, pontaArma.position, transform.rotation);

        trailScript = trail.GetComponent<BulletTrail>();

        if (hit.collider != null)
        {
            Debug.Log(hit.collider);
            trailScript.SetaTarget(hit.point);

            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                Debug.Log("Player levou tiro");
                OnHitPlayer?.Invoke(armaData.danoArma);
            }
        }
        else
        {
            trailScript.SetaTarget(pontaArma.position + transform.up * armaData.rangeArma);
        }
    }

    Vector2 SpreadArma(Vector2 dirOriginal, float maxSpread)
    {
        float anguloOriginal = Mathf.Atan2(dirOriginal.y, dirOriginal.x);

        float offset = UnityEngine.Random.Range(-maxSpread, maxSpread) * Mathf.Deg2Rad;

        float anguloFinal = anguloOriginal + offset;
        return new Vector2(Mathf.Cos(anguloFinal), Mathf.Sin(anguloFinal));
    }

}
