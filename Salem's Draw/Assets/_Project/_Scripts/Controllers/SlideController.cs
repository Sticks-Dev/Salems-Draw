using Kickstarter.Inputs;
using UnityEngine;

namespace Salems_Draw
{
    public class SlideController : MonoBehaviour, IInputReceiver
    {
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
            rawInput = input;
        }
        #endregion
    }
}
