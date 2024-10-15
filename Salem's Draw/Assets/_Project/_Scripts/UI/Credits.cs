using Kickstarter.Bootstrapper;
using Kickstarter.DependencyInjection;
using Kickstarter.Extensions;
using UnityEngine;
using UnityEngine.UIElements;

namespace Salems_Draw
{
    [RequireComponent(typeof(UIDocument))]
    public class Credits : MonoBehaviour
    {
        [Inject] private SceneLoader sceneLoader;

        [SerializeField] private StyleSheet stylesheet;
        [SerializeField] private string[] names;

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
            container.CreateChild<Label>("you_win").text = "You Win";

            container.CreateChild<Label>("credits").text = "Credits";

            var creditsList = container.CreateChild<ScrollView>("credits_list");
            creditsList.verticalScrollerVisibility = ScrollerVisibility.Hidden;

            AddNames(creditsList.contentContainer);

            var button = container.CreateChild<Button>("menu_button");
            button.text = "Main Menu";
            button.clickable.clicked += () => sceneLoader.LoadSceneGroup("Main Menu");

            var quitButton = container.CreateChild<Button>("quit_button");
            quitButton.text = "Quit";
            quitButton.clickable.clicked += Application.Quit;
        }

        private void AddNames(VisualElement creditsList)
        {
            foreach (var name in names)
            {
                creditsList.CreateChild<Label>("name").text = name;
            }
        }
    }
}
