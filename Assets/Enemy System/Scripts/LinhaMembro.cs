using System.Collections.Generic;
using UnityEngine;

public class LinhaMembro : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public List<Transform> pontos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.positionCount = pontos.Count;
    }

    // Update is called once per frame
    void Update()
    {
        if (pontos.Count > 1)
        {
            for (int i = 0; i < pontos.Count; i++)
            {
                lineRenderer.SetPosition(i, pontos[i].position);
            }
        }
    }
}
