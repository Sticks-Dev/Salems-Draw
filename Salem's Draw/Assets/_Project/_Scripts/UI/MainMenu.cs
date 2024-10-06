using Kickstarter.Bootstrapper;
using Kickstarter.DependencyInjection;
using Kickstarter.Extensions;
using UnityEngine;
using UnityEngine.UIElements;

namespace Salems_Draw
{
    [RequireComponent(typeof(UIDocument))]
    public class MainMenu : MonoBehaviour
    {
        [Inject] private SceneLoader sceneLoader;

        [SerializeField] private StyleSheet stylesheet;

        private void OnValidate()
        {
            BuildDocument();
        }

        private void Start()
        {
            BuildDocument();
        }

        private void BuildDocument()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;
            if (root == null)
                return;

            root.Clear();
            root.styleSheets.Add(stylesheet);

            var container = root.CreateChild<VisualElement>("container");

            container.CreateChild<Label>("title").text = "Salem's Draw";
            
            var buttonContainer = container.CreateChild<VisualElement>("button_container");
            var startButton = buttonContainer.CreateChild<Button>("start_button");
            startButton.text = "Start";
            startButton.clickable.clicked += () => sceneLoader.LoadSceneGroup("Forest");
        }
    }
}
