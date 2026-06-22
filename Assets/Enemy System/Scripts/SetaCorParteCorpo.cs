using UnityEngine;

public class SetaCorParteCorpo : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    InimigoTipo1 inimigo;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inimigo = GetComponentInParent<InimigoTipo1>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (inimigo)
        {
            spriteRenderer.color = inimigo.corPadrao;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
