using UnityEngine;

public class TagColliderDetector : MonoBehaviour
{
    public Transform Door;
    public string TAG_NAME;

    private DoorTeletransporter door_collider_script;

    private void Awake()
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == TAG_NAME)
        {
            Debug.Log("entered");
            door_collider_script.Enable();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == TAG_NAME)
        {
            Debug.Log("exit");
            door_collider_script.Disable();
        }
    }
}
