using UnityEngine;
using UnityEngine.Serialization;

public class PlayerInput : MonoBehaviour
{
    public CharacterController CharacterController = null;
    
    private InputMaster Controls = null;
    
    private void Awake()
    {
        Controls = new InputMaster();

        if (CharacterController == null)
        {
            CharacterController = GetComponent<CharacterController>();
        }

        Controls.Player.Jump.performed += context => CharacterController.Jump();
    }

    private void FixedUpdate()
    {
        CharacterController.Move(Controls.Player.Movement.ReadValue<Vector2>());
        CharacterController.Aim(Controls.Player.Look.ReadValue<Vector2>());
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
