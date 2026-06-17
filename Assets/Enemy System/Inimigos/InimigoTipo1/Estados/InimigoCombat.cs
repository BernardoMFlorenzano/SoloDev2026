using UnityEngine;

public class InimigoCombat : EstadoBase<InimigoTipo1>
{
    public InimigoCombat(InimigoTipo1 contextoAtual) : base(contextoAtual) { }

    Vector2 direcao;
    Vector2 destinoAtual;



    // Ações ao entrar no estado
    public override void EntraEstado()
    {
        Debug.Log("Entrou em estado" + this);

        ctx.TrocaModoArma(1);   // Mira
        
    }

    // Ações de update
    public override void UpdateEstado()
    {
        ctx.Agent.nextPosition = ctx.transform.position;    // Sincroniza a posiçao interna do agente com a real

        if (Vector2.Distance(ctx.transform.position, ctx.Player.position) < ctx.inimigoData.distanciaMinPlayer*0.75)
            destinoAtual = (Vector2)ctx.transform.position - direcao;

        ctx.Agent.SetDestination(destinoAtual);  // Calcula o caminho
    }

    // Ações de Fixed update
    public override void FixedUpdateEstado()
    {
        direcao = ctx.Player.position - ctx.transform.position;
        ctx.DirVisao.OlhaParaDirecao(direcao.normalized, ctx.inimigoData.velRotacaoVisao);

        if (Vector2.Distance(ctx.transform.position, ctx.Player.position) < ctx.inimigoData.distanciaMinPlayer*0.75)
        {
            Vector3 velocidade3D = ctx.Agent.desiredVelocity;
            Vector2 direcaoMov = new Vector2(velocidade3D.x, velocidade3D.y);

            direcaoMov.Normalize();

            ctx.Rb.AddForce(direcaoMov * ctx.Rb.mass * ctx.inimigoData.velInimigoWalk * ctx.velMult);
        }
        
    }

    // Troca de estado
    public override void ChecaTransicoes()
    {
        if (!ctx.Fov.ChecaVisao() || Vector2.Distance(ctx.transform.position, ctx.Player.position) > ctx.inimigoData.distanciaMinPlayer)    // Se não esta vendo player ou esta muito longe 
        {
            ctx.TrocaEstado(ctx.EstadoChase);
            return;
        }
    }

    // Ações ao sair do estado
    public override void SaiDeEstado()
    {
        ctx.TrocaModoArma(0);   // para de Mirar
    }
}
