using UnityEngine;

public class ExemploEstadoChase : EstadoBase<InimigoStateMachine>
{
    public ExemploEstadoChase(InimigoStateMachine contextoAtual) : base(contextoAtual) { }

    Vector2 direcao;

    public override void EntraEstado()
    {
        
    }

    public override void UpdateEstado()
    {
        direcao = ctx.Player.position - ctx.transform.position;
    }

    public override void FixedUpdateEstado()
    {
        ctx.Rb.linearVelocity = direcao.normalized * ctx.vel;
    }

    public override void ChecaTransicoes()
    {
        if (Vector2.Distance(ctx.Player.position, ctx.transform.position) > 5f)
        {
            ctx.TrocaEstado(ctx.EstadoIdle);
            return;
        }
    }

    public override void SaiDeEstado()
    {
        
    }
}
