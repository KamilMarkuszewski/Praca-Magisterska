using UnityEngine;
using System.Collections;
using Core.Main;
using Core.PlayerData;
using System;
using Core.Interfaces;


public class CoreScript : MonoBehaviour
{
    void Awake()
    {
            Gui.Initializator.Init();
            StateMachine.Initializator.Init();
            Map.Initializator.Init();
            Graphs.Initializator.Init();
            Core.Initializator.Init();
    }

    #region Services

    private static MatchDataService _matchDataService;
    public static MatchDataService matchDataService
    {
        get
        {
            if (_matchDataService == null) _matchDataService = ServiceLocator.GetService<MatchDataService>();
            return _matchDataService;
        }
    }
    #endregion 

    // Use this for initialization
    void Start()
    {
        try
        {
            switch (Application.loadedLevelName)
            {

                case "scGameMenu":
                    // Scene0 - Splash
                    break;

                case "scPlay":
                    // Scene3 - Play
                    matchDataService.AfterLoaded();
                    break;
            }



            ExceptionHandler.drawErrorWindow();
        }
        catch (Exception e)
        {
            ExceptionHandler.catchException(e);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
