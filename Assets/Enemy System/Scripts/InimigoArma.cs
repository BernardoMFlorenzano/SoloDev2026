using System.Collections.Generic;
using UnityEngine;

public class InimigoArma : MonoBehaviour
{
    public Vector3 posPadraoRepouso;
    public Vector3 rotPadraoRepouso;
    public Vector3 posPadraoMirando;
    public Vector3 rotPadraoMirando;

    public List<Transform> maosPos;   // Locais que targets das mãos vão
    

    float modo = 0; // Repouso 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.localPosition = posPadraoRepouso;
        transform.localEulerAngles = rotPadraoRepouso;
    }

    // Update is called once per frame
    void Update()
    {
        
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

}
