using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Jogavel : MonoBehaviour, IInteragivel
{
    [SerializeField] int tipoObjeto = 3;
    [SerializeField] GameObject objetoInputPrefab;
    bool podeInteragir = true;
    bool jogavel = false;    // Estado false: objeto no chão, Estado true: na mão do player
    GameObject objetoInputInst = null;

    [Header("Som")]
    [SerializeField] float rangeSom = 10f;
    [SerializeField] int tipoSom = 0;
    Som somColisao;

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

    void OnCollisionEnter2D(Collision2D collision)
    {
        somColisao = new Som(transform.position, rangeSom);

        if (tipoSom == 0)
            somColisao.tipoSom = Som.TipoSom.Desconhecido;
        else if (tipoSom == 1)
            somColisao.tipoSom = Som.TipoSom.Perigo;

        GameSonsManager.PropagaSom(somColisao);

        Debug.Log("Objeto fez som de colisão");
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
