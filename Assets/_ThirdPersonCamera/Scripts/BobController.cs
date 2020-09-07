using UnityEngine;

namespace BobsAdventure
{
[RequireComponent(typeof(Rigidbody))]
public class BobController : CharacterController
{
    [Header("Movement")]
    public float MovementForce = 1000.0f;
    public float GroundRayCheck = 2.0f;
    public float JumpForce = 500.0f;
    
    [Header("Aim")]
    [SerializeField] private Transform Head;
    [SerializeField] private Transform Body;

    [Header("Gun")] 
    [SerializeField] private Transform Gun;
    [SerializeField] private LineRenderer TargetLaser;

    private Rigidbody Rigidbody;
    private Transform OwnTransform;
    private Transform PlayerCamera;

    private void Awake()
    {
        OwnTransform = transform;
        PlayerCamera = GameManager.PlayerCamera.transform;
        Rigidbody = GetComponent<Rigidbody>();
        
        if (Head == null)
        {
            Head = transform;
            Debug.LogWarning("HeadParent was not set for " + gameObject.name);
        }
        if (Body == null)
        {
            Body = transform;
            Debug.LogWarning("BodyParent was not set for " + gameObject.name);
        }

        if (TargetLaser == null)
        {
            TargetLaser = GetComponentInChildren<LineRenderer>();
        }
        if (Gun == null)
        {
            Gun = transform;
        }
    }

    private void LateUpdate()
    {
        LookAtCrosshair();
        AimGun();
    }

    public override void Move(Vector2 pInput)
    {
        if (pInput.sqrMagnitude == 0)
            return;

        Vector3 headingDirection = new Vector3(pInput.x, 0, pInput.y);
        
        Body.forward = Vector3.ProjectOnPlane(PlayerCamera.TransformDirection(headingDirection), Vector3.up).normalized;
        Rigidbody.AddForce(Body.forward * (MovementForce * Time.fixedDeltaTime));
    }

    private void LookAtCrosshair()    
    {
        Head.transform.LookAt(GameManager.PlayerRayAimHit);
    }

    private void AimGun()
    {
        Gun.transform.LookAt(GameManager.PlayerRayAimHit);
        TargetLaser.SetPosition(0, TargetLaser.transform.position);
        TargetLaser.SetPosition(1, GameManager.PlayerRayAimHit);
    }
    
    public override void Jump()
    {
        bool onGround = Physics.Raycast(OwnTransform.position, -OwnTransform.up, GroundRayCheck);
        if (!onGround)
            return;
        Rigidbody.AddForce(OwnTransform.up * JumpForce);
    }
}
}

