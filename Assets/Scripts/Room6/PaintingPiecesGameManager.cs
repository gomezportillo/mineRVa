using UnityEngine;

public class PaintingPiecesGameManager : MonoBehaviour
{
    public GameObject ExitDoor;
    public GameObject dialogManagerHolder;

    private DialogManager dialogManagerScript;
    private int currentFinishedPaintings;
    public GameObject[] paintingCheckers;

    private readonly int MAX_PAINTINGS = 4;
    private DoorTeletransporter doorTeletransporterScript;

    void Awake()
    {
        if (dialogManagerHolder)
        {
            dialogManagerScript = dialogManagerHolder.GetComponent<DialogManager>();
        }

        if (ExitDoor != null)
        {
            doorTeletransporterScript = ExitDoor.GetComponent<DoorTeletransporter>();
            if (doorTeletransporterScript != null)
            {
                doorTeletransporterScript.Disable();
            }
        }

        currentFinishedPaintings = 0;
    }

    public void StartGame()
    {
        foreach (GameObject script in paintingCheckers)
        {
            script.GetComponent<PaintingChecker>().TurnOnLight();
        }
    }

    public void PaintingFinished()
    {
        currentFinishedPaintings++;

        // The guard will encourage the player to stop
        ShowGuardErrorDialog(currentFinishedPaintings);

        if (currentFinishedPaintings == MAX_PAINTINGS)
        {
            if (doorTeletransporterScript != null)
            {
                doorTeletransporterScript.Enable();
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
