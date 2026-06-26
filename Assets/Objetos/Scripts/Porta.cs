using Unity.VisualScripting;
using UnityEngine;

public class Porta : MonoBehaviour, IInteragivel
{
    [SerializeField] GameObject portaFisica;
    [SerializeField] int tipoObjeto = 0;
    [SerializeField] GameObject objetoInputPrefab;
    [Header("Som")]
    [SerializeField] float rangeSom = 5f;
    [SerializeField] int tipoSom = 0;
    Som somTocado;
    bool podeInteragir = true;
    bool aberto = false;  
    GameObject objetoInputInst = null;

    void Start()
    {

        AbreFechaPorta();
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

    public bool GetEstado()
    {
        return aberto;
    }

    void MudaEstado()
    {
        aberto = !aberto;

        AbreFechaPorta();

        TocaSom();            
    }

    void AbreFechaPorta()
    {
        if (aberto)
        {
            portaFisica.SetActive(false);
        }
        else if (!aberto)
        {
            portaFisica.SetActive(true);
        }
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
