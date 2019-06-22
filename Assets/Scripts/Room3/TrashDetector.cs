using UnityEngine;

public class TrashDetector : MonoBehaviour
{
    public GameObject door;
    public GameObject dialogManagerHolder;

    private DialogManager dialogManagerScript;
    private DoorTeletransporter doorTeletransporterScript;
    private readonly string TAG_NAME = "Trash";

    private readonly int MAX_TRASH = 4;
    private int trash_counter = 0;

    private void Start()
    {
        if (dialogManagerHolder != null)
        {
            dialogManagerScript = dialogManagerHolder.GetComponent<DialogManager>();
        }

        if (door != null)
        {
            doorTeletransporterScript = door.GetComponent<DoorTeletransporter>();
            if (doorTeletransporterScript != null)
            {
                doorTeletransporterScript.Disable();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == TAG_NAME)
        {
            Debug.Log("+1 trash");
            trash_counter++;

            if (trash_counter == MAX_TRASH)
            {
                doorTeletransporterScript.Enable();
                dialogManagerScript.ShowWinDialog();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == TAG_NAME)
        {
            trash_counter--;
        }
    }
}
