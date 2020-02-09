using UnityEngine;

namespace defaultNamespace
{
    public class MeshMaker : MonoBehaviour
    {
        private FlatGenNoise _noiseGen;
        private float[] _bwImage;
        private Vector3[] _vertexArray;
        private FlatTexturing _flatTexturing;
        void Start()
        {
            _noiseGen = new FlatGenNoise();
            _bwImage = _noiseGen.CalcNoise();
            _flatTexturing = new FlatTexturing(_bwImage);
            
            Mesh flatMesh = GetComponent<MeshFilter>().mesh; //Instantiate new Mesh
            Color[] color = _flatTexturing.makeColor();
            flatMesh.SetColors(color);
            _vertexArray = flatMesh.vertices;
            _vertexArray = AddFloatNoise(_vertexArray);
            flatMesh.SetVertices(_vertexArray);

            
            flatMesh.UploadMeshData(true);
            flatMesh.RecalculateBounds();
        }

        Vector3[] AddFloatNoise(Vector3[] vertexArray)
        {
            for (int i = 0; i < vertexArray.Length; i++)
            {
                vertexArray[i] += Vector3.up * (_bwImage[i] * (float).1);
            }
            return vertexArray;
        }
        public float[] GetbwImage()
        {
            return _bwImage;
        }
    }
}