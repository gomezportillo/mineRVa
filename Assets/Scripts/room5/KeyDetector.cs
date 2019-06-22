using UnityEngine;
using VRTK;

public class KeyDetector : MonoBehaviour
{
    public GameObject Door;
    public string KEY_TAG;

    private DoorTeletransporter doorTeletransporterScript;

    private void Start()
    {
        if (Door != null)
        {
            doorTeletransporterScript = Door.GetComponent<DoorTeletransporter>();
            if (doorTeletransporterScript != null)
            {
                doorTeletransporterScript.Disable();
            }
            else
            {
                Debug.Log("Door Teletransporter Script cannot be found on object");
            }
        }
    }

    private void OnEnable()
    {
        GetComponent<VRTK_SnapDropZone>().ObjectSnappedToDropZone += OnKeySnapped;
    }

    private void OnDisable()
    {
        GetComponent<VRTK_SnapDropZone>().ObjectSnappedToDropZone -= OnKeySnapped;
    }

    internal void OnKeySnapped(object sender, SnapDropZoneEventArgs e)
    {
        if (e.snappedObject.tag == KEY_TAG)
        {
            if (doorTeletransporterScript != null)
            {
                doorTeletransporterScript.Enable();
            }
        }
    }
}
