using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BobController : CharacterController
{
    public float MovmentForce = 1000.0f;
    public float MaxMovementSpeed = 5.0f;
    public float TurnSpeed = 150.0f;
    public float GroundRayCheck = 2.0f;
    public float JumpForce = 500.0f;
    
    private Transform OwnTransform;
    private Rigidbody Body;

    private void Awake()
    {
        OwnTransform = transform;
        Body = GetComponent<Rigidbody>();
    }

    public override void Move(Vector2 pInput)
    {
        Turn(pInput.x);
        MoveForward(pInput.y);
    }

    private void Turn(float pTurnInput)
    {
        float turnAmount = pTurnInput * TurnSpeed * Time.fixedDeltaTime;
        Body.AddTorque(OwnTransform.up * turnAmount);
        // OwnTransform.RotateAround(transform.position, OwnTransform.up, turnAmount);
    }

    private void MoveForward(float pForwardInput)
    {
        if (Body.velocity.magnitude > MaxMovementSpeed)
            return;
        Body.AddForce(transform.forward * (MovmentForce * pForwardInput * Time.fixedDeltaTime));
        // OwnTransform.position += ;
    }

    public override void Jump()
    {
        bool onGround = Physics.Raycast(OwnTransform.position, -OwnTransform.up, GroundRayCheck);
        if (!onGround)
            return;
        Body.AddForce(OwnTransform.up * JumpForce);
    }
}
