using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public GameObject startScreen;
    public GameObject gameOverScreen;
    public GameObject gameObjects;
    public GameObject bubbleEffect;

    // Add this for fade on start screen
    public CanvasGroup startScreenCanvasGroup;

    private bool gameStarted = false;
    private bool gameOver = false;

    void Start()
    {
        startScreen.SetActive(true);
        gameOverScreen.SetActive(false);
        gameObjects.SetActive(false);

        if (startScreenCanvasGroup == null && startScreen != null)
            startScreenCanvasGroup = startScreen.GetComponent<CanvasGroup>();
        
        if (startScreenCanvasGroup != null)
        {
            startScreenCanvasGroup.alpha = 1f;
            startScreenCanvasGroup.interactable = true;
            startScreenCanvasGroup.blocksRaycasts = true;
        }
    }

    void Update()
    {
        if (!gameStarted && Input.anyKeyDown)
        {
            StartCoroutine(FadeOutStartScreen());
        }
    }

    private IEnumerator FadeOutStartScreen()
    {
        gameStarted = true;

        float fadeDuration = 1f;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            if (startScreenCanvasGroup != null)
                startScreenCanvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            yield return null;
        }

        if (startScreenCanvasGroup != null)
        {
            startScreenCanvasGroup.alpha = 0f;
            startScreenCanvasGroup.interactable = false;
            startScreenCanvasGroup.blocksRaycasts = false;
        }

        startScreen.SetActive(false);
        gameObjects.SetActive(true);
    }

    public void TriggerGameOver()
    {
        if (gameOver) return;  // prevent multiple triggers
        gameOver = true;

        StartCoroutine(ShakeThenGameOver());
    }

    private IEnumerator ShakeThenGameOver()
    {
        // Shake camera
        yield return StartCoroutine(Camera.main.GetComponent<CameraShake>().Shake(0.5f, 0.2f));

        gameObjects.SetActive(false);

        gameOverScreen.SetActive(true);

        if (bubbleEffect != null)
        {
            GameObject fish = GameObject.FindWithTag("Fish");
            if (fish != null)
            {
                bubbleEffect.transform.position = fish.transform.position;
                bubbleEffect.SetActive(true);
                var ps = bubbleEffect.GetComponent<ParticleSystem>();
                if (ps != null) ps.Play();
            }
        }

        CanvasGroup canvasGroup = gameOverScreen.GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            yield return StartCoroutine(FadeInGameOverUI(canvasGroup, 1.0f)); // 1 second fade-in
        }
    }

    private IEnumerator FadeInGameOverUI(CanvasGroup canvasGroup, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(elapsed / duration);
            yield return null;
        }

        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
