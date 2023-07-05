using Profile;
using Tool;
using UnityEngine;

namespace Ui.GameMenu
{
    internal class GameMenuController : BaseController
    {
        private readonly ResourcePath _resourcePath = new ResourcePath("Prefabs/UI/GameMenu");
        private readonly ProfilePlayer _profilePlayer;

        public GameMenuController(Transform placeForUi, ProfilePlayer profilePlayer)
        {
            _profilePlayer = profilePlayer;

            GameMenuView view = LoadView(placeForUi);
            view.Init(Back);
        }

        private GameMenuView LoadView(Transform placeForUi)
        {
            GameObject prefabs = ResourcesLoader.LoadPrefab(_resourcePath);
            GameObject objectView = Object.Instantiate(prefabs, placeForUi, false);
            AddGameObject(objectView);

            return objectView.GetComponent<GameMenuView>();
        }

        private void Back() => _profilePlayer.CurrentState.Value = GameState.Start;

    }
}