using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelController : MonoBehaviour
{
    public RoundData[] rounds;
    private int currentRound = 0;
    private int prefabsToDestroyInRound = 0; // Cantidad de prefabs que deben ser destruidos en la ronda actual
    private float timeBetweenRounds = 5.0f;
    private float timeBetweenScenes = 7.0f;

    public BoxCollider2D spawnArea;
    public TextMeshProUGUI roundText;

    private void Start()
    {
        StartRound();
    }

    void StartRound()
    {
        if (currentRound >= rounds.Length)
        {
            // Todas las rondas han terminado, cambiar de escena
            StartCoroutine(ChangeSceneAfterDelay(timeBetweenScenes));
            return;
        }

        RoundData round = rounds[currentRound];
        roundText.text = "Ronda: " + (currentRound + 1);

        prefabsToDestroyInRound = round.numberOfPrefabs;
        GeneratePrefabs(round);
    }

    void GeneratePrefabs(RoundData round)
    {
        StartCoroutine(GeneratePrefabsCoroutine(round));
    }

    IEnumerator GeneratePrefabsCoroutine(RoundData round)
    {
        int prefabsGenerated = 0;
        while (prefabsGenerated < round.numberOfPrefabs)
        {
            if (spawnArea != null)
            {
                Vector2 spawnPosition = new Vector2(
                    Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x),
                    Random.Range(spawnArea.bounds.min.y, spawnArea.bounds.max.y)
                );

                GameObject prefabToGenerate = round.prefabTypes[Random.Range(0, round.prefabTypes.Length)];
                Instantiate(prefabToGenerate, spawnPosition, Quaternion.identity);

                prefabsGenerated++;
                yield return new WaitForSeconds(0.1f); // Intervalo entre la generación de prefabs
            }
            else
            {
                Debug.LogError("No se ha asignado el BoxCollider2D en el Inspector del LevelController.");
            }
        }
    }

    public void PrefabDestroyed()
    {
        prefabsToDestroyInRound--;
        if (prefabsToDestroyInRound <= 0)
        {
            // La ronda actual ha terminado
            currentRound++;
            roundText.text = "Cambiando de ronda...";
            StartCoroutine(StartNextRoundAfterDelay(timeBetweenRounds));
        }
    }

    IEnumerator StartNextRoundAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartRound();
    }

    IEnumerator ChangeSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("No hay más escenas disponibles. Has completado el juego.");
            // Puedes hacer algo adicional aquí, como mostrar un mensaje de victoria.
        }
    }
}
