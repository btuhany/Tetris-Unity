using UnityEngine;
public static class PlayerInput
{
    public static bool Right => Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D);
    public static bool Left => Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A);
    public static bool RightRelease => Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D);
    public static bool LeftRelease => Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A);
    public static bool Down => Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S);
    public static bool Rotate => Input.GetKeyDown(KeyCode.Space);
}
