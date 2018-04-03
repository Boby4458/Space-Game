using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkManager : MonoBehaviour {

    Tri masterTri;

    public MeshFilter mf;
    Mesh mesh;

    List<Vector3> newVerticies = new List<Vector3>();
    List<int> newTriangles = new List<int>();
    List<Vector2> newUVs = new List<Vector2>();

    public List<Tri> smallestTris = new List<Tri>();
    public List<Tri> trisToRemove;

    public Transform divisionAgent;

    public int minSubdivisions = PlanetCreator.maxSubdivisions;
    public int maxSubdivisions = PlanetCreator.minSubdivisions;

    public int[] divisionDistances;

    public Vector3 startPointA;
    public Vector3 startPointB;
    public Vector3 startPointC;

    static float g = 1.61803399f;

    private void Start()
    {
        mesh = mf.mesh;

        masterTri = new Tri(
        Tri.offsetAtPos(startPointA),
        Tri.offsetAtPos(startPointB),
        Tri.offsetAtPos(startPointC),
        0, null, this);

        smallestTris.Add(masterTri);
       
        UpdateMesh();
    }

    private void Update()
    {
        trisToRemove = new List<Tri>();
        foreach (Tri t in smallestTris.ToArray())
        {
            float dist =( 240 - (1 + (t.curDiv * 40))) * PlanetCreator.sizeMultiplier;
                if (Vector3.Distance(divisionAgent.position, t.worldPosition()) <= dist || t.curDiv < minSubdivisions)
                {
                    t.Subdivide();

                    if (t.curDiv < t.maxDiv)
                    {
                        foreach (Tri tc in t.childTris.ToArray())
                        {
                            if(!smallestTris.Contains(tc)) { 
                            smallestTris.Add(tc);
                        }
                        }

                        trisToRemove.Add(t);
                    }
                }
                else {
                    if (Vector3.Distance(divisionAgent.position, t.worldPosition()) >= dist + (50 * PlanetCreator.sizeMultiplier) && t.curDiv > minSubdivisions) {
                        if (t.parentTri != null) {
                            t.parentTri.unSubdivide();
                            if (!smallestTris.Contains(t.parentTri))
                            {
                                smallestTris.Add(t.parentTri);
                            }
                        }
                    }
                }
            
            
        }
        foreach (Tri t in trisToRemove.ToArray()) {
            smallestTris.Remove(t);
        }
        trisToRemove.Clear();
            UpdateMesh();
    }

    void UpdateMesh() {

        mesh.Clear();

         newVerticies = new List<Vector3>();
         newTriangles = new List<int>();

        foreach (Tri t in smallestTris.ToArray()) {

            AddTri(t.pos1,t.pos2,t.pos3);

        }

        mesh.vertices = newVerticies.ToArray();
        mesh.triangles = newTriangles.ToArray();
        mesh.RecalculateNormals();

    }

    void AddTri(Vector3 a, Vector3 b, Vector3 c){

        int curVertCount = newVerticies.Count;

        newVerticies.Add(Tri.offsetAtPos(a));
        newVerticies.Add(Tri.offsetAtPos(b));
        newVerticies.Add(Tri.offsetAtPos(c));

        newTriangles.Add(curVertCount + 0);
        newTriangles.Add(curVertCount + 1);
        newTriangles.Add(curVertCount + 2);
    }
   

}

[System.Serializable]
public class Tri{

    public ChunkManager myChunk;

    public Vector3 pos1;
    public Vector3 pos2;
    public Vector3 pos3;

    public int curDiv;
    public int maxDiv;

    public List<Tri> childTris;
    public Tri parentTri;

    public static Vector3 offsetAtPos(Vector3 pos)
    {

        float noiseScale = 6 * PlanetCreator.sizeMultiplier;
        float amplitude = 0.1f;

        float offset = ThreePerlinNoise.Perlin3D(pos.x / noiseScale + 1000, pos.y / noiseScale + 1000, pos.z / noiseScale + 1000) * amplitude - (amplitude/2) + 1;

        Vector3 edgePos = pos.normalized;

        return edgePos * offset * 100 * PlanetCreator.sizeMultiplier;
        
    }

    public void Subdivide() {

        if (curDiv < maxDiv) {

            Vector3 a = pos1.normalized * 100 * PlanetCreator.sizeMultiplier;
            Vector3 b = pos2.normalized * 100 * PlanetCreator.sizeMultiplier;
            Vector3 c = pos3.normalized * 100 * PlanetCreator.sizeMultiplier;

            Vector3 edge12 = FindMidPos(a,b).normalized * 100 * PlanetCreator.sizeMultiplier;
            Vector3 edge23 = FindMidPos(b,c).normalized * 100 * PlanetCreator.sizeMultiplier;
            Vector3 edge13 = FindMidPos(c,a).normalized * 100 * PlanetCreator.sizeMultiplier;

            childTris = new List<Tri>();
        
            childTris.Add(new Tri(a,edge12,edge13, curDiv + 1, this));
            childTris.Add(new Tri(edge13, edge23, c, curDiv + 1, this));
            childTris.Add(new Tri(edge23, edge13, edge12, curDiv + 1, this));
            childTris.Add(new Tri(edge12, b, edge23, curDiv + 1, this));
        }
    }
    public void unSubdivide() {
        if (childTris != null)
        {
            List<Tri> childTrisToRemove = new List<Tri>();
            foreach (Tri t in childTris)
            {
                t.unSubdivide();
                childTrisToRemove.Add(t);
                myChunk.trisToRemove.Add(t);
            }
            foreach (Tri t in childTrisToRemove)
            {
                childTris.Remove(t);
            }
        }
    }

    // constructor 1
    public Tri(Vector3 posA, Vector3 posB, Vector3 posC, int currentDiv, Tri parent,ChunkManager chunk) {

        curDiv = currentDiv;

        pos1 = posA;
        pos2 = posB;
        pos3 = posC;

        parentTri = parent;
        myChunk = chunk;
        maxDiv = PlanetCreator.maxSubdivisions;
    }
    // constructor 2
    public Tri(Vector3 posA, Vector3 posB, Vector3 posC, int currentDiv, Tri parent)
    {

        curDiv = currentDiv;

        pos1 = posA;
        pos2 = posB;
        pos3 = posC;

        parentTri = parent;
        myChunk = parentTri.myChunk;
        maxDiv = PlanetCreator.maxSubdivisions;

    }

    // checks if this triangle has any children
    // returns true if no children are found
    // else returns false
    public bool IsHighestSubdiv()
    {
        if(childTris == null || childTris.Count == 0 )
        {
            return true;
        }
        return false;
    }

    // find center of this tri in global space
    public Vector3 worldPosition() {

        Vector3 pos = FindMidPos3(
            myChunk.transform.TransformPoint(pos1.normalized * 100 * PlanetCreator.sizeMultiplier),
            myChunk.transform.TransformPoint(pos2.normalized * 100 * PlanetCreator.sizeMultiplier),
            myChunk.transform.TransformPoint(pos3.normalized * 100 * PlanetCreator.sizeMultiplier));


        return pos;
    }
    // middle position between 2 vectors
    Vector3 FindMidPos(Vector3 a, Vector3 b) {

        return (a + b) / 2;
    }
  
    // middle position between 3 vectors
    Vector3 FindMidPos3(Vector3 a, Vector3 b, Vector3 c)
    {

        return new Vector3(
            (a.x + b.x + c.x) / 3,
            (a.y + b.y + c.y) / 3,
            (a.z + b.z + c.z) / 3);
    }


}
// 3d  perlin noise generator
public static class ThreePerlinNoise
{

    public static float Perlin3D(float x, float y, float z)
    {

        float AB = Mathf.PerlinNoise(x, y);
        float BC = Mathf.PerlinNoise(y, z);
        float AC = Mathf.PerlinNoise(x, z);

        float BA = Mathf.PerlinNoise(y, x);
        float CB = Mathf.PerlinNoise(z, y);
        float CA = Mathf.PerlinNoise(z, x);

        float ABC = AB + BC + AC + BA + CB + CA;
        return ABC / 6;

    }
}
