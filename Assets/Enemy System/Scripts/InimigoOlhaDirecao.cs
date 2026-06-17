using UnityEngine;

public class InimigoOlhaDirecao : MonoBehaviour
{
    Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    // Função para alterar direcao que o inimigo olha de forma não instantanea (deve ser rodada no fixedUpddate)
    public void OlhaParaDirecao(Vector3 direcaoAlvo, float velRot)
    {
        float anguloAlvo = Mathf.Atan2(direcaoAlvo.y, direcaoAlvo.x) * Mathf.Rad2Deg - 90f;

        float rotacaoIt = Mathf.MoveTowardsAngle(rb.rotation, anguloAlvo, velRot * Time.fixedDeltaTime);

        rb.MoveRotation(rotacaoIt);
    }
}
