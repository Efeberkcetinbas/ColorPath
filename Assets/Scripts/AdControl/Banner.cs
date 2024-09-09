using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using System.Collections;

public class Banner : MonoBehaviour
{
 
    [SerializeField] BannerPosition _bannerPosition = BannerPosition.BOTTOM_CENTER;
    [SerializeField] string _androidAdUnitId = "Banner_Android";
    [SerializeField] string _iOSAdUnitId = "Banner_iOS";
    private string _adUnitId;

    void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnSuccessUI, ShowBannerAd);
        EventManager.AddHandler(GameEvent.OnFailUI, ShowBannerAd);
        EventManager.AddHandler(GameEvent.OnNextLevel, HideBannerAd);
        EventManager.AddHandler(GameEvent.OnGameStart, HideBannerAd);
        EventManager.AddHandler(GameEvent.OnRestartLevel,HideBannerAd);
    }

    void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnSuccessUI, ShowBannerAd);
        EventManager.RemoveHandler(GameEvent.OnFailUI, ShowBannerAd);
        EventManager.RemoveHandler(GameEvent.OnNextLevel, HideBannerAd);
        EventManager.RemoveHandler(GameEvent.OnGameStart, HideBannerAd);
        EventManager.RemoveHandler(GameEvent.OnRestartLevel,HideBannerAd);
    }

    void Start()
    {
        // Set the correct Ad Unit ID based on platform
        #if UNITY_IOS
            _adUnitId = _iOSAdUnitId;
        #elif UNITY_ANDROID
            _adUnitId = _androidAdUnitId;
        #endif

        // Set the banner position
        Advertisement.Banner.SetPosition(_bannerPosition);

        // Load the banner ad
        LoadBanner();
    }

    public void LoadBanner()
    {
        BannerLoadOptions options = new BannerLoadOptions
        {
            loadCallback = OnBannerLoaded,
            errorCallback = OnBannerError
        };

        Advertisement.Banner.Load(_adUnitId, options);
    }

    void OnBannerLoaded()
    {
        Debug.Log("Banner loaded");
        // Optionally show the banner after it's loaded
        Advertisement.Banner.Show(_adUnitId);
    }

    void OnBannerError(string message)
    {
        Debug.Log($"Banner Error: {message}");
    }

    public void ShowBannerAd()
    {
        Advertisement.Banner.Show(_adUnitId);
    }

    public void HideBannerAd()
    {
        Advertisement.Banner.Hide();
    }

    
}