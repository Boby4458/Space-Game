using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetCreator : MonoBehaviour {

    public ChunkManager chunk;

    public Transform Player;
    public static int maxSubdivisions = 6;
    public static int minSubdivisions = 2;

    static float g = 1.61803399f;

    public static float sizeMultiplier = 5;

    Vector3[] vertexPositions = {
         new Vector3(-1, g, 0),
         new Vector3(1, g, 0),
         new Vector3(1, -g, 0),
         new Vector3(-1, -g, 0),
         new Vector3(0, 1, g),
         new Vector3(0, -1, g),
         new Vector3(0, -1, -g),
         new Vector3(0, 1, -g),
         new Vector3(-g, 0, -1),
         new Vector3(-g, 0, 1),
         new Vector3(g, 0, 1),
         new Vector3(g, 0, -1)};

     int[] TrianglePointIndecies = {
        0,4,1,
        0,1,7,
        0,7,8,
        0,8,9,
        0,9,4,
        4,5,10,
        1,4,10,
        1,10,11,
        7,1,11,
        7,11,6,
        8,7,6,
        8,3,9,
        9,3,5,
        4,9,5,
        2,6,11,
        2,3,6,
        2,5,3,
        2,10,5,
        2,11,10,
        8,6,3};

    Vector3 startPos;

	void Start () {
        startPos = transform.position;
        transform.position = Vector3.zero;
        for (int i = 0; i < 20; i ++) {

            int triIndex = i*3;

            ChunkManager newChunk;
            newChunk = Instantiate(chunk);
            newChunk.divisionAgent = Player;

            newChunk.startPointA = vertexPositions[TrianglePointIndecies[triIndex + 0]] * 100 * sizeMultiplier;
            newChunk.startPointB = vertexPositions[TrianglePointIndecies[triIndex + 1]] * 100 * sizeMultiplier;
            newChunk.startPointC = vertexPositions[TrianglePointIndecies[triIndex + 2]] * 100 * sizeMultiplier;

            newChunk.transform.parent = transform;

        }
        transform.position = startPos;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 100 * sizeMultiplier); 
    }
}
