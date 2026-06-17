using System;
using UnityEngine;

public class InimigoPatrol : EstadoBase<InimigoTipo1>
{
    public InimigoPatrol(InimigoTipo1 contextoAtual) : base(contextoAtual) { }

    Transform destinoAtual = null;
    int destinoAtualIndex = 0;


    // Ações ao entrar no estado
    public override void EntraEstado()
    {
        Debug.Log("Entrou em estado" + this);

        for (int i = 0; i < ctx.rotaPatrulha.Count; i++)
        {
            if (!destinoAtual)
            {
                destinoAtual = ctx.rotaPatrulha[i];
                destinoAtualIndex = i;
            }
            else
            {
                if (Vector2.Distance(destinoAtual.position, ctx.transform.position) > Vector2.Distance(ctx.rotaPatrulha[i].position, ctx.transform.position)) 
                {
                    destinoAtual = ctx.rotaPatrulha[i];
                    destinoAtualIndex = i;
                }
            }
                
        }
    }

    // Ações de update
    public override void UpdateEstado()
    {
        ctx.Agent.nextPosition = ctx.transform.position;    // Sincroniza a posiçao interna do agente com a real

        if (Vector2.Distance(destinoAtual.position, ctx.transform.position) < ctx.inimigoData.distanciaMinPatrolPoint)
        {
            destinoAtualIndex = (destinoAtualIndex + 1) % ctx.rotaPatrulha.Count;
            destinoAtual = ctx.rotaPatrulha[destinoAtualIndex];
        }
            
        ctx.Agent.SetDestination(destinoAtual.position);  // Calcula o caminho
    }

    // Ações de Fixed update
    public override void FixedUpdateEstado()
    {
        if (destinoAtual == null) 
            return;

        Vector3 velocidade3D = ctx.Agent.desiredVelocity;
        Vector2 direcao = new Vector2(velocidade3D.x, velocidade3D.y);
    
        direcao.Normalize(); // Retorna direcao

        ctx.Rb.AddForce(direcao * ctx.Rb.mass * ctx.inimigoData.velInimigoWalk * ctx.velMult);

        ctx.DirVisao.OlhaParaDirecao(direcao, ctx.inimigoData.velRotacaoVisao);
    }

    // Troca de estado
    public override void ChecaTransicoes()
    {
        if (ctx.SomAtual != null)
        {
            if (ctx.SomAtual.tipoSom == Som.TipoSom.Desconhecido || ctx.SomAtual.tipoSom == Som.TipoSom.Perigo)
            {
                ctx.TrocaEstado(ctx.EstadoSurprise);
                return;
            }
        }

        if (ctx.Player != null && ctx.Fov.ChecaVisao())
        {
            ctx.TrocaEstado(ctx.EstadoSurprise);
            return;
        }

        if (destinoAtual == null)
        {
            ctx.TrocaEstado(ctx.EstadoIdle);
            return;
        }
    }

    // Ações ao sair do estado
    public override void SaiDeEstado()
    {
        destinoAtual = null;
        destinoAtualIndex = 0;
    }
}
