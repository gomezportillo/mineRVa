using UnityEngine;

//REF. https://www.youtube.com/watch?v=6C4KfuW2q8Y

public class JardinDoorOpenner : MonoBehaviour
{
    public Transform door_left;
    public Transform door_right;

    private Vector3 left_open_position = new Vector3(1, 0, -9);
    private Vector3 right_open_position = new Vector3(-1, 0, -9);

    public float open_speed;

    private bool open = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            open = true;
        }

        if (open)
        {
            door_left.position = Vector3.Lerp(door_left.position,
                                              left_open_position,
                                              Time.deltaTime * open_speed);

            door_right.position = Vector3.Lerp(door_right.position,
                                               right_open_position,
                                               Time.deltaTime * open_speed);
        }
        
    }
}
