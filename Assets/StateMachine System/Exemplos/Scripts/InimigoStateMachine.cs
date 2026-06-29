using UnityEngine;

public class InimigoStateMachine : MaquinaEstados<InimigoStateMachine>
{
    public float vel = 5f;

    public Rigidbody2D Rb { get; private set; }
    public Transform Player { get; private set; }

    public ExemploEstadoIdle EstadoIdle { get; private set; }
    public ExemploEstadoChase EstadoChase { get; private set; }

    void Awake()
    {
        EstadoIdle = new ExemploEstadoIdle(this);
        EstadoChase = new ExemploEstadoChase(this);

        Rb = GetComponent<Rigidbody2D>();
        Player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TrocaEstado(EstadoIdle);
    }

}
