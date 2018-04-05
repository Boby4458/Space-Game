using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SkyboxGenerator : MonoBehaviour {

    static int res = 32;

    public Material material;
    Texture2D[] textures;

    public List<Vector3> skyObjects = new List<Vector3>();
    public Vector3 currentPosition;

    public GameObject skyObjectRendererObject;

     Camera cam;
    public Camera camToInstantiate;

    public Vector3[] camRotations;

    void Start() {

        cam = Instantiate(camToInstantiate, currentPosition,Quaternion.identity);
        cam.transform.position = currentPosition;
        textures = new Texture2D[6];

        foreach (Vector3 pos in skyObjects) {
            GameObject newObject;
            newObject = Instantiate(skyObjectRendererObject, pos,Random.rotation);
            newObject.transform.LookAt(cam.transform.position);
            newObject.GetComponentInChildren<Renderer>().material.color = new Color(Random.Range(0,255), Random.Range(0, 255), Random.Range(0, 255));
        }


        for (int i = 0; i < 6; i++) {

            cam.transform.rotation = Quaternion.Euler(camRotations[i]);
            cam.Render();

            textures[i] = toTexture2D(cam.targetTexture);
            textures[i].wrapMode = TextureWrapMode.Clamp;
        }


        material.EnableKeyword("_FrontTex");
            material.SetTexture("_FrontTex", textures[0]);

            material.EnableKeyword("_BackTex");
            material.SetTexture("_BackTex", textures[1]);

            material.EnableKeyword("_LeftTex");
            material.SetTexture("_LeftTex", textures[2]);

            material.EnableKeyword("_RightTex");
            material.SetTexture("_RightTex", textures[3]);

            material.EnableKeyword("_UpTex");
            material.SetTexture("_UpTex", textures[4]);

            material.EnableKeyword("_DownTex");
            material.SetTexture("_DownTex", textures[5]);
        
    }

    Texture2D toTexture2D(RenderTexture rTex)
    {
        Texture2D tex = new Texture2D(512, 512, TextureFormat.RGB24, false);
        RenderTexture.active = rTex;
        tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
        tex.Apply();
        return tex;
    }
}
