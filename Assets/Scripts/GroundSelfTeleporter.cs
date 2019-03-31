using UnityEngine;

public class GroundSelfTeleporter : MonoBehaviour
{
    public string GroundTag = "Ground";

    private Vector3 initial_position;
    private Quaternion initial_rotation;
    private Vector3 initial_velocity;
    private Vector3 initial_ang_velocity;

    private void Awake()
    {
        initial_position = transform.position;
        initial_rotation = transform.rotation;
        initial_velocity = new Vector3(0, 0, 0);
        initial_ang_velocity = new Vector3(0, 0, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == GroundTag)
        {
            Debug.Log("Teleporting object after touching ground...");

            transform.position = initial_position;
            transform.rotation = initial_rotation;
            transform.GetComponent<Rigidbody>().velocity = initial_velocity;
            transform.GetComponent<Rigidbody>().angularVelocity = initial_ang_velocity;
        }
    }
}
