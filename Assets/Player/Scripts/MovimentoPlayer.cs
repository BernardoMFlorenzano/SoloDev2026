using System.Collections;
using UnityEngine;
using UnityEngine.U2D.IK;
using UnityEngine.UIElements;

public class MovimentoPlayer : MonoBehaviour
{
    GameController gameController;
    public PlayerData playerData;
    public bool movAtivo = true;
    Rigidbody2D rb;
    Vector2 direcao;
    DirecaoCorpo direcaoCorpo;
    bool correndo = false;
    bool sneaking = false;
    bool emLunge = false;
    bool podeDarLunge = true;

    [Header("Animações")]
    public bool animacaoAtaque = false;

    [SerializeField] LimbSolver2D bracoEsqSolver;
    [SerializeField] LimbSolver2D bracoDirSolver;
    [SerializeField] LimbSolver2D pernaEsqSolver;
    [SerializeField] LimbSolver2D pernaDirSolver;
    [SerializeField] CCDSolver2D corpoSolver;
    [SerializeField] CCDSolver2D caudaSolver;
    [SerializeField] float weightPadrao = 1f;
    [SerializeField] float weightReduzidoPadrao = 0f;
    Animator animator;
    PernasHandler pernasHandler;
    bool bracoEscolhido = false;    // varia entre esquerda e direita
    Coroutine corAnimAtaque;


    public bool GetCorrendo()
    {
        return correndo;
    }

    public bool GetSneaking()
    {
        return sneaking;
    }

    private void OnEnable()
    {
        InputPlayerManager.OnMoveInput += SetaDirecaoInput;
        InputPlayerManager.OnAttackInput += Ataca;
        //InputPlayerManager.OnLungeInput += Lunge;
        InputPlayerManager.OnSprintInput += SetaSprintInput;
        InputPlayerManager.OnCrouchInput += SetaCrouchInput;
    }
    private void OnDisable()
    {
        InputPlayerManager.OnMoveInput -= SetaDirecaoInput;
        InputPlayerManager.OnAttackInput -= Ataca;
        //InputPlayerManager.OnLungeInput -= Lunge;
        InputPlayerManager.OnSprintInput -= SetaSprintInput;
        InputPlayerManager.OnCrouchInput -= SetaCrouchInput;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        animator.enabled = false;

        direcaoCorpo = GetComponent<DirecaoCorpo>();
        pernasHandler = GetComponent<PernasHandler>();
    }

    public void SetaDirecaoInput(Vector2 dir)
    {
        direcao = dir;
    }

    public Vector2 RetornaDirecaoInput()
    {
        return direcao;
    }

    public void SetaSprintInput(bool val)
    {
        correndo = val;
    }

    public void SetaCrouchInput(bool val)
    {
        sneaking = val;
    }

    void Update()
    {
        if (gameController)
        {
            if (!gameController.inputAtivo)
            {
                movAtivo = false;
                direcaoCorpo.ativo = false;
            }
            if (!gameController.playerVivo)
            {
                pernasHandler.pernasAtivas = false;
            }
            
        }
    }

    void FixedUpdate()
    {
        if (emLunge || !movAtivo)
        {
            return;    
        }

        if (sneaking)
        {
            rb.AddForce(playerData.velPlayer*rb.mass*playerData.velMovMult*playerData.velMovSneak*direcao);
        }

        else if (ChecaDirecaoMovimento())
        {
            if (!correndo)
            {
                rb.AddForce(playerData.velPlayer*rb.mass*playerData.velMovMult*playerData.velMovFrontWalk*direcao);
            }
            else if (correndo)
            {
                rb.AddForce(playerData.velPlayer*rb.mass*playerData.velMovMult*playerData.velMovRun*direcao);
            }
        }

        else if (!ChecaDirecaoMovimento())
        {
            rb.AddForce(playerData.velPlayer*rb.mass*playerData.velMovMult*playerData.velMovStrafe*direcao);
        }

        
    }

    void Ataca()
    {
        if (animator != null && !animacaoAtaque)
        {
            animator.enabled = true;
            animator.SetBool("Braco", bracoEscolhido);
            animator.SetTrigger("Ataca");
            animacaoAtaque = true;

            if (bracoEscolhido == false)
            {
                corAnimAtaque = StartCoroutine(AnimAtaque(bracoEsqSolver));
            }
            else if (bracoEscolhido == true)
            {
                corAnimAtaque = StartCoroutine(AnimAtaque(bracoDirSolver));
            }
               
            bracoEscolhido = !bracoEscolhido;
        }
    }

    public void AtaqueMordida()
    {
        if (animator != null && !animacaoAtaque)
        {
            if (corAnimAtaque != null)
            {
                StopCoroutine(corAnimAtaque);
            }
            animator.enabled = true;
            animator.SetTrigger("Mordida");
            animacaoAtaque = true;

            corAnimAtaque = StartCoroutine(AnimAtaque(bracoDirSolver));
        }
    }

    IEnumerator AnimAtaque(LimbSolver2D bracoSolver)
    {
        if (bracoSolver)
            bracoSolver.weight = weightReduzidoPadrao;
        corpoSolver.weight = weightReduzidoPadrao;

        yield return new WaitUntil(() => animacaoAtaque == false);

        if (bracoSolver)
            bracoSolver.weight = weightPadrao;
        corpoSolver.weight = weightPadrao;

        animator.enabled = false;
    }

    void Lunge()
    {
        if (podeDarLunge && !emLunge)
        {
            StartCoroutine(CorLunge());
        }
    }

    IEnumerator CorLunge()
    {
        emLunge = true;
        podeDarLunge = false;
        if (pernasHandler != null)
            pernasHandler.pernasAtivas = false;
        if (direcaoCorpo)
            direcaoCorpo.ativo = false;


        pernasHandler.ForcarPasso(direcaoCorpo.RetornaDirecaoMouse()*2);
        ImpulsoLunge();

        yield return new WaitForSeconds(playerData.tempoLunge);

        emLunge = false;
        if (pernasHandler != null)
            pernasHandler.pernasAtivas = true;
        if (direcaoCorpo)
            direcaoCorpo.ativo = true;
        StartCoroutine(CooldownLunge());
    }

    void ImpulsoLunge()
    {
        rb.AddForce(direcaoCorpo.RetornaDirecaoMouse()*playerData.impulsoLunge, ForceMode2D.Impulse);
    }

    IEnumerator CooldownLunge()
    {
        yield return new WaitForSeconds(playerData.cooldownLunge);
        podeDarLunge = true;
    }

    bool ChecaDirecaoMovimento()
    {
        if (Vector3.Angle(direcaoCorpo.RetornaDirecaoMouse(), rb.linearVelocity.normalized) < playerData.anguloMouseMovimentoMin)
            return true;
        else 
            return false;
    }

    public void ReativaMov()
    {
        movAtivo = true;
        direcaoCorpo.ativo = true;
        pernasHandler.pernasAtivas = true;
    }

}
