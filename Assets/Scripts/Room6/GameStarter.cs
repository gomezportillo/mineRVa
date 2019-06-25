using UnityEngine;

public class GameStarter : MonoBehaviour
{
    public GameObject gameManagerHolder;

    private PaintingPiecesGameManager gameManagerScript;
    private readonly string PLAYER_TAG = "[BodyColliderContainer]";

    private void Awake()
    {
        gameManagerScript = gameManagerHolder.GetComponent<PaintingPiecesGameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains(PLAYER_TAG))
        {
            gameManagerScript.StartGame();
            GetComponent<SphereCollider>().enabled = false;
        }
    }
}
