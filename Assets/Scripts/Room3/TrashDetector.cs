using System;
using UnityEngine;
using VRTK;

public class TrashDetector : MonoBehaviour
{
    public GameObject exitDoor;
    public GameObject dialogManagerHolder;
    public VRTK_SnapDropZone[] snapDropZones;

    private DialogManager dialogManagerScript;
    private DoorTeletransporter doorTeletransporterScript;
    private readonly string TRASH_TAG = "Trash";

    private readonly int MAX_TRASH = 4;
    private int trashCounter = 0;

    private void Start()
    {
        if (dialogManagerHolder != null)
        {
            dialogManagerScript = dialogManagerHolder.GetComponent<DialogManager>();
        }

        if (exitDoor != null)
        {
            doorTeletransporterScript = exitDoor.GetComponent<DoorTeletransporter>();
            if (doorTeletransporterScript != null)
            {
                doorTeletransporterScript.Disable();
            }
        }

        foreach (VRTK_SnapDropZone sdz in snapDropZones)
        {
            sdz.ObjectSnappedToDropZone += OnTrashSnapped;
            sdz.ObjectUnsnappedFromDropZone += OnTrashUnsnapped;
        }
    }

    void OnDestroy()
    {
        foreach (VRTK_SnapDropZone sdz in snapDropZones)
        {
            sdz.ObjectSnappedToDropZone -= OnTrashSnapped;
            sdz.ObjectUnsnappedFromDropZone -= OnTrashUnsnapped;
        }
    }

    internal void OnTrashSnapped(object sender, SnapDropZoneEventArgs e)
    {
        if (e.snappedObject.tag == TRASH_TAG)
        {
            Debug.Log("+1 trash");
            trashCounter++;

            if (trashCounter == MAX_TRASH)
            {
                doorTeletransporterScript.Enable();
                dialogManagerScript.ShowWinDialog();
            }
        }
    }

    private void OnTrashUnsnapped(object sender, SnapDropZoneEventArgs e)
    {
        if (e.snappedObject.tag == TRASH_TAG)
        {
            trashCounter--;
        }
    }
}
