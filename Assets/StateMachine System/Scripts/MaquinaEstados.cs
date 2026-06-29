using UnityEngine;

// Definição genêrica de MaquinaEstados
public abstract class MaquinaEstados : MonoBehaviour
{

}

// Definição para possibilitar a criação de classes filhas de forma segura
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

        estadoAtual = novoEstado;
        estadoAtual.EntraEstado();
    }
}