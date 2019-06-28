using UnityEngine;

public class CollisionIgnorer : MonoBehaviour
{
    public GameObject ignoredObject;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == ignoredObject.name)
        {
            Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
        }
    }
}