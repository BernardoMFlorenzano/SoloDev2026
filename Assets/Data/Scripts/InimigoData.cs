using UnityEngine;

[CreateAssetMenu(fileName = "InimigoData", menuName = "Scriptable Objects/InimigoData")]
public class InimigoData : ScriptableObject
{
    public float vidaMax = 1f;
    [Header("Velocidade")]
    public float velInimigoWalk;
    public float velInimigoRun;


    [Header("FOV e Visão")]
    [Range(0, 360)] public float anguloVisao = 120f;
    public float raioVisao = 8f;

    public float velRotacaoVisao = 360f;


    [Header("Checks Distancias")]
    public float distanciaMinPlayer = 1f;
    public float distanciaMinPatrolPoint = 0.5f;

    [Header("Comportamento")]
    public float tempoReacaoSurpreso = 1f;  // Tempo que inimigo leva para voltar a agir depois de ficar surpreso
    public float tempoParaChase = 10f;      // Tempo que persegue player sem ver e escutar nada
    public float tempoDesisteSearch = 20f;  // Tempo que procura por player apos perder ele de vista e não escutar nada

    public float rangeSomAlerta = 30f;      // Range do som de alerta que da a outros inimigos quando avista player

}
