using UnityEngine;

namespace BattleScripts
{
    internal interface IEnemy
    {
        void Update(PlayerData playerData);
    }

    internal class Enemy : IEnemy
    {
        private const float KMoney = 5f;
        private const float KPower = 1.5f;
        private const float MaxHealthPlayer = 20;
        private const float PercentMinusHealth = 0.1f;
        private const float KWantedLevel = 0.1f;

        private readonly string _name;

        private int _moneyPlayer;
        private int _healthPlayer;
        private int _powerPlayer;
        private int _wantedLevel;


        public Enemy(string name) =>
            _name = name;


        public void Update(PlayerData playerData)
        {
            switch (playerData.DataType)
            {
                case DataType.Money:
                    _moneyPlayer = playerData.Value;
                    break;

                case DataType.Health:
                    _healthPlayer = playerData.Value;
                    break;

                case DataType.Power:
                    _powerPlayer = playerData.Value;
                    break;
                
                case DataType.WantedLevel:
                    _wantedLevel = playerData.Value;
                    break;
            }

            Debug.Log($"Notified {_name} change to {playerData.DataType:F}");
        }

        public int CalcPower()
        {
            float kHealth = CalcKHealth();
            float moneyRatio = _moneyPlayer / KMoney;
            float powerRatio = _powerPlayer / KPower;
            float wandetRatio = _wantedLevel * KWantedLevel;

            return (int)((moneyRatio + kHealth + powerRatio) * wandetRatio);
        }

        private float CalcKHealth() =>
            _healthPlayer > MaxHealthPlayer ? (_healthPlayer - (_healthPlayer * PercentMinusHealth)) * _wantedLevel : 5 * _wantedLevel ;
            //_healthPlayer > MaxHealthPlayer ? 100 : 5; // старая формула
    }
}

