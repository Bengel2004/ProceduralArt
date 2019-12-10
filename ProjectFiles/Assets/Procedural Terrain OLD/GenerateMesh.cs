﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class GenerateMesh : MonoBehaviour
{
    // Start is called before the first frame update
    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;

    public int xSize = 20;
    public int zSize = 20;

    float noiseMultiplier = 0;
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        //CreateShape();
        //UpdateMesh();
    }

    void Update()
    {
        CreateShape();
        UpdateMesh();
        noiseMultiplier += .01f;
    }
    private void CreateShape()
    {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float y = Mathf.PerlinNoise(x * .3f, z * .3f) * 2f;
                Debug.Log(AudioVisualizer.pitchValue);
              //  vertices[i] = new Vector3(x, Mathf.Sin(y), z);
                vertices[i] = new Vector3(x, Mathf.Sin(y), z);
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

        



        //vertices = new Vector3[]
        //{
        //    new Vector3(0,0,0),
        //    new Vector3(0,0,1),
        //    new Vector3(1,0,0),
        //    new Vector3(1,0,1),
        //};

        //triangles = new int[]
        //{
        //    0, 1, 2,
        //    1, 3, 2,
        //};
    }

    private void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
    }

    //private void OnDrawGizmos()
    //{
    //    if (vertices == null)
    //        return;

    //    for (int i = 0; i < vertices.Length; i++)
    //    {
    //        Gizmos.DrawSphere(vertices[i], .1f);
    //    }
    //}   
    

    // Update is called once per frame
  

}
