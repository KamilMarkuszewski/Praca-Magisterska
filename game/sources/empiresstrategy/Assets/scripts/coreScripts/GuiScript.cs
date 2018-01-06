using UnityEngine;
using System.Collections;
using Gui;
using Core.Main;
using Core.Interfaces;

public class GuiScript : MonoBehaviour
{

    #region Services

    GuiService _guiService;
    GuiService guiService
    {
        get
        {
            if (_guiService == null)
            {
                _guiService = ServiceLocator.GetService<GuiService>();
            }
            return _guiService;
        }
    }

    #endregion

    // Use this for initialization
    void Start()
    {
        guiService.Start();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Awake()
    {

    }

    void OnGUI()
    {
        guiService.onGUI();
    }
}
