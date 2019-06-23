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
        SceneManager.LoadScene(FIRST_SCENE, LoadSceneMode.Single);
    }

    private void OnClickCredits()
    {
        menuBackground.SetActive(false);
        creditsBackground.SetActive(true);
    }

    private void OnClickCreditsBack()
    {
        creditsBackground.SetActive(false);
        menuBackground.SetActive(true);
    }

    private void OnClickExit()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }
}
