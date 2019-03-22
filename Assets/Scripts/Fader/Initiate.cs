using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// REF. https://assetstore.unity.com/packages/tools/particles-effects/simple-fade-scene-transition-system-81753

public static class Initiate
{
    static bool fading = false;

    public static void Fade(string scene, Color col, float multiplier)
    {
        if (fading)
        {
            return;
        }

        GameObject fader = new GameObject();
        fader.name = "Fader";
        Canvas myCanvas = fader.AddComponent<Canvas>();
        myCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        fader.AddComponent<Fader>();
        fader.AddComponent<CanvasGroup>();
        fader.AddComponent<Image>();

        Fader scr = fader.GetComponent<Fader>();
        scr.fadeDamp = multiplier;
        scr.fadeScene = scene;
        scr.fadeColor = col;
        scr.start = true;
        fading = true;
        scr.InitiateFader();
    }

    public static void DoneFading()
    {
        fading = false;
    }
}
