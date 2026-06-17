using UnityEngine;

public abstract class MaquinaEstados : MonoBehaviour
{

}

public abstract class MaquinaEstados<T> : MaquinaEstados where T : MaquinaEstados
{
    protected EstadoBase<T> estadoAtual; // Variavel que guarda o estado atual


    // No Update, roda função update do estado e checa se haverá alguma transição
    protected virtual void Update()
    {
        if (estadoAtual != null)
        {
            estadoAtual.UpdateEstado();
            estadoAtual.ChecaTransicoes();
        }
    }

    // No FixedUpdate, roda função fixed update do estado
    protected virtual void FixedUpdate()
    {
        if (estadoAtual != null)
        {
            estadoAtual.FixedUpdateEstado();
        }
    }

    // Troca estado atual para o novo estado
    public void TrocaEstado(EstadoBase<T> novoEstado)
    {
        if (estadoAtual != null)
        {
            estadoAtual.SaiDeEstado();
        }

        //Debug.Log($"[FSM] Trocando de {estadoAtual} para {novoEstado}");

        estadoAtual = novoEstado;
        estadoAtual.EntraEstado();
    }
}