using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SignInOut : MonoBehaviour
{
    public Text buttonText;
    public bool isLogged;

    private Setup googleplay;
    // Start is called before the first frame update
    void Start()
    {
        googleplay = Setup.Get();
    }

    public void switchState()
    {
        if(isLogged)
        {
            googleplay.SignOut();
            buttonText.text = "Sign In";
        }
        else
        {
            googleplay.SignIn();
            buttonText.text = "Sign Out";
        }

        isLogged = !isLogged;
    }
}
