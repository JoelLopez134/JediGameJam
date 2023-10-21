using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BalasHud : MonoBehaviour
{
    public Image imagenBalas;
    public Sprite[] spritesBalas;
    public float tiempoRecarga = 3.16f; 

    private int balasRestantes = 3;
    private bool recargando = false;
    private int disparosRealizados = 0; 



    void Start()
    {
        ActualizarImagenBalas();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !recargando)
        {
            if (balasRestantes > 0)
            {
                balasRestantes--;
                disparosRealizados++;

                ActualizarImagenBalas();

                if (disparosRealizados >= 3)
                {
                    StartCoroutine(RecargarBalas());
                }
            }
        }
    }
    IEnumerator RecargarBalas()
    {
        recargando = true;
        yield return new WaitForSeconds(tiempoRecarga);
        balasRestantes = 3;
        disparosRealizados = 0; 
        ActualizarImagenBalas();
        recargando = false;
    }

    void ActualizarImagenBalas()
    {
        int indiceSprite = Mathf.Clamp(balasRestantes, 0, spritesBalas.Length - 1);
        imagenBalas.sprite = spritesBalas[indiceSprite];
    }
    public bool IsRecargando { get; private set; }
    public void IniciarRecarga()
    {
        IsRecargando = true;
        StartCoroutine(TerminarRecarga());
    }

    private IEnumerator TerminarRecarga()
    {
        yield return new WaitForSeconds(tiempoRecarga); 
        IsRecargando = false;
    }
}