using UnityEngine;

enum PaintingState
{
    disabled,
    fading,
};

public class PaintingColliderDetector : MonoBehaviour
{
    public Transform GameManagerHolder;
    public Material LightMaterial;
    public Material BaseMaterial;

    private readonly string TAG_NAME = "Arrow";
    private readonly float FadingDuration = 2.0f;
    private PaintingState state;
    private ArrowGameManager GameManagerScript;
    private Material[] mats;

    private void Awake()
    {
        state = PaintingState.disabled;
        GameManagerScript = GameManagerHolder.GetComponent<ArrowGameManager>();
    }

    private void Update()
    {
        switch (state)
        {
            case PaintingState.disabled:
                mats = GetComponent<Renderer>().materials;
                mats[1] = BaseMaterial;
                GetComponent<Renderer>().materials = mats;
                break;

            case PaintingState.fading:
                float lerp = Mathf.PingPong(Time.time, FadingDuration) / FadingDuration;
                GetComponent<Renderer>().materials[1].Lerp(LightMaterial, BaseMaterial, lerp);
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (state != PaintingState.disabled && other.tag == TAG_NAME)
        {
            Debug.Log("Arrow dectected!");
            GameManagerScript.AnnounceDetectedArrow(this.name);
        }
    }

    public void Enable()
    {
        state = PaintingState.fading;
    }

    public void Disable()
    {
        state = PaintingState.disabled;
    }
}
