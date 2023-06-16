using Features.Inventory;
using Game.Car;
using Tool;

namespace Profile
{
    internal class ProfilePlayer
    {
        public readonly SubscriptionProperty<GameState> CurrentState;
        public readonly CarModel CurrentCar;
        public readonly InventoryModel Inventory;

        public ProfilePlayer(float speedCar, float jumpHeightCar, GameState initialState) : this(speedCar, jumpHeightCar)
        {
            CurrentState.Value = initialState;
        }

        public ProfilePlayer(float speedCar, float jumpHeightCar)
        {
            CurrentState = new SubscriptionProperty<GameState>();
            CurrentCar = new CarModel(speedCar, jumpHeightCar);
            Inventory = new InventoryModel();
        }
    }
}
