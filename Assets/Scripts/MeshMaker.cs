using System.IO;
using defaultNamespace;
using UnityEngine;

public class MeshMaker : MonoBehaviour
{
    private FlatGenNoise _noise;
    private float[] y;
    private FlatTexturing _flatTexturing;
    private Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;

    public Renderer textureRenderer;
    public int xSize = 50;
    public int zSize = 50;
    void Start()
    {
        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        _noise = new FlatGenNoise();
        y = _noise.CalcNoise();
        // _flatTexturing = new FlatTexturing(y);
        
        // Texture2D tex = new Texture2D(Constants.ChunkWidth, Constants.ChunkHeight);
        // Color[] colors = _flatTexturing.makeColor();
        // tex.SetPixels(colors);
        // tex.Apply();
        //
        // textureRenderer.sharedMaterial.mainTexture = tex;
        // textureRenderer.transform.localScale = new Vector3(Constants.ChunkWidth,1, Constants.ChunkHeight);
        
        CreateShape();
        UpdateMesh();
    }

    void CreateShape()
    {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];
        for (int x = 0, i = 0; x <= xSize; x++)
        {
            for (int z = 0; z <= zSize; z++)
            {
                // float y = Mathf.PerlinNoise(x * .3f, z * .3f) * 2f;
                vertices[i] = new Vector3(x, y[i], z);     //y[i] with FlatGenNoise
                i++;
            }
        }

        triangles = new int[xSize * zSize * 6];
        int vert = 0;
        int tris = 0;
        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;
                vert++;
                tris += 6;
            }
            vert++;
        }
    }

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        
        mesh.RecalculateBounds();
    }

    private void OnDrawGizmos()
    {
        if (vertices == null)
        {
            return;
        }
        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], .1f);
        }
    }
}
// using System;
//     using UnityEngine;
//     
//     namespace defaultNamespace
//     {
//         public class MeshMaker : MonoBehaviour
//         {
//             private FlatGenNoise _noiseGen;
//             private float[] _bwImage;
//             private Vector3[] _vertexArray;
//             private FlatTexturing _flatTexturing;
//             void Start()
//             {
//                 _noiseGen = new FlatGenNoise();
//                 _bwImage = _noiseGen.CalcNoise();
//                 _flatTexturing = new FlatTexturing(_bwImage);
//     
//                 Mesh flatMesh = GetComponent<MeshFilter>().mesh; //Instantiate new Mesh
//                 Vector3[] bwImage2 = new Vector3[_bwImage.Length];
//                 for (int i = 0; i < _bwImage.Length; i++)
//                 {
//                     Vector3 vec = new Vector3(0, (float)(_bwImage[i] * 1), 0);
//                 }
//             
//                 flatMesh.vertices = bwImage2;
//     
//                 int[] newTriangles = new int[_bwImage.Length];
//                 for (int i = 0; i < _bwImage.Length; i++)
//                 {
//                     newTriangles[i] = (int)_bwImage[i];
//                 }
//                 flatMesh.triangles = newTriangles;
//                 Color[] color = _flatTexturing.makeColor(); 
//                 flatMesh.SetColors(color);
//                 _vertexArray = flatMesh.vertices;
//                 Debug.Log(_vertexArray.Length);
//                 Debug.Log(_bwImage.Length);
//                 Debug.Log(flatMesh.triangles.Length);
//                 // _vertexArray = AddFloatNoise(_vertexArray);
//                 //flatMesh.SetVertices(_vertexArray);
//                 
//                 flatMesh.UploadMeshData(true);
//                 // flatMesh.RecalculateBounds();
//             }
//             Vector3[] AddFloatNoise(Vector3[] vertexArray)
//             {
//                 for (int i = 0; i < vertexArray.Length; i++)
//                 {
//                     vertexArray[i] += Vector3.up * (_bwImage[i] * (float)1);
//                 }
//                 return vertexArray;
//             }
//             public float[] GetbwImage()
//             {
//                 return _bwImage;
//             }
//         }
//     }
// Builds a Mesh containing a single triangle with uvs.
// Create arrays of vertices, uvs and triangles, and copy them into the mesh.
