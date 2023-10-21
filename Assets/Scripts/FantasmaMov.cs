using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FantasmaMov : MonoBehaviour
{
    public float velocidad = 2.0f;
    public Animator animator;

    private LevelController levelController;

    private ScoreManager scoreManager;
    private Vector2 posicionInicial;
    private Vector2 objetivo;
    private GameObject limitesDelSector;
    private SpriteRenderer spriteRenderer;

    public int vidas = 3;  // Número de vidas del fantasma
    private bool disparado = false;
    private bool saliendo = false;
    public float tiempoParaSalir = 10.0f;
    public float tiempoParaDesvanecer = 3.0f;

    private ImageUIDestroyer imageUIDestroyer;

    private CambiarSpriteEnSecuencia cambiarSpriteScript;

    public int puntajeFantasta = 10;

    public AudioSource audioSource;
    private bool deathSoundPlayed = false;

    void Start()
    {
        levelController = FindObjectOfType<LevelController>();
        audioSource = GetComponentInChildren<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError("No se encontró el componente AudioSource en el objeto hijo.");
        }

        GameObject objetoConImageUIDestroyer = GameObject.FindWithTag("corazonHud");

        if (objetoConImageUIDestroyer != null)
        {
            // Obtén el componente ImageUIDestroyer en el objeto encontrado
            imageUIDestroyer = objetoConImageUIDestroyer.GetComponent<ImageUIDestroyer>();

            if (imageUIDestroyer == null)
            {
                Debug.LogError("El objeto encontrado no tiene el script ImageUIDestroyer.");
            }
        }
        else
        {
            Debug.LogError("No se encontró ningún objeto con el tag especificado.");
        }
        // Busca el objeto por su tag
        GameObject objetoConCambiarSprite = GameObject.FindGameObjectWithTag("hudFantasma");

        if (objetoConCambiarSprite != null)
        {
            // Obtén el componente CambiarSpriteEnSecuencia en el objeto encontrado
            cambiarSpriteScript = objetoConCambiarSprite.GetComponent<CambiarSpriteEnSecuencia>();

            if (cambiarSpriteScript == null)
            {
                Debug.LogError("El objeto encontrado no tiene el script CambiarSpriteEnSecuencia.");
            }
        }
        else
        {
            Debug.LogError("No se encontró ningún objeto con el tag especificado.");
        }


        posicionInicial = transform.position;
        limitesDelSector = GameObject.FindGameObjectWithTag("limitesDelSector");
        spriteRenderer = GetComponent <SpriteRenderer>();
        ObtenerNuevoObjetivo();

        GameObject scoreManagerObject = GameObject.FindGameObjectWithTag("ScoreManagerTag");
        if (scoreManagerObject != null)
        {
            scoreManager = scoreManagerObject.GetComponent<ScoreManager>();
        }
        else
        {
            Debug.LogError("No se encontró el objeto ScoreManager con el tag 'ScoreManagerTag'. Asegúrate de que el objeto ScoreManager tenga el tag correcto.");
        }
        StartCoroutine(SalirDespuesDeTiempo(tiempoParaSalir));
    }
    IEnumerator SalirDespuesDeTiempo(float tiempo)
    {
        yield return new WaitForSeconds(tiempo);
        if (!disparado && !saliendo)
        {
            saliendo = true;
            objetivo = new Vector2(limitesDelSector.transform.position.x, limitesDelSector.transform.position.y);

            // Restar una vida al jugador
            if (imageUIDestroyer != null)
            {
                imageUIDestroyer.RemoveImage();
            }
            if (levelController != null)
            {
                levelController.PrefabDestroyed();
            }
            // Iniciar el desvanecimiento
            StartCoroutine(Desvanecer());
        }
    }
    IEnumerator Desvanecer()
    {
        float inicio = Time.time;
        Color color = spriteRenderer.color;

        while (Time.time - inicio < tiempoParaDesvanecer)
        {
            float t = (Time.time - inicio) / tiempoParaDesvanecer;
            color.a = Mathf.Lerp(1.0f, 0.0f, t);
            spriteRenderer.color = color;
            yield return null;
        }

        Destroy(gameObject);
    }
    void Update()
    {
        if (!disparado)
        {
            if (saliendo)
            {
                // No es necesario moverse, ya que estamos desvaneciéndolo
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, objetivo, velocidad * Time.deltaTime);
                if ((Vector2)transform.position == objetivo)
                {
                    ObtenerNuevoObjetivo();
                }
            }

            if (objetivo.x < transform.position.x)
            {
                spriteRenderer.flipX = true;
            }
            else
            {
                spriteRenderer.flipX = false;
            }
        }
    }

    void ObtenerNuevoObjetivo()
    {
        if (!disparado && limitesDelSector != null)
        {
            float objetivoX = Random.Range(limitesDelSector.GetComponent<Collider2D>().bounds.min.x, limitesDelSector.GetComponent<Collider2D>().bounds.max.x);
            float objetivoY = Random.Range(limitesDelSector.GetComponent<Collider2D>().bounds.min.y, limitesDelSector.GetComponent<Collider2D>().bounds.max.y);
            objetivo = new Vector2(objetivoX, objetivoY);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Disparo") && !disparado)
        {
            vidas--; // Restar una vida
            
            
            disparado = true;
            StartCoroutine(DestruirDespuesDeDelay(1f));

            // Cambiar la opacidad del sprite cuando recibe un disparo
            StartCoroutine(LowerOpacity(0.3f, 0.5f)); // Bajar la opacidad durante 0.3 segundos


        }
        if (vidas <= 0)
        {
            animator.SetTrigger("shoot");
        }
    }

    IEnumerator DestruirDespuesDeDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (vidas <= 0)
        {
            if (!deathSoundPlayed)
            {
                if (audioSource != null)
                {
                    audioSource.Play();
                }
            }
            if (levelController != null)
            {
                levelController.PrefabDestroyed();
            }


            if (cambiarSpriteScript != null)
            {
                cambiarSpriteScript.OnEnemigoDestruido(); // Cambia el sprite cuando el fantasma es destruido
            }
            else
            {
                Debug.LogWarning("El objeto CambiarSpriteEnSecuencia no se ha encontrado o no tiene el script.");
            }

            scoreManager.IncreaseScore(puntajeFantasta);
            yield return new WaitForSeconds(delay);

            animator.SetTrigger("dead");


            float tiempoDeDescenso = 3f;
            Vector3 destino = transform.position - new Vector3(0f, 15f, 0f);
            Vector3 inicio = transform.position;
            float tiempoTranscurrido = 0f;

            while (tiempoTranscurrido < tiempoDeDescenso)
            {
                tiempoTranscurrido += Time.deltaTime;
                float fraccionDeTiempo = tiempoTranscurrido / tiempoDeDescenso;
                transform.position = Vector3.Lerp(inicio, destino, fraccionDeTiempo);
                yield return null;
            }

            yield return new WaitForSeconds(3f);

            Destroy(gameObject);

        }
        else
        {
            disparado = false;
            ObtenerNuevoObjetivo();
        }
    }

    IEnumerator LowerOpacity(float duration, float lowerOpacity)
    {
        float startOpacity = spriteRenderer.color.a;
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            float t = timeElapsed / duration;
            Color color = spriteRenderer.color;
            color.a = Mathf.Lerp(startOpacity, lowerOpacity, t);
            spriteRenderer.color = color;
            yield return null;
        }

        yield return new WaitForSeconds(0.3f);

        // Restaurar la opacidad original
        while (timeElapsed > 0f)
        {
            timeElapsed -= Time.deltaTime;
            float t = timeElapsed / duration;
            Color color = spriteRenderer.color;
            color.a = Mathf.Lerp(startOpacity, lowerOpacity, t);
            spriteRenderer.color = color;
            yield return null;
        }
    }
}