using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;

public class Setup : MonoBehaviourSingleton<Setup>
{
#if UNITY_ANDROID

    // Start is called before the first frame update
    void Start()
    {
        InitializeGPS();
    }

    private void InitializeGPS()
    {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
        // requests an ID token be generated.  This OAuth token can be used to
        //  identify the player to other services such as Firebase.
        .RequestIdToken()
        .Build();

        PlayGamesPlatform.InitializeInstance(config);
        // recommended for debugging:
        PlayGamesPlatform.DebugLogEnabled = true;
        // Activate the Google Play Games platform
        PlayGamesPlatform.Activate();
    }

    public void SignIn()
    {
        Social.localUser.Authenticate((bool success) =>
        {
            if (success)
            {
                Debug.Log("LOG Correcto");
            }
            else
            {
                Debug.Log("LOG Incorrecto");
            }
            // handle success or failure
        });
    }

    public void SignOut()
    {
        PlayGamesPlatform.Instance.SignOut();
    }
#endif
}
