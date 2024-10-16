using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshExport : MonoBehaviour
{
    private CompositeCollider2D Collider2D;
    private MeshFilter Filter;

    public Mesh mesh;
    
    // Start is called before the first frame update
    private void Awake()
    {
        Collider2D = GetComponent<CompositeCollider2D>();
        Filter = GetComponent<MeshFilter>();
    }

    void Start()
    {
        mesh = Collider2D.CreateMesh(true, true);
        Filter.mesh = mesh;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
