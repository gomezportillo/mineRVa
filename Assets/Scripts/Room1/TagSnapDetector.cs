using UnityEngine;
using VRTK;

public class TagSnapDetector : MonoBehaviour
{
    public Transform Door;
    public string APPLE_TAG;
    public GameObject DialogManagerHolder;

    private DialogManager dialogManagerScript;
    private DoorTeletransporter doorTeletransporterScript;
    private bool win;

    private void Awake()
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

        if (DialogManagerHolder)
        {
            dialogManagerScript = DialogManagerHolder.GetComponent<DialogManager>();
        }

        win = false;
    }

    private void OnEnable()
    {
        GetComponent<VRTK_SnapDropZone>().ObjectSnappedToDropZone += OnFruitSnapped;
        GetComponent<VRTK_SnapDropZone>().ObjectUnsnappedFromDropZone += OnFruitUnsnapped;
    }

    private void OnDisable()
    {
        GetComponent<VRTK_SnapDropZone>().ObjectSnappedToDropZone -= OnFruitSnapped;
        GetComponent<VRTK_SnapDropZone>().ObjectUnsnappedFromDropZone -= OnFruitUnsnapped;
    }

    internal void OnFruitSnapped(object sender, SnapDropZoneEventArgs e)
    {
        if (e.snappedObject.tag == APPLE_TAG)
        {
            doorTeletransporterScript.Enable();

            if (!win && dialogManagerScript)
            {
                win = true;
                dialogManagerScript.ShowWinDialog();
            }
        }
        else if (dialogManagerScript)
        {
            dialogManagerScript.ShowErrorDialog();
        }
    }

    internal void OnFruitUnsnapped(object sender, SnapDropZoneEventArgs e)
    {
        doorTeletransporterScript.Disable();
    }
}
