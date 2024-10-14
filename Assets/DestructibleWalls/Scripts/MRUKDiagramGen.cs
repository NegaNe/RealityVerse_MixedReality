using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MIConvexHull;
using Meta.XR.MRUtilityKit;
using System.Threading;
using System.Linq;


    public class MRUKDiagramGen : MonoBehaviour
    {
        [SerializeField] private int gridRow = 7;
        [SerializeField] private int gridColl = 7;
        [SerializeField] private bool ApplyExclusionArea = false;
        [SerializeField] private bool includeEdgeSites = true;
        [SerializeField] private float exclusionSiteHeightFromBottom = 2.0f;
        [SerializeField] private LayerMask cellLayer;
        [SerializeField] private LayerMask ExclusionZoneLayer;


        private List<Vector3> sites = new List<Vector3>();
        private List<DefaultTriangulationCell<DefaultVertex>> voronoiCells = new List<DefaultTriangulationCell<DefaultVertex>>();
        private MRUKVoronoiCells voronoiCellsGeneration;


        private void Start()
        {
            voronoiCellsGeneration = GetComponent<MRUKVoronoiCells>();
            GenerateSitesBasedOnMeshBounds();
            GenerateVonoroiCellsDiagram();

        }

        private void GenerateSitesBasedOnMeshBounds()
        {
            Bounds bound = GetComponent<MeshRenderer>().localBounds;
            Transform meshTransform = GetComponent<Transform>();

            float cellWidth = bound.size.x / gridColl;
            float cellHeight = bound.size.z / gridRow;

            //generate sites / vert buat boundary
            for (int i = 0; i < gridRow; i++)
            {
                for (int j = 0; j < gridColl; j++)
                {
                    Vector3 sitePos = new Vector3(bound.min.x + j * cellWidth +
                    Random.Range(0f, cellWidth), bound.center.y, bound.min.z + i * cellHeight +
                    Random.Range(0f, cellHeight));


                    //subject to change
                    if (ApplyExclusionArea && sitePos.z - bound.min.z < exclusionSiteHeightFromBottom && !IsEdge(i, j, gridRow, gridColl))
                    {
                        continue;
                    }
                    Vector3 localSitePos = meshTransform.InverseTransformPoint(sitePos);
                    sites.Add(meshTransform.TransformPoint(localSitePos));
                }
            }

            if (includeEdgeSites)
            {
                AddEdges(bound);
            }

        }
        bool IsEdge(int rowIndex, int columnIndex, int totalRows, int totalColumns)
        {
            return rowIndex == 0 || columnIndex == 0 || rowIndex == totalRows - 1 || columnIndex == totalColumns - 1;
        }

        void AddEdges(Bounds bounds)
        {
            float cellWidth = bounds.size.x / gridColl;
            float cellHeight = bounds.size.z / gridRow;

            for (int j = 0; j <= gridColl; j++)
            {
                float x = bounds.min.x + j * cellWidth;
                Vector3 topEdgeSite = new Vector3(x, bounds.center.y, bounds.max.z);
                Vector3 bottomEdgeSite = new Vector3(x, bounds.center.y, bounds.min.z);

                sites.Add(topEdgeSite);
                sites.Add(bottomEdgeSite);
            }

            for (int i = 0; i <= gridRow; i++)
            {
                float z = bounds.min.z + i * cellHeight;
                Vector3 leftEdgeSite = new Vector3(bounds.min.x, bounds.center.y, z);
                Vector3 rightEdgeSite = new Vector3(bounds.max.x, bounds.center.y, z);

                sites.Add(leftEdgeSite);
                sites.Add(rightEdgeSite);
            }
        }

        private void GenerateVonoroiCellsDiagram()
        {
            var points = sites.Select(site => new DefaultVertex { Position = new double[] { site.x, site.z } }).ToList();
            var triangulation = Triangulation.CreateDelaunay(points, Constants.DefaultPlaneDistanceTolerance); // CORE: refer to voronoi library.
            voronoiCells.Clear();
            foreach (var cell in triangulation.Cells) voronoiCells.Add(cell);

            CreateMeshForCells();
        }

        void CreateMeshForCells() // apply mesh ke cells
        {
            foreach (var cell in voronoiCells)
            {
                GameObject cellObj = voronoiCellsGeneration.CreateMeshCells(cell);
                int LayerIndex = ApplyExclusionArea && CellTouchesExclusionZone(cell) ? LayerMaskToLayer(ExclusionZoneLayer) : LayerMaskToLayer(cellLayer);
                if (LayerIndex >= 0)
                {
                    cellObj.layer = LayerIndex; //set layer ke cellMeshes
                }
            }
        }

        int LayerMaskToLayer(LayerMask layerMask)
        {
            //int LayerIndex = (int)Mathf.Log(layerMask.value, 2); // IT JUST WORKS?? => OLD CODE
            return layerMask.value != 0 ? Mathf.Clamp((int)Mathf.Log(layerMask.value, 2), 0, 31) : -1; // simplified, still, it just works.
        }

        bool CellTouchesExclusionZone(DefaultTriangulationCell<DefaultVertex> cell)
        {
            Bounds bound = GetComponent<MeshRenderer>().localBounds;
            float exclusionZoneMinZ = bound.min.z + exclusionSiteHeightFromBottom;

            foreach (var vertex in cell.Vertices)
            {
                Vector3 vertexPosition = new Vector3((float)vertex.Position[0], 0, (float)vertex.Position[1]);
                if (vertexPosition.z < exclusionZoneMinZ)
                {
                    return true;
                }
            }
            return false;
        }
    }


