using System;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

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

    public float YawSpeed = 30.0f;
    public float PitchSpeed = 30.0f;
    public Vector2 PitchLimit = new Vector2(-89.0f, 89.0f);
    
    public float AimRayLength = 100.0f;
    
    [NonSerialized] public Vector3 AimRayEnd;
    [NonSerialized] public Vector3 AimRayHit;
    
    private Rigidbody Rigidbody;
    private Transform OwnTransform;

    private void Awake()
    {
        OwnTransform = transform;
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
    }

    public override void Move(Vector2 pInput)
    {
        if (pInput.sqrMagnitude == 0)
            return;

        Vector3 headingDirection = new Vector3(pInput.x, 0, pInput.y);
        
        Body.forward = Vector3.ProjectOnPlane(Head.TransformDirection(headingDirection), Vector3.up).normalized;
        Rigidbody.AddForce(Body.forward * (MovementForce * Time.fixedDeltaTime));
    }

    public override void Aim(Vector2 pAimInput)
    {
        float yawAmount = YawSpeed * Time.fixedDeltaTime * pAimInput.x;
        float pitchAmount = PitchSpeed * Time.fixedDeltaTime * -pAimInput.y;

        Quaternion newRotation = Head.localRotation;
        newRotation *= Quaternion.Euler(pitchAmount, yawAmount, 0.0f);
        newRotation = ClampPitch(newRotation);
        Vector3 newEuler = newRotation.eulerAngles;
        newEuler.z = 0;
        // Head.localRotation *= Quaternion.Euler(pitchAmount, yawAmount, 0.0f);
        // Head.localRotation = ClampPitch(Head.localRotation);    
        Head.localRotation = Quaternion.Euler(newEuler);
        AimRay();
        AimGun();
    }

    private Quaternion ClampPitch(Quaternion pRotation)
    {
        pRotation.x /= pRotation.w;
        pRotation.y /= pRotation.w;
        pRotation.z /= pRotation.w;
        pRotation.w = 1f;
        
        float angleX = 2f * Mathf.Rad2Deg * Mathf.Atan(pRotation.x);
        angleX = Mathf.Clamp(angleX, PitchLimit.x, PitchLimit.y);
            
        pRotation.x = Mathf.Tan(Mathf.Deg2Rad * angleX * 0.5f);

        return pRotation;;
    }

    private void AimRay()
    {
        Vector3 headForward = Head.forward;
        Vector3 headPosition = Head.position;
        
        GameManager.PlayerRayAimEnd = headPosition + (headForward * AimRayLength);

        if (Physics.Raycast(headPosition, headForward, out RaycastHit hit, AimRayLength))
        {
            GameManager.PlayerRayAimHit = hit.point;
        }
        else
        {
            GameManager.PlayerRayAimHit = GameManager.PlayerRayAimEnd;
        }
        
        Debug.DrawRay(headPosition, headForward, Color.red);
    }

    private void AimGun()
    {
        Gun.rotation = Quaternion.LookRotation(GameManager.PlayerRayAimEnd - Gun.position);
        TargetLaser.SetPosition(0, Gun.position);
        TargetLaser.SetPosition(1, GameManager.PlayerRayAimEnd);
    }
    
    public override void Jump()
    {
        bool onGround = Physics.Raycast(OwnTransform.position, -OwnTransform.up, GroundRayCheck);
        if (!onGround)
            return;
        Rigidbody.AddForce(OwnTransform.up * JumpForce);
    }
}
