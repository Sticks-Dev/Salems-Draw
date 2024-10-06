using Kickstarter.Extensions;
using UnityEngine;
using UnityEngine.UIElements;

namespace Salems_Draw
{
    public class AttackCooldown : MonoBehaviour
    {
        [SerializeField] private StyleSheet styles;

        private ProgressBar progressBar;

        private void Start()
        {
            BuildDocument();
            GetComponent<Dagger>().CooldownProgressChanged += OnCooldownProgressChanged;
        }

        private void OnValidate()
        {
            BuildDocument();
        }

        private void BuildDocument()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;
            if (root == null)
                return;

            root.Clear();
            root.styleSheets.Add(styles);
            
            var columnReverseContainer = root.CreateChild<VisualElement>("column_reverse");
            var rowReverseContainer = columnReverseContainer.CreateChild<VisualElement>("row_reverse");

            progressBar = rowReverseContainer.CreateChild<ProgressBar>("progress_bar");
            progressBar.lowValue = 0f;
            progressBar.highValue = 1f;
            progressBar.value = 1f;
        }

        private void OnCooldownProgressChanged(float progress)
        {
            progressBar.value = progress;
        }
    }
}
