using UnityEngine;

public class ExemploEstadoIdle : EstadoBase<InimigoStateMachine>
{
    public ExemploEstadoIdle(InimigoStateMachine contextoAtual) : base(contextoAtual) { }


    public override void EntraEstado()
    {
        ctx.Rb.linearVelocity = Vector2.zero;
    }

    public override void UpdateEstado()
    {
        
    }

    public override void FixedUpdateEstado()
    {
        
    }

    public override void ChecaTransicoes()
    {
        if (Vector2.Distance(ctx.Player.position, ctx.transform.position) <= 5f)
        {
            ctx.TrocaEstado(ctx.EstadoChase);
            return;
        }
    }

    public override void SaiDeEstado()
    {
        
    }
}
