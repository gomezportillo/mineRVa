﻿using System.IO;
using UnityEngine;
using VRTK;
using TMPro;

// <summary>
// This script takes into account both if the player is near the painting and if
// they are pressing the controller button 2. If both are true, it will display the 
// information of the painting from a txt file, and will hide it on releasing the button.
// </summary>

public class PaintingDescriptionManager : MonoBehaviour
{
    public Languajes languaje = Languajes.Spanish;

    public string roomFolderName;
    public string descriptionFile;

    public VRTK_ControllerEvents rightControllerEvents;
    public VRTK_ControllerEvents leftControllerEvents;

    public GameObject AvailabilityIcon;
    public GameObject TextBackground;
    public TextMeshProUGUI TextObject;

    public AudioClip soundEffect;

    private readonly string BASE_PATH = "Assets/Text/Painting_descriptions/";
    private readonly string PLAYER_TAG = "[BodyColliderContainer]";

    private bool playerIsNear = false;
    private string paintingDescription;

    private void Awake()
    {
        // Hides the text canvas
        if (AvailabilityIcon != null)
        {
            AvailabilityIcon.SetActive(false);
        }

        if (TextBackground != null)
        {
            TextBackground.SetActive(false);
        }

        string file_path = BASE_PATH +
                           GetFolderFromLanguaje(languaje) +
                           roomFolderName + '/' + 
                           descriptionFile;

        if (System.IO.File.Exists(file_path) && TextObject != null)
        {
            StreamReader reader = new StreamReader(file_path);
            paintingDescription = reader.ReadToEnd();
            reader.Close();
        }
        else
        {
            Debug.Log("File " + descriptionFile + " not found on " + BASE_PATH);
        }
    }

    // REF. https://www.youtube.com/watch?v=W9mub3CvTvQ
    // REF. https://www.tangledrealitystudios.com/development-tips/awake-vs-start-vs-onenable-and-when-to-use-them/
    private void OnEnable()
    {
        //Assings a listener to a controller event
        rightControllerEvents.ButtonTwoPressed += ControllerEvents_ButtonTwoPressed;
        leftControllerEvents.ButtonTwoPressed += ControllerEvents_ButtonTwoPressed;

        rightControllerEvents.ButtonTwoReleased += ControllerEvents_ButtonTwoReleased;
        leftControllerEvents.ButtonTwoReleased += ControllerEvents_ButtonTwoReleased;
    }

    private void OnDisable()
    {
        //Removing the listeners from the controllers
        rightControllerEvents.ButtonTwoPressed -= ControllerEvents_ButtonTwoPressed;
        leftControllerEvents.ButtonTwoPressed -= ControllerEvents_ButtonTwoPressed;

        rightControllerEvents.ButtonTwoReleased -= ControllerEvents_ButtonTwoReleased;
        leftControllerEvents.ButtonTwoReleased -= ControllerEvents_ButtonTwoReleased;

    }
    private void ControllerEvents_ButtonTwoPressed(object sender, ControllerInteractionEventArgs e)
    {
        if (playerIsNear)
        {
            TextObject.text = paintingDescription;
            AvailabilityIcon.SetActive(false);
            TextBackground.SetActive(true);
            PlaySoundEffect();
        }
    }

    private void ControllerEvents_ButtonTwoReleased(object sender, ControllerInteractionEventArgs e)
    {
        TextBackground.SetActive(false);
        if (playerIsNear)
        {
            AvailabilityIcon.SetActive(true);
        }
    }

    private void PlaySoundEffect()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource)
        {
            audioSource.Stop();
            audioSource.PlayOneShot(soundEffect);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // When the player gets close to the painting
        if (other.name.Contains(PLAYER_TAG))
        {
            playerIsNear = true;
            AvailabilityIcon.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // When the player gets far from the painting
        if (other.name.Contains(PLAYER_TAG))
        {
            playerIsNear = false;
            AvailabilityIcon.SetActive(false);
            TextBackground.SetActive(false);
        }
    }

    private string GetFolderFromLanguaje(Languajes lang)
    {
        switch (lang)
        {
            case Languajes.Spanish:
                return "spanish/";
            default:
                return null;
        }
    }
}
