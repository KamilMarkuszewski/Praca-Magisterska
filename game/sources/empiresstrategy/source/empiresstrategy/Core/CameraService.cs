using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Core.Main;
using Core.PlayerData;
using Core.Interfaces;

namespace Core
{
    public class CameraService
    {
        #region Services

        private static PlayerDataService _playerDataService;
        public static PlayerDataService playerDataService
        {
            get
            {
                if (_playerDataService == null) _playerDataService = ServiceLocator.GetService<PlayerDataService>();
                return _playerDataService;
            }
        }

        private static SceneLoaderService _sceneLoader;
        public static SceneLoaderService sceneLoader
        {
            get
            {
                if (_sceneLoader == null) _sceneLoader = ServiceLocator.GetService<SceneLoaderService>();
                return _sceneLoader;
            }
        }


        #endregion

        #region Properties

        private Camera _cam;
        public Camera cam
        {
            get
            {
                if (_cam == null)
                {
                    _cam = Camera.main;
                }
                return _cam;
            }
            set
            {
                _cam = value;
            }
        }



        private Camera _mapCam;
        public Camera mapCam
        {
            get
            {
                if (_mapCam == null)
                {
                    _mapCam = FindMapCamera();
                }
                return _mapCam;
            }
            set
            {
                _mapCam = value;
            }
        }



        float height = 3.0f;

        #endregion

        public void Start()
        {
            try
            {

            }
            catch (Exception e)
            {
                ExceptionHandler.catchException(e);
            }
        }

        public void Update()
        {
            try
            {
                if (sceneLoader.current == SceneLoaderService.Scene.scPlay)
                {
                    inputKeys();
                    inputMouse();
                }
            }
            catch (Exception e)
            {
                ExceptionHandler.catchException(e);
            }
        }

        private void inputMouse()
        {
            float _x = Input.mousePosition.x;
            float _y = Input.mousePosition.y;

            //Debug.Log(_x + " : " + _y);
            //Debug.Log(Screen.width + " : " + Screen.height);

            if (_y < 10)
            {
                moveDown(playerDataService.mySettings.mouseScrollSpeed);
            }
            if (_y > Screen.height - 10)
            {
                moveUp(playerDataService.mySettings.mouseScrollSpeed);
            }
            if (_x < 10)
            {
                moveLeft(playerDataService.mySettings.mouseScrollSpeed);
            }
            if (_x > Screen.width - 10)
            {
                moveRight(playerDataService.mySettings.mouseScrollSpeed);
            }

        }

        private void inputKeys()
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                moveUp(playerDataService.mySettings.keysScrollSpeed);
            }

            if (Input.GetKey(KeyCode.DownArrow))
            {
                moveDown(playerDataService.mySettings.keysScrollSpeed);
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                moveLeft(playerDataService.mySettings.keysScrollSpeed);
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                moveRight(playerDataService.mySettings.keysScrollSpeed);
            }
        }

        public void onScroll()
        {
            Event e = Event.current;
            if (e.type == EventType.ScrollWheel)
            {
                if (e.delta.y > 2.0)
                {
                    height -= 1.1f;
                }
                if (e.delta.y < -2.0)
                {
                    height += 1.1f;
                }
                if (height < 0.0)
                {
                    height = 0.0f;
                }
                if (height > 20.0)
                {
                    height = 20.0f;
                }
                Camera.main.fieldOfView = 60 - height;
            }

        }

        #region MoveScreen

        private void moveUp(float speed)
        {
            Vector3 pos = cam.transform.position;
            pos.z += speed;
            if (pos.z <= 800 && pos.z >= -800)
            {
                cam.transform.position = pos;
            }
        }

        private void moveDown(float speed)
        {
            Vector3 pos = cam.transform.position;
            pos.z -= speed;
            if (pos.z <= 800 && pos.z >= -800)
            {
                cam.transform.position = pos;
            }
        }


        private void moveLeft(float speed)
        {
            Vector3 pos = cam.transform.position;
            pos.x -= speed;
            if (pos.x <= 800 && pos.x >= -800)
            {
                cam.transform.position = pos;
            }
        }


        private void moveRight(float speed)
        {
            Vector3 pos = cam.transform.position;
            pos.x += speed;
            if (pos.x <= 800 && pos.x >= -800)
            {
                cam.transform.position = pos;
            }
        }

        #endregion

        #region MiniMap

        private Camera FindMapCamera()
        {
            foreach (var c in Camera.allCameras)
            {
                if (c != Camera.main) return c;
            }
            return null;
        }

        #endregion

    }
}
