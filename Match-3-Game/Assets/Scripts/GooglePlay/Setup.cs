using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;

public class Setup : MonoBehaviourSingleton<Setup>
{
    //public Text title;


#if UNITY_ANDROID
    //
    // Start is called before the first frame update
    void Start()
    {
        InitializeGPS();
        SignIn();
        UploadScore(GameManager.Get().score);
    }

    private void InitializeGPS()
    {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.Activate();
    }

    public void SignIn()
    {
        Social.localUser.Authenticate((bool success) => {

            if(success)
            {
                Debug.Log("Good!");
            }
            else
            {
                Debug.Log("Baaad.");
            }
            // handle success or failure
        });
    }

    public void SignOut()
    {
        PlayGamesPlatform.Instance.SignOut();
    }

    public void UnlockAchievementTest()
    {
        Social.ReportProgress("CgkIhYDc8t4eEAIQAg", 100.0f, (bool success) =>
        {
            // handle success or failure
        });
    }

    public void OpenAchievements()
    {
        Social.ShowAchievementsUI();
    }

    public void OpenLeaderboards()
    {
        Social.ShowLeaderboardUI();
    }

    public void UploadScore(int score)
    {
        Social.ReportScore(score, "CgkIhYDc8t4eEAIQAw", (bool success) =>
        {
            if (success)
            {
                Debug.Log("log to leaderboard succeeded");
                //title.text = "It actually worked!";
            }
            else
            {
                Debug.Log("log to leaderboard failed");
                //title.text = "it didnt worked :c!";
            }

            // handle success or failure
        });
    }
#endif
}
