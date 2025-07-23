using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FishController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float bounceForce = 4f;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool canMove = true;
    private Transform fishVisual;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        fishVisual = transform.Find("FishVisual");
        if (fishVisual == null)
            Debug.LogWarning("FishVisual not found — the fish will move but won't rotate visually.");
    }

    void Update()
    {
        if (!canMove) return;

        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        moveInput = new Vector2(moveX, moveY).normalized;

        if (fishVisual != null && moveInput != Vector2.zero)
        {
            float angle = Mathf.Atan2(moveInput.y, moveInput.x) * Mathf.Rad2Deg;
            fishVisual.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }

    void FixedUpdate()
    {
        if (canMove)
        {
            rb.linearVelocity = moveInput * moveSpeed;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Fish collided with: " + collision.gameObject.name);

            ContactPoint2D contact = collision.contacts[0];

            Vector2 bounceDir;

            if (moveInput != Vector2.zero)
            {
                bounceDir = Vector2.Reflect(moveInput, contact.normal);
            }
            else
            {
                bounceDir = contact.normal;
            }

            bounceDir.Normalize();

            rb.linearVelocity = Vector2.zero;
            rb.AddForce(bounceDir * bounceForce, ForceMode2D.Impulse);

            canMove = false;
            Invoke(nameof(EnableMovement), 0.5f);
        }
    }

    void EnableMovement()
    {
        canMove = true;
    }
}
