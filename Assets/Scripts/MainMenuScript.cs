using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider slider;

    public void LoadScene(string levelName)
    {
		//UnityEngine.SceneManagement.SceneManager.LoadScene(levelName);
		StartCoroutine(LoadAsync(levelName));
    }

    IEnumerator LoadAsync (string levelName)
	{
        AsyncOperation operation = SceneManager.LoadSceneAsync(levelName);

        loadingScreen.SetActive(true);

        while(!operation.isDone)
        {
            //show loading progress
            float progress = Mathf.Clamp01(operation.progress/0.9f);
            //Debug.Log(progress);
            slider.value = progress;

            yield return null;
        }
	}

    public void ExitGame()
    {
        Application.Quit();
    }
}
