using UnityEngine;

public class MeshExport : MonoBehaviour
{
    private CompositeCollider2D _collider2D;
    private MeshFilter _filter;

    public Mesh Mesh;
    
    // Start is called before the first frame update
    private void Awake()
    {
        _collider2D = GetComponent<CompositeCollider2D>();
        _filter = GetComponent<MeshFilter>();
    }

    void Start()
    {
        Mesh = _collider2D.CreateMesh(true, true);
        _filter.mesh = Mesh;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
