using UnityEngine;

public class IgnoreCollision : MonoBehaviour
{
    public Transform IgnoreWith;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == IgnoreWith.name)
        {
            Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
        }
    }
}