using UnityEngine;

public class OverShoulderCamera : MonoBehaviour
{
    public Vector3 DesiredOrbitAngles = new Vector3(10.0f, -50.0f, 0);
    public float CameraDistance = 2.0f;
    
    private Transform OwnTransform;
    private Vector3 DesiredPosition;

    [SerializeField] private Transform FollowTarget = null;

    private void Awake()
    {
        if (FollowTarget == null)
        {
            FollowTarget = GameManager.PlayerTransform;
        }
        
        OwnTransform = transform;
    }

    private void LateUpdate()
    {
        SetDesiredPosition();
        MoveToDesiredPosition();
        PreventClipping();
        OwnTransform.rotation = Quaternion.LookRotation(GameManager.PlayerRayAimEnd);
    }

    private void SetDesiredPosition()
    {
        Quaternion offsetRotation = Quaternion.Euler(DesiredOrbitAngles);
        Vector3 offsetDirection = FollowTarget.TransformDirection(offsetRotation * Vector3.forward).normalized;
        DesiredPosition = FollowTarget.position - (offsetDirection * CameraDistance);
    }

    private void MoveToDesiredPosition()
    {
        OwnTransform.position = DesiredPosition;
    }

    private void PreventClipping()
    {
        if (!Physics.Linecast(FollowTarget.position, OwnTransform.position, out RaycastHit hit))
            return;
        transform.position = hit.point;
    }
}
