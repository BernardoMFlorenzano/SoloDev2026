using UnityEngine;

public class AtaqueCabeca : MonoBehaviour
{
    InimigoTipo1 inimigoScript;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Inimigo"))
        {
            inimigoScript = collision.GetComponent<InimigoTipo1>();
            inimigoScript.RecebeDano(1);
        }
    }
}
