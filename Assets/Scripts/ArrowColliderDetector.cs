using UnityEngine;

public class ArrowColliderDetector : MonoBehaviour
{
    private readonly string TAG_NAME = "Arrow";
    private bool swapcolor = false;

    private void Awake()
    {
        GetComponent<Renderer>().material.color = Color.yellow;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == TAG_NAME)
        {
            Debug.Log("Arrow dectected!");
            swapcolor = !swapcolor;

            if (swapcolor)
            {
                GetComponent<Renderer>().material.color = Color.red;
            }
            else
            {
                GetComponent<Renderer>().material.color = Color.yellow;
            }
        }
    }

}
