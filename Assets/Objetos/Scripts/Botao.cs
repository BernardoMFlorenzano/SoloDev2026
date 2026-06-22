using Unity.VisualScripting;
using UnityEngine;

public class Botao : MonoBehaviour, IInteragivel
{
    [SerializeField] int tipoObjeto = 0;
    [SerializeField] GameObject objetoInputPrefab;
    [Header("Som")]
    [SerializeField] float rangeSom = 5f;
    [SerializeField] int tipoSom = 0;
    Som somTocado;
    bool podeInteragir = true;
    bool estado = false; 
    //SpriteRenderer spriteRenderer;
    GameObject objetoInputInst = null;

    void Start()
    {
        //spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public bool PodeInteragir()
    {
        return podeInteragir;
    }

    public void Interagir()
    {
        if (!podeInteragir) 
            return;
        
        MudaEstado();
    }

    public void MostraInput()
    {
        if (podeInteragir && objetoInputInst == null)
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

    void MudaEstado()
    {
        estado = !estado;
        
        TocaSom();            

    }

    void TocaSom()
    {
        somTocado = new Som(transform.position, rangeSom);

        if (tipoSom == 0)
            somTocado.tipoSom = Som.TipoSom.Desconhecido;
        else if (tipoSom == 1)
            somTocado.tipoSom = Som.TipoSom.Perigo;

        GameSonsManager.PropagaSom(somTocado);
    }
}
