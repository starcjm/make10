using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPManager : Singleton<IAPManager>, IStoreListener
{
    //구매 과정 제어 함수 제공
    private IStoreController storeController;
    //여러 플랫폼을 위한 확장 처리
    private IExtensionProvider storeExtensionProvider;

    private Dictionary<string, string> price = new Dictionary<string, string>();

    public bool IsInitalized => storeController != null && storeExtensionProvider != null;

    public string GetPrice(string productID)
    {
        if(price.ContainsKey(productID))
        {
            return price[productID];
        }
        else
        {
            return "";
        }
    }

    public void Init()
    {
        DontDestroyOnLoad(gameObject);
        InitIapData();
    }

    private void InitIapData()
    {
        if (IsInitalized) return;

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        builder.AddProduct(
            Const.PRODUCT_NO_ADS, ProductType.NonConsumable,
            new IDs()
            {
                {Const.ANDROID_NO_ADS_ID, AndroidStore.GooglePlay},
                {Const.IPHONE_NO_ADS_COIN_ID, AppleAppStore.Name },
            }
        );
        builder.AddProduct(
            Const.PRODUCT_COIN_200, ProductType.Consumable,
            new IDs()
            {
                {Const.ANDROID_COIN_200_ID, AndroidStore.GooglePlay},
                {Const.IPHONE_COIN_200_ID, AppleAppStore.Name },
            }
        );
        builder.AddProduct(
            Const.PRODUCT_COIN_500, ProductType.Consumable,
            new IDs()
            {
                {Const.ANDROID_COIN_500_ID, AndroidStore.GooglePlay},
                {Const.IPHONE_COIN_500_ID, AppleAppStore.Name },
            }
        );
        builder.AddProduct(
            Const.PRODUCT_COIN_1250, ProductType.Consumable,
            new IDs()
            {
                {Const.ANDROID_COIN_1250_ID, AndroidStore.GooglePlay},
                {Const.IPHONE_COIN_1250_ID, AppleAppStore.Name },
            }
        );
        builder.AddProduct(
            Const.PRODUCT_COIN_3500, ProductType.Consumable,
            new IDs()
            {
                {Const.ANDROID_COIN_3500_ID, AndroidStore.GooglePlay},
                {Const.IPHONE_COIN_3500_ID, AppleAppStore.Name },
            }
        );
        UnityPurchasing.Initialize(this, builder);
    }
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("iap 초기화");
        storeController = controller;
        storeExtensionProvider = extensions;

        var products = storeController.products.all;
        for(int i=0; i<products.Length; ++i)
        {
            price.Add(products[i].definition.id, products[i].metadata.localizedPriceString);
        }
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.LogError($"유니티 iap 초기화실패  :  {error} ");
    }

    public void OnPurchaseFailed(Product i, PurchaseFailureReason p)
    {
        Debug.LogWarning($"구매 실패 :  {i.definition.id}, {p}");
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
    {
        Debug.Log($"구매 성공 ID : {e.purchasedProduct.definition.id}");

        GameManager.Instance.BuyCompleteShopItem(e.purchasedProduct.definition.id);
        
        return PurchaseProcessingResult.Complete;
    }

    public void Purchase(string produceId)
    {
        if (!IsInitalized) return;

        var product = storeController.products.WithID(produceId);
        if (product != null && product.availableToPurchase)
        {
            Debug.Log($"구매시도 : {product.definition.id}");
            storeController.InitiatePurchase(product);
        }
        else
        {
            Debug.Log($"구매 시도 불가 - {produceId}");
        }
    }

    public void RestorePurchase()
    {
        if (!IsInitalized) return;

        if (Application.platform == RuntimePlatform.IPhonePlayer
        || Application.platform == RuntimePlatform.OSXPlayer)
        {
            Debug.Log("구매 시도 복구");

            var appleExt = storeExtensionProvider.GetExtension<IAppleExtensions>();
            appleExt.RestoreTransactions(
                result => Debug.Log($"구매 복구 시도 결과 - {result}"));
        }
    }

    public bool HadPurchase(string produceId)
    {
        if (!IsInitalized) return false;

        var product = storeController.products.WithID(produceId);
        if(product != null)
        {
            return product.hasReceipt;
        }
        return false;
    }
}
