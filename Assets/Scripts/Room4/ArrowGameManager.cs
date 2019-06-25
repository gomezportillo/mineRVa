using UnityEngine;

public class ArrowGameManager : MonoBehaviour
{
    public Transform[] paintings;
    public int MaxShoots = 10;
    public GameObject dialogManagerHolder;

    private DialogManager dialogManagerScript;
    public GameObject exitDoor;

    private int successfulShots = 0;
    private int currentIndex = 0;
    private PaintingColliderDetector[] painting_scripts;
    private DoorTeletransporter doorTeletransporterScript;
    private bool started = false;

    void Start()
    {
        // Retrieve the scripts from the paintings
        painting_scripts = new PaintingColliderDetector[paintings.Length];

        for (int i = 0; i < paintings.Length; i++)
        {
            painting_scripts[i] = paintings[i].GetComponent<PaintingColliderDetector>();
        }

        // Disable the exit door collider
        if (exitDoor != null)
        {
            doorTeletransporterScript = exitDoor.GetComponent<DoorTeletransporter>();
            if (doorTeletransporterScript != null)
            {
                doorTeletransporterScript.Disable();
            }
        }

        if (dialogManagerHolder != null)
        {
            dialogManagerScript = dialogManagerHolder.GetComponent<DialogManager>();
        }
    }

    // Method to be used by the paintings to inform about an arrow collision
    public void AnnounceDetectedArrow(string painting_name)
    {
        Debug.Log(painting_name);
        painting_scripts[currentIndex].Disable();
        successfulShots++;

        if (successfulShots >= MaxShoots)
        {
            Win();
        }
        else
        {
            currentIndex = GenerateNewIndex();
            painting_scripts[currentIndex].Enable();
        }
    }

    // When the bow is picked up if informs the game to start
    public void Enable()
    {
        if (!started)
        {
            currentIndex = GenerateNewIndex();
            painting_scripts[currentIndex].Enable();
            started = true;
        }
    }

    // Get a new painting index to be the next one to be shooted
    private int GenerateNewIndex()
    {
        int new_random = currentIndex;

        if (paintings.Length > 1)
        {
            while (new_random == currentIndex)
            {
                new_random = Random.Range(0, paintings.Length);
            }
            return new_random;
        }
        else
        {
            return new_random;
        }
    }

    private void Win()
    {
        // make all painting blink yellow
        for (int i = 0; i < painting_scripts.Length; i++)
        {
            painting_scripts[i].Win();
        }

        if (doorTeletransporterScript != null)
        {
            doorTeletransporterScript.Enable();
        }

        if (dialogManagerScript != null)
        {
            dialogManagerScript.ShowWinDialog();
        }
    }
}
