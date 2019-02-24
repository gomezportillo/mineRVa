using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorCollider : MonoBehaviour
{
    public string Scene_name;

    void Start()
    {
    }

    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collisioning");
        SceneManager.LoadScene(Scene_name);
    }
}
