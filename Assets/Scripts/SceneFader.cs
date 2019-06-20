using UnityEngine;
using UnityEngine.UI;

enum FaderState
{
    FadingToBlack,
    FadingFromBlack
};

public class SceneFader : MonoBehaviour
{
    public Image FaderImage;
    public float FadeSpeed = 1f;

    private bool finished = false;

    //private State State = State.FadingFromBlack;

    void Awake()
    {
        if (FaderImage != null)
        {
            FaderImage.GetComponent<Image>().color = new Color32(0, 0, 0, 255);
            FaderImage.enabled = true;
        }
        else
        {
            finished = true;
        }
    }

    void Update()
    {
        if (!finished)
        {
            FaderImage.GetComponent<Image>().color = Color.Lerp(FaderImage.GetComponent<Image>().color,
                                                                Color.clear,
                                                                FadeSpeed * Time.deltaTime);

            if (FaderImage.GetComponent<Image>().color.a < 0.05f)
            {
                FaderImage.enabled = false;
                finished = true;
            }
        }
    }
}
