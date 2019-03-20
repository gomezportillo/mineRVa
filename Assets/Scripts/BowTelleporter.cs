using UnityEngine;

public class BowTelleporter : MonoBehaviour
{
    public Transform Bow;

    private readonly string TAG_NAME = "Bow";

    private Vector3 initial_position;
    private Quaternion initial_rotation;
    private Vector3 initial_velocity;

    private void Awake()
    {
        initial_position = Bow.position;
        initial_rotation = Bow.rotation;
        initial_velocity = new Vector3(0, 0, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == TAG_NAME)
        {
            Debug.Log("Telleporting bow...");
            Bow.position = initial_position;
            Bow.rotation = initial_rotation;
            Bow.GetComponent<Rigidbody>().velocity = initial_velocity;
        }
    }
}
