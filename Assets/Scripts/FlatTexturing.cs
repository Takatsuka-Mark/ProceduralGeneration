﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

namespace defaultNamespace
{
    
    //Takes float array representing colors 0-255 (black and white) and outputs a colored texture 2d
    public class FlatTexturing
    {
        private float[] bwImage;
        //private Texture2D Colors;
        private Color[] Pixels;

        public FlatTexturing(float[] bwImage){ 
            this.bwImage = bwImage;
            //Colors = new Texture2D(Constants.ChunkWidth, Constants.ChunkHeight);
            Pixels = new Color[Constants.ChunkWidth * Constants.ChunkHeight];
        }
        
        //Takes in 2d array of nums 0-255 representing colors from black to white and returns texture2d of full range of 
        //colors according to height
        public Color[] makeColor()
        {
            for (int i = 0; i < bwImage.Length; i++)
            {
                var value = bwImage[i];
                if (value < 75.0) //20,20,20 - black
                {
                    Pixels[i] = new Color(20, 20, 20);    
                }
                else if (value < 155.0) //140,62,14 - brown
                {
                    Pixels[i] = new Color(140, 62, 14);
                }
                else if (value < 185.0) //10,145,15 - green
                {
                    Pixels[i] = new Color(10, 145, 15);
                }
                else //203,243,255 - light blue
                {
                    Pixels[i] = new Color(203, 243, 255);
                }
            }
            // Colors.SetPixels(Pixels);
            // Colors.Apply();
            return Pixels;
        }
    }
}