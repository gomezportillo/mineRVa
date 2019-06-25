using UnityEngine;

public class GroundSelfTeleporter : MonoBehaviour
{
    public string GroundTag = "Ground";

    private Vector3 initial_position;
    private Quaternion initial_rotation;
    private Vector3 initialVelocity;
    private Vector3 initialAngularVelocity;

    private void Awake()
    {
        initial_position = transform.position;
        initial_rotation = transform.rotation;
        initialVelocity = new Vector3(0, 0, 0);
        initialAngularVelocity = new Vector3(0, 0, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == GroundTag)
        {
            Debug.Log("Teleporting object after touching ground...");

            transform.position = initial_position;
            transform.rotation = initial_rotation;
            transform.GetComponent<Rigidbody>().velocity = initialVelocity;
            transform.GetComponent<Rigidbody>().angularVelocity = initialAngularVelocity;
        }
    }
}
