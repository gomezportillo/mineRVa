using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowColliderDetector : MonoBehaviour {

    private readonly string TAG_NAME = "Arrow";


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == TAG_NAME)
        {
            Debug.Log("Arrow dectected!");
        }
    }

}
