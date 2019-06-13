using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorTeletransporter : MonoBehaviour
{
    public string scene_name;
    public GameObject LockIcon;
    public bool IsExitDoor = false;

    private bool TeletransporterEnabled;

    private void Awake()
    {
        TeletransporterEnabled = true;
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
        if (TeletransporterEnabled)
        {
            MyLoadScene(scene_name, LoadSceneMode.Single);
        }
    }

    private void MyLoadScene(string scene_name, LoadSceneMode mode)
    {
        if (scene_name != "" && !SceneManager.GetSceneByName(scene_name).isLoaded)
        {
            SceneManager.LoadScene(scene_name, mode);
        }
    }

    public void Enable()
    {
        TeletransporterEnabled = true;

        if (LockIcon != null)
        {
            LockIcon.SetActive(false);
        }
    }

    public void Disable()
    {
        TeletransporterEnabled = false;

        if (LockIcon != null)
        {
            LockIcon.SetActive(true);
        }
    }
}
