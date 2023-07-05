using Profile;
using Tool;
using UnityEngine;

namespace Features.Battle
{
    internal class StartBattleController : BaseController
    {
        private readonly ResourcePath _resourcePath = new ResourcePath("Prefabs/Battle/StartBattleView");

        private readonly StartBattleView _view;
        private readonly ProfilePlayer _profilePlayer;


        public StartBattleController(Transform placeForUi, ProfilePlayer profilePlayer)
        {
            _profilePlayer = profilePlayer;
            _view = LoadView(placeForUi);
            _view.Init(StartBattle);
        }


        private StartBattleView LoadView(Transform placeForUi)
        {
            GameObject prefab = ResourcesLoader.LoadPrefab(_resourcePath);
            GameObject objectView = Object.Instantiate(prefab, placeForUi, false);
            AddGameObject(objectView);

            return objectView.GetComponent<StartBattleView>();
        }

        private void StartBattle() =>
            _profilePlayer.CurrentState.Value = GameState.Battle;
    }
}
