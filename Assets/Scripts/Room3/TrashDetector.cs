using UnityEngine;

public class TrashDetector : MonoBehaviour
{
    public Transform door;

    private DoorTeletransporter door_collider_script;
    private readonly string TAG_NAME = "Trash";

    private readonly int MAX_TRASH = 4;
    private int trash_counter = 0;

    private void Awake()
    {
        door_collider_script = door.GetComponent<DoorTeletransporter>();
        if (door_collider_script != null)
        {
            door_collider_script.Disable();
        }
        else
        {
            Debug.Log("Door Collider Script cannot be found on Door object");
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
                door_collider_script.Enable();
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
