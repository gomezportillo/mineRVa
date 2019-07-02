using UnityEngine;
using VRTK;
using System.Text.RegularExpressions;
using System;

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

    private int MAX_PIECES;

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

        MAX_PIECES = snapDropZones.Length;
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

            string objectName = e.snappedObject.name;
            string zoneName = sender.ToString();

            if (PieceIsCorrect(objectName, zoneName))
            {
                correctPieces++;
                if (correctPieces == MAX_PIECES)
                {
                    isCorrectlyFinished = true;

                    ChangeBulbColor(LightGreen);
                    LockAllPiecesInPlace();

                    gameManagerScript.PaintingFinished();
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

            string objectName = e.snappedObject.name;
            string zoneName = sender.ToString();

            if (PieceIsCorrect(objectName, zoneName))
            {
                correctPieces--;
            }

            // In case the light was red
            ChangeBulbColor(LightYellow);
        }
    }


    public void TurnOnLight()
    {
        ChangeBulbColor(LightYellow);
    }

    private void ChangeBulbColor(Material m)
    {
        if (m != null)
        {
            if (Bulb != null)
            {
                    Material[] mats = Bulb.GetComponent<Renderer>().materials;
                    mats[2] = m;
                    Bulb.GetComponent<Renderer>().materials = mats;
            }

            if (Light != null)
            {
                Light.gameObject.SetActive(true);
                Light.GetComponent<Light>().color = m.color;
            }
        }
    }

    private void LockAllPiecesInPlace()
    {
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

    private bool PieceIsCorrect(string objectName, string zoneName)
    {
        string piece_name = objectName.Split('_')[0].ToLower();
        string sdz_number = Regex.Match(zoneName, @"\d+").Value;
        string snapped_number = Regex.Match(objectName, @"\d+").Value;

        bool correctPainting = piece_name == selfName;
        bool correctPosition = sdz_number == snapped_number;

        return correctPainting && correctPosition;
    }
}
