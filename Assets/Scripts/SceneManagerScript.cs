using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    public void CambiarEscena(string nombreDeEscena)
    {
        // Cambia a la escena especificada por nombre.
        SceneManager.LoadScene(nombreDeEscena);
    }

    public void CerrarJuego()
    {
        // Cierra la aplicación (juego) cuando se ejecuta en una plataforma que lo permite.
        Application.Quit();
    }
}
