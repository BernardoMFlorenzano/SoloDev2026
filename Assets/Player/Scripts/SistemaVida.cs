using System;
using UnityEngine;

public class SistemaVida : MonoBehaviour
{
    public static event Action OnDeathPlayer;
    public PlayerData playerData;
    public bool morto = false;

    float vidaAtual;

    void OnEnable()
    {
        InimigoArma.OnHitPlayer += RecebeDano;
    }

    void OnDisable()
    {
        InimigoArma.OnHitPlayer -= RecebeDano;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        vidaAtual = playerData.maxVida;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RecebeDano(float dano)
    {
        vidaAtual -= dano;

        if (vidaAtual <= 0 && !morto)
        {
            MataPlayer();
        }
    }

    void MataPlayer()
    {
        Debug.Log("Player Morreu!");
        morto = true;
        OnDeathPlayer?.Invoke();
    }
}
