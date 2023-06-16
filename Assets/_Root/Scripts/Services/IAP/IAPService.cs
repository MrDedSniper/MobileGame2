using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Purchasing;

namespace Services.IAP
{
    internal class IAPService : MonoBehaviour, IStoreListener, IIAPService
    {
        [Header("Components")]
        [SerializeField] private ProductLibrary _productLibrary;

        [field: Header("Events")]
        [field: SerializeField] public UnityEvent Initialized { get; private set; }
        [field: SerializeField] public UnityEvent PurchaseSucceed { get; private set; }
        [field: SerializeField] public UnityEvent PurchaseFailed { get; private set; }
        public bool IsInitialized { get; private set; }

        private IExtensionProvider _extensionProvider;
        private PurchaseValidator _purchaseValidator;
        private PurchaseRestorer _purchaseRestorer;
        private IStoreController _controller;


        private void Awake() =>
            InitializeProducts();

        private void InitializeProducts()
        {
            StandardPurchasingModule purchasingModule = StandardPurchasingModule.Instance();
            ConfigurationBuilder builder = ConfigurationBuilder.Instance(purchasingModule);

            foreach (Product product in _productLibrary.Products)
                builder.AddProduct(product.Id, product.ProductType);

            Log("Products initialized");
            UnityPurchasing.Initialize(this, builder);
        }


        void IStoreListener.OnInitialized(IStoreController controller, IExtensionProvider extensionsProvider)
        {
            IsInitialized = true;
            _controller = controller;
            _extensionProvider = extensionsProvider;
            _purchaseValidator = new PurchaseValidator();
            _purchaseRestorer = new PurchaseRestorer(_extensionProvider);

            Log("Инициализировано");
            Initialized?.Invoke();
        }

        void IStoreListener.OnInitializeFailed(InitializationFailureReason error)
        {
            IsInitialized = false;
            Error("Ошибка инициализации");
        }

        public void OnInitializeFailed(InitializationFailureReason error, string message)
        {
           // throw new System.NotImplementedException();
        }

        PurchaseProcessingResult IStoreListener.ProcessPurchase(PurchaseEventArgs args)
        {
            if (_purchaseValidator.Validate(args))
                PurchaseSucceed.Invoke();
            else
                OnPurchaseFailed(args.purchasedProduct.definition.id, "NonValid");

            return PurchaseProcessingResult.Complete;
        }

        void IStoreListener.OnPurchaseFailed(UnityEngine.Purchasing.Product product, PurchaseFailureReason failureReason) =>
            OnPurchaseFailed(product.definition.id, failureReason.ToString());

        private void OnPurchaseFailed(string productId, string reason)
        {
            Error($"Failed {productId}: {reason}");
            PurchaseFailed?.Invoke();
        }

        private void OnPurchaseSucceed(UnityEngine.Purchasing.Product product)
        {
            string productId = product.definition.id;
            decimal amount = (decimal) (product.definition.payout?.quantity ?? 1);
            string currency = product.metadata.isoCurrencyCode;
            ServiceRoster.Analytics.SendTransaction(productId, amount, currency);
            
            Debug.Log($"Куплено: {productId}");
            PurchaseSucceed?.Invoke();
        }


        public void Buy(string id)
        {
            if (IsInitialized)
                _controller.InitiatePurchase(id);
            else
                Error($"Покупка {id} провалена. Нет инициализации.");
        }

        public string GetCost(string productID)
        {
            UnityEngine.Purchasing.Product product = _controller.products.WithID(productID);
            return product != null ? product.metadata.localizedPriceString : "N/A";
        }

        public void RestorePurchases()
        {
            if (IsInitialized)
                _purchaseRestorer.Restore();
            else
                Error("Восстановление покупок провалено. Нет инициализации.");
        }


        private void Log(string message) => Debug.Log(WrapMessage(message));
        private void Error(string message) => Debug.LogError(WrapMessage(message));
        private string WrapMessage(string message) => $"[{GetType().Name}] {message}";
    }
}
