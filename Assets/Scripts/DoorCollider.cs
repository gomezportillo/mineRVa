using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorCollider : MonoBehaviour
{
    public string scene_name;
    public float fadingTime = 10.0f;

    private bool debug = true;

    private bool collider_enabled = true;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // Next scene
        {
            MyLoadScene(scene_name, LoadSceneMode.Single);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        MyLoadScene(scene_name, LoadSceneMode.Single);
    }

    private void MyLoadScene(string scene_name, LoadSceneMode mode)
    {
        if (collider_enabled)
        {
            if (scene_name != "" && !SceneManager.GetSceneByName(scene_name).isLoaded)
            {
                if (mode == LoadSceneMode.Additive)
                {
                    SceneManager.LoadScene(scene_name, mode);
                }
                else
                {
                    Initiate.Fade(scene_name, Color.black, fadingTime);
                }
            }
        }
    }

    public void Enable()
    {
        collider_enabled = true;
    }

    public void Disable()
    {
        collider_enabled = false;
    }
}
