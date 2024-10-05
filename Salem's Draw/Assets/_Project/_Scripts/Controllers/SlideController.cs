using Kickstarter.DependencyInjection;
using Kickstarter.Inputs;
using System;
using System.Collections;
using UnityEngine;

namespace Salems_Draw
{
    public class SlideController : MonoBehaviour, IInputReceiver, IDependencyProvider
    {
        [Inject]
        private MoveController MoveController
        {
            get => moveController;
            set
            {
                moveController = value;
                moveController.OnSprintStatusChange += OnSprintStatusChange;
            }
        }
        private MoveController moveController;
        [Provide] private SlideController Self => this;

        #region Input Handler
        [SerializeField] private FloatInput slideInput;

        private float rawInput;

        public void RegisterInputs(Player.PlayerIdentifier playerIdentifier)
        {
            slideInput.RegisterInput(OnSlideInputChange, playerIdentifier);
        }

        public void DeregisterInputs(Player.PlayerIdentifier playerIdentifier)
        {
            slideInput.DeregisterInput(OnSlideInputChange, playerIdentifier);
        }

        private void OnSlideInputChange(float input)
        {
            if (input == 1)
            {
                AttemptSlide();
                return;
            }
            StartCoroutine(EndSlide());
        }
        #endregion

        #region Sliding
        [SerializeField] private float slideSpeed;
        [SerializeField] private float slideDuration;
        [SerializeField] private float slideToggleDuration;
        [SerializeField, Range(0, 1)] private float slidingHeightMultiplier;

        private bool isSprinting;
        private bool isSliding;
        private bool IsSliding
        {
            get => isSliding;
            set
            {
                isSliding = value;
                OnSlideStatusChange?.Invoke(IsSliding);
            }
        }
        private Coroutine slideRoutine;

        public event Action<bool> OnSlideStatusChange;
        private const float initialScale = 1;

        private void OnSprintStatusChange(float sprintStatus)
        {
            isSprinting = sprintStatus == 1;
        }

        private void AttemptSlide()
        {
            if (IsSliding)
                return;
            slideRoutine = StartCoroutine(Slide());
        }

        private IEnumerator Slide()
        {
            IsSliding = true;
            MoveController.SetSpeed(slideSpeed);

            yield return LerpHeight(slidingHeightMultiplier);
            yield return new WaitForSeconds(slideDuration);

            yield return EndSlide();
        }

        private IEnumerator LerpHeight(float targetHeight)
        {
            var fixedUpdate = new WaitForFixedUpdate();
            yield return fixedUpdate;

            float timer = 0;
            float initialHeight = transform.root.localScale.y;
            while (timer < slideToggleDuration)
            {
                timer += Time.fixedDeltaTime;
                var height = Mathf.Lerp(initialHeight, targetHeight, timer / slideToggleDuration);
                transform.root.localScale = new Vector3(initialScale, height, initialScale);
                yield return fixedUpdate;
            }

            transform.root.localScale = new Vector3(initialScale, targetHeight, initialScale);
        }

        private IEnumerator EndSlide()
        {
            if (slideRoutine != null)
                StopCoroutine(slideRoutine);
            yield return LerpHeight(initialScale);
            MoveController.SetSpeed(isSprinting ? MoveController.SprintSpeed : MoveController.WalkingSpeed);
            IsSliding = false;
        }
        #endregion
    }
}
