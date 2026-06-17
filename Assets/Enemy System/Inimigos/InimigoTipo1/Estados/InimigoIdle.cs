using UnityEngine;

public class InimigoIdle : EstadoBase<InimigoTipo1>
{
    public InimigoIdle(InimigoTipo1 contextoAtual) : base(contextoAtual) { }

    



    // Ações ao entrar no estado
    public override void EntraEstado()
    {
        Debug.Log("Entrou em estado" + this);
        
    }

    // Ações de update
    public override void UpdateEstado()
    {
        ctx.Agent.nextPosition = ctx.transform.position;    // Sincroniza a posiçao interna do agente com a real

        if (ctx.emGuarda)
        {
            ctx.Agent.SetDestination(ctx.PosPadrao);  // Calcula o caminho
        }
    }

    // Ações de Fixed update
    public override void FixedUpdateEstado()
    {
        if (ctx.emGuarda)
        {
            if (Vector2.Distance(ctx.PosPadrao, ctx.transform.position) > 0.1f)
            {
                Vector3 velocidade3D = ctx.Agent.desiredVelocity;
                Vector2 direcao = new Vector2(velocidade3D.x, velocidade3D.y);
            
                direcao.Normalize(); // Retorna direcao

                ctx.Rb.AddForce(direcao * ctx.Rb.mass * ctx.inimigoData.velInimigoWalk * ctx.velMult);

                ctx.DirVisao.OlhaParaDirecao(direcao, ctx.inimigoData.velRotacaoVisao);
            }
            else
            {
                
                ctx.transform.position = ctx.PosPadrao;
                ctx.Rb.linearVelocity = Vector2.zero;

                //Debug.Log("chegou posicao default " + ctx.transform.position);

                ctx.DirVisao.OlhaParaDirecao(ctx.DirPadrao, ctx.inimigoData.velRotacaoVisao);
            }
            
        }
        
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

        else if (ctx.rotaPatrulha.Count > 0)
        {
            ctx.TrocaEstado(ctx.EstadoPatrol);
            return;
        }
    }

    // Ações ao sair do estado
    public override void SaiDeEstado()
    {
        
    }
}
