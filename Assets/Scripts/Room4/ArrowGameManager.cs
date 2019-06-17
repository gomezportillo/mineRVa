using UnityEngine;

public class ArrowGameManager : MonoBehaviour
{
    public Transform[] paintings;
    public int MaxShoots = 6;

    public Transform Door;

    private int SuccessfulShots = 0;
    private int CurrentIndex = 0;
    private PaintingColliderDetector[] painting_scripts;
    private DoorTeletransporter door_script;
    private bool started = false;

    void Start()
    {
        // Retrieve the scripts from the paintings
        painting_scripts = new PaintingColliderDetector[paintings.Length];

        for (int i = 0; i < paintings.Length; i++)
        {
            painting_scripts[i] = paintings[i].GetComponent<PaintingColliderDetector>();
        }

        // Disable the exit door collider
        if (Door != null)
        {
            door_script = Door.GetComponent<DoorTeletransporter>();
            if (door_script != null)
            {
                door_script.Disable();
            }
        }

        //CurrentIndex = GenerateNewIndex();
        //painting_scripts[CurrentIndex].Enable();
    }

    void Update()
    {
    }

    // Method to be used by the paintings to inform about an arrow collision
    public void AnnounceDetectedArrow(string painting_name)
    {
        Debug.Log(painting_name);
        painting_scripts[CurrentIndex].Disable();
        SuccessfulShots++;

        if (SuccessfulShots >= MaxShoots)
        {
            for (int i = 0; i < painting_scripts.Length; i++)
            {
                painting_scripts[i].Win();
            }

            // Win. Unlock exit
            if (door_script != null)
            {
                door_script.Enable();
            }
        }
        else
        {
            CurrentIndex = GenerateNewIndex();
            painting_scripts[CurrentIndex].Enable();
        }
    }

    // When the bow is picked up if informs the game to start
    public void Enable()
    {
        if (!started)
        {
            CurrentIndex = GenerateNewIndex();
            painting_scripts[CurrentIndex].Enable();
            started = true;
        }
    }

    // Get a new painting index to be the next one to be shooted
    private int GenerateNewIndex()
    {
        int new_random = CurrentIndex;

        if (paintings.Length > 1)
        {
            while (new_random == CurrentIndex)
            {
                new_random = Random.Range(0, paintings.Length);
            }
            return new_random;
        }
        else
        {
            return new_random;
        }
    }
}
