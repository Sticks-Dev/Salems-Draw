using Kickstarter.Inputs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Salems_Draw
{
    public class MoveController : LocomotionController, IInputReceiver
    {
        [Header("Inputs")]
        [SerializeField] private Vector2Input movementInput;
        [SerializeField] private FloatInput sprintInput;
        [SerializeField] private FloatInput jumpInput;

        private Vector3 rawMovementInput;
        private bool slamGround;

        private const float groundSlamForce = 50;

        #region InputHandler
        public void RegisterInputs(Player.PlayerIdentifier playerIdentifier)
        {
            movementInput.RegisterInput(OnMovementInputChange, playerIdentifier);
            sprintInput.RegisterInput(OnSprintInputChange, playerIdentifier);
            jumpInput.RegisterInput(OnJumpInputChange, playerIdentifier);
        }

        public void DeregisterInputs(Player.PlayerIdentifier playerIdentifier)
        {
            movementInput.DeregisterInput(OnMovementInputChange, playerIdentifier);
            sprintInput.DeregisterInput(OnSprintInputChange, playerIdentifier);
            jumpInput.DeregisterInput(OnJumpInputChange, playerIdentifier);
        }

        private void OnMovementInputChange(Vector2 input)
        {
            rawMovementInput = new Vector3(input.x, 0, input.y);
        }

        private void OnSprintInputChange(float input)
        {
            movementSpeed = input == 0 ? walkingSpeed : sprintSpeed;
        }

        private void OnJumpInputChange(float input)
        {
            if (input == 0)
                return;
            Jump();
        }
        #endregion

        #region UnityEvents
        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            base.Start();
        }

        private void FixedUpdate()
        {
            CheckGrounded();
            MoveTowards(rawMovementInput);
        }
        #endregion
    }
}
