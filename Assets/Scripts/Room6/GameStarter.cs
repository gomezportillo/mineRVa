using UnityEngine;

public class GameStarter : MonoBehaviour
{
    public Transform GameManagerHolder;

    private PaintingPiecesGameManager GameManagerScript;

    private void Awake()
    {
        GameManagerScript = GameManagerHolder.GetComponent<PaintingPiecesGameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Painting_piece" && other.tag != "Script")
        {
            GameManagerScript.StartGame();
            GetComponent<SphereCollider>().enabled = false;
        }
    }
}
