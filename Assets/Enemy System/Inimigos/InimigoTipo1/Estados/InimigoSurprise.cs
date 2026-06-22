using System.Collections;
using UnityEngine;

public class InimigoSurprise : EstadoBase<InimigoTipo1>
{
    public InimigoSurprise(InimigoTipo1 contextoAtual) : base(contextoAtual) { }

    Som somOriginal;    // Som que fez entrar no estado

    float multRotacao = 2f;
    bool surpreso = true;
    bool viuPlayer = false;
    bool vendoPlayer = false;
    Coroutine corTempoReacao;
    Vector2 direcao;


    // Ações ao entrar no estado
    public override void EntraEstado()
    {
        Debug.Log("Entrou em estado" + this);

        if (ctx.Fov.ChecaVisao())   // Se está vendo player
        {
            viuPlayer = true;
            vendoPlayer = true;
            corTempoReacao = ctx.StartCoroutine(TempoReacao(ctx.inimigoData.tempoReacaoSurpreso*0.5f));

            direcao = ctx.Player.position - ctx.transform.position;
            direcao.Normalize();
        }
        else
        {
            if (ctx.SomAtual != null)
            {
                somOriginal = ctx.SomAtual;
                ctx.ResetaSom();

                if (somOriginal.tipoSom == Som.TipoSom.Desconhecido)
                    corTempoReacao = ctx.StartCoroutine(TempoReacao(ctx.inimigoData.tempoReacaoSurpreso*2));
                else if (somOriginal.tipoSom == Som.TipoSom.Perigo)
                    corTempoReacao = ctx.StartCoroutine(TempoReacao(ctx.inimigoData.tempoReacaoSurpreso*1));
                else 
                    corTempoReacao = ctx.StartCoroutine(TempoReacao(ctx.inimigoData.tempoReacaoSurpreso*1));

                direcao = somOriginal.pos - ctx.transform.position;
                direcao.Normalize();
            }
        }

    }

    // Ações de update
    public override void UpdateEstado()
    {
        if (vendoPlayer)
        {
            if (!ctx.Fov.ChecaVisao())    // Parou de ver player
            {
                ctx.dirPlayerMemoria = ctx.RbPlayer.linearVelocity.normalized;
                ctx.posPlayerMemoria = ctx.Player.position;
                Debug.Log(ctx.dirPlayerMemoria);
                vendoPlayer = false;
            }
        }
    }

    // Ações de Fixed update
    public override void FixedUpdateEstado()
    {
        ctx.DirVisao.OlhaParaDirecao(direcao, ctx.inimigoData.velRotacaoVisao*multRotacao);
    }

    // Troca de estado
    public override void ChecaTransicoes()
    {
        if (!surpreso)
        {
            if (viuPlayer)
            {
                // Avisa outros inimigos
                Som somAlerta = new Som(ctx.transform.position, ctx.inimigoData.rangeSomAlerta)
                {
                    tipoSom = Som.TipoSom.Perigo
                };

                GameSonsManager.PropagaSom(somAlerta);

                if (ctx.comportamento != 0) // Não esta indefeso 
                {
                    ctx.TrocaEstado(ctx.EstadoChase);   // Persegue player 
                    return;
                }
                else
                {
                    ctx.TrocaEstado(ctx.EstadoPanic);   // Persegue player 
                    return;
                }

                
            }

            if (ctx.SomAtual != null && ctx.SomAtual != somOriginal)
            {
                if (ctx.SomAtual.tipoSom == Som.TipoSom.Desconhecido && somOriginal.tipoSom != Som.TipoSom.Perigo)
                {
                    ctx.TrocaEstado(ctx.EstadoSearch);    // Investiga 
                    return;
                }
                else if (ctx.SomAtual.tipoSom == Som.TipoSom.Perigo)
                {
                    if (ctx.comportamento != 0)
                    {
                        ctx.TrocaEstado(ctx.EstadoChase);   // Persegue fonte do barulho  
                        return;  
                    }
                    else
                    {
                        ctx.TrocaEstado(ctx.EstadoPanic);   // Foge da fonte do barulho  
                        return;
                    }
                }
            }
            else if (somOriginal != null)
            {
                ctx.SetaSom(somOriginal);
                if (somOriginal.tipoSom == Som.TipoSom.Desconhecido)
                {
                    ctx.TrocaEstado(ctx.EstadoSearch);    // Investiga 
                    return;
                }
                else if (somOriginal.tipoSom == Som.TipoSom.Perigo)
                {
                    if (ctx.comportamento != 0)
                    {
                        ctx.TrocaEstado(ctx.EstadoChase);   // Persegue fonte do barulho 
                        return;   
                    }
                    else
                    {
                        ctx.TrocaEstado(ctx.EstadoPanic);   // Foge da fonte do barulho  
                        return;
                    }
                }
            }
            
        }

        else if (ctx.SomAtual != null && !vendoPlayer)
        {
            if (somOriginal == null)
            {
                ctx.TrocaEstado(ctx.EstadoSurprise);
            }
            else if (ctx.SomAtual.tipoSom == Som.TipoSom.Perigo && ctx.SomAtual.tipoSom != somOriginal.tipoSom)
            {
                ctx.TrocaEstado(ctx.EstadoSurprise);
            }

            else if (ctx.SomAtual.tipoSom == somOriginal.tipoSom)
            {
                direcao = ctx.SomAtual.pos - ctx.transform.position;
                ctx.ResetaSom();
            }


        }
    }

    // Ações ao sair do estado
    public override void SaiDeEstado()
    {
        surpreso = true;
        viuPlayer = false;
        vendoPlayer = false;
        somOriginal = null;
        ctx.StopCoroutine(corTempoReacao);
        corTempoReacao = null;
    }

    IEnumerator TempoReacao(float t)
    {
        yield return new WaitForSeconds(t);
        surpreso = false;
    }
}
