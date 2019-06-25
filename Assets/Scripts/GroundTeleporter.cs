using UnityEngine;

public class GroundTeleporter : MonoBehaviour
{
    public string GroundTag = "Ground";
    public Transform instance;

    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Vector3 initialVelocity;
    private Vector3 initialAngularVelocity;

    private void Awake()
    {
        initialPosition = instance.position;
        initialRotation = instance.rotation;
        initialVelocity = new Vector3(0, 0, 0);
        initialAngularVelocity = new Vector3(0, 0, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == GroundTag && instance != null)
        {
            Debug.Log("Teleporting object " + instance.name + " to initial position");

            instance.position = initialPosition;
            instance.rotation = initialRotation;
            instance.GetComponent<Rigidbody>().velocity = initialVelocity;
            instance.GetComponent<Rigidbody>().angularVelocity = initialAngularVelocity;
        }
    }
}
