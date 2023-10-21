using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ImageUIDestroyer : MonoBehaviour
{
    public List<Image> imagesUI = new List<Image>();
    private int currentIndex = 0;
    public float fadeDuration = 1.0f; // Duración del desvanecimiento
    public string nextSceneName = "NombreDeLaSiguienteEscena";

    public void RemoveImage()
    {
        if (currentIndex < imagesUI.Count)
        {
            Image imageToRemove = imagesUI[currentIndex];

            // Iniciar el desvanecimiento
            StartCoroutine(FadeAndDestroy(imageToRemove.gameObject));

            currentIndex++;

            // Verificar si se han eliminado todas las imágenes
            if (currentIndex >= imagesUI.Count)
            {
                // Cargar la siguiente escena
                SceneManager.LoadScene(nextSceneName);
            }
        }
    }

    private IEnumerator FadeAndDestroy(GameObject gameObjectToFade)
    {
        Image image = gameObjectToFade.GetComponent<Image>();
        if (image != null)
        {
            float startAlpha = image.color.a;
            float startTime = Time.time;

            while (Time.time - startTime < fadeDuration)
            {
                float elapsedTime = Time.time - startTime;
                float alpha = Mathf.Lerp(startAlpha, 0f, elapsedTime / fadeDuration);

                Color newColor = image.color;
                newColor.a = alpha;
                image.color = newColor;

                yield return null;
            }

            // Asegurarse de que la alpha sea 0 al final
            Color finalColor = image.color;
            finalColor.a = 0f;
            image.color = finalColor;

            // Destruir el objeto después del desvanecimiento
            Destroy(gameObjectToFade);
        }
    }
}
