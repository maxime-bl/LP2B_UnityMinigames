using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenuController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StartCoroutine(LoadScene_Game("SceneMenu"));
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            StartCoroutine(LoadScene_Game(SceneManager.GetActiveScene().name));
        }
    }

    IEnumerator LoadScene_Game(string scene_name)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene_name);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
