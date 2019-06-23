using UnityEngine;

public class DialogDelayer : MonoBehaviour
{
    public GameObject dialogCollider;
    public GameObject dialogBackground;
    public int timeToDelay;

    private void Awake()
    {
        if (dialogCollider != null)
        {
            dialogCollider.SetActive(false);
        }

        if (dialogBackground != null)
        {
            dialogBackground.SetActive(false);
        }
    }

    private void Start()
    {
        Invoke("ActivateDialogCollider", timeToDelay);
    }

    public void ActivateDialogCollider()
    {
        if (dialogCollider != null)
        {
            dialogCollider.SetActive(true);
        }
    }
}
