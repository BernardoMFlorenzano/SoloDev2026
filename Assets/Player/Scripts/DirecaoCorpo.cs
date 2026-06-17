using UnityEngine;
using UnityEngine.InputSystem;

public class DirecaoCorpo : MonoBehaviour
{
    public int modo = 0;    // Modo 0: segue mouse, Modo 1: segue direcao do movimento
    public bool ativo = true;
    [SerializeField] Transform corpo;
    public Transform targetCorpo;
    public float distanciaTargetCorpoMax = 1.5f;
    public float distanciaTargetCorpoMin = 0f;


    MovimentoPlayer movimentoPlayer;
    Vector3 mousePos; 
    float distanciaTarget;
    Vector3 rotacao;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        movimentoPlayer = GetComponent<MovimentoPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!ativo)
        {
            return;
        }
        
        if (modo == 0)
        {
            mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.value);
            mousePos.z = 0f;
            distanciaTarget = Vector2.Distance(corpo.position, mousePos);

            rotacao = mousePos - corpo.position;

            if (distanciaTarget < distanciaTargetCorpoMax && distanciaTarget > distanciaTargetCorpoMin)
            {
                targetCorpo.position = mousePos;
            }
            else if (distanciaTarget < distanciaTargetCorpoMin)
            {
                targetCorpo.position = corpo.position + rotacao.normalized * distanciaTargetCorpoMin;
            }
            else if (distanciaTarget > distanciaTargetCorpoMax)
            {
                targetCorpo.position = corpo.position + rotacao.normalized * distanciaTargetCorpoMax;
            }
        }
        
        else if (modo == 1)
        {
            if (movimentoPlayer.RetornaDirecaoInput() != Vector2.zero)
                rotacao = movimentoPlayer.RetornaDirecaoInput();
                
            targetCorpo.position = corpo.position + rotacao.normalized * distanciaTargetCorpoMax;
        }
        
    }

    public Vector3 RetornaDirecaoMouse()
    {
        return rotacao.normalized;
    }
}
