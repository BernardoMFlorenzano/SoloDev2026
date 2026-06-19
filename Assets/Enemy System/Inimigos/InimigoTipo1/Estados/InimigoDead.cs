using UnityEngine;

public class InimigoDead : EstadoBase<InimigoTipo1>
{
    public InimigoDead(InimigoTipo1 contextoAtual) : base(contextoAtual) { }



    // Ações ao entrar no estado
    public override void EntraEstado()
    {
        Debug.Log("Entrou em estado" + this);
        Debug.Log("AI Ai Ai ai o dinossaro me modeu");
        
    }

    // Ações de update
    public override void UpdateEstado()
    {
        
    }

    // Ações de Fixed update
    public override void FixedUpdateEstado()
    {
        
    }

    // Troca de estado
    public override void ChecaTransicoes()
    {
        
    }

    // Ações ao sair do estado
    public override void SaiDeEstado()
    {
        
    }
}
