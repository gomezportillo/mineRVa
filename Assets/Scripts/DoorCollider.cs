using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorCollider : MonoBehaviour
{
    public string scene_name;

    protected bool debug = true;

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
        if (debug)
        {
            Debug.Log(message: "Loading scene " + scene_name + " in mode " + mode);
        }

        if (scene_name != "" && !SceneManager.GetSceneByName(scene_name).isLoaded)
        {
            SceneManager.LoadScene(scene_name, mode);
        }
    }
}
