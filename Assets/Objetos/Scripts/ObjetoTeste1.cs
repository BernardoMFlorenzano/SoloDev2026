using UnityEngine;

public class ObjetoTeste1 : MonoBehaviour, IInteragivel
{
    [SerializeField] int tipoObjeto = 1;
    [SerializeField] GameObject objetoInputPrefab;
    bool podeInteragir = true;
    //bool estado = false; 
    GameObject objetoInputInst = null;

    void Start()
    {

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
        //MudaEstado();
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
}
