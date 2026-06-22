using UnityEngine;

public class BulletTrail : MonoBehaviour
{
    Vector3 startPos;
    Vector3 targetPos;
    float progresso;

    [SerializeField] float velBala = 40f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        progresso += Time.deltaTime * velBala;
        transform.position = Vector3.Lerp(startPos, targetPos, progresso);
    }

    public void SetaTarget(Vector3 target)
    {
        targetPos = target;
    }
}
