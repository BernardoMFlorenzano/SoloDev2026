using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
public class PlayerData : ScriptableObject
{
    public float velPlayer;

    [Header("Variação no movimento")]
    public float anguloMouseMovimentoMin = 30f; // Angulo minimo que a direçao do mouse e do movimento devem ter de diferença pra considerar movimento pra frente
    public float velMovMult = 1f;
    public float velMovFrontWalk = 1f;
    public float velMovStrafe = 0.75f;
    public float velMovBackward = 0.75f;
    public float velMovRun = 1.5f;
    public float velMovSneak = 0.5f;

    [Header("Lunge")]
    public float velMovLunge;
    public float impulsoLunge = 30f;
    public float tempoLunge = 0.75f;
    public float cooldownLunge = 2f;
}
