using UnityEngine;

namespace BobsAdventure
{
    public class PlayerInput : MonoBehaviour
    {
        public CharacterController CharacterController = null;
        public OverShoulderCamera PlayerCamera = null;
    
        private InputMaster Controls = null;
    
        private void Awake()
        {
            Controls = new InputMaster();

            if (CharacterController == null)
            {
                CharacterController = GetComponent<CharacterController>();
            }

            PlayerCamera = GameManager.PlayerCamera.GetComponent<OverShoulderCamera>();
            Controls.Player.Jump.performed += context => CharacterController.Jump();
        }

        private void FixedUpdate()
        {
            CharacterController.Move(Controls.Player.Movement.ReadValue<Vector2>());
        }

        private void LateUpdate()
        {
            PlayerCamera.Aim(Controls.Player.Look.ReadValue<Vector2>());
            PlayerCamera.Zoom(Controls.Player.Zoom.ReadValue<float>());
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
}
