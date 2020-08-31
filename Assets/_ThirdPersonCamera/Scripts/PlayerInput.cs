using UnityEngine;
using UnityEngine.Serialization;

public class PlayerInput : MonoBehaviour
{
    public CharacterController CharacterController = null;
    public FollowCamera PlayerCamera = null;
    
    private InputMaster Controls = null;

    private void Awake()
    {
        Controls = new InputMaster();

        if (CharacterController == null)
        {
            CharacterController = GetComponent<CharacterController>();
        }

        if (PlayerCamera == null)
        {
            PlayerCamera = GameManager.PlayerCamera;
        }

        Controls.Player.Jump.performed += context => CharacterController.Jump();
    }

    private void FixedUpdate()
    {
        CharacterController.Move(Controls.Player.Movement.ReadValue<Vector2>());
        PlayerCamera.TurnCamera(Controls.Player.Look.ReadValue<Vector2>());
    }

    private void OnEnable()
    {
        Controls.Enable();
    }

    private void OnDisable()
    {
        Controls.Disable();
    }
}
