using Kickstarter.DependencyInjection;
using Kickstarter.Inputs;
using System;
using UnityEngine;

namespace Salems_Draw
{
    public class MoveController : LocomotionController, IInputReceiver, IDependencyProvider
    {
        [Provide] private MoveController Self => this;

        #region Input Handler
        [Header("Inputs")]
        [SerializeField] private Vector2Input movementInput;
        [SerializeField] private FloatInput sprintInput;

        private Vector3 rawMovementInput;

        public event Action<float> OnSprintStatusChange;

        public void RegisterInputs(Player.PlayerIdentifier playerIdentifier)
        {
            movementInput.RegisterInput(OnMovementInputChange, playerIdentifier);
            sprintInput.RegisterInput(OnSprintInputChange, playerIdentifier);
        }

        public void DeregisterInputs(Player.PlayerIdentifier playerIdentifier)
        {
            movementInput.DeregisterInput(OnMovementInputChange, playerIdentifier);
            sprintInput.DeregisterInput(OnSprintInputChange, playerIdentifier);
        }

        private void OnMovementInputChange(Vector2 input)
        {
            rawMovementInput = new Vector3(input.x, 0, input.y);
        }

        private void OnSprintInputChange(float input)
        {
            movementSpeed = input == 0 ? walkingSpeed : sprintSpeed;
            OnSprintStatusChange?.Invoke(input);
            SetSpeed(input == 0 ? walkingSpeed : sprintSpeed);
        }
        #endregion

        #region Unity Events
        private void FixedUpdate()
        {
            CheckGrounded();
            MoveTowards(rawMovementInput);
        }
        #endregion
    }
}
