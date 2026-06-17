using System;
using System.Collections.Generic;
using UnityEngine;

public class CaudaProcedural : MonoBehaviour
{
    public List<Transform> bones = new List<Transform>();
    float[] boneDistancias;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Salva as distancias
        boneDistancias = new float[bones.Count];
        for (int i = 1; i < bones.Count; i++)
        {
            float distanciaBoneAnt = Vector3.Distance(bones[i-1].transform.position, bones[i].transform.position);
            boneDistancias[i] = distanciaBoneAnt;
        }
    }

    Vector3 LimitaDistancia(Transform bone, Transform ancora, float dist)
    {
        Vector3 direcao = (bone.transform.position - ancora.transform.position).normalized;

        return (direcao * dist) + ancora.transform.position;
    }

    float LimitaRotacao(Transform bone, Transform ancora)
    {
        Vector2 direcao = (bone.transform.position - ancora.transform.position).normalized;

        float angulo = Mathf.Atan2(direcao.y, direcao.x) * Mathf.Rad2Deg;

        return angulo;
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 1; i < bones.Count; i++)
        {
            bones[i].transform.SetPositionAndRotation(LimitaDistancia(bones[i], bones[i-1], boneDistancias[i]), Quaternion.Euler(0,0,LimitaRotacao(bones[i], bones[i-1])));
        }
    }
}
