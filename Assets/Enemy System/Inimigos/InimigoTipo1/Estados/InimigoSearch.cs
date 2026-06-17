using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class InimigoSearch : EstadoBase<InimigoTipo1>
{
    public InimigoSearch(InimigoTipo1 contextoAtual) : base(contextoAtual) { }


    Som somProcura;    // Som que vai procurar
    bool desisteSearch = false; 
    Coroutine corTimer = null;
    Coroutine corDirecao = null;

    Vector2 centroBusca;
    Vector2 destinoAtual;
    Vector2 direcaoAleatoria;
    //bool temDestino = false;

    // Ações ao entrar no estado
    public override void EntraEstado()
    {
        Debug.Log("Entrou em estado" + this);

        if (ctx.SomAtual != null)
        {
            somProcura = ctx.SomAtual;
            centroBusca = somProcura.pos;
            destinoAtual = somProcura.pos;
            ctx.ResetaSom();
        }
        else
        {
            if (ctx.posPlayerMemoria != Vector2.zero)
            {
                centroBusca = ctx.posPlayerMemoria;
                destinoAtual = ctx.posPlayerMemoria + ctx.dirPlayerMemoria*2;
            }
            else
            {
                centroBusca = ctx.transform.position;
                destinoAtual = ctx.transform.position;
            }
        }

        //destinoAtual = ProcurarPontoAleatorio(centroBusca, 6f); 

        if (ctx.emGuarda)
            corTimer = ctx.StartCoroutine(TimerDesisteSearch());

        corDirecao = ctx.StartCoroutine(GeraDirecaoOlhar());
    }

    // Ações de update
    public override void UpdateEstado()
    {
        ctx.Agent.nextPosition = ctx.transform.position;    // Sincroniza a posiçao interna do agente com a real

        if (ctx.SomAtual != null)
        {
            somProcura = ctx.SomAtual;
            centroBusca = somProcura.pos;
            destinoAtual = somProcura.pos;
            ctx.ResetaSom();
        }

        // Se o inimigo chegou perto do ponto atual, gera outro 
        if (Vector2.Distance(ctx.transform.position, destinoAtual) < ctx.inimigoData.distanciaMinPatrolPoint)
        {
            destinoAtual = ProcurarPontoAleatorio(centroBusca, 10f);
        }

        ctx.Agent.SetDestination(destinoAtual);  // Calcula o caminho
    }

    // Ações de Fixed update
    public override void FixedUpdateEstado()
    {
        Vector3 velocidade3D = ctx.Agent.desiredVelocity;
        Vector2 direcao = new Vector2(velocidade3D.x, velocidade3D.y);
    
        direcao.Normalize(); // Retorna direcao

        ctx.Rb.AddForce(direcao * ctx.Rb.mass * ctx.inimigoData.velInimigoWalk * ctx.velMult);

        float similaridade;

        if (direcaoAleatoria != Vector2.zero)   // Fazer isso se comportamento for Em Alerta
        {
            ctx.DirVisao.OlhaParaDirecao(direcaoAleatoria, ctx.inimigoData.velRotacaoVisao*0.25f);
        }
        else
        {
            ctx.DirVisao.OlhaParaDirecao(direcao, ctx.inimigoData.velRotacaoVisao*0.5f);
        }

        similaridade = Vector2.Dot(ctx.transform.up, direcaoAleatoria);

        if (similaridade >= 0.99f)
        {
            direcaoAleatoria = Vector2.zero;
        }


        
    }

    // Troca de estado
    public override void ChecaTransicoes()
    {
        if (ctx.comportamento == 0 || ctx.comportamento == 1)   // Indefeso ou Normal
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
        }
        else    // Em Alerta
        {
            if (ctx.SomAtual != null)
            {
                if (ctx.SomAtual.tipoSom == Som.TipoSom.Desconhecido || ctx.SomAtual.tipoSom == Som.TipoSom.Perigo)
                {
                    ctx.TrocaEstado(ctx.EstadoChase);
                    return;
                }
            }

            if (ctx.Player != null && ctx.Fov.ChecaVisao())
            {
                ctx.TrocaEstado(ctx.EstadoChase);
                return;
            }
        }

        if (desisteSearch)
        {
            ctx.TrocaEstado(ctx.EstadoIdle);
            return;
        }
        
    }

    // Ações ao sair do estado
    public override void SaiDeEstado()
    {
        desisteSearch = false;
        somProcura = null;
        if (corTimer != null)
            ctx.StopCoroutine(corTimer);
        corTimer = null;
        if (corDirecao != null)
            ctx.StopCoroutine(corDirecao);
        corDirecao = null;
        direcaoAleatoria = Vector2.zero;
    }

    IEnumerator TimerDesisteSearch()
    {
        yield return new WaitForSeconds(ctx.inimigoData.tempoDesisteSearch);    
        desisteSearch = true;
    }

    private Vector2 ProcurarPontoAleatorio(Vector2 centro, float raio)
    {
        // Gera uma posição aleatória
        Vector2 dirAleatoria = Random.insideUnitCircle * raio;
        Vector2 pontoAlvo = centro + dirAleatoria;

        NavMeshHit hit;

        if (NavMesh.SamplePosition(pontoAlvo, out hit, raio, NavMesh.AllAreas)) // Filtra baseado no navmesh
        {
            return hit.position;
        }

        return Vector2.zero; // Retorna zero se falhar
    }

    IEnumerator GeraDirecaoOlhar()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);

            direcaoAleatoria = Random.onUnitCircle;
        }
    }
    

}
