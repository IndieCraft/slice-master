using UnityEngine;
using System;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;

public class VPAdsController : MonoBehaviour
{
    public static VPAdsController instance;
    public Admob admob;

    private DateTime _expireTime;
    private AppOpenAd appOpenAd;
    private BannerView bannerView;
    private InterstitialAd interstitial;
    private RewardedInterstitialAd rewardedInterstitial;
    public RewardedAd rewardedAd;
    [HideInInspector]
    public ButtonManager BTN;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);
        //AppStateEventNotifier.AppStateChanged += OnAppStateChanged;
    }

    private void OnDestroy()
    {
        //AppStateEventNotifier.AppStateChanged -= OnAppStateChanged;
    }

    private void Start()
    {
        List<String> testDevicesIds = new List<String>() { AdRequest.TestDeviceSimulator };

#if UNITY_IPHONE
        testDevicesIds.Add("96e23e80653bb28980d3f40beb58915");
#elif UNITY_ANDROID
        testDevicesIds.Add("19D74B62DF9D68876638762AB4E16AA6");
#endif

        RequestConfiguration requestConfiguration = new RequestConfiguration.Builder()
            .SetTagForChildDirectedTreatment(TagForChildDirectedTreatment.False)
            //.SetTestDeviceIds(testDevicesIds)
            .build();
        MobileAds.SetRequestConfiguration(requestConfiguration);
        MobileAds.Initialize(HandleInitCompleteAction);

        if(!CheckPremium())
        {
            RequestBanner();
            RequestInterstitial();
        }
        RequestAndLoadRewardedAd();
    }

    private void HandleInitCompleteAction(InitializationStatus initialization)
    {
        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {
            Debug.Log("AdMob successfully initialized.");
        });
    }
    public void HandleMediationTestSuiteDismissed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleMediationTestSuiteDismissed event received");
    }
    private AdRequest CreateAdRequest()
    {
        return new AdRequest.Builder().AddKeyword("game").Build();
    }

    public void DestroyAds()
    {
        bannerView.Hide();
        bannerView.Destroy();
        interstitial.Destroy();
    }

    private bool CheckPremium()
    {
        return ZPlayerPrefs.GetInt("remove_ads") == 1;
    }

    #region APP OPEN
    public void LoadAppOpenAd()
    {
#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
        string adUnitId = admob.androidAppOpen.Trim();
        if (VPConfigBase.instance.debug) adUnitId = admob.debugAppOpen.Trim();
#elif UNITY_IPHONE
        string adUnitId = admob.iosAppOpen.Trim();
#else
        string adUnitId = "unexpected_platform";
#endif
        // Clean up the old ad before loading a new one.
        if (appOpenAd != null)
        {
            appOpenAd.Destroy();
            appOpenAd = null;
        }

        // send the request to load the ad.
        AppOpenAd.Load(adUnitId, ScreenOrientation.Landscape, CreateAdRequest(),
            (AppOpenAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("app open ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("App open ad loaded with response : "
                          + ad.GetResponseInfo());

                _expireTime = DateTime.Now + TimeSpan.FromHours(4);

                appOpenAd = ad;
                RegisterAppOpenEventHandlers(ad);
            });
    }
    public bool IsOpenAdAvailable
    {
        get
        {
            return appOpenAd != null
                   && DateTime.Now < _expireTime;
        }
    }
    public void ShowAppOpenAd()
    {
        if (appOpenAd != null)
        {
            appOpenAd.Show();
        }
        else
        {
            Debug.LogError("App open ad is not ready yet.");
        }
    }
    private void OnAppStateChanged(AppState state)
    {
        Debug.Log("App State changed to : " + state);

        // if the app is Foregrounded and the ad is available, show it.
        if (state == AppState.Foreground)
        {
            if (IsOpenAdAvailable)
            {
                ShowAppOpenAd();
            }
        }
    }
    #endregion

    #region BANNER
    public void RequestBanner()
    {
#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
        string adUnitId = admob.androidBanner.Trim();
        if (VPConfigBase.instance.debug) adUnitId = admob.debugBanner.Trim();
#elif UNITY_IPHONE
        string adUnitId = admob.iosBanner.Trim();
#else
        string adUnitId = "unexpected_platform";
#endif

        if (bannerView != null)
        {
            bannerView.Destroy();
        }

        // Create a 320x50 banner at the top of the screen.
        bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);

        // Register for ad events.
        bannerView.OnBannerAdLoaded += HandleBannerAdLoaded;
        bannerView.OnBannerAdLoadFailed += HandleBannerAdFailedToLoad;
        bannerView.OnAdFullScreenContentOpened += HandleBannerAdOpened;
        bannerView.OnAdFullScreenContentClosed += HandleBannerAdClosed;

        // Load a banner ad.
        bannerView.LoadAd(this.CreateAdRequest());
    }
    #endregion

    #region INTERSTITIAL

    public void RequestInterstitial()
    {
#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
        string adUnitId = admob.androidInterstitial.Trim();
        if (VPConfigBase.instance.debug) adUnitId = admob.debugInterstitial.Trim();
#elif UNITY_IPHONE
        string adUnitId = admob.iosInterstitial.Trim();
#else
        string adUnitId = "unexpected_platform";
#endif

        if (interstitial != null)
        {
            interstitial.Destroy();
        }

        InterstitialAd.Load(adUnitId, CreateAdRequest(),
            (InterstitialAd ad, LoadAdError error) =>
        {
            // if error is not null, the load request failed.
            if (error != null || ad == null)
            {
                Debug.LogError("Interstitial ad failed to load an ad " +
                               "with error : " + error);
                return;
            }

            Debug.Log("Interstitial ad loaded with response : "
                      + ad.GetResponseInfo());

            interstitial = ad;
            RegisterInterstitialEventHandlers(ad);
        });
    }

    public void ShowInterstitial()
    {
        if (CheckPremium()) return;

        if (interstitial != null && interstitial.CanShowAd())
        {
            interstitial.Show();
        }
        else
        {
            RequestInterstitial();
            MonoBehaviour.print("Interstitial ad is not ready yet");
        }
    }
    #endregion

    #region REWARDED INTERSTITIAL
    public void RequestAndLoadRewardedInterstitialAd()
    {
#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
        string adUnitId = admob.androidRewardedInterstitial.Trim();
        if (VPConfigBase.instance.debug) adUnitId = admob.debugRewardedInterstitial.Trim();
#elif UNITY_IPHONE
        string adUnitId = admob.iosRewardedInterstitial.Trim();
#else
        string adUnitId = "unexpected_platform";
#endif
        if (rewardedInterstitial != null)
        {
            rewardedInterstitial.Destroy();
            rewardedInterstitial = null;
        }

        RewardedInterstitialAd.Load(adUnitId, CreateAdRequest(), 
            (RewardedInterstitialAd ad, LoadAdError error) =>
        {
            // if error is not null, the load request failed.
            if (error != null || ad == null)
            {
                Debug.LogError("Rewarded Interstitial ad failed to load an ad " +
                               "with error : " + error);
                return;
            }

            Debug.Log("Rewarded Interstitial ad loaded with response : "
                      + ad.GetResponseInfo());

            rewardedInterstitial = ad;
            RegisterRewardedInterstitialEventHandlers(ad);
            });
    }

    public void ShowRewardedInterstitialAd()
    {
        const string rewardMsg =
            "Rewarded interstitial ad rewarded the user. Type: {0}, amount: {1}.";

        if (rewardedInterstitial != null)
        {
            rewardedInterstitial.Show((Reward reward) =>
            {
                // TODO: Reward the user.
                Debug.Log(String.Format(rewardMsg, reward.Type, reward.Amount));
            });
        }
    }
    #endregion

    #region REWARDED

    public void RequestAndLoadRewardedAd()
    {
#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
        string adUnitId = admob.androidRewarded.Trim();
        if (VPConfigBase.instance.debug) adUnitId = admob.debugRewarded.Trim();
#elif UNITY_IPHONE
        string adUnitId = admob.iosRewarded.Trim();
#else
        string adUnitId = "unexpected_platform";
#endif
        if (rewardedAd != null)
        {
            rewardedAd.Destroy();
            rewardedAd = null;
        }

        RewardedAd.Load(adUnitId, CreateAdRequest(), 
            (RewardedAd ad, LoadAdError error) =>
        {
            // if error is not null, the load request failed.
            if (error != null || ad == null)
            {
                Debug.LogError("Rewarded ad failed to load an ad " +
                               "with error : " + error);
                return;
            }

            Debug.Log("Rewarded ad loaded with response : "
                      + ad.GetResponseInfo());

            rewardedAd = ad;
            RegisterRewardedAdEventHandlers(ad);
        });
    }

    public void ShowRewardedAd()
    {
        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show((Reward reward) =>
            {
                BTN.RewardVideo();
            });
        }
        else
        {
            RequestAndLoadRewardedAd();
            MonoBehaviour.print("Reward based video ad is not ready yet");
        }
    }
    #endregion

    #region Callback handlers
    private void RegisterAppOpenEventHandlers(AppOpenAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("App open ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("App open ad recorded an impression.");
        };
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("App open ad full screen content opened.");
        };
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("App open ad full screen content closed.");
            LoadAppOpenAd();
        };
        ad.OnAdFullScreenContentFailed += (AdError adError) =>
        {
            Debug.LogError("App open ad failed to open full screen content " +
                           "with error : " + adError.GetMessage());
            LoadAppOpenAd();
        };
    }

    public void HandleBannerAdLoaded()
    {
        //HideBanner();
        print("HandleAdLoaded event received.");
    }

    public void HandleBannerAdFailedToLoad(LoadAdError loadAdError)
    {
        print("HandleFailedToReceiveAd event received with message: " + loadAdError.GetMessage());
    }

    public void HandleBannerAdOpened()
    {
        print("HandleAdOpened event received");
    }

    public void HandleBannerAdClosed()
    {
        print("HandleAdClosed event received");
    }

    private void RegisterInterstitialEventHandlers(InterstitialAd ad)
    {
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Interstitial ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Interstitial ad recorded an impression.");
        };
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Interstitial ad full screen content opened.");
        };
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Interstitial ad full screen content closed.");
            RequestInterstitial();
        };
        ad.OnAdFullScreenContentFailed += (AdError adError) =>
        {
            Debug.LogError("Interstitial ad failed to open full screen content " +
                           "with error : " + adError.GetMessage());
            RequestInterstitial();
        };
    }

    private void RegisterRewardedInterstitialEventHandlers(RewardedInterstitialAd ad)
    {
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Rewarded interstitial ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Rewarded interstitial ad recorded an impression.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Rewarded interstitial ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded interstitial ad full screen content closed.");
            RequestAndLoadRewardedInterstitialAd();
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError adError) =>
        {
            Debug.LogError("Rewarded interstitial ad failed to open full screen content " +
                           "with error : " + adError);
            RequestAndLoadRewardedInterstitialAd();
        };
    }

    private void RegisterRewardedAdEventHandlers(RewardedAd ad)
    {
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Rewarded ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Rewarded ad recorded an impression.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Rewarded ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded ad full screen content closed.");
            RequestAndLoadRewardedAd();
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError adError) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + adError);
            RequestAndLoadRewardedAd();
        };
    }
    #endregion

    [Serializable]
    public class Admob
    {
        [Header("App Open")]
        public string androidAppOpen;
        public string iosAppOpen;
        [Header("Banner")]
        public string androidBanner;
        public string iosBanner;
        [Header("Interstitial")]
        public string androidInterstitial;
        public string iosInterstitial;
        [Header("Rewarded Interstitial")]
        public string androidRewardedInterstitial;
        public string iosRewardedInterstitial;
        [Header("Rewarded Video")]
        public string androidRewarded;
        public string iosRewarded;
        [Header("Debugging")]
        public string debugAppOpen;
        public string debugBanner;
        public string debugInterstitial;
        public string debugRewardedInterstitial;
        public string debugRewarded;
    }
}