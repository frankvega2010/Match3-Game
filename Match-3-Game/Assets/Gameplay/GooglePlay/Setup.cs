using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;

public class Setup : MonoBehaviour
{
#if UNITY_ANDROID && !UNITY_EDITOR

    // Start is called before the first frame update
    void Start()
    {
        InitializeGPS();
        Login();
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

    private void Login()
    {
        Social.localUser.Authenticate((bool success) => 
        {
            if(success)
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
#endif
}
