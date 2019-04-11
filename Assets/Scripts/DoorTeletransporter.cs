using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DoorTeletransporter : MonoBehaviour
{
    public string scene_name;
    public float fadingTime = 10.0f;
    public GameObject LockIcon;
    public bool IsExitDoor = false;

    private bool collider_enabled = true;

    private void Start()
    {
        collider_enabled = true;
    }

    void Update()
    {
        if (IsExitDoor && Input.GetKeyDown(KeyCode.Space)) // Next scene
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
                SceneManager.LoadScene(scene_name, mode);
            }
        }
    }

    public void Enable()
    {
        collider_enabled = true;

        if (LockIcon != null)
        {
            LockIcon.SetActive(false);
        }
    }

    public void Disable()
    {
        collider_enabled = false;

        if (LockIcon != null)
        {
            LockIcon.SetActive(true);
        }
    }
}
