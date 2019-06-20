using System.IO;
using UnityEngine;
using VRTK;
using TMPro;

public enum SpeakingState
{
    NOT_STARTED,
    SPEAKING,
    WAITING_INPUT,
    PAUSED,
    FINISHED
}

public class DialogManager : MonoBehaviour
{
    public string RoomName;
    public float TimeBetweetLetters = 0.05f;

    public VRTK_ControllerEvents rightControllerEvents;
    public VRTK_ControllerEvents leftControllerEvents;

    public GameObject DialogBackground;
    public GameObject NextDialogIcon;
    public TextMeshProUGUI DialogObject;

    public GameObject Guard;

    private int currentFileCounter;
    private int currentLetterIndex;
    private string currentDialog;
    private float lastDeltaTime = 0f;
    private SpeakingState speaking;

    private readonly string BASE_PATH = "Assets/Text/Dialogs/";
    private readonly string PLAYER_TAG = "[BodyColliderContainer]";

    private void Awake()
    {
        currentFileCounter = -1;
        currentLetterIndex = 0;
        speaking = SpeakingState.NOT_STARTED;

        DialogBackground.SetActive(false);
        NextDialogIcon.SetActive(false);
    }

    private void OnEnable()
    {
        rightControllerEvents.ButtonTwoReleased += ControllerEvents_MenuButton;
        leftControllerEvents.ButtonTwoReleased += ControllerEvents_MenuButton;
    }

    private void OnDisable()
    {
        rightControllerEvents.ButtonTwoReleased -= ControllerEvents_MenuButton;
        leftControllerEvents.ButtonTwoReleased -= ControllerEvents_MenuButton;
    }

    private void Update()
    {
        // rotate gurd due to animation
        Guard.transform.Rotate(0f, -0.005f, 0f, Space.Self);

        if (speaking == SpeakingState.SPEAKING)
        {
            lastDeltaTime += Time.deltaTime;
            if (lastDeltaTime > TimeBetweetLetters)
            {
                if (currentLetterIndex < currentDialog.Length)
                {
                    DialogObject.text = currentDialog.Substring(0, currentLetterIndex);
                    currentLetterIndex++;
                    lastDeltaTime = 0f;
                }
                else
                {
                    // dialog file has ended
                    speaking = SpeakingState.WAITING_INPUT;
                    NextDialogIcon.SetActive(true);
                }
            }
            else
            {
                // wait for the time between letters
                return;
            }
        }
        else
        {
            // currently not speaking
            return;
        }
    }

    private void ControllerEvents_MenuButton(object sender, ControllerInteractionEventArgs e)
    {
        if (speaking == SpeakingState.WAITING_INPUT)
        {
            if (LoadNextDialog())
            {
                StartSpeaking();
            }
            else
            {
                StopSpeaking();
            }
        }
        else if (speaking == SpeakingState.SPEAKING)
        {
            currentLetterIndex = currentDialog.Length - 1;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains(PLAYER_TAG))
        {
            if (speaking != SpeakingState.FINISHED)
            {
                if (speaking == SpeakingState.NOT_STARTED)
                {
                    Guard.GetComponent<Animator>().Play("Greetings");
                    LoadNextDialog();
                }

                StartSpeaking();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name.Contains(PLAYER_TAG))
        {
            if (speaking == SpeakingState.SPEAKING ||
            speaking == SpeakingState.WAITING_INPUT)
            {
                PauseSpeaking();
            }
        }
    }


    public void StartSpeaking()
    {
        DialogBackground.SetActive(true);
        NextDialogIcon.SetActive(false);
        speaking = SpeakingState.SPEAKING;
    }

    public void StopSpeaking()
    {
        DialogBackground.SetActive(false);
        NextDialogIcon.SetActive(false);
        speaking = SpeakingState.FINISHED;
    }

    public void PauseSpeaking()
    {
        DialogBackground.SetActive(false);
        NextDialogIcon.SetActive(false);
        speaking = SpeakingState.PAUSED;
        currentLetterIndex = 0;
    }

    public void ShowWinDialog()
    {
        currentDialog = getDialogFileContent("win");
        StartSpeaking();
    }

    public void ShowErrorDialog()
    {
        currentDialog = getDialogFileContent("error");
        StartSpeaking();
    }

    private bool LoadNextDialog()
    {
        currentFileCounter++;
        currentDialog = getDialogFileContent(currentFileCounter.ToString());

        return currentDialog != null;
    }

    private string getDialogFileContent(string file_name)
    {
        string file_path = BASE_PATH + RoomName + '/' + file_name + ".txt";
        string file_content = null;
        
        if (System.IO.File.Exists(file_path))
        {
            StreamReader reader = new StreamReader(file_path);
            file_content = reader.ReadToEnd();
            reader.Close();

            file_content = file_content.Remove(file_content.Length - 1);
        }
        else
        {
            speaking = SpeakingState.FINISHED;
        }

        currentLetterIndex = 0;
        return file_content;
    }
}
