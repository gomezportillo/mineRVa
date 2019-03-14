using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleColliderDetector : MonoBehaviour
{
    public Transform door;

    private DoorCollider door_collider_script;

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
        if (other.tag == "Apple")
        {
            Debug.Log("entered");
            door_collider_script.Enable();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Apple")
        {
            Debug.Log("exit");
            door_collider_script.Disable();
        }

    }
}
