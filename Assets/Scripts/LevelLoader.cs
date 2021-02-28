using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;

    public float transitionTime = 1f;

    public void LoadNextLevel(int index)
    {
        StartCoroutine(LoadLevel(index));
    }

    public void LoadNextLevelAdditive(int index)
    {
        StartCoroutine(LoadLevelAdditive(index));
    }

    public void LoadNextIndexAdditive()
    {
        StartCoroutine(LoadLevelAdditive(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelIndex);
        if(SceneManager.GetSceneByBuildIndex(levelIndex).name == "Lobby")
        {
            SceneManager.LoadScene("PlayerInfo", LoadSceneMode.Additive); //Has UI and player stats
        }
    }

    IEnumerator BackToMenu()
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene("Menu");
    }

    IEnumerator LoadLevelAdditive(int levelIndex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        string oldSceneName = SceneManager.GetActiveScene().name;
        if(oldSceneName == "PlayerInfo")
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName("Lobby"));
            oldSceneName = SceneManager.GetActiveScene().name;
        }
        SceneManager.LoadScene(levelIndex, LoadSceneMode.Additive);
        yield return new WaitForSeconds(0.1f);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(SceneManager.GetSceneByBuildIndex(levelIndex).name));
        SceneManager.UnloadSceneAsync(oldSceneName);
    }
}
