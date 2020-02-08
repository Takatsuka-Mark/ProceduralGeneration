using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace defaultNamespace
{
    
    //Takes float array representing colors 0-255 (black and white) and outputs a colored texture 2d
    public class FlatTexturing
    {
        private float[,] bwImage;
        private Texture2D _colors;

        public FlatTexturing(float[,] bwImage){ 
            this.bwImage = bwImage;
        }
        
        //Takes in 2d array of nums 0-255 representing colors from black to white and returns texture2d of full range of 
        //colors according to height
        public Texture2D makeColor()
        {
            var noiseMap = new FlatGenNoise();
            
            return _colors;
        }




    }
}