using System;
using System.Timers;
using Demo.GameInputState;
using Demo.Player.Player_Scripts.Player_Installers;
using UnityEngine;
using UnityEngine.InputSystem;
#if UNITY_EDITOR

using UnityEditor;

#endif


namespace PlayerMovement.Scripts
{
    public class RbPlayerController : MonoBehaviour
    {

        #region Player Components

        [Header("Player Components")]
        [SerializeField] private Animator  characterAnimator;
        [SerializeField] private Rigidbody rigidbody;

        #endregion
        
        #region Physics Calculation

        private Rigidbody _hitRigidbody;
        
        [Header("Ground Calculation Ray")]
        [Range(0,5),SerializeField] private float groundRayDistance;
        [Range(0,1),SerializeField] private float groundRayOriginOffset;
        private Ray _groundRay;
        
        [Header("Spring Force")]
        [Range(0,1),SerializeField] private float rideHeight;
        [Range(0,5000),SerializeField]private float rideSpringDamper;
        [Range(0,5000),SerializeField]private float rideSpringStrength;

        [Header("Rotation Forces")] 
        [SerializeField]private float upRightRotationDamper;
        [SerializeField]private float upRightRotationStrength;

        [Header("Locomotion")] 
        [SerializeField] private float maxSpeed;
        [SerializeField] private float acceleration;
        [SerializeField] private float breakAcceleration;
        
        [SerializeField] private AnimationCurve accelerationCurve;
        
        private Vector3 _maxVelocity;
        private Vector3 _dir;
        private InputController _inputController;

        #endregion

        #region Properties
        private Vector3 Velocity     => rigidbody.velocity;
        private Vector3 RayDir => transform.TransformDirection(_groundRay.direction);

        #endregion

        #region LifeCycle Methods

        private void Awake() => FillReferences();

        private void Start()
        {
            var inputConfiguration = new GameObject("Input Configuration");
            inputConfiguration.transform.SetParent(transform);
            
            var config = inputConfiguration.AddComponent<InputConfigurationInstaller>();
            
            _inputController.SetInput(config.GetInput());
            _inputController.Initialize();
        }

        private void FixedUpdate()
        {
            SetGroundRay();
            ApplyUprightForce();
            CalculateMovement();
            
            if (GroundRayHit(out var hit)) ApplySpringForce(hit);
        }

        private void OnEnable()
        {
            PlayerInputState.MovePlayer += ChangeDirection;
        }

        private void OnDisable()
        {
            PlayerInputState.MovePlayer -= ChangeDirection;
        }

        #endregion

        private void FillReferences()
        {
            TryGetComponent(out rigidbody);
            TryGetComponent(out characterAnimator);
            TryGetComponent(out _inputController);
        }

        private void SetGroundRay() => _groundRay = new Ray(transform.position + groundRayOriginOffset * Vector3.up, Vector3.down * groundRayDistance);

        private void ApplySpringForce(RaycastHit hit)
        {
            var otherVel = Vector3.zero;
            
            if (_hitRigidbody != null) otherVel = _hitRigidbody.velocity;

            var rayDirVel = Vector3.Dot(RayDir, Velocity);
            var otherDirVel = Vector3.Dot(RayDir, otherVel);

            var relVel = rayDirVel - otherDirVel;

            var x = hit.distance - rideHeight;

            var springForce = (x * rideSpringStrength) - (relVel * rideSpringDamper);
            
            rigidbody.AddForce(RayDir * springForce);

            if(_hitRigidbody != null) _hitRigidbody.AddForceAtPosition(RayDir * -springForce, hit.point);
        }

        private void ApplyUprightForce()
        {
            var dampTorque = upRightRotationDamper * -rigidbody.angularVelocity;
            
            if (_dir == Vector3.zero)
            {
                var springTorque = upRightRotationStrength * Vector3.Cross(rigidbody.transform.up, Vector3.up);
                rigidbody.AddTorque(springTorque + dampTorque, ForceMode.Acceleration);
                return;
            }
            
            var lookRotation = transform.position + _dir * upRightRotationStrength;
            
            Quaternion targetOrientation = Quaternion.LookRotation(lookRotation);       
            Quaternion rotationChange = targetOrientation * Quaternion.Inverse(rigidbody.rotation);

            rotationChange.ToAngleAxis(out float angle, out Vector3 axis);
            if (angle > 180f)
                angle -= 360f;

            if (Mathf.Approximately(angle, 0)) {
                rigidbody.angularVelocity = Vector3.zero;
                return;
            }

            angle *= Mathf.Deg2Rad;

            var targetAngularVelocity = axis * angle / Time.deltaTime;

            float catchUp = 1.0f;
            targetAngularVelocity *= catchUp;

            rigidbody.AddTorque(targetAngularVelocity - rigidbody.angularVelocity + dampTorque, ForceMode.VelocityChange);

        }
        
        private bool GroundRayHit(out RaycastHit hit) => Physics.Raycast(_groundRay,out hit ,groundRayDistance);
        
        private void ChangeDirection(object sender, InputAction.CallbackContext context)
        {
            var dir = context.ReadValue<Vector2>();
            _dir = new Vector3(dir.x,0,dir.y);
        }

        private void CalculateMovement()
        {
            characterAnimator.SetFloat("Speed",rigidbody.velocity.magnitude);
            
            if (_dir == Vector3.zero || _dir.normalized != Velocity.normalized) rigidbody.AddForce(-Velocity * breakAcceleration);

            if (Velocity.magnitude > maxSpeed)
            {
                rigidbody.velocity = Vector3.ClampMagnitude(Velocity, maxSpeed);
                return;
            }

            _dir.Normalize();

            _maxVelocity = maxSpeed * _dir;
            
            var velDot = Mathf.Lerp(Velocity.magnitude, maxSpeed,Time.deltaTime);

            var acc = acceleration * accelerationCurve.Evaluate(velDot/maxSpeed);

            var finalVelocity = Velocity + _dir * acc;
           
            rigidbody.AddForce(finalVelocity);
        }

        private void OnDrawGizmos()
        {
            Handles.color = Color.yellow;
            Handles.DrawLine(transform.position,transform.position+_dir,8);
            
            if (!GroundRayHit(out var hit)) return;
            
            Handles.color = Color.green;
            Handles.DrawWireCube(hit.point,Vector3.one * 0.15f);
            
            Handles.color = Color.red;
            Handles.DrawLine(_groundRay.origin,_groundRay.GetPoint(groundRayDistance),5);
            Handles.Label(hit.point,"Raycast",new GUIStyle(){ fontSize = 20});
            
            Handles.color = Color.magenta;
            Handles.DrawLine(_groundRay.origin + transform.forward, _groundRay.GetPoint(rideHeight) + transform.forward,5);
            Handles.Label(_groundRay.origin + transform.forward,"RideHeight",new GUIStyle(){ fontSize = 20});

            Handles.color = Color.cyan;
            Handles.DrawLine(_groundRay.GetPoint(rideHeight) + transform.forward, hit.point + transform.forward,5);
            Handles.Label(_groundRay.GetPoint(rideHeight) + transform.forward,"Force Vector",new GUIStyle(){ fontSize = 20});
        }
    }
}
