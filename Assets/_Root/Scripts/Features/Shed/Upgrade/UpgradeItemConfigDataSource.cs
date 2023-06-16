using System.Collections.Generic;
using UnityEngine;

namespace Features.Shed.Upgrade
{
    [CreateAssetMenu(fileName = nameof(UpgradeItemConfigDataSource), menuName = "Configs/" + nameof(UpgradeItemConfigDataSource))] 
    
    internal class UpgradeItemConfigDataSource : ScriptableObject
    {
        [SerializeField] private UpgradeItemConfig[] _itemConfigs;

        public IReadOnlyList<UpgradeItemConfig> ItemConfigs => _itemConfigs;
    }
}