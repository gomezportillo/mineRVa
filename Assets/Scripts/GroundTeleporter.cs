using UnityEngine;

public class GroundTeleporter : MonoBehaviour
{
    public string GroundTag = "Ground";
    public Transform instance;

    private Vector3 initial_position;
    private Quaternion initial_rotation;
    private Vector3 initial_velocity;
    private Vector3 initial_ang_velocity;

    private void Awake()
    {
        initial_position = instance.position;
        initial_rotation = instance.rotation;
        initial_velocity = new Vector3(0, 0, 0);
        initial_ang_velocity = new Vector3(0, 0, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == GroundTag && instance != null)
        {
            Debug.Log("Teleporting object " + instance.name + " to initial position");

            instance.position = initial_position;
            instance.rotation = initial_rotation;
            instance.GetComponent<Rigidbody>().velocity = initial_velocity;
            instance.GetComponent<Rigidbody>().angularVelocity = initial_ang_velocity;
        }
    }
}
