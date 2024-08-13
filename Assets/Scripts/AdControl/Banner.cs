using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
 
public class Banner : MonoBehaviour
{
 
    [Header("Banner Ad Settings")]
    [SerializeField] BannerPosition _bannerPosition;
    [SerializeField] string _androidAdUnitId = "Banner_Android";
    [SerializeField] string _iOSAdUnitId = "Banner_iOS";
    string _adUnitId = null;

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnGameStart, ShowBannerAd);
        EventManager.AddHandler(GameEvent.OnSuccessUI, ShowBannerAd);
        EventManager.AddHandler(GameEvent.OnFailUI, ShowBannerAd);
        EventManager.AddHandler(GameEvent.OnNextLevel, HideBannerAd);
        EventManager.AddHandler(GameEvent.OnGameStart,HideBannerAd);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnGameStart, ShowBannerAd);
        EventManager.RemoveHandler(GameEvent.OnSuccessUI, ShowBannerAd);
        EventManager.RemoveHandler(GameEvent.OnFailUI, ShowBannerAd);
        EventManager.RemoveHandler(GameEvent.OnNextLevel, HideBannerAd);
        EventManager.RemoveHandler(GameEvent.OnGameStart,HideBannerAd);
    }

    private void Start()
    {
        if (Advertisement.isInitialized)
        {
            // Get the Ad Unit ID for the current platform
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
        else
        {
            // Initialize Unity Ads
            Advertisement.Initialize("YOUR_GAME_ID", true); // Replace "YOUR_GAME_ID" with your actual game ID
        }
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
    }

    void OnBannerError(string message)
    {
        Debug.Log($"Banner Error: {message}");
    }

    void ShowBannerAd()
    {
        if (Advertisement.IsReady(_adUnitId))
        {
            Advertisement.Banner.Show(_adUnitId);
        }
        else
        {
            Debug.Log("Banner ad is not ready to be shown.");
        }
    }

    void HideBannerAd()
    {
        Advertisement.Banner.Hide();
    }

    
}