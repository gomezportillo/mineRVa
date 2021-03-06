﻿using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VRTK;

public class FinalDialogActivator : MonoBehaviour
{
    public VRTK_InteractableObject brush;
    public VRTK_InteractableObject painting;
    public VRTK_InteractableObject box;
    public GameObject dialogManagerHolder;
    public Image faderImage;

    private DialogManager dialogManagerScript;
    private bool isConfessed;
    private bool isFadingToBlack;
    private const int TIME_TO_FINAL_ROOM = 3;

    void Awake()
    {
        if (brush)
        {
            brush.InteractableObjectGrabbed += BrushGrabbed;
        }

        if (painting)
        {
            painting.InteractableObjectGrabbed += PaintingGrabbed;
        }

        if (box)
        {
            box.InteractableObjectGrabbed += BoxGrabbed;
        }

        if (dialogManagerHolder)
        {
            dialogManagerScript = dialogManagerHolder.GetComponent<DialogManager>();
        }

        if (faderImage != null)
        {
            faderImage.GetComponent<Image>().color = Color.clear;
        }
    }

    private void Start()
    {
        if (dialogManagerScript)
        {
            dialogManagerScript.ActivateSuspiciousAnimation();
        }
        isConfessed = false;
        isFadingToBlack = false;
    }

    private void Update()
    {
        if (isFadingToBlack)
        {
            faderImage.GetComponent<Image>().color = Color.Lerp(faderImage.GetComponent<Image>().color,
                                                                Color.black,
                                                                TIME_TO_FINAL_ROOM / 1.5f * Time.deltaTime);
        }
    }

    private void BrushGrabbed(object sender, InteractableObjectEventArgs e)
    {
        ShowGuardDialog("error-brush");
    }

    private void BoxGrabbed(object sender, InteractableObjectEventArgs e)
    {
        ShowGuardDialog("error-box");
    }

    private void PaintingGrabbed(object sender, InteractableObjectEventArgs e)
    {
        if (!isConfessed)
        {
            ShowGuardDialog("error-painting");
            dialogManagerScript.ActivateConfession();
            isConfessed = true;
        }
    }

    private void ShowGuardDialog(string error_file)
    {
        if (dialogManagerScript)
        {
            dialogManagerScript.ShowCustomDialog(error_file);
        }
    }

    public void GuardHasEndedConfessing()
    {
        Debug.Log("FINISHED CONFESSING");
        isFadingToBlack = true;
        Invoke("GoToFinalRoom", TIME_TO_FINAL_ROOM);
    }

    private void GoToFinalRoom()
    {
        SceneManager.LoadScene("room8", LoadSceneMode.Single);
        faderImage.enabled = true;
    }
}
