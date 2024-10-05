using Kickstarter.Inputs;
using UnityEngine;

namespace Salems_Draw
{
    public class RotationController : MonoBehaviour, IInputReceiver
    {
        #region Input Handler
        [SerializeField] private Vector2Input lookInput;

        private Vector2 rawInput;

        public void RegisterInputs(Player.PlayerIdentifier playerIdentifier)
        {
            lookInput.RegisterInput(OnLookInputChange, playerIdentifier);
        }

        public void DeregisterInputs(Player.PlayerIdentifier playerIdentifier)
        {
            lookInput.DeregisterInput(OnLookInputChange, playerIdentifier);
        }

        public void OnLookInputChange(Vector2 lookInput)
        {
            rawInput += lookInput;
        }
        #endregion

        #region Unity Events
        private void FixedUpdate()
        {
            RotateBody();
            RotateCamera();
            ResetInput();
        }
        #endregion

        #region Rotation
        [SerializeField, Range(0, 1)] private float horizontalRotationSpeed = 1f;
        [SerializeField, Range(0, 1)] private float verticalRotationSpeed = 1f;
        [SerializeField, Range(0, 90)] private float maxVerticalRotation = 85f;

        private void RotateBody()
        {
            var rotation = transform.root.rotation.eulerAngles;
            rotation += Vector3.up * rawInput.x * horizontalRotationSpeed;
            transform.root.rotation = Quaternion.Euler(rotation);
        }

        private void RotateCamera()
        {
            var rotation = transform.localRotation.eulerAngles.x;
            rotation += -rawInput.y * verticalRotationSpeed;
            if (rotation > 180)
                rotation -= 360;
            rotation = Mathf.Clamp(rotation, -maxVerticalRotation, maxVerticalRotation);
            transform.localRotation = Quaternion.Euler(new(rotation, 0, 0));
        }

        private void ResetInput()
        {
            rawInput = Vector2.zero;
        }
        #endregion
    }
}
