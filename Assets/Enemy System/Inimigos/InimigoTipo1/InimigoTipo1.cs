using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(InimigoFov))]
[RequireComponent(typeof(InimigoOlhaDirecao))]

public class InimigoTipo1 : MaquinaEstados<InimigoTipo1>
{
    public InimigoData inimigoData;
    public float velMult = 1f;
    public int comportamento = 1;   // 0 == Indefeso, 1 == Normal, 2 == Em Alerta
    public GameObject armaPadrao;
    public GameObject corpoMorte;   // Corpo que vai spawnar depois de morrer
    public Color corPadrao; // Cor do inimigo;

    public float Vida { get; private set; }


    public GameController gameController{ get; private set; }
    public Transform Player { get; private set; }
    public Rigidbody2D RbPlayer { get; private set; }
    public Rigidbody2D Rb { get; private set; }
    public NavMeshAgent Agent { get; private set; }
    public InimigoFov Fov { get; private set; }
    public InimigoOlhaDirecao DirVisao { get; private set; }
    public InimigoArma Arma { get; private set; }
    public Som SomAtual { get; private set; }


    public List<Transform> rotaPatrulha = new();   
    
    public bool emGuarda = false;   // Se estiver em guarda, tenta não se distanciar muito de sua posição inicial
    public bool morto = false;

    public Vector2 dirPlayerMemoria;    // direcao da velocidade do player
    public Vector2 posPlayerMemoria;    // posicao do player
    public Vector2 PosPadrao { get; private set; }
    public Vector2 DirPadrao { get; private set; }


    // Estados possiveis:
    public InimigoIdle EstadoIdle { get; private set; }
    public InimigoPatrol EstadoPatrol { get; private set; }
    public InimigoSurprise EstadoSurprise { get; private set; }
    public InimigoSearch EstadoSearch { get; private set; }
    public InimigoChase EstadoChase { get; private set; }
    public InimigoCombat EstadoCombat { get; private set; }
    public InimigoPanic EstadoPanic { get; private set; }
    public InimigoDead EstadoDead { get; private set; }


    private void Awake()
    {

        // Inicializa Estados
        EstadoIdle = new InimigoIdle(this);
        EstadoPatrol = new InimigoPatrol(this);
        EstadoSurprise = new InimigoSurprise(this);
        EstadoSearch = new InimigoSearch(this);
        EstadoChase = new InimigoChase(this);
        EstadoCombat = new InimigoCombat(this);
        EstadoPanic = new InimigoPanic(this);
        EstadoDead = new InimigoDead(this);


        

        Rb = GetComponent<Rigidbody2D>();
        Agent = GetComponent<NavMeshAgent>();
        Fov = GetComponent<InimigoFov>();
        DirVisao = GetComponent<InimigoOlhaDirecao>();
        Arma = GetComponentInChildren<InimigoArma>();
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        


        // Importante: Para o navmesh agent de diretamente mexer o inimigo
        Agent.updatePosition = false;
        Agent.updateRotation = false;
        Agent.updateUpAxis = false; 

        PosPadrao = transform.position;
        DirPadrao = transform.rotation * Vector2.up;
        
        if (playerObj != null)
        {
            Player = playerObj.transform;
            RbPlayer = playerObj.GetComponent<Rigidbody2D>();
        }

        Vida = inimigoData.vidaMax;

        if (Arma == null && armaPadrao != null)
        {
            TrocaArma(armaPadrao);
            comportamento = 1; // Normal
        }
        else if (Arma == null && armaPadrao == null)
        {
            comportamento = 0;  // Indefeso
        }
            

        ResetaSom();
    }

    private void Start()
    {
        // Estado Inicial
        TrocaEstado(EstadoIdle);
    }

    public void SetaSom(Som som)
    {
        SomAtual = som;
    }

    public void ResetaSom()
    {
        SomAtual = null;
    }

    public void TrocaModoArma(int novoModo)
    {
        if (Arma)
        {
            Arma.TrocaModo(novoModo);
        }
    } 

    public void TrocaArma(GameObject armaObject)
    {
        if (Arma)
            Destroy(Arma.gameObject);

        
        InimigoArma arma = Instantiate(armaObject, transform).GetComponent<InimigoArma>();

        Arma = arma;
        TrocaModoArma(0);
    }

    public void SetaAtirando(bool novo)
    {
        if (Arma)
            Arma.SetaAtirando(novo);
    }

    public void RecebeDano(int dano)
    {
        Vida -= dano;

        if (Vida <= 0)
        {
            TrocaEstado(EstadoDead);
            morto = true;
        }

    }

    public void Morre()
    {
        GameObject corpo = Instantiate(corpoMorte, transform.position, Quaternion.identity);
        corpo.GetComponent<SpriteRenderer>().color = corPadrao;

        Destroy(gameObject);
    }

}
