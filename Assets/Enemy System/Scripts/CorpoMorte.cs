using System.Collections.Generic;
using UnityEngine;

public class CorpoMorte : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    public List<Sprite> sprites;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        float randomZ = Random.Range(0f, 360f);
        transform.eulerAngles = new Vector3(0f, 0f, randomZ);

        int randomSprite = Random.Range(0, sprites.Count);
        spriteRenderer.sprite = sprites[randomSprite];
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
