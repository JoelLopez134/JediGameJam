using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ImageUIDestroyer : MonoBehaviour
{
    // Lista de imágenes de interfaz de usuario que se eliminarán
    public List<Image> imagesUI = new List<Image>();

    // Índice que rastrea la posición actual en la lista de imágenes
    private int currentIndex = 0;

    // Duración del desvanecimiento
    public float fadeDuration = 1.0f;

    // Nombre de la siguiente escena a cargar después de eliminar todas las imágenes
    public string nextSceneName = "NombreDeLaSiguienteEscena";

    // Método para remover una imagen
    public void RemoveImage()
    {
        // Verificar que el índice actual no exceda el tamaño de la lista de imágenes
        if (currentIndex < imagesUI.Count)
        {
            // Obtener la imagen a eliminar
            Image imageToRemove = imagesUI[currentIndex];

            // Iniciar el desvanecimiento
            StartCoroutine(FadeAndDestroy(imageToRemove.gameObject));

            // Incrementar el índice para apuntar a la próxima imagen
            currentIndex++;

            // Verificar si se han eliminado todas las imágenes
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

        // Verificar si se encontró un componente Image
        if (image != null)
        {
            // Obtener la alpha inicial de la imagen
            float startAlpha = image.color.a;
            float startTime = Time.time;

            // Iterar mientras el tiempo transcurrido sea menor que la duración del desvanecimiento
            while (Time.time - startTime < fadeDuration)
            {
                // Calcular el tiempo transcurrido
                float elapsedTime = Time.time - startTime;

                // Calcular el nuevo valor de alpha mediante interpolación lineal
                float alpha = Mathf.Lerp(startAlpha, 0f, elapsedTime / fadeDuration);

                // Crear un nuevo color con la alpha actualizada
                Color newColor = image.color;
                newColor.a = alpha;
                image.color = newColor;

                // Pausar la ejecución de la corutina hasta el siguiente frame
                yield return null;
            }

            // Asegurarse de que la alpha sea 0 al final del desvanecimiento
            Color finalColor = image.color;
            finalColor.a = 0f;
            image.color = finalColor;

            // Destruir el objeto después del desvanecimiento
            Destroy(gameObjectToFade);
        }
    }
}
