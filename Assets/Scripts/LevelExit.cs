using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [SerializeField] float levelLoadDelay;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
           StartCoroutine(LoadNextLevel());
        }
    }

    IEnumerator LoadNextLevel()
    {
        yield return new WaitForSecondsRealtime(levelLoadDelay);

        // var es un tipo de dato anónimo fuertemente tipado, es el compilador quien determina qué tipo de dato es
        // En este caso, antes sería: int currentSceneIndex = ...;
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Comprobamos si estamos en la última escena, de ser ese el caso volvemos a la escena 0
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }

        FindObjectOfType<ScenePersist>().ResetScenePersist();
        SceneManager.LoadScene(nextSceneIndex);
    }
}
