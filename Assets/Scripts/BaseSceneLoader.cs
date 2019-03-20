using UnityEngine;
using UnityEngine.SceneManagement;

public class BaseSceneLoader : MonoBehaviour
{
    protected string base_scene_name = "Base_room";

    void Start()
    {
        if (!SceneManager.GetSceneByName(base_scene_name).isLoaded)
        {
            SceneManager.LoadScene(base_scene_name, LoadSceneMode.Additive);
        }
    }
}
