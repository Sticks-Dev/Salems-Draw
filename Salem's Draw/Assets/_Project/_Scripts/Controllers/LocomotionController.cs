using System;
using System.Collections;
using UnityEngine;

namespace Salems_Draw
{
    public abstract class LocomotionController : MonoBehaviour, IMover, ICooldown
    {
        [SerializeField] protected float walkingSpeed;
        [SerializeField] protected float sprintSpeed;
        [SerializeField] private float staminaDuration = 5f;

        protected bool isGrounded;
        protected float movementSpeed;
        public bool CanMove { private get; set; } = true;
        public float WalkingSpeed => walkingSpeed;
        public float SprintSpeed => sprintSpeed;


        // Cached References & Constant Values
        protected Rigidbody body;
        private const float radiusMultiplier = 0.5f;
        private const float groundDistance = 1f;
        private const float airborneMovementMultiplier = 40f;
        private float groundRadius;
        private Transform cameraTransform;
        private float stamina = 1f;
        private Coroutine staminaRoutine;
        private bool sprinting = false;

        public event Action<float> CooldownProgressChanged;

        #region UnityEvents
        protected virtual void Awake()
        {
            transform.root.TryGetComponent(out body);
        }

        protected virtual void Start()
        {
            var capsule = transform.root.GetComponentInChildren<CapsuleCollider>();
            groundRadius = capsule.radius * radiusMultiplier;
            cameraTransform = Camera.main.transform;
            movementSpeed = walkingSpeed;
        }
        #endregion

        protected void MoveTowards(Vector3 direction)
        {
            direction = cameraTransform.TransformDirection(direction);
            direction = Vector3.ProjectOnPlane(direction, Vector3.up).normalized;
            if (!CanMove)
                return;
            if (!isGrounded)
            {
                AirborneMoveTowards(direction);
                return;
            }
            var currentVelocity = Vector3.ProjectOnPlane(body.velocity, transform.up);
            var desiredVelocity = direction * movementSpeed;
            if (currentVelocity.sqrMagnitude > movementSpeed * movementSpeed && desiredVelocity != Vector3.zero)
                return;
            body.AddForce(desiredVelocity - currentVelocity, ForceMode.VelocityChange);
        }

        private void AirborneMoveTowards(Vector3 direction)
        {
            if (direction == Vector3.zero)
                return;
            var currentVelocity = Vector3.ProjectOnPlane(body.velocity, Vector3.up);
            bool velocityExceeds = currentVelocity.sqrMagnitude > walkingSpeed * walkingSpeed;
            bool isGainingSpeed = Vector3.Dot(direction, currentVelocity) > 0;

            if (isGainingSpeed && velocityExceeds)
                return;

            var desiredForce = direction.normalized * airborneMovementMultiplier;
            body.AddForce(desiredForce, ForceMode.Force);
        }

        protected void CheckGrounded()
        {
            var ray = new Ray(transform.position + Vector3.up, -Vector3.up);
            bool wasGrounded = isGrounded;
            isGrounded = Physics.SphereCast(ray, groundRadius, groundDistance);
        }

        public void SetSpeed(float speed)
        {
            movementSpeed = speed;
            if (movementSpeed == sprintSpeed)
            {
                if (staminaRoutine != null)
                    StopCoroutine(staminaRoutine);
                staminaRoutine = StartCoroutine(UpdateStamina(-1));
                sprinting = true;
                return;
            }
            if (sprinting)
            {
                if (staminaRoutine != null)
                    StopCoroutine(staminaRoutine);
                staminaRoutine = StartCoroutine(UpdateStamina(1));
            }
            sprinting = false;
        }

        private IEnumerator UpdateStamina(float multiple)
        {
            if (multiple > 0)
            {
                multiple /= 2; // Walking recharges stamina twice as slow
                yield return new WaitForSeconds(3); // Delay before stamina starts recharging
            }
            var delay = new WaitForEndOfFrame();
            float duration = staminaDuration * multiple;
            while (true)
            {
                stamina += Time.deltaTime / duration;
                stamina = Mathf.Clamp(stamina, 0, 1);
                if (stamina <= 0)
                    SetSpeed(walkingSpeed);
                CooldownProgressChanged?.Invoke(stamina);
                yield return delay;
            }
        }
    }
}
