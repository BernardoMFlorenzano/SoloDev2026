using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ObjetoTeste2 : MonoBehaviour, IInteragivel
{
    [SerializeField] int tipoObjeto = 3;
    [SerializeField] GameObject objetoInputPrefab;
    bool podeInteragir = true;
    bool jogavel = false;    // Estado false: objeto no chão, Estado true: na mão do player
    GameObject objetoInputInst = null;

    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    public bool PodeInteragir()
    {
        return podeInteragir;
    }

    public void Interagir()
    {
        if (!podeInteragir) 
            return;

        EscondeInput();

        if (jogavel)
        {
            podeInteragir = false;
            rb.bodyType = RigidbodyType2D.Dynamic;
            StartCoroutine(ResetaInteracao());
        }
        else
        {
            jogavel = true;
        }

    }

    IEnumerator ResetaInteracao()
    {
        yield return new WaitForSeconds(0.5f);  // delay inicial para objeto estar interagivel novamente
        yield return new WaitUntil(() => rb.linearVelocity.magnitude < 0.1f || rb.IsSleeping());

        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        jogavel = false;
        podeInteragir = true;
    }

    public void MostraInput()
    {
        if (podeInteragir && objetoInputInst == null && !jogavel)
        {
            objetoInputInst = Instantiate(objetoInputPrefab, transform.position + Vector3.up, Quaternion.identity);
        }
    }

    public void EscondeInput()
    {
        if (objetoInputInst != null)
        {
            Destroy(objetoInputInst);
            objetoInputInst = null;
        }
    }

    public int GetTipo()
    {
        return tipoObjeto;
    }
}
