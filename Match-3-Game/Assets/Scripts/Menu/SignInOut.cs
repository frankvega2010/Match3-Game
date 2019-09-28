using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SignInOut : MonoBehaviour
{
    private Setup googleplay;
    // Start is called before the first frame update
    void Start()
    {
        googleplay = Setup.Get();
        Setup.Get().UploadScore(GameManager.Get().score);
    }

    public void signIn()
    {
        googleplay.SignIn();
    }

    public void signOut()
    {
        googleplay.SignOut();
    }
}
