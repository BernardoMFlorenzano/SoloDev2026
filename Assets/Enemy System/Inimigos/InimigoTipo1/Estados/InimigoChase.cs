using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class InimigoChase : EstadoBase<InimigoTipo1>
{
    public InimigoChase(InimigoTipo1 contextoAtual) : base(contextoAtual) { }

    //private Transform playerLocal;
    Vector2 posChase;
    Som somPersegue;    // Som que vai perseguir
    bool paraChase = false;

    Coroutine corTimer = null;


    // Ações ao entrar no estado
    public override void EntraEstado()
    {
        Debug.Log("Entrou em estado" + this);

        if (ctx.SomAtual != null)
        {
            somPersegue = ctx.SomAtual;
            ctx.ResetaSom();
            
        }
        
    }

    // Ações de update
    public override void UpdateEstado()
    {
        ctx.Agent.nextPosition = ctx.transform.position;    // Sincroniza a posiçao interna do agente com a real

        if (ctx.SomAtual != null)
        {
            
            somPersegue = ctx.SomAtual;
            ctx.ResetaSom();
        }

        CalculaCaminho();

        ctx.Agent.SetDestination(posChase);  // Calcula o caminho

        CheckTimerChase();   
    }

    void CalculaCaminho()
    {
        NavMeshPath path = new();

        if (ctx.Fov.ChecaVisao())   // Se está vendo player atualmente
        {
            posChase = ctx.Player.position;

            // Atualiza Memoria
            ctx.dirPlayerMemoria = ctx.RbPlayer.linearVelocity.normalized;
            ctx.posPlayerMemoria = ctx.Player.position;
        }

        else if (somPersegue != null)
        {
            posChase = somPersegue.pos;

            if (Vector2.Distance(posChase, ctx.transform.position) < ctx.inimigoData.distanciaMinPatrolPoint)
            {
                somPersegue = null; // Chegou em fonte do som
            }
        }

        else
        {
            
            if (posChase == Vector2.zero)
            {
               posChase = ctx.posPlayerMemoria + ctx.dirPlayerMemoria;
            }

            else if (Vector2.Distance(posChase, ctx.transform.position) < ctx.inimigoData.distanciaMinPatrolPoint)
            {
                
                if (ctx.Agent.CalculatePath(posChase + ctx.dirPlayerMemoria*2, path))
                {
                    if (path.status == NavMeshPathStatus.PathComplete)
                    {
                        posChase += ctx.dirPlayerMemoria*2;
                    }
                }
                
            }
            

        }


        
    }

    // Ações de Fixed update
    public override void FixedUpdateEstado()
    {
        if (Vector2.Distance(posChase, ctx.transform.position) > ctx.inimigoData.distanciaMinPatrolPoint)
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
        if (ctx.Fov.ChecaVisao() && Vector2.Distance(ctx.transform.position, ctx.Player.position) <= ctx.inimigoData.distanciaMinPlayer)    // Se esta perto o suficiente e vendo player 
        {
            ctx.TrocaEstado(ctx.EstadoCombat);
            return;
        }

        if (paraChase)
        {
            ctx.TrocaEstado(ctx.EstadoSearch);
            return;
        }

    
    }

    // Ações ao sair do estado
    public override void SaiDeEstado()
    {
        paraChase = false;
        posChase = Vector2.zero;
        somPersegue = null;
        if (corTimer != null)
            ctx.StopCoroutine(corTimer);
        corTimer = null;
    }

    IEnumerator TimerDesisteChase()
    {
        yield return new WaitForSeconds(ctx.inimigoData.tempoParaChase);
        paraChase = true;
    }

    void CheckTimerChase()
    {
        // Se está vendo o player acaba com timer
        if (ctx.Fov.ChecaVisao())
        {
            if (corTimer != null)
            {
                ctx.StopCoroutine(corTimer);
                corTimer = null;
            }
            paraChase = false;
            return;
        }

        // se não ve nem escutou nada, comeca timer
        if (corTimer == null && somPersegue == null)
        {
            corTimer = ctx.StartCoroutine(TimerDesisteChase());
        }
    }
}
