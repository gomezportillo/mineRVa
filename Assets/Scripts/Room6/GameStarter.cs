using UnityEngine;

public class GameStarter : MonoBehaviour
{
    public Transform GameManagerHolder;

    private PaintingPiecesGameManager GameManagerScript;
    private readonly string PLAYER_TAG = "[BodyColliderContainer]";

    private void Awake()
    {
        GameManagerScript = GameManagerHolder.GetComponent<PaintingPiecesGameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains(PLAYER_TAG))
        {
            GameManagerScript.StartGame();
            GetComponent<SphereCollider>().enabled = false;
        }
    }
}
