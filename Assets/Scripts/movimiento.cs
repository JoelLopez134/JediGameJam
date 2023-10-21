using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movimiento : MonoBehaviour
{
    public Camera mainCamera; // La cámara que deseas que tiemble
    public float shakeDuration = 1.0f; // Duración del temblor en segundos
    public float shakeMagnitude = 0.1f; // Magnitud del temblor
    public float cameraZPosition = -10.0f; // Posición en Z deseada

    private Vector3 originalPosition;
    private float shakeTimer = 0.0f;

    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main; // Si no se asigna una cámara, se toma la cámara principal por defecto
        }

        originalPosition = mainCamera.transform.position;
        originalPosition.z = cameraZPosition;
        mainCamera.transform.position = originalPosition;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Detecta un clic del mouse
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
    }
}