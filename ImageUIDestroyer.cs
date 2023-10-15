using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ImageUIDestroyer : MonoBehaviour
{
    // Lista de im�genes de interfaz de usuario que se eliminar�n
    public List<Image> imagesUI = new List<Image>();

    // �ndice que rastrea la posici�n actual en la lista de im�genes
    private int currentIndex = 0;

    // Duraci�n del desvanecimiento
    public float fadeDuration = 1.0f;

    // Nombre de la siguiente escena a cargar despu�s de eliminar todas las im�genes
    public string nextSceneName = "NombreDeLaSiguienteEscena";

    // M�todo para remover una imagen
    public void RemoveImage()
    {
        // Verificar que el �ndice actual no exceda el tama�o de la lista de im�genes
        if (currentIndex < imagesUI.Count)
        {
            // Obtener la imagen a eliminar
            Image imageToRemove = imagesUI[currentIndex];

            // Iniciar el desvanecimiento
            StartCoroutine(FadeAndDestroy(imageToRemove.gameObject));

            // Incrementar el �ndice para apuntar a la pr�xima imagen
            currentIndex++;

            // Verificar si se han eliminado todas las im�genes
            if (currentIndex >= imagesUI.Count)
            {
                // Cargar la siguiente escena
                SceneManager.LoadScene(nextSceneName);
            }
        }
    }

    // Corrutina para realizar el desvanecimiento gradual
    private IEnumerator FadeAndDestroy(GameObject gameObjectToFade)
    {
        // Obtener el componente Image de la imagen a desvanecer
        Image image = gameObjectToFade.GetComponent<Image>();

        // Verificar si se encontr� un componente Image
        if (image != null)
        {
            // Obtener la alpha inicial de la imagen
            float startAlpha = image.color.a;
            float startTime = Time.time;

            // Iterar mientras el tiempo transcurrido sea menor que la duraci�n del desvanecimiento
            while (Time.time - startTime < fadeDuration)
            {
                // Calcular el tiempo transcurrido
                float elapsedTime = Time.time - startTime;

                // Calcular el nuevo valor de alpha mediante interpolaci�n lineal
                float alpha = Mathf.Lerp(startAlpha, 0f, elapsedTime / fadeDuration);

                // Crear un nuevo color con la alpha actualizada
                Color newColor = image.color;
                newColor.a = alpha;
                image.color = newColor;

                // Pausar la ejecuci�n de la corutina hasta el siguiente frame
                yield return null;
            }

            // Asegurarse de que la alpha sea 0 al final del desvanecimiento
            Color finalColor = image.color;
            finalColor.a = 0f;
            image.color = finalColor;

            // Destruir el objeto despu�s del desvanecimiento
            Destroy(gameObjectToFade);
        }
    }
}
