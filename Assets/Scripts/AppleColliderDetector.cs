using UnityEngine;

public class AppleColliderDetector : MonoBehaviour
{
    public Transform door;

    private DoorCollider door_collider_script;
    private readonly string TAG_NAME = "Apple";

    private void Awake()
    {
        door_collider_script = door.GetComponent<DoorCollider>();
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
