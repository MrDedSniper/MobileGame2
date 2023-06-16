using System.Collections.Generic;

namespace Features.Shed.Upgrade
{
    internal interface IUpgradeHandlerRepository : IRepository
    {
        IReadOnlyDictionary<string, IUpgradeHandler> Items { get; }
    }

    internal class UpgradeHandlerRepository : BaseRepository<string, IUpgradeHandler, UpgradeItemConfig>, IUpgradeHandlerRepository
    {
        public UpgradeHandlerRepository(IEnumerable<UpgradeItemConfig> configs) : base(configs)
        { }
        protected override string GetKey(UpgradeItemConfig config) => config.Id;

        protected override IUpgradeHandler CreateItem(UpgradeItemConfig config) => config.Type switch
        {
            UpgradeType.Speed => new SpeedUpgradeHandler(config.Value),
            UpgradeType.JumpHeight => new JumpHeightUpgradeHandler(config.Value),
            _=> StubUpgradeHandler.Default
        };
    }
}