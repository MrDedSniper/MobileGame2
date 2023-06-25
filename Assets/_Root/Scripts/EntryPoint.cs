using Profile;
using Tool.Analytics;
using UnityEngine;

internal class EntryPoint : MonoBehaviour
{
    [SerializeField] private InitialProfilePlayer _initialProfilePlayer;

    [SerializeField] private Transform _placeForUi;
    
    private MainController _mainController;
    
    [SerializeField] private AnalyticsManager _analyticsManager;
    private void Start()
    {
        var profilePlayer = CreateProfilePlayer(_initialProfilePlayer);
        _mainController = new MainController(_placeForUi, profilePlayer);
        
        _analyticsManager.SendMainMenuOpenedEvent();
    }
    
    private void OnDestroy()
    {
        _mainController.Dispose();
    }
    private ProfilePlayer CreateProfilePlayer(InitialProfilePlayer initialProfilePlayer) =>
        new ProfilePlayer(initialProfilePlayer.Car.Speed, initialProfilePlayer.Car.JumpHeight, initialProfilePlayer.State);

}
