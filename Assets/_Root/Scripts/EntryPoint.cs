using Profile;
using Tool.Analytics;
using UnityEngine;

internal class EntryPoint : MonoBehaviour
{
    [SerializeField] private float _speedCar;
    [SerializeField] private float _jumpHeightCar;
    [SerializeField] private  GameState InitialState;

    [SerializeField] private Transform _placeForUi;
    private MainController _mainController;
    
    [SerializeField] private AnalyticsManager _analyticsManager;
    private void Start()
    {
        var profilePlayer = new ProfilePlayer(_speedCar, _jumpHeightCar, InitialState);
        _mainController = new MainController(_placeForUi, profilePlayer);
        
        _analyticsManager.SendMainMenuOpenedEvent();
    }

    private void OnDestroy()
    {
        _mainController.Dispose();
    }
}
