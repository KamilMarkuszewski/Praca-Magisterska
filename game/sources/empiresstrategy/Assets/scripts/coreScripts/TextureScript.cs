using UnityEngine;
using System.Collections;
using Core;
using Core.Main;
using System.Collections.Generic;
using System;
using Core.Interfaces;

public class TextureScript : MonoBehaviour
{


    #region Services

    TextureService _textureService;
    TextureService textureService
    {
        get
        {
            if (_textureService == null)
            {
                _textureService = ServiceLocator.GetService<TextureService>();
            }
            return _textureService;
        }
    }

    #endregion

    #region Properties
    private IDictionary<TextureService.TextureNames, Texture2D> textures;

    #endregion


    // Use this for initialization
    void Start()
    {
        try
        {
            packTextures();
            textureService.Init(textures);
        }
        catch (Exception e)
        {
            ExceptionHandler.catchException(e);
        }
    }

    void packTextures()
    {
        textures = new Dictionary<TextureService.TextureNames, Texture2D>();
        textures.Add(TextureService.TextureNames.GuiMiniMap, GuiMiniMapTxt);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public Texture2D GuiMiniMapTxt;
}
