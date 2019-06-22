using UnityEngine;
using VRTK;

public class ArrowDetector : MonoBehaviour
{
    public GameObject DialogManagerHolder;

    private readonly string ARROW_TAG = "Arrow";
    private DialogManager dialogManagerScript;
   
    private void Awake()
    {
        if (DialogManagerHolder)
        {
            dialogManagerScript = DialogManagerHolder.GetComponent<DialogManager>();
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == ARROW_TAG)
        {
            if (dialogManagerScript != null)
            {
                dialogManagerScript.ShowErrorDialog();
            }
        }
    }
}
