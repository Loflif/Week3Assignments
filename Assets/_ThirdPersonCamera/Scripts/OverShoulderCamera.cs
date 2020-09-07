using UnityEngine;

namespace BobsAdventure
{
    public class OverShoulderCamera : MonoBehaviour
    {
        public Vector3 DesiredOrbitAngles = new Vector3(10.0f, -50.0f, 0);
        public float DistanceToTarget = 2.0f;

        [Header("Aim")]
        public float YawSpeed = 30.0f;
        public float PitchSpeed = 30.0f;
        public Vector2 PitchLimit = new Vector2(-89.0f, 89.0f);
        public float CrosshairRayLength = 100.0f;
        public float CrosshairRayStartOffset = 3.0f;
        public Vector3 FocusOffset = new Vector3(3.0f, 2.0f, 0);

        [Header("Zoom")] 
        public float ZoomSpeed = 1.0f;
        public Vector2 ZoomLimit = new Vector2(0.5f, 10.0f);

        private Vector3 FocusPoint;
        private Transform OwnTransform;
        private Vector3 DesiredPosition;
        private Camera Camera = null;

        [SerializeField] private Transform FollowTarget = null;

        private Vector3 CameraHalfExtends
        {
            get
            {
                Vector3 halfExtends;
                halfExtends.y = Camera.nearClipPlane *
                                Mathf.Tan(0.5f * Mathf.Deg2Rad * Camera.fieldOfView);
                halfExtends.x = halfExtends.y * Camera.aspect;
                halfExtends.z = 0.0f;
                return halfExtends;
            }
        }
        
        private void Awake()
        {
            if (FollowTarget == null)
            {
                FollowTarget = GameManager.PlayerTransform;
            }
            
            OwnTransform = transform;
            Camera = GetComponent<Camera>();
        }

        private void LateUpdate()
        {
            SetFocusPoint();
            SetDesiredPosition();
            MoveToDesiredPosition();
            FaceFocusPoint();
            PreventClipping();
            CrosshairRaycast();
        }

        private void SetFocusPoint()
        {
            FocusPoint = (FollowTarget.position +
                          FocusOffset.x * OwnTransform.right + 
                          FocusOffset.y * OwnTransform.up + 
                          FocusOffset.z * OwnTransform.forward);

        }

        private void SetDesiredPosition()
        {
            Quaternion offsetRotation = Quaternion.Euler(DesiredOrbitAngles);
            
            Vector3 offsetDirection = (offsetRotation * Vector3.forward).normalized;
            DesiredPosition = FocusPoint - (offsetDirection * DistanceToTarget);
        }

        private void FaceFocusPoint()
        {
            Vector3 focusPointDirection = FollowTarget.TransformDirection(FocusPoint - OwnTransform.position).normalized;
            OwnTransform.forward = focusPointDirection;
            Debug.DrawRay(transform.position, focusPointDirection * 20.0f, Color.blue);
        }

        private void MoveToDesiredPosition()
        {
            OwnTransform.position = DesiredPosition;
        }

        public void Aim(Vector2 pAimInput)
        {
            float yawAmount = YawSpeed * Time.deltaTime * pAimInput.x;
            float pitchAmount = PitchSpeed * Time.deltaTime * -pAimInput.y;
            
            DesiredOrbitAngles += new Vector3(pitchAmount, yawAmount, 0);
            DesiredOrbitAngles.x = Mathf.Clamp(DesiredOrbitAngles.x, PitchLimit.x, PitchLimit.y);
        }
        
        private void PreventClipping()
        {
            if(!Physics.BoxCast(FocusPoint, CameraHalfExtends, -OwnTransform.forward, out RaycastHit hit, OwnTransform.rotation, DistanceToTarget - Camera.nearClipPlane))
                return;
            OwnTransform.position = FocusPoint - (OwnTransform.forward * (hit.distance + Camera.nearClipPlane));
        }
        
        public void Zoom(float pZoomAmount)
        {
            DistanceToTarget += pZoomAmount * ZoomSpeed * Time.deltaTime;
            DistanceToTarget = Mathf.Clamp(DistanceToTarget, ZoomLimit.x, ZoomLimit.y);
        }

        private void CrosshairRaycast()
        {
            GameManager.PlayerRayAimEnd = OwnTransform.position + (OwnTransform.forward * CrosshairRayLength);

            if (Physics.Raycast(OwnTransform.position + (CrosshairRayStartOffset * OwnTransform.forward), OwnTransform.forward, out RaycastHit hit, CrosshairRayLength))
            {
                GameManager.PlayerRayAimHit = hit.point;
            }
            else
            {
                GameManager.PlayerRayAimHit = GameManager.PlayerRayAimEnd;
            }
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(FocusPoint, 0.2f);
        }
    }

}
