using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject disparoPrefab;
    public float velocidadDisparo = 10.0f;
    public float tiempoDeVida = 2.0f;
    public BalasHud hudBalas;

    private int balasRestantes = 3; 
    private bool recargando = false;

    //camara
    public Camera mainCamera; // La cámara que deseas que tiemble
    public float shakeDuration = 1.0f; // Duración del temblor en segundos
    public float shakeMagnitude = 0.1f; // Magnitud del temblor
    public float cameraZPosition = -10.0f; // Posición en Z deseada

    private Vector3 originalPosition;
    private float shakeTimer = 0.0f;
    //audio
    public AudioClip sonidoDisparo,sonidoRecarga; // Asigna tu clip de audio en el Inspector
    private AudioSource audioSource;
    public float volumenSonido = 1.0f;

    void Start()
    {

        hudBalas = GetComponentInChildren<BalasHud>();


        if (hudBalas == null)
        {
            Debug.LogError("No se encontró el script BalasHud en el objeto hijo. Asegúrate de que el objeto del HUD esté configurado como hijo de este objeto.");
        }
        //camara 
        if (mainCamera == null)
        {
            mainCamera = Camera.main; // Si no se asigna una cámara, se toma la cámara principal por defecto
        }

        originalPosition = mainCamera.transform.position;
        originalPosition.z = cameraZPosition;
        mainCamera.transform.position = originalPosition;
        //disparo 
        audioSource = gameObject.AddComponent<AudioSource>(); // Agrega un AudioSource al objeto
        audioSource.playOnAwake = false;
        audioSource.clip = sonidoDisparo;
        audioSource.clip = sonidoRecarga;
    }

    void Update()
    {
        //camara
        if (Input.GetMouseButtonDown(0) && recargando != true) // Detecta un clic del mouse
        {
            shakeTimer = shakeDuration;
        }

        if (shakeTimer > 0)
        {
            Vector3 shakeOffset = Random.insideUnitCircle * shakeMagnitude;
            shakeOffset.z = 0; // Asegura que el temblor sea solo en el plano XY
            mainCamera.transform.position = new Vector3(originalPosition.x + shakeOffset.x, originalPosition.y + shakeOffset.y, originalPosition.z);
            shakeTimer -= Time.deltaTime;
        }
        else
        {
            shakeTimer = 0f;
            mainCamera.transform.position = originalPosition;
        }
        //
        if (!recargando)
        {
            if (Input.GetMouseButtonDown(0) && balasRestantes > 0)
            {
                Disparar();
            }
            else if (Input.GetMouseButtonDown(1) && balasRestantes < 3)
            {
                IniciarRecarga();
            }
        }
       
    }

    void Disparar()
    {

        Vector3 posicionMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        posicionMouse.z = 0; 
        GameObject disparo = Instantiate(disparoPrefab, posicionMouse, Quaternion.identity);


        audioSource.volume = volumenSonido;
        audioSource.PlayOneShot(sonidoDisparo);

        Destroy(disparo, tiempoDeVida);

        balasRestantes--;

        if (balasRestantes == 0)
        {
            IniciarRecarga();
            audioSource.PlayOneShot(sonidoRecarga);
        }
    }

    void IniciarRecarga()
    {
        recargando = true;
        StartCoroutine(TerminarRecarga());
    }

    IEnumerator TerminarRecarga()
    {
        yield return new WaitForSeconds(2.0f);
        balasRestantes = 3;
        recargando = false;
    }
}