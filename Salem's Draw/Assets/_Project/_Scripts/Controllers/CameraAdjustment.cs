using Kickstarter.DependencyInjection;
using System.Collections;
using UnityEngine;

namespace Salems_Draw
{
    public class CameraAdjustment : MonoBehaviour
    {
        [Inject] private SlideController slideController
        {
            set
            {
                value.OnSlideStatusChange += OnSlideStatusChange;
            }
        }

        #region Unity Events
        private void Start()
        {
            initialHeight = transform.localPosition.y;
        }
        #endregion

        #region Camera Adjustment
        [SerializeField] private float slideCameraHeight;
        [SerializeField] private float slideCameraLerpDuration;
        
        private float initialHeight;

        private void OnSlideStatusChange(bool isSliding)
        {
            StartCoroutine(LerpHeight(isSliding ? slideCameraHeight : initialHeight));
        }

        private IEnumerator LerpHeight(float targetHeight)
        {
            float initialHeight = transform.position.y;
            float elapsedTime = 0;
            while (elapsedTime < slideCameraLerpDuration)
            {
                transform.localPosition = 
                    new Vector3(transform.localPosition.x, Mathf.Lerp(initialHeight, targetHeight, elapsedTime / slideCameraLerpDuration), transform.localPosition.z);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            transform.localPosition = new Vector3(transform.localPosition.x, targetHeight, transform.localPosition.z);
        }
        #endregion
    }
}
