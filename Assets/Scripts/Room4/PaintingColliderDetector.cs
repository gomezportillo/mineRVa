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
    private float fading_lerp;
    private ArrowGameManager GameManagerScript;

    private void Awake()
    {
        state = PaintingState.Disabled;
        fading_lerp = 0;
        GameManagerScript = GameManagerHolder.GetComponent<ArrowGameManager>();
    }

    private void Update()
    {
        switch (state)
        {
            case PaintingState.Disabled:
                break;

            case PaintingState.FadingToDisabled:
                if (fading_lerp <= 1)
                {
                    fading_lerp += Time.deltaTime / FadingOutDuration;

                    GetComponent<Renderer>().materials[0].Lerp(SuccessMaterial,
                                                               PaintingMaterial,
                                                               fading_lerp);
                    GetComponent<Renderer>().materials[1].Lerp(SuccessMaterial,
                                                               FrameMaterial,
                                                               fading_lerp);
                }
                else
                {
                    state = PaintingState.Disabled;
                }
                break;

            case PaintingState.Enabled:
                fading_lerp = Mathf.PingPong(Time.time, FadingInDuration) / FadingInDuration;
                GetComponent<Renderer>().materials[1].Lerp(LightMaterial,
                                                           FrameMaterial,
                                                           fading_lerp);
                break;

            case PaintingState.Winning:
                GetComponent<Renderer>().materials[0].Lerp(WinMaterial,
                                                           PaintingMaterial,
                                                           fading_lerp);
                GetComponent<Renderer>().materials[1].Lerp(WinMaterial,
                                                           FrameMaterial,
                                                           fading_lerp);
                fading_lerp += Time.deltaTime / FadingOutDuration;
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (state != PaintingState.Disabled && other.tag == TAG_NAME)
        {
            Debug.Log("Arrow dectected!");
            GameManagerScript.AnnounceDetectedArrow(this.name);
        }
    }

    public void Enable()
    {
        state = PaintingState.Enabled;
    }

    public void Disable()
    {
        state = PaintingState.FadingToDisabled;
        fading_lerp = 0;
    }

    public void Win()
    {
        state = PaintingState.Winning;
        fading_lerp = 0;
    }
}
