using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class AnalyticsManager : MonoBehaviour
{
    private void Awake()
    {
        
    }

    public void TermineNivel(int lvl,int vidas,int puntaje)
    {
        Analytics.CustomEvent("level_finished", new Dictionary<string, object>
        {
            {"nivel",lvl},
            {"vidas",vidas},
            {"puntaje",puntaje}
        });
    }

    public void PuntajeTotal(int puntaje)
    {
        Analytics.CustomEvent("level_finished", new Dictionary<string, object>
        {
            {"nivel",puntaje}
        });
    }
}
