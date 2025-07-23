using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class water_motion : MonoBehaviour
{
    public float frequency = 1f;
    public float amplitude = 0.5f;

    private float timer = 0f;
    private float initialY;
    private float phase;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        initialY = rb.position.y;
        phase = Random.Range(0f, 2 * Mathf.PI);
    }

    void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
        float newY = initialY + Mathf.Sin(timer * frequency + phase) * amplitude;
        Vector2 newPos = new Vector2(rb.position.x, newY);

        rb.MovePosition(newPos); // 👈 This preserves physics like collisions or chasing
    }
}
