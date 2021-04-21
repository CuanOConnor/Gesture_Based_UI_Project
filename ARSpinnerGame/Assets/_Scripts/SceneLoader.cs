using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : Singleton<SceneLoader>
{

    private string sceneToBeLoaded;

    public void LoadScene(string _sceneName)
    {
        sceneToBeLoaded = _sceneName;


        StartCoroutine(InitializeSceneLoading());
    }



    IEnumerator InitializeSceneLoading()
    {

        //First, we load the Loading scene
        yield return SceneManager.LoadSceneAsync("Scene_Loading");

        //Load the actual scene
        StartCoroutine(LoadActualScene());
      


    }

    IEnumerator LoadActualScene()
    {

        var asyncronousSceneLoading = SceneManager.LoadSceneAsync(sceneToBeLoaded);

        //this value stops the scene from displaying when it is still loading...
        asyncronousSceneLoading.allowSceneActivation = false;

        while (!asyncronousSceneLoading.isDone)
        {
            Debug.Log(asyncronousSceneLoading.progress);

            if (asyncronousSceneLoading.progress >= 0.9f )
            {
                //Finally, show the scene.
                asyncronousSceneLoading.allowSceneActivation = true; 
            }


            yield return null;

        }


        

    }


}
