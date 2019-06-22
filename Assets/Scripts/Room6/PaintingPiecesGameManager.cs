using UnityEngine;

public class PaintingPiecesGameManager : MonoBehaviour
{
    public GameObject ExitDoor;
    public GameObject dialogManagerHolder;

    private DialogManager dialogManagerScript;
    private int CurrentFinishedPaintings;
    public GameObject[] PaintingCheckers;

    private readonly int MAX_PAINTINGS = 4;
    private DoorTeletransporter DoorScript;

    void Awake()
    {
        if (dialogManagerHolder)
        {
            dialogManagerScript = dialogManagerHolder.GetComponent<DialogManager>();
        }

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

        // The guard will encourage the player to stop
        ShowGuardErrorDialog(CurrentFinishedPaintings);

        if (CurrentFinishedPaintings == MAX_PAINTINGS)
        {
            if (DoorScript != null)
            {
                DoorScript.Enable();
            }
        }
    }

    private void ShowGuardErrorDialog(int index)
    {
        if (dialogManagerScript)
        {
            string error_file = "error" + index.ToString();
            dialogManagerScript.ShowCustomDialog(error_file);
        }
    }
}
