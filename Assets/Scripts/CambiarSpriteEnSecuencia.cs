using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CambiarSpriteEnSecuencia : MonoBehaviour
{
    public List<Image> imagesUI = new List<Image>();
    public List<Sprite> spritesEnSecuencia = new List<Sprite>();

    private int indiceActual = 0;

    private void Start()
    {
        if (imagesUI.Count == 0 || spritesEnSecuencia.Count == 0)
        {
            Debug.LogError("Asegúrate de asignar al menos una imagen de UI y sprites en el script.");
            enabled = false; // Desactivar el script si no se han asignado imágenes de UI o sprites.
        }
    }

    public void CambiarSpriteSiguiente()
    {
        if (indiceActual < imagesUI.Count && indiceActual < spritesEnSecuencia.Count)
        {
            imagesUI[indiceActual].sprite = spritesEnSecuencia[indiceActual];
            indiceActual++;
        }
    }

    // Puedes llamar a este método desde otro script cuando un enemigo sea destruido
    public void OnEnemigoDestruido()
    {
        CambiarSpriteSiguiente();
    }
}