using UnityEngine;

public class HeadBob : MonoBehaviour
{
    public float frequency = 1.2f;
    public float verticalAmount = 0.05f;
    public float horizontalAmount = 0.03f;
    public float smoothing = 10f;

    Vector3 startPos;
    Transform player;
    Vector3 lastPlayerPos;
    float timer;

    void Start()
    {
        startPos = transform.localPosition;
        player = transform.parent;
        lastPlayerPos = player.position;
    }

    void Update()
    {
        Vector3 delta = player.position - lastPlayerPos;
        delta.y = 0f;
        float speed = delta.magnitude / Time.deltaTime;
        lastPlayerPos = player.position;

        Vector3 target = startPos;

        if (speed > 0.1f)
        {
            timer += Time.deltaTime * speed * frequency;
            target.y += Mathf.Sin(timer * 2f) * verticalAmount;
            target.x += Mathf.Cos(timer) * horizontalAmount;
        }
        else
        {
            timer = 0f;
        }

        transform.localPosition = Vector3.Lerp(transform.localPosition, target, Time.deltaTime * smoothing);
    }
}