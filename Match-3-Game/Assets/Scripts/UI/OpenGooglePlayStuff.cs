using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenGooglePlayStuff : MonoBehaviour
{
    Setup googleplay;

    // Start is called before the first frame update
    void Start()
    {
        googleplay = Setup.Get();
    }

    public void OpenAchievements()
    {
        //Setup.OpenAchievements();
        googleplay.OpenAchievements();
    }

    public void OpenLeaderboards()
    {
        //Setup.OpenLeaderboards();
        googleplay.OpenLeaderboards();
    }
}
