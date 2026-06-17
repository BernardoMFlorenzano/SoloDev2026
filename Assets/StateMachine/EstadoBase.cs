using UnityEngine;

public abstract class EstadoBase<T> where T : MaquinaEstados
{
    protected T ctx;    // Variavel que guarda a maquina de estados especifica

    public EstadoBase(T contextoAtual)
    {
        ctx = contextoAtual;
    }

    public abstract void EntraEstado();         // Ações ao entrar no estado
    public abstract void UpdateEstado();        // Ações de update do estado
    public abstract void FixedUpdateEstado();   // Ações de fixed update do estado
    public abstract void SaiDeEstado();         // Ações ao sair do estado
    public abstract void ChecaTransicoes();     // Verificação se troca de estado
}

