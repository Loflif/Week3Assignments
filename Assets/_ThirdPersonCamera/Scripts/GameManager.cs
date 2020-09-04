using UnityEngine;

[DefaultExecutionOrder(-100)]
public class GameManager : MonoBehaviour
{
    public static Camera PlayerCamera { get; private set; }

    public static Transform PlayerTransform { get; private set; }
    
    public static Vector3 PlayerRayAimHit { get; set; }
    public static Vector3 PlayerRayAimEnd { get; set; }

    public static bool LockCursor
    {
        set
        {
            if (value)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }
    
    private void Awake()
    {
        PlayerCamera = Camera.main;
        PlayerTransform = FindObjectOfType<PlayerInput>().transform;
        Cursor.visible = false;
    }

    private void OnEnable()
    {
        LockCursor = true;
    }

    private void OnDisable()
    {
        LockCursor = false;
    }
}
