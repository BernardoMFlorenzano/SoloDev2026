using System.Collections.Generic;
using UnityEngine;

public class InimigoBracos : MonoBehaviour
{
    [SerializeField] List<Transform> bracosTargets = new List<Transform>();
    [SerializeField] List<Transform> bracosPosPadrao = new List<Transform>();

    InimigoArma arma;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        arma = GetComponent<InimigoTipo1>().Arma;
    }

    // Update is called once per frame
    void Update()
    {
        if (arma == null)   // Não tiver arma 
        {
            for (int i = 0; i < bracosTargets.Count; i++)
            {
                if (bracosTargets[i].position != bracosPosPadrao[i].position)
                {
                    bracosTargets[i].position = bracosPosPadrao[i].position;
                } 
            }
        } 
        else
        {
            for (int i = 0; i < bracosTargets.Count; i++)
            {
                if (bracosTargets[i].position != arma.maosPos[i].position)
                {
                    bracosTargets[i].position = arma.maosPos[i].position;
                } 
            }
        }
    }
}
