using System.IO;
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
    public string roomFolderName;
    public string descriptionFile;
    public VRTK_ControllerEvents rightControllerEvents;
    public VRTK_ControllerEvents leftControllerEvents;
    public GameObject AvailabilityIcon;
    public GameObject TextBackground;
    public TextMeshProUGUI TextObject;

    private readonly string BASE_PATH = "Assets/Text/Painting_descriptions/";
    private readonly string PLAYER_TAG = "[BodyColliderContainer]";

    private bool player_near = false;
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

        // Reads the painting description and stores it on a variable
        string file_path = BASE_PATH + roomFolderName + '/' + descriptionFile;

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

    // REF. // REF. https://www.youtube.com/watch?v=W9mub3CvTvQ
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
        if (player_near)
        {
            TextObject.text = paintingDescription;
            AvailabilityIcon.SetActive(false);
            TextBackground.SetActive(true);
        }
    }

    private void ControllerEvents_ButtonTwoReleased(object sender, ControllerInteractionEventArgs e)
    {
        TextBackground.SetActive(false);
        if (player_near)
        {
            AvailabilityIcon.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // When the player gets close to the painting
        if (other.name.Contains(PLAYER_TAG))
        {
            player_near = true;
            AvailabilityIcon.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // When the player gets far from the painting
        if (other.name.Contains(PLAYER_TAG))
        {
            player_near = false;
            AvailabilityIcon.SetActive(false);
            TextBackground.SetActive(false);
        }
    }
}
