using UnityEngine;

enum PaintingState
{
    Disabled,
    Enabled,
    FadingToDisabled,
    Winning
};

public class PaintingColliderDetector : MonoBehaviour
{
    public Transform GameManagerHolder;

    public Material LightMaterial;
    public Material SuccessMaterial;
    public Material WinMaterial;
    public Material PaintingMaterial;
    public Material FrameMaterial;

    private readonly string TAG_NAME = "Arrow";
    private readonly float FadingInDuration = 2.0f;
    private readonly float FadingOutDuration = 1.5f;

    private PaintingState state;
    private float fadingLerp;
    private ArrowGameManager gameManagerScript;

    private void Awake()
    {
        state = PaintingState.Disabled;
        fadingLerp = 0;
        gameManagerScript = GameManagerHolder.GetComponent<ArrowGameManager>();
    }

    private void Update()
    {
        switch (state)
        {
            case PaintingState.Disabled:
                break;

            case PaintingState.FadingToDisabled:
                if (fadingLerp <= 1)
                {
                    fadingLerp += Time.deltaTime / FadingOutDuration;

                    GetComponent<Renderer>().materials[0].Lerp(SuccessMaterial,
                                                               PaintingMaterial,
                                                               fadingLerp);
                    GetComponent<Renderer>().materials[1].Lerp(SuccessMaterial,
                                                               FrameMaterial,
                                                               fadingLerp);
                }
                else
                {
                    state = PaintingState.Disabled;
                }
                break;

            case PaintingState.Enabled:
                fadingLerp = Mathf.PingPong(Time.time, FadingInDuration) / FadingInDuration;
                GetComponent<Renderer>().materials[1].Lerp(LightMaterial,
                                                           FrameMaterial,
                                                           fadingLerp);
                break;

            case PaintingState.Winning:
                GetComponent<Renderer>().materials[0].Lerp(WinMaterial,
                                                           PaintingMaterial,
                                                           fadingLerp);
                GetComponent<Renderer>().materials[1].Lerp(WinMaterial,
                                                           FrameMaterial,
                                                           fadingLerp);
                fadingLerp += Time.deltaTime / FadingOutDuration;
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (state != PaintingState.Disabled && other.tag == TAG_NAME)
        {
            Debug.Log("Arrow dectected!");
            gameManagerScript.AnnounceDetectedArrow(this.name);
        }
    }

    public void Enable()
    {
        state = PaintingState.Enabled;
    }

    public void Disable()
    {
        state = PaintingState.FadingToDisabled;
        fadingLerp = 0;
    }

    public void Win()
    {
        state = PaintingState.Winning;
        fadingLerp = 0;
    }
}
