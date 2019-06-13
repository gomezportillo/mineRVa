using UnityEngine;

public class PaintingPiecesGameManager : MonoBehaviour
{
    public GameObject ExitDoor;

    private int CurrentFinishedPaintings;
    public GameObject[] PaintingCheckers;

    private readonly int MAX_PAINTINGS = 4;
    private DoorTeletransporter DoorScript;

    void Awake()
    {
        // Disable the exit door collider
        if (ExitDoor != null)
        {
            DoorScript = ExitDoor.GetComponent<DoorTeletransporter>();
            if (DoorScript != null)
            {
                DoorScript.Disable();
            }
        }

        CurrentFinishedPaintings = 0;
    }

    public void StartGame()
    {
        foreach (GameObject script in PaintingCheckers)
        {
            script.GetComponent<PaintingChecker>().TurnOnLight();
        }
    }

    public void PaintingFinished()
    {
        CurrentFinishedPaintings++;
        Debug.Log("[GM] Painting finished");

        if(CurrentFinishedPaintings == MAX_PAINTINGS)
        {
            Debug.Log("[GM] Win!");

            if (DoorScript != null)
            {
                DoorScript.Enable();
            }
        }
    }
}
