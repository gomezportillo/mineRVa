using UnityEngine;
using VRTK;

public class TagSnapDetector : MonoBehaviour
{
    public Transform Door;
    public string APPLE_TAG;
    public GameObject DialogManagerHolder;

    private DialogManager dialogManagerScript;
    private DoorTeletransporter door_collider_script;
    private bool win;

    private void Awake()
    {
        if (Door != null)
        {
            door_collider_script = Door.GetComponent<DoorTeletransporter>();
            if (door_collider_script != null)
            {
                door_collider_script.Disable();
            }
            else
            {
                Debug.Log("Door Collider Script cannot be found on Door object");
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
            door_collider_script.Enable();

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
        door_collider_script.Disable();
    }
}
