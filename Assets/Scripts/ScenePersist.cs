using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenePersist : MonoBehaviour
{
    void Awake()
    {
        int numScenePersists = FindObjectsOfType<ScenePersist>().Length;
        if (numScenePersists > 1)
        {
            // GO se destruirá cuando morimos o saltamos a otra escena (cuando hay más de 2 GS)
            Destroy(gameObject);
        }
        else
        {
            // Si solo hay un GS no será destruido por Unity
            DontDestroyOnLoad(gameObject);
        }
    }

    // Reinicia la persistencia de la escena (restaura todo)
    public void ResetScenePersist()
    {
        Destroy(gameObject);
    }
}
