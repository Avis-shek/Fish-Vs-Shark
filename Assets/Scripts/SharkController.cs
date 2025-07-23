using UnityEngine;
using UnityEngine.SceneManagement;

public class SharkController : MonoBehaviour
{
    public Transform fish;
    public float chaseSpeed = 3f;

    private Rigidbody2D rb;
    private Transform sharkVisual;
    private AudioSource audioSource;  // Add AudioSource for sound
    public AudioClip sharkCatchSound; // Assign this in Inspector

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component missing on Shark!");
        }

        sharkVisual = transform.Find("SharkVisual");
        if (sharkVisual == null)
        {
            Debug.LogError("SharkVisual child not found! Make sure the visual sprite is inside the Shark GameObject.");
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogWarning("No AudioSource found on Shark! Adding one.");
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
    }

 void FixedUpdate()
{
    if (fish == null)
    {
        Debug.LogWarning("Fish reference is null!");
        return;
    }

    Vector2 direction = ((Vector2)fish.position - rb.position).normalized;
    Debug.Log($"Chasing direction: {direction}");

    Vector2 newPos = rb.position + direction * chaseSpeed * Time.fixedDeltaTime;
    rb.MovePosition(newPos);

    if (sharkVisual != null)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 180f;
        sharkVisual.rotation = Quaternion.Euler(0, 0, angle);
    }
}


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            ContactPoint2D contact = collision.contacts[0];
            Vector2 direction = ((Vector2)fish.position - rb.position).normalized;
            Vector2 bounceDirection = Vector2.Reflect(direction, contact.normal);

            rb.linearVelocity = Vector2.zero;
            rb.AddForce(bounceDirection * 3f, ForceMode2D.Impulse);

            Debug.Log("Shark bounced off obstacle!");
        }
        else if (collision.gameObject.CompareTag("Fish"))
        {
            Debug.Log("Shark touched fish! Game Over.");

            // Play catch sound
            if (audioSource != null && sharkCatchSound != null)
            {
                audioSource.PlayOneShot(sharkCatchSound);
            }

            FindObjectOfType<GameManager>().TriggerGameOver();
        }
    }
}
