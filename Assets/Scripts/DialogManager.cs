using System.IO;
using UnityEngine;
using VRTK;
using TMPro;
using System;

public enum SpeakingState
{
    NOT_STARTED,
    SPEAKING,
    WAITING_INPUT,
    PAUSED,
    FINISHED,
}

public class DialogManager : MonoBehaviour
{
    public string RoomName;
    public float TimeBetweetLetters = 0.05f;

    public VRTK_ControllerEvents rightControllerEvents;
    public VRTK_ControllerEvents leftControllerEvents;

    public AudioClip manVoice, womanVoice, ringSound;

    public GameObject DialogBackground;
    public GameObject NextDialogIcon;
    public TextMeshProUGUI DialogObject;

    public GameObject Guard;
    public GameObject finalDialogActivator;
    public GameObject DoorToUnlock;

    private int currentFileCounter;
    private int currentLetterIndex;
    private string currentDialog;
    private float lastDeltaTime = 0f;
    private SpeakingState speaking;

    private readonly string BASE_PATH = "Assets/Text/Dialogs/";
    private readonly string PLAYER_TAG = "[BodyColliderContainer]";
    private bool suspiciousGuard;
    private bool confessingGuard;

    private void Awake()
    {
        currentFileCounter = -1;
        currentLetterIndex = 0;
        speaking = SpeakingState.NOT_STARTED;
        suspiciousGuard = false;
        confessingGuard = false;

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
        UpdateDialogs();
        UpdateGuardPosition();
    }

    private void UpdateDialogs()
    {
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

    private void UpdateGuardPosition()
    {
        // rotate guard due to animation
        if (Guard)
        {
            Guard.transform.Rotate(0f, -0.0025f, 0f, Space.Self);
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
            GetComponent<AudioSource>().Stop();
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
                    if (!suspiciousGuard)
                    {
                        TriggerGuardAnimation("Greetings");
                    }
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
        DialogFinished();
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
        TriggerGuardAnimation("Yes");
    }

    public void ShowErrorDialog()
    {
        currentDialog = getDialogFileContent("error");
        StartSpeaking();
        TriggerGuardAnimation("No");
    }

    public void ShowCustomDialog(string file_name)
    {
        currentDialog = getDialogFileContent(file_name);
        StartSpeaking();

        if (file_name.Contains("error") && !suspiciousGuard)
        {
            TriggerGuardAnimation("No");
        }

        if (suspiciousGuard && file_name.Contains("error2"))
        {
            currentFileCounter++;
        }
    }

    private bool LoadNextDialog()
    {
        currentFileCounter++;
        string file_name;

        if (confessingGuard)
        {
            file_name = "confess_" + currentFileCounter.ToString();
        }
        else
        {
            file_name = currentFileCounter.ToString();
        }

        currentDialog = getDialogFileContent(file_name);
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

            file_content = CleanDialogString(file_content);
        }
        else
        {
            speaking = SpeakingState.FINISHED;
        }

        PlaySoundEffect(file_content);

        currentLetterIndex = 0;
        return file_content;
    }

    private void PlaySoundEffect(string dialog)
    {
        if (dialog != null)
        {
            AudioSource audioSource = GetComponent<AudioSource>();
            audioSource.Stop();

            if (dialog.ToLower().Contains("ring"))
            {
                audioSource.PlayOneShot(ringSound, 0.6f);
            }
            else if (RoomName == "room0" || RoomName == "room8")
            {
                audioSource.GetComponent<AudioSource>().PlayOneShot(womanVoice, 0.4f);
            }
            else
            {
                audioSource.GetComponent<AudioSource>().PlayOneShot(manVoice, 0.5f);
            }
        }
    }

    private string CleanDialogString(string str)
    {
        if (str.Substring(0, str.Length - 1).Equals('\n'))
        {
            str.Remove(str.Length - 1);
        }
        return str;
    }

    private void TriggerGuardAnimation(string anim_name)
    {
        if (Guard)
        {
            Animator animator = Guard.GetComponent<Animator>();
            if (animator)
            {
                animator.CrossFade(anim_name, 0.5f);
            }
        }
    }

    public void ActivateSuspiciousAnimation()
    {
        suspiciousGuard = true;

        Animator animator = Guard.GetComponent<Animator>();
        if (animator)
        {
            animator.CrossFade("Suspicious", 0.5f);
            animator.SetBool("isIdle", false);
            animator.SetBool("isSuspicious", true);
        }
    }

    public void ActivateConfession()
    {
        confessingGuard = true;
        currentFileCounter = 0;
    }

    private void DialogFinished()
    {
        switch (RoomName)
        {
            case "room7":
                if (confessingGuard && finalDialogActivator != null)
                {
                    finalDialogActivator.GetComponent<FinalDialogActivator>().GuardHasEndedConfessing();
                }
                break;

            case "room8":
                if (DoorToUnlock != null)
                {
                    DoorToUnlock.GetComponent<DoorTeletransporter>().Enable();
                }
                break;
        }
    }
}
