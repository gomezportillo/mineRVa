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

    private int CurrentPieces;
    private int CorrectPieces;
    private bool CorrectlyFinished;
    private string SelfName;

    private PaintingPiecesGameManager GameManagerScript;
    public VRTK_SnapDropZone[] SnapDropZones;

    private readonly int MAX_PIECES = 4;

    void Awake()
    {
        CurrentPieces = CorrectPieces = 0;
        CorrectlyFinished = false;

        if (Bulb != null)
        {
            Light = Bulb.transform.GetChild(0);
            Light.gameObject.SetActive(false);
        }
        ChangeBulbColor(LightYellow);

        if (GameManager != null)
        {
            GameManagerScript = GameManager.GetComponent<PaintingPiecesGameManager>();
        }

        foreach (VRTK_SnapDropZone sdz in SnapDropZones)
        {
            sdz.ObjectSnappedToDropZone += OnPieceSnapped;
            sdz.ObjectUnsnappedFromDropZone += OnPieceUnsnapped;
        }

        SelfName = this.name.Split('_')[1].ToLower();
    }

    void OnDestroy()
    {
        foreach (VRTK_SnapDropZone sdz in SnapDropZones)
        {
            sdz.ObjectSnappedToDropZone -= OnPieceSnapped;
            sdz.ObjectUnsnappedFromDropZone -= OnPieceUnsnapped;
        }
    }

    internal void OnPieceSnapped(object sender, SnapDropZoneEventArgs e)
    {
        if (!CorrectlyFinished)
        {
            CurrentPieces++;

            string piece_name = e.snappedObject.name.Split('_')[0].ToLower();
            string sdz_number = Regex.Match(sender.ToString(), @"\d+").Value;
            string snapped_number = Regex.Match(e.snappedObject.name, @"\d+").Value;

            Debug.Log("Snapped piece #" + snapped_number + " in SDZ #" + sdz_number);

            bool correct = (piece_name == SelfName) && (sdz_number == snapped_number);
            if (correct)
            {
                CorrectPieces++;
                if (CorrectPieces == MAX_PIECES)
                {
                    CorrectlyFinished = true;
                    ChangeBulbColor(LightGreen);
                    GameManagerScript.PaintingFinished();

                    // locking pieces in place disabling its box colliders
                    foreach (VRTK_SnapDropZone sdz in SnapDropZones)
                    {
                        GameObject piece = sdz.GetCurrentSnappedObject();
                        if (piece != null)
                        {
                            piece.GetComponent<BoxCollider>().enabled = false;
                        }
                    }
                }
            }

            if (CurrentPieces == MAX_PIECES && !CorrectlyFinished)
            {
                ChangeBulbColor(LightRed);
            }
        }
    }

    internal void OnPieceUnsnapped(object sender, SnapDropZoneEventArgs e)
    {
        if (!CorrectlyFinished)
        {
            CurrentPieces--;

            string piece_name = e.snappedObject.name.Split('_')[0].ToLower();
            string sdz_number = Regex.Match(sender.ToString(), @"\d+").Value;
            string snapped_number = Regex.Match(e.snappedObject.name, @"\d+").Value;

            Debug.Log("Unsnapped piece #" + snapped_number + " in SDZ #" + sdz_number);

            bool correct = (piece_name == SelfName) && (sdz_number == snapped_number);
            if (correct)
            {
                CorrectPieces--;
            }

            // In case the light was red
            ChangeBulbColor(LightYellow);
        }
    }

    private void ChangeBulbColor(Material m)
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

    public void TurnOnLight()
    {
        if (Light != null)
        {
            Light.gameObject.SetActive(true);
        }
    }
}
