using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Core
{
    public class TextureService
    {
        public IDictionary<TextureNames, Texture2D> textures;

        public enum TextureNames
        {
            None,
            GuiMiniMap
        };

        public void Init(IDictionary<TextureNames, Texture2D> textures)
        {
            this.textures = textures;
        }


        public Texture2D GetTexture(TextureNames name)
        {
            Texture2D txt;
            try
            {
                textures.TryGetValue(name, out txt);
            }
            catch (Exception)
            {
                return null;
            }
            return txt;
        }
    }
}
