using Kickstarter.Bootstrapper;
using Kickstarter.DependencyInjection;
using Kickstarter.Extensions;
using UnityEngine;
using UnityEngine.UIElements;

namespace Salems_Draw
{
    [RequireComponent(typeof(UIDocument))]
    public class GameOver : MonoBehaviour
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
            var root = GetComponent<UIDocument>()?.rootVisualElement;
            if (root == null)
                return;

            root.Clear();
            root.styleSheets.Add(stylesheet);

            var container = root.CreateChild<VisualElement>("container");
            container.CreateChild<Label>("game_over").text = "Game Over";
            
            var button = container.CreateChild<Button>("restart_button");
            button.text = "Restart?";
            button.clickable.clicked += () => sceneLoader.LoadSceneGroup("Forest");
        }
    }
}
