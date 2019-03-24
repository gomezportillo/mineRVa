using System.IO;
using UnityEngine;
using VRTK;
using TMPro;

public class DialogManager : MonoBehaviour
{
    public string RoomName;
    public int NumberOfDialogs = 0;
    public float TimeBetweetLetters = 0.05f;

    public VRTK_ControllerEvents rightControllerEvents;
    public VRTK_ControllerEvents leftControllerEvents;

    public GameObject DialogBackground;
    public TextMeshProUGUI DialogObject;

    private readonly string BASE_PATH = "Assets/Text/Dialogs/";
    private int currentFileCounter = 0;
    private string currentDialog;
    private bool finished = false;

    private int currentLetterIndex = 0;
    private float lastDeltaTime = 0f;

    private void Awake()
    {
        // Hides the text canvas
        DialogBackground.SetActive(false);

        if (NumberOfDialogs > 0)
        {
            currentDialog = getDialogFileContent();
        }
        else
        {
            finished = true;
        }
    }

    private void Update()
    {
        if (!finished)
        {
            DialogBackground.SetActive(true);

            lastDeltaTime += Time.deltaTime;
            if (lastDeltaTime > TimeBetweetLetters)
            {
                if (currentLetterIndex < currentDialog.Length)
                {
                    DialogObject.text = currentDialog.Substring(0, currentLetterIndex);
                    currentLetterIndex++;
                    lastDeltaTime = 0f;
                }
                else // dialog file has ended
                {
                    currentFileCounter++;
                    currentDialog = getDialogFileContent();
                    currentLetterIndex = 0;
                }
            }
            else
            {
                return;
            }
        }
        else
        {
            DialogBackground.SetActive(false);
            return;
        }
    }

    private string getDialogFileContent()
    {
        string file_path = BASE_PATH + RoomName + '/' + currentFileCounter.ToString() + ".txt";
        string file_content = null;

        if (System.IO.File.Exists(file_path))
        {
            StreamReader reader = new StreamReader(file_path);
            file_content = reader.ReadToEnd();
            reader.Close();
        }
        else
        {
            finished = true;
        }

        return file_content;
    }
}
