using UnityEngine;
using UnityEngine.SceneManagement;

public class BaseSceneLoader : MonoBehaviour
{
    protected string baseSceneName = "Base_room";

    void Start()
    {
        if (!SceneManager.GetSceneByName(baseSceneName).isLoaded)
        {
            SceneManager.LoadScene(baseSceneName, LoadSceneMode.Additive);
        }
    }
}
