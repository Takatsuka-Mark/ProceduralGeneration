using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/*
 *
 *
 * This uses an algorithm described in:
 *     http://staffwww.itn.liu.se/~stegu/simplexnoise/simplexnoise.pdf
 */


namespace defaultNamespace
{
    public class SimplexNoise
    {
        private int[] _permSource = {
            151, 160, 137, 91, 90, 15, 131, 13, 201, 95, 96, 53, 194, 233, 7, 225, 140, 36, 103, 30, 69, 142, 8, 99, 
            37, 240, 21, 10, 23, 190, 6, 148, 247, 120, 234, 75, 0, 26, 197, 62, 94, 252, 219, 203, 117, 35, 11, 32, 
            57, 177, 33, 88, 237, 149, 56, 87, 174, 20, 125, 136, 171, 168, 68, 175, 74, 165, 71, 134, 139, 48, 27, 
            166, 77, 146, 158, 231, 83, 111, 229, 122, 60, 211, 133, 230, 220, 105, 92, 41, 55, 46, 245, 40, 244, 102, 
            143, 54, 65, 25, 63, 161, 1, 216, 80, 73, 209, 76, 132, 187, 208, 89, 18, 169, 200, 196, 135, 130, 116, 
            188, 159, 86, 164, 100, 109, 198, 173, 186, 3, 64, 52, 217, 226, 250, 124, 123, 5, 202, 38, 147, 118, 126, 
            255, 82, 85, 212, 207, 206, 59, 227, 47, 16, 58, 17, 182, 189, 28, 42, 223, 183, 170, 213, 119, 248, 152, 
            2, 44, 154, 163, 70, 221, 153, 101, 155, 167, 43, 172, 9, 129, 22, 39, 253, 19, 98, 108, 110, 79, 113, 
            224, 232, 178, 185, 112, 104, 218, 246, 97, 228, 251, 34, 242, 193, 238, 210, 144, 12, 191, 179, 162, 241, 
            81, 51, 145, 235, 249, 14, 239, 107, 49, 192, 214, 31, 181, 199, 106, 157, 184, 84, 204, 176, 115, 121, 
            50, 45, 127, 4, 150, 254, 138, 236, 205, 93, 222, 114, 67, 29, 24, 72, 243, 141, 128, 195, 78, 66, 215, 
            61, 156, 180
        };

        private int[] _perm;
        private int[][] _grad =
        {
            new[] {1, 1, 0}, new[] {-1, 1, 0}, new[] {1, -1, 0},
            new[] {-1, -1, 0}, new[] {1, 0, 1}, new[] {-1, 0, 1},
            new[] {1, 0, -1}, new[] {-1, 0, -1}, new[] {0, 1, 1},
            new[] {0, -1, 1}, new[] {0, 1, -1}, new[] {0, -1, -1}
        };

        private int RandomSize = 256;

        public SimplexNoise()
        {
            RandomizeSource(66);
        }
        
        public SimplexNoise(int seed)
        {
            RandomizeSource(Constants.Seed);
        }


        public double generateNoise(Vector3 vertex)
        {
            double x = vertex.x;
            double y = vertex.y;
            double z = vertex.z;
            
            // "Noise contributions from the four corners"
            double n0 = 0.0, n1 = 0.0, n2 = 0.0, n3 = 0.0;

            // Skew input space to determine which cell we're in
            double F3 = 1.0 / 3.0;
            double s = (x + y + z) * F3;
            int i = fastFloor(x + s);
            int j = fastFloor(y + s);
            int k = fastFloor(z + s);

            double G3 = 1.0 / 6.0;
            double t = (i + j + k) * G3;
            
            double X0 = i - t;    // unskew cell origin to (x, y, z) space
            double Y0 = j - t;
            double Z0 = k - t;
            double x0 = x - X0;    // distance from origin
            double y0 = y - Y0;
            double z0 = z - Z0;
            
            // determine the simplex we are in
            int i1, j1, k1;    // offsets for 2nd corner of simplex in (i, j, k) coords
            int i2, j2, k2;    // offsets for 3rd corner of simplex
            
            if (x0 >= y0)          // set the X Y Z Order 
            {
                if (y0 >= z0)
                {
                    i1 = 1;
                    j1 = 0;
                    k1 = 0;
                    i2 = 1;
                    j2 = 1;
                    k2 = 0;
                }
                else if (x0 >= z0)
                {
                    i1 = 1;
                    j1 = 0;
                    k1 = 0;
                    i2 = 1;
                    j2 = 0;
                    k2 = 1;
                }
                else
                {
                    i1 = 0;
                    j1 = 0;
                    k1 = 1;
                    i2 = 1;
                    j2 = 0;
                    k2 = 1;
                }
            }
            else
            {
                if (y0 < z0)
                {
                    i1 = 0;
                    j1 = 0;
                    k1 = 1;
                    i2 = 0;
                    j2 = 1;
                    k2 = 1;
                }
                else if (x0 < z0)
                {
                    i1 = 0;
                    j1 = 1;
                    k1 = 0;
                    i2 = 0;
                    j2 = 1;
                    k2 = 1;
                }
                else
                {
                    i1 = 0;
                    j1 = 1;
                    k1 = 0;
                    i2 = 1;
                    j2 = 1;
                    k2 = 0;
                }
            }

            // A step of (1, 0, 0) in (i, j, k) means we are going (1 - c, -c, -c) in (x, y, z)
            //    c = 1/6
            double x1 = x0 - i1 + G3;    // offsets for 2nd corner in (x, y, z) coord
            double y1 = y0 - j1 + G3;
            double z1 = z0 - k1 + G3;
            double x2 = x0 - i2 + F3;    // offsets for 3rd corner
            double y2 = y0 - j2 + F3;
            double z2 = z0 - k2 + F3;
            double x3 = x0 - 1.0 + 3.0 * G3;    // offsets for final corner
            double y3 = y0 - 1.0 + 3.0 * G3;
            double z3 = z0 - 1.0 + 3.0 * G3;

            // hashed gradient indices for four simplex corners
            int ii = i & 255;
            int jj = j & 255;
            int kk = k & 255;


            int gi0 = _perm[ii + _perm[jj + _perm[kk]]] % 12;
            int gi1 = _perm[ii + i1 + _perm[jj + j1 + _perm[kk + k1]]] % 12;
            int gi2 = _perm[ii + i2 + _perm[jj + j2 + _perm[kk + k2]]] % 12;
            int gi3 = _perm[ii + 1  + _perm[jj + 1  + _perm[kk + 1 ]]] % 12;
            
            // Calculate the "contribution" from the corners
            double t0 = 0.6 - x0 * x0 - y0 * y0 - z0 * z0;    // TODO check to see if 0.6 generates worse results
            double t1 = 0.6 - x1 * x1 - y1 * y1 - z1 * z1;
            double t2 = 0.6 - x2 * x2 - y2 * y2 - z2 * z2;
            double t3 = 0.6 - x3 * x3 - y3 * y3 - z3 * z3;
            if (t0 < 0)
                n0 = 0.0;
            else
            {
                t0 *= t0;
                n0 = t0 * t0 * dot(_grad[gi0], x0, y0, z0);
            }
            
            if (t1 < 0)
                n1= 0.0;
            else
            {
                t1 *= t1;
                n1= t1 * t1 * dot(_grad[gi1], x1, y1, z1);
            }
            
            if (t2 < 0)
                n2 = 0.0;
            else
            {
                t2 *= t2;
                n2 = t2 * t2 * dot(_grad[gi2], x2, y2, z2);
            }
            
            if (t3 < 0)
                n3 = 0.0;
            else
            {
                t3 *= t3;
                n3 = t3 * t3 * dot(_grad[gi3], x3, y3, z3);
            }

            // finally, we add contributions from each corer to get final value.
            // result is scaled to [-1, 1]
            return 32.0 * (n0 + n1 + n2 + n3);
        }
        

        private void RandomizeSource(int seed)
        {
            _perm = new int[RandomSize * 2];
            
            // basically we convert the seed into bytes, and xor the values with the bytes
            var seedBytes = new byte[4];
            UnpackLittleUint(seed, ref seedBytes);
            
            for (int i = 0; i < _permSource.Length; i++)
            {
                _perm[i] = _permSource[i] ^ seedBytes[0];

                for (int j = 1; j < 4; j++)
                {
                    _perm[i] ^= seedBytes[j];
                }

                _perm[i + RandomSize] = _perm[i];
            }
        }

        private int fastFloor(double x)
        {
            return x > 0 ? (int) x : (int) x - 1;
        }
        
        private double dot(int[] g, double x, double y, double z)
        {
            return g[0] * x + g[1] * y + g[2] * z;
        }

        private double mix(double a, double b, double t)
        {
            return (1 - t) * a + t * b;
        }

        private double fade(double t)
        {
            return t * t * t * (t * (t * 6 - 15) + 10);
        }

        private byte[] UnpackLittleUint(int value, ref byte[] buffer)
        {
            if (buffer.Length < 4)
                Array.Resize(ref buffer, 4);
            buffer[0] = (byte) (value & 0x00ff);
            buffer[1] = (byte) ((value & 0xff00) >> 8);
            buffer[2] = (byte) ((value & 0x00ff0000) >> 16);
            buffer[3] = (byte) ((value & 0xff000000) >> 24);

            return buffer;
        }

    }
}
