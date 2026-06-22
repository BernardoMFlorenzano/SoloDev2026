using UnityEngine;
using UnityEngine.AI;

public class InimigoPanic : EstadoBase<InimigoTipo1>
{
    public InimigoPanic(InimigoTipo1 contextoAtual) : base(contextoAtual) { }

    Vector2 posFoge;
    Vector2 dirFoge;
    Som somFoge;    // Som que vai fugir
    bool paraFuga = false;

    Coroutine corTimer = null;

    // Ações ao entrar no estado
    public override void EntraEstado()
    {
        Debug.Log("Entrou em estado" + this);
        
        if (ctx.SomAtual != null)
        {
            somFoge = ctx.SomAtual;
            ctx.ResetaSom();
            
        }
    }

    // Ações de update
    public override void UpdateEstado()
    {
        ctx.Agent.nextPosition = ctx.transform.position;    // Sincroniza a posiçao interna do agente com a real

        if (ctx.SomAtual != null)
        {
            somFoge = ctx.SomAtual;
            ctx.ResetaSom();
        }

        CalculaCaminho();

        ctx.Agent.SetDestination(posFoge);  // Calcula o caminho

        //CheckTimerFuga();   
    }

    void CalculaCaminho()
    {
        NavMeshPath path = new();

        if (ctx.Fov.ChecaVisao())   // Se está vendo player atualmente
        {
            posFoge = ctx.Player.position + (ctx.Player.position - ctx.transform.position).normalized * 5;

            // Atualiza Memoria
            ctx.dirPlayerMemoria = ctx.RbPlayer.linearVelocity.normalized;
            ctx.posPlayerMemoria = ctx.Player.position;
        }

        else if (somFoge != null)
        {
            posFoge = somFoge.pos + (somFoge.pos - ctx.transform.position).normalized * 5;

            if (Vector2.Distance(posFoge, ctx.transform.position) < ctx.inimigoData.distanciaMinPatrolPoint)
            {
                somFoge = null; // Chegou em ponto de fuga
            }
        }

        else
        {
            
            if (posFoge == Vector2.zero)
            {
               posFoge = ctx.posPlayerMemoria - ctx.dirPlayerMemoria*5;
            }

            else if (Vector2.Distance(posFoge, ctx.transform.position) < ctx.inimigoData.distanciaMinPatrolPoint)
            {
                
                if (ctx.Agent.CalculatePath(posFoge - ctx.dirPlayerMemoria*2, path))
                {
                    if (path.status == NavMeshPathStatus.PathComplete)
                    {
                        posFoge -= ctx.dirPlayerMemoria*2;
                    }
                }
                
            }
            

        }


        
    }

    // Ações de Fixed update
    public override void FixedUpdateEstado()
    {
        if (Vector2.Distance(posFoge, ctx.transform.position) > ctx.inimigoData.distanciaMinPatrolPoint)
        {
            Vector3 velocidade3D = ctx.Agent.desiredVelocity;
            Vector2 direcao = new Vector2(velocidade3D.x, velocidade3D.y);
        
            direcao.Normalize(); // Retorna direcao

            ctx.Rb.AddForce(direcao * ctx.Rb.mass * ctx.inimigoData.velInimigoRun * ctx.velMult);

            
            ctx.DirVisao.OlhaParaDirecao(direcao, ctx.inimigoData.velRotacaoVisao);
        }
    }

    // Troca de estado
    public override void ChecaTransicoes()
    {
        if (ctx.comportamento != 0)
        {
            ctx.TrocaEstado(ctx.EstadoChase);
            return;
        }
    }

    // Ações ao sair do estado
    public override void SaiDeEstado()
    {
        paraFuga = false;
        posFoge = Vector2.zero;
        dirFoge = Vector2.zero;
        somFoge = null;
    }
}
