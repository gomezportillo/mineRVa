using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public Button playButton;
    public Button creditsButton;
    public Button creditsBackButton;
    public Button exitButton;

    public GameObject menuBackground;
    public GameObject creditsBackground;

    public AudioClip soundEffect;

    private string FIRST_SCENE = "room0";

    private void Start()
    {
        if (playButton != null)
        {
            playButton.onClick.AddListener(OnClickPlay);
        }
        if (creditsButton != null)
        {
            creditsButton.onClick.AddListener(OnClickCredits);
        }
        if (creditsBackButton != null)
        {
            creditsBackButton.onClick.AddListener(OnClickCreditsBack);
        }
        if (exitButton != null)
        {
            exitButton.onClick.AddListener(OnClickExit);
        }

    }

    private void OnClickPlay()
    {
        PlaySoundEffect();
        SceneManager.LoadScene(FIRST_SCENE, LoadSceneMode.Single);
    }

    private void OnClickCredits()
    {
        PlaySoundEffect();
        menuBackground.SetActive(false);
        creditsBackground.SetActive(true);
    }

    private void OnClickCreditsBack()
    {
        PlaySoundEffect();
        creditsBackground.SetActive(false);
        menuBackground.SetActive(true);
    }

    private void OnClickExit()
    {
        PlaySoundEffect();
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    private void PlaySoundEffect()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.Stop();
            if (soundEffect != null)
            {
                audioSource.PlayOneShot(soundEffect);
            }
        }
    }
}
