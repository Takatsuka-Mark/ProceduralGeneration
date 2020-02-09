using System.Collections.Generic;
using System.Runtime;
using defaultNamespace;
using UnityEngine;
using UnityEngine.UIElements;

namespace DefaultNamespace
{
    public class QuickHullAlgo
    {
        private const int INSIDE = -1;
        
        struct HorizonEdge
        {
            public int face;
            public int edge0;
            public int edge1;
        }

        struct Face
        {
            public int id;
            
            public int vertex0;
            public int vertex1;
            public int vertex2;

            public int face0;
            public int face1;
            public int face2;

            public Vector3 normal;

            public Face(int id, int vertex0, int vertex1, int vertex2, int face0, int face1, int face2, Vector3 normal)
            {
                this.id = id;
                this.vertex0 = vertex0;
                this.vertex1 = vertex1;
                this.vertex2 = vertex2;
                this.face0 = face0;
                this.face1 = face1;
                this.face2 = face2;
                this.normal = normal;
            }

            public bool Equals(Face other)
            {
                return (vertex0.Equals(other.vertex0)) &&
                       (vertex1.Equals(other.vertex1)) &&
                       (vertex2.Equals(other.vertex2)) &&
                       (this.id == other.id) &&
                       (face0.Equals(other.face0)) &&
                       (face1.Equals(other.face1)) &&
                       (face2.Equals(other.face2)) &&
                       (this.normal == other.normal);
            }
        }

        struct PointFace
        {
            public int point;
            public int face;
            public float distance;

            public PointFace(int point, int face, float distance)
            {
                this.point = point;
                this.face = face;
                this.distance = distance;
            }
        }

        private Dictionary<int, Face> faces;
        private List<PointFace> openSet;
        private HashSet<int> sectedSet;
        private List<HorizonEdge> horizon;
        private Dictionary<int, int> hullVertices;
        private int lastOpenSet = -1;
        private int currFaceId = 0;
        private int faceCount = 0;

        public void GenerateHull(List<Vector3> points, bool splitVerts, ref List<Vector3> verts, ref List<int> tris,
            ref List<Vector3> normals)
        {
            if (points.Count < 4)
            {
                throw new System.ArgumentException("Need at least 4 points to generate a convex hull");
            }
            
            Initialize(points, splitVerts);
            
            GenerateInitialHull(points);

            while (lastOpenSet >= 0)
            {
                GrowHull(points);
            }
            
            ExportMesh(points, splitVerts, ref verts, ref tris, ref normals);
        }
        
        void Initialize(List<Vector3> points, bool splitVerts)
        {
            faceCount = 0;
            lastOpenSet = -1;

            if (faces == null)
            {
                faces = new Dictionary<int, Face>();
                sectedSet = new HashSet<int>();
                horizon = new List<HorizonEdge>();
                openSet = new List<PointFace>();
            }
            else
            {
                faces.Clear();
                sectedSet.Clear();
                horizon.Clear();
                openSet.Clear();

                if (openSet.Capacity < points.Count)
                {
                    openSet.Capacity = points.Count;
                }
            }

            if (!splitVerts)
            {
                if (hullVertices == null)
                {
                    hullVertices = new Dictionary<int, int>();
                }
                else
                {
                    hullVertices.Clear();
                }
            }
        }
        
        void GenerateInitialHull(List<Vector3> points)
        {
            int b0, b1, b2, b3;
            FindInitialHullIndices(points, out b0, out b1, out b2, out b3);

            var v0 = points[b0];
            var v1 = points[b1];
            var v2 = points[b2];
            var v3 = points[b3];

            var above = Dot(v3 - v1, Cross(v1 - v0, v2 - v0)) > 0.0f;
            
            faceCount = 0;
            if (above) {
                faces[faceCount++] = new Face(faceCount,b0, b2, b1, 3, 1, 2, Normal(points[b0], points[b2], points[b1]));
                faces[faceCount++] = new Face(faceCount,b0, b1, b3, 3, 2, 0, Normal(points[b0], points[b1], points[b3]));
                faces[faceCount++] = new Face(faceCount,b0, b3, b2, 3, 0, 1, Normal(points[b0], points[b3], points[b2]));
                faces[faceCount++] = new Face(faceCount,b1, b2, b3, 2, 1, 0, Normal(points[b1], points[b2], points[b3]));
            } else {
                faces[faceCount++] = new Face(faceCount,b0, b1, b2, 3, 2, 1, Normal(points[b0], points[b1], points[b2]));
                faces[faceCount++] = new Face(faceCount,b0, b3, b1, 3, 0, 2, Normal(points[b0], points[b3], points[b1]));
                faces[faceCount++] = new Face(faceCount,b0, b2, b3, 3, 1, 0, Normal(points[b0], points[b2], points[b3]));
                faces[faceCount++] = new Face(faceCount,b1, b3, b2, 2, 0, 1, Normal(points[b1], points[b3], points[b2]));
            }

            for (int i = 0; i < points.Count; i++)
            {
                if (i == b0 || i == b1 || i == b2 || i == b3) continue;
                
                openSet.Add(new PointFace(i, -2, 0.0f));
            }
            openSet.Add(new PointFace(b0, INSIDE, float.NaN));
            openSet.Add(new PointFace(b1, INSIDE, float.NaN));
            openSet.Add(new PointFace(b2, INSIDE, float.NaN));
            openSet.Add(new PointFace(b3, INSIDE, float.NaN));
            
            lastOpenSet = openSet.Count - 5;

            for (int i = 0; i <= lastOpenSet; i++)
            {
                var assigned = false;
                var fp = openSet[i];

                for (var j = 0; j < 4; j++)
                {
                    var face = faces[j];
                    var dist = PointFaceDistance(points[fp.point], points[face.vertex0], face);

                    if (dist > 0)
                    {
                        fp.face = j;
                        fp.distance = dist;
                        openSet[i] = fp;

                        assigned = true;
                        break;
                    }
                }

                if (!assigned)
                {
                    fp.face = INSIDE;
                    fp.distance = float.NaN;

                    openSet[i] = openSet[lastOpenSet];
                    openSet[lastOpenSet] = fp;

                    lastOpenSet--;
                    i--;
                }
            }
        }
        
        void FindInitialHullIndices(List<Vector3> points, out int b0, out int b1, out int b2, out int b3)
        {
            var count = points.Count;

            for (var i = 0; i < count - 3; i++)
            {
                for (var j = i + 1; j < count - 2; j++)
                {
                    var p0 = points[i];
                    var p1 = points[j];
                    
                    if(areCoincident(p0,p1)) continue;
                    for (var k = j + 1; k < count - 1; k++)
                    {
                        var p2 = points[k];

                        if (areColinear(p0, p1, p2)) continue;
                        
                        for(var l = k + 1; l < count; l++)
                        {
                            var p3 = points[l];

                            if (areCoplanar(p0, p1, p2, p3)) continue;
                            b0 = i;
                            b1 = j;
                            b2 = k;
                            b3 = l;
                            return;
                        }
                    }
                }
            }
            throw new System.ArgumentException("Can't generate hull, points are coplanar");
        }
        
        void GrowHull(List<Vector3> points)
        {
            var farthestPoint = 0;
            var dist = openSet[0].distance;

            for (int i = 1; i <= lastOpenSet; i++)
            {
                if (openSet[i].distance > dist)
                {
                    farthestPoint = i;
                    dist = openSet[i].distance;
                }
            }
            
            FindHorizon(points, points[openSet[farthestPoint].point], openSet[farthestPoint].face, faces[openSet[farthestPoint].face]);
            ReassignPoints(points);
        }
        
        void FindHorizon(List<Vector3> points, Vector3 point, int fi, Face face)
        {
            sectedSet.Clear();
            horizon.Clear();

            sectedSet.Add(fi);

            {
                var oppositeFace = faces[face.face0];

                var dist = PointFaceDistance(point, points[oppositeFace.vertex0], oppositeFace);

                if (dist <= 0.0f)
                {
                    horizon.Add(new HorizonEdge
                    {
                        face = face.face0,
                        edge0 = face.vertex1,
                        edge1 = face.face2
                    });
                }
                else
                {
                    SearchHorizon(points, point, fi, face.face0, oppositeFace);
                }
            }

            if (!sectedSet.Contains(face.face1))
            {
                var oppositeFace = faces[face.face1];

                var dist = PointFaceDistance(point, points[oppositeFace.vertex0], oppositeFace);

                if (dist <= 0.0f)
                {
                    horizon.Add(new HorizonEdge
                    {
                        face = face.face1,
                        edge0 = face.vertex2,
                        edge1 = face.vertex0
                    });
                }
                else
                {
                    SearchHorizon(points, point, fi, face.face1, oppositeFace);
                }
            }

            if (!sectedSet.Contains(face.face2))
            {
                var oppositeFace = faces[face.face2];

                var dist = PointFaceDistance(point, points[oppositeFace.vertex0], oppositeFace);

                if (dist <= 0.0f)
                {
                    horizon.Add(new HorizonEdge
                    {
                        face = face.vertex2,
                        edge0 =  face.vertex0,
                        edge1 = face.vertex1
                    });
                }
                else
                {
                    SearchHorizon(points, point, fi, face.face2, oppositeFace);
                }
            }
        }

        void SearchHorizon(List<Vector3> points, Vector3 point, int prevFaceId, int faceCoun, Face face)
        {
            sectedSet.Add(faceCoun);

            int nextFaceIndex0;
            int nextFaceIndex1;
            int e0;
            int e1;
            int e2;

            if (prevFaceId == face.face0)
            {
                nextFaceIndex0 = face.face1;
                nextFaceIndex1 = face.face2;

                e0 = face.vertex2;
                e1 = face.vertex1;
                e2 = face.vertex0;
            }
            else
            {
                nextFaceIndex0 = face.face0;
                nextFaceIndex1 = face.face1;

                e0 = face.vertex1;
                e1 = face.vertex2;
                e2 = face.vertex0;
            }

            if (!sectedSet.Contains(nextFaceIndex0))
            {
                var oppositeFace = faces[nextFaceIndex0];

                var dist = PointFaceDistance(point, points[oppositeFace.vertex0], oppositeFace);

                if (dist <= 0.0f)
                {
                    horizon.Add(new HorizonEdge
                    {
                        face = nextFaceIndex0,
                        edge0 = e0,
                        edge1 = e1
                    });
                }
                else
                {
                    SearchHorizon(points, point, faceCoun, nextFaceIndex0,oppositeFace);
                }
            }

            if (!sectedSet.Contains(nextFaceIndex1))
            {
                var oppositeFace = faces[nextFaceIndex1];
                var dist = PointFaceDistance(point, points[oppositeFace.vertex0], oppositeFace);

                if (dist <= 0.0f)
                {
                    horizon.Add(new HorizonEdge
                    {
                        face = nextFaceIndex1,
                        edge0 = e1,
                        edge1 = e2
                    });
                }
                else
                {
                    SearchHorizon(points, point, faceCoun, nextFaceIndex1, oppositeFace);
                }
            }
        }
        
        void ConstCone(List<Vector3> points, int farthestPoint)
        {
            foreach (var fi in sectedSet)
            {
                faces.Remove(fi);
            }

            var firstNewFace = faceCount;

            for (int i = 0; i < horizon.Count; i++)
            {
                var v0 = farthestPoint;
                var v1 = horizon[i].edge0;
                var v2 = horizon[i].edge1;

                var o0 = horizon[1].face;
                var o1 = (i == horizon.Count - 1) ? firstNewFace : firstNewFace + i + 1;
                var o2 = (i == 0) ? (firstNewFace + horizon.Count - 1) : firstNewFace + i - 1;

                var fi = faceCount++;
                
                faces[fi] = new Face(faceCount,v0,v1,v2,o0,o1,o2, Normal(points[v0], points[v1],points[v2]));

                var horizonFace = faces[horizon[i].face];

                if (horizonFace.vertex0 == v1)
                {
                    horizonFace.face1 = fi;
                }else if (horizonFace.vertex1 == v2)
                {
                    horizonFace.face2 = fi;
                }
                else
                {
                    horizonFace.face0 = fi;
                }

                faces[horizon[i].face] = horizonFace;
            }
        }
        
        void ReassignPoints(List<Vector3> points)
        {
            for (int i = 0; i <= lastOpenSet; i++)
            {
                var fp = openSet[i];
                if (sectedSet.Contains(fp.face))
                {
                    var assigned = false;
                    var point = points[fp.point];

                    foreach (var kvp in faces)
                    {
                        var fi = kvp.Key;
                        var face = kvp.Value;

                        var dist = PointFaceDistance(point, points[face.vertex0], face);
                        if (dist > Constants.Epsilon)
                        {
                            fp.face = fi;
                            fp.distance = dist;

                            openSet[i] = fp;
                            break;
                        }
                    }

                    if (!assigned)
                    {
                        fp.face = INSIDE;
                        fp.distance = float.NaN;

                        openSet[i] = openSet[lastOpenSet];
                        openSet[lastOpenSet] = fp;

                        i--;
                        lastOpenSet--;
                    }
                } 
            }
        }
        
        void ExportMesh(List<Vector3> points, bool splitVerts, ref List<Vector3> verts, ref List<int> tris,
            ref List<Vector3> normals)
        {
            if (verts == null)
                verts = new List<Vector3>();
            else
                verts.Clear();
            
            if (tris == null)
                tris = new List<int>();
            else
                tris.Clear();
            if (normals == null)
                normals = new List<Vector3>();
            else
                normals.Clear();
            foreach (var face in faces.Values)
            {
                int vi0, vi1, vi2;

                if (splitVerts)
                {
                    vi0 = verts.Count; verts.Add(points[face.vertex0]);
                    vi1 = verts.Count; verts.Add(points[face.vertex1]);
                    vi2 = verts.Count; verts.Add(points[face.vertex2]);
                    
                    normals.Add(face.normal);
                    normals.Add(face.normal);
                    normals.Add(face.normal);
                }
                else
                {
                    if (!hullVertices.TryGetValue(face.vertex0, out vi0))
                    {
                        vi0 = verts.Count;
                        hullVertices[face.vertex0] = vi0;
                        verts.Add(points[face.vertex0]);
                    }
                    if (!hullVertices.TryGetValue(face.vertex1, out vi1))
                    {
                        vi1 = verts.Count;
                        hullVertices[face.vertex1] = vi1;
                        verts.Add(points[face.vertex1]);
                    }
                    if (!hullVertices.TryGetValue(face.vertex2, out vi2))
                    {
                        vi2 = verts.Count;
                        hullVertices[face.vertex2] = vi2;
                        verts.Add(points[face.vertex2]);
                    }
                }
                tris.Add(vi0);
                tris.Add(vi1);
                tris.Add(vi2);
            }
        }
        
        static float Dot(Vector3 a, Vector3 b) {
            return a.x*b.x + a.y*b.y + a.z*b.z;
        }

        static Vector3 Cross(Vector3 a, Vector3 b) {
            return new Vector3(
                a.y*b.z - a.z*b.y,
                a.z*b.x - a.x*b.z,
                a.x*b.y - a.y*b.x);
        }
        
        float PointFaceDistance(Vector3 point, Vector3 pointOnFace, Face face) {
            return Dot(face.normal, point - pointOnFace);
        }
        
        Vector3 Normal(Vector3 a, Vector3 b, Vector3 c)
        {
            return Cross(a-b, a - c).normalized;
        }
        
        bool areCoincident(Vector3 a, Vector3 b)
        {
            return (a - b).magnitude <= Constants.Epsilon;
        }

        bool areColinear(Vector3 a, Vector3 b, Vector3 c)
        {
            return Cross((c - a), (c - b)).magnitude <= Constants.Epsilon;
        }

        bool areCoplanar(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
        {
            var n1 = Cross(c - a, c - b);
            var n2 = Cross(d - a, d - b);

            return areColinear(Vector3.zero, n1, n2);
        }
        
    }
}