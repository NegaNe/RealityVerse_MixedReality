using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MIConvexHull;


public class MRUKVoronoiCells : MonoBehaviour
{
[SerializeField] private Material meshMaterial;


public GameObject CreateMeshCells(DefaultTriangulationCell<DefaultVertex> cell){
    GameObject cellObject = new GameObject("VoronoiCell");
    cellObject.transform.SetParent(transform, false);

    MeshFilter meshFilter = cellObject.AddComponent<MeshFilter>();
    MeshRenderer meshRenderer = cellObject.AddComponent<MeshRenderer>();
    MeshCollider meshCollider = cellObject.AddComponent<MeshCollider>();
    Mesh mesh = new Mesh();

    List<Vector3> vertices = GetVertices(cell);
    int[] triangles = CellTriangulate(vertices);

    mesh.vertices = vertices.ToArray();
    mesh.triangles = triangles;
    mesh.RecalculateNormals();
    meshFilter.mesh = mesh;
    meshRenderer.material = meshMaterial;
    meshCollider.convex=true;
    meshCollider.sharedMesh = mesh;



    return cellObject;

}

private static List<Vector3> GetVertices (DefaultTriangulationCell<DefaultVertex> cell)
{
    List<Vector3> vertices = new List<Vector3>();
    foreach(var vertex in cell.Vertices)
    {
        vertices.Add(new Vector3( (float)vertex.Position[0], 0, (float)vertex.Position[1]));
    }
    return vertices;
}

private static int[] CellTriangulate(List<Vector3> vertices)
{
    List<int> triangles = new List<int>();
    if(vertices.Count < 3) return triangles.ToArray();

    for (int i = 1 ; i< vertices.Count -1 ; i++)  //loop 'around' vertices biar jd segi-3 :3
    {
        triangles.Add(0);
        triangles.Add(i);
        triangles.Add(i + 1);
        
    }
    return triangles.ToArray();
}


}
