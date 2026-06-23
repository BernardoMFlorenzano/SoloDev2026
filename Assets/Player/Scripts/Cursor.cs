using TreeEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class Cursor : MonoBehaviour
{
    [SerializeField] float distanciaMaxCursor;  // Distancia maxima que o cursor pode ficar do outro player
    Transform player;
    Vector3 mousePos; 
    float distancia;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.value);
        mousePos.z = 0f;

        if (player != null)
        {
            distancia = Vector2.Distance(player.position, mousePos);
            if (distancia > distanciaMaxCursor)
            {
                transform.position = player.position + (mousePos - player.position).normalized * distanciaMaxCursor;
            }
            else 
                transform.position = mousePos;
        }
        else 
            transform.position = mousePos;
    }
}
