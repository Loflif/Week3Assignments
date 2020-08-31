using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform FollowTarget = null;
    public float YawSpeed = 150.0f;
    public float PitchSpeed = 150.0f;

    public float PositionLerpSpeed = 20.0f;
    public float RotationLerpSpeed = 30.0f;

    [SerializeField] private Vector3 StartOffset;

    private Transform DesiredOffset;
    private Transform OwnTransform;

    private Vector3 LookTarget;
    
    private void Awake()
    {
        OwnTransform = transform;
        DesiredOffset = OwnTransform;
        if (FollowTarget == null)
            FollowTarget = GameManager.PlayerTransform;
        DesiredOffset.position = StartOffset;
    }

    public void TurnCamera(Vector2 pLookInput)
    {
        // DesiredOffset.RotateAround(LookTarget, Vector3.up, pLookInput.x * YawSpeed * Time.fixedDeltaTime);
        // DesiredOffset.RotateAround(LookTarget, Vector3.right, pLookInput.y * PitchSpeed * Time.fixedDeltaTime);
    }
    
    private void FixedUpdate()
    {
        SetLookTarget();
        MoveTowardsDesiredPosition();
        LookAtTarget();
    }

    private void SetLookTarget()
    {
        LookTarget = FollowTarget.position + (Vector3.up * DesiredOffset.position.y);
    }

    private void MoveTowardsDesiredPosition()
    {
        Vector3 forwardOffsetDirection = Vector3.Normalize(Vector3.ProjectOnPlane(FollowTarget.forward, Vector3.up));
        Vector3 sidewaysOffsetDirection = Vector3.Normalize(Vector3.ProjectOnPlane(FollowTarget.right, Vector3.up));
        Vector3 desiredPosition = FollowTarget.position;

        Vector3 desiredPositionalOffset = DesiredOffset.position;
        
        desiredPosition += (desiredPositionalOffset.x * sidewaysOffsetDirection);
        desiredPosition += (desiredPositionalOffset.y * Vector3.up);
        desiredPosition += (desiredPositionalOffset.z * forwardOffsetDirection);
        
        OwnTransform.position = Vector3.Lerp(OwnTransform.position, desiredPosition, PositionLerpSpeed * Time.fixedDeltaTime);
    }
    

    private void LookAtTarget()
    {
        Vector3 lookDirection = Vector3.Normalize(LookTarget - transform.position);
        Quaternion desiredRotation = Quaternion.LookRotation(lookDirection, Vector3.up);
        OwnTransform.rotation = Quaternion.Lerp(OwnTransform.rotation, desiredRotation,
            RotationLerpSpeed * Time.fixedDeltaTime);
    }
}
