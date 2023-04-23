using UnityEngine;
public static class PlayerInput
{
    public static bool Right => Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D);
    public static bool Left => Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A);
    public static bool Down => Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S);
    public static bool Rotate => Input.GetKeyDown(KeyCode.Space);
}
