using UnityEngine;

public class BowInformer : MonoBehaviour
{
    public Transform GameManagerHolder;

    private ArrowGameManager GameManagerScript;

    private void Awake()
    {
        GameManagerScript = GameManagerHolder.GetComponent<ArrowGameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name != "Table")
        {
            GameManagerScript.Enable();
        }
    }
}
