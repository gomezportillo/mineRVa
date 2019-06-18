using UnityEngine;
using VRTK;

public class FinalDialogManager : MonoBehaviour
{
    public VRTK_InteractableObject brush;
    public VRTK_InteractableObject painting;

    void Awake()
    {
        if (brush)
        {
            brush.InteractableObjectGrabbed += BrushGrabbed;
        }

        if(painting)
        {
            painting.InteractableObjectGrabbed += PaintingGrabbed;

        }
    }

    private void BrushGrabbed(object sender, InteractableObjectEventArgs e)
    {
        Debug.Log("bush grabbed");
    }

    private void PaintingGrabbed(object sender, InteractableObjectEventArgs e)
    {
        Debug.Log("painting grabbed. GAME OVER!");
    }
}
