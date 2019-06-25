using UnityEngine;
using VRTK;
using System.Text.RegularExpressions;

// This classs gets informed when a piece is placed and checks if it is correct.
// Then, turns the light accordingly and informs the game manager
public class PaintingChecker : MonoBehaviour
{
    public GameObject Bulb;
    public GameObject GameManager;

    public Material LightRed;
    public Material LightGreen;
    public Material LightYellow;

    private Transform Light;

    private int currentPieces;
    private int correctPieces;
    private bool isCorrectlyFinished;
    private string selfName;

    private PaintingPiecesGameManager gameManagerScript;
    public VRTK_SnapDropZone[] snapDropZones;

    private readonly int MAX_PIECES = 4;

    void Awake()
    {
        currentPieces = correctPieces = 0;
        isCorrectlyFinished = false;

        if (Bulb != null)
        {
            Light = Bulb.transform.GetChild(0);
            Light.gameObject.SetActive(false);
        }

        if (GameManager != null)
        {
            gameManagerScript = GameManager.GetComponent<PaintingPiecesGameManager>();
        }

        foreach (VRTK_SnapDropZone sdz in snapDropZones)
        {
            sdz.ObjectSnappedToDropZone += OnPieceSnapped;
            sdz.ObjectUnsnappedFromDropZone += OnPieceUnsnapped;
        }

        selfName = this.name.Split('_')[1].ToLower();
    }

    void OnDestroy()
    {
        foreach (VRTK_SnapDropZone sdz in snapDropZones)
        {
            sdz.ObjectSnappedToDropZone -= OnPieceSnapped;
            sdz.ObjectUnsnappedFromDropZone -= OnPieceUnsnapped;
        }
    }

    internal void OnPieceSnapped(object sender, SnapDropZoneEventArgs e)
    {
        if (!isCorrectlyFinished)
        {
            currentPieces++;

            string piece_name = e.snappedObject.name.Split('_')[0].ToLower();
            string sdz_number = Regex.Match(sender.ToString(), @"\d+").Value;
            string snapped_number = Regex.Match(e.snappedObject.name, @"\d+").Value;

            Debug.Log("Snapped piece #" + snapped_number + " in SDZ #" + sdz_number);

            bool correct = (piece_name == selfName) && (sdz_number == snapped_number);
            if (correct)
            {
                correctPieces++;
                if (correctPieces == MAX_PIECES)
                {
                    ChangeBulbColor(LightGreen);
                    isCorrectlyFinished = true;
                    gameManagerScript.PaintingFinished();

                    // locking pieces in place disabling its box colliders
                    foreach (VRTK_SnapDropZone sdz in snapDropZones)
                    {
                        GameObject piece = sdz.GetCurrentSnappedObject();
                        if (piece != null)
                        {
                            piece.GetComponent<BoxCollider>().enabled = false;
                            piece.transform.GetChild(0).GetComponent<BoxCollider>().enabled = false;
                        }
                    }
                }
            }

            if (currentPieces == MAX_PIECES && !isCorrectlyFinished)
            {
                ChangeBulbColor(LightRed);
            }
        }
    }

    internal void OnPieceUnsnapped(object sender, SnapDropZoneEventArgs e)
    {
        if (!isCorrectlyFinished)
        {
            currentPieces--;

            string piece_name = e.snappedObject.name.Split('_')[0].ToLower();
            string sdz_number = Regex.Match(sender.ToString(), @"\d+").Value;
            string snapped_number = Regex.Match(e.snappedObject.name, @"\d+").Value;

            Debug.Log("Unsnapped piece #" + snapped_number + " in SDZ #" + sdz_number);

            bool correct = (piece_name == selfName) && (sdz_number == snapped_number);
            if (correct)
            {
                correctPieces--;
            }

            // In case the light was red
            ChangeBulbColor(LightYellow);
        }
    }

    private void ChangeBulbColor(Material m)
    {
        if (!isCorrectlyFinished)
        {
            if (Bulb != null)
            {
                if (m != null)
                {
                    Material[] mats = Bulb.GetComponent<Renderer>().materials;
                    mats[2] = m;
                    Bulb.GetComponent<Renderer>().materials = mats;
                }
            }

            if (Light != null)
            {
                Light.gameObject.SetActive(true);
                Light.GetComponent<Light>().color = m.color;
            }
        }
    }

    public void TurnOnLight()
    {
        ChangeBulbColor(LightYellow);
    }
}
