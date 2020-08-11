﻿using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Multiplayer.Extensions
{
    public static class UI
    {
        public static Texture2D LoadPNG(string filePath, int width, int height)
        {
            Texture2D tex = null;
            byte[] fileData;
            if (File.Exists(filePath))
            {
                fileData = File.ReadAllBytes(filePath);
                tex = new Texture2D(width, height);
                tex.LoadImage(fileData);
            }
            return tex;
        }
    }
}