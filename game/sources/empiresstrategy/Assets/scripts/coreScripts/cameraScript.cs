using UnityEngine;
using System.Collections;
using Core;
using Core.Main;
using Core.Interfaces;

public class cameraScript : MonoBehaviour
{


    #region Services

    CameraService _cameraService;
    CameraService cameraService
    {
        get
        {
            if (_cameraService == null)
            {
                _cameraService = ServiceLocator.GetService<CameraService>();
            }
            return _cameraService;
        }
    }

    #endregion

    // Use this for initialization
    void Start()
    {
        cameraService.Start();
    }

    // Update is called once per frame
    void Update()
    {
        cameraService.Update();
    }

    void Awake()
    {

    }

    void OnGUI()
    {

    }
}
