using UnityEngine;

public class BowInformer : MonoBehaviour
{
    public GameObject gameManagerHolder;

    private ArrowGameManager gameManagerScript;

    private void Awake()
    {
        gameManagerScript = gameManagerHolder.GetComponent<ArrowGameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name != "Table")
        {
            gameManagerScript.Enable();
        }
    }
}
