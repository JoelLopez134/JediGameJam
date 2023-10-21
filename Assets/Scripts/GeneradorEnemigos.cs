using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneradorPatos : MonoBehaviour
{
    public GameObject patoPrefab;
    public float tiempoEntreGeneracion = 2.0f;
    public Vector2 tamanoDelCollider = Vector2.one;

    private Collider2D generadorCollider;
    private float tiempoUltimaGeneracion;

    void Start()
    {
        generadorCollider = GetComponent<Collider2D>();
        tiempoUltimaGeneracion = Time.time;
    }

    void Update()
    {
        if (Time.time - tiempoUltimaGeneracion >= tiempoEntreGeneracion)
        {
            GenerarPatoAleatorio();
            tiempoUltimaGeneracion = Time.time;
        }
    }

    void GenerarPatoAleatorio()
    {
        float xAleatorio = Random.Range(generadorCollider.bounds.min.x, generadorCollider.bounds.max.x);
        float yAleatorio = Random.Range(generadorCollider.bounds.min.y, generadorCollider.bounds.max.y);

        Vector3 posicionGeneracion = new Vector3(xAleatorio, yAleatorio, 0f);

        Instantiate(patoPrefab, posicionGeneracion, Quaternion.identity);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, tamanoDelCollider);
    }
}




