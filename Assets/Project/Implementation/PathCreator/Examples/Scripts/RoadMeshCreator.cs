﻿using System.Collections.Generic;
using DrivingSimulation;
using PathCreation.Utility;
using UnityEngine;


namespace PathCreation.Examples {
    public class RoadMeshCreator : PathSceneTool {
        [Header ("Road settings")]
        public float roadWidth = .4f;
        [Range (0, .5f)]
        public float thickness = .15f;
        public bool flattenSurface;

        [Header ("Material settings")]
        public Material roadMaterial;
        public Material undersideMaterial;
        public bool autoTiling = true;
        public float textureTiling = 1;

        [Header("Additional Settings")]
        public bool isLeftAreaColliderActive = false;
        public bool isRightAreaColliderActive = false;

        GameObject meshHolder;

        MeshFilter meshFilter;
        MeshRenderer meshRenderer;
        Mesh mesh;

        public GameObject MeshHolder
        {
            get { return meshHolder; }
        }

        protected override void PathUpdated () {
            if (pathCreator != null) {
                Initialize();
                AssignMeshComponents ();
                AssignMaterials ();
                CreateRoadMesh ();
                CreateRoadLeftAreaCollider();
                CreateRoadRightAreaCollider();
            }
        }

        private void Awake()
        {
            if (autoTiling)
                AssignAutoTilingMaterial();
        }

        void Initialize()
        {
            if (meshHolder == null) return;

            foreach (Transform c in meshHolder.transform)
            {
                DestroyImmediate(c.gameObject);
            }
        }

        void CreateRoadMesh () {
            Vector3[] verts = new Vector3[path.NumPoints * 8];
            Vector2[] uvs = new Vector2[verts.Length];
            Vector3[] normals = new Vector3[verts.Length];

            int numTris = 2 * (path.NumPoints - 1) + ((path.isClosedLoop) ? 2 : 0);
            int[] roadTriangles = new int[numTris * 3];
            int[] underRoadTriangles = new int[numTris * 3];
            int[] sideOfRoadTriangles = new int[numTris * 2 * 3];

            int vertIndex = 0;
            int triIndex = 0;

            // Vertices for the top of the road are layed out:
            // 0  1
            // 8  9
            // and so on... So the triangle map 0,8,1 for example, defines a triangle from top left to bottom left to bottom right.
            int[] triangleMap = { 0, 8, 1, 1, 8, 9 };
            int[] sidesTriangleMap = { 4, 6, 14, 12, 4, 14, 5, 15, 7, 13, 15, 5 };

            bool usePathNormals = !(path.space == PathSpace.xyz && flattenSurface);

            for (int i = 0; i < path.NumPoints; i++) {
                Vector3 localUp = (usePathNormals) ? Vector3.Cross (path.GetTangent (i), path.GetNormal (i)) : path.up;
                Vector3 localRight = (usePathNormals) ? path.GetNormal (i) : Vector3.Cross (localUp, path.GetTangent (i));

                // Find position to left and right of current path vertex
                Vector3 vertSideA = path.GetPoint (i) - localRight * Mathf.Abs (roadWidth);
                Vector3 vertSideB = path.GetPoint (i) + localRight * Mathf.Abs (roadWidth);

                // Add top of road vertices
                verts[vertIndex + 0] = vertSideA;
                verts[vertIndex + 1] = vertSideB;
                // Add bottom of road vertices
                verts[vertIndex + 2] = vertSideA - localUp * thickness;
                verts[vertIndex + 3] = vertSideB - localUp * thickness;

                // Duplicate vertices to get flat shading for sides of road
                verts[vertIndex + 4] = verts[vertIndex + 0];
                verts[vertIndex + 5] = verts[vertIndex + 1];
                verts[vertIndex + 6] = verts[vertIndex + 2];
                verts[vertIndex + 7] = verts[vertIndex + 3];

                // Set uv on y axis to path time (0 at start of path, up to 1 at end of path)
                uvs[vertIndex + 0] = new Vector2 (0, path.times[i]);
                uvs[vertIndex + 1] = new Vector2 (1, path.times[i]);

                // Top of road normals
                normals[vertIndex + 0] = localUp;
                normals[vertIndex + 1] = localUp;
                // Bottom of road normals
                normals[vertIndex + 2] = -localUp;
                normals[vertIndex + 3] = -localUp;
                // Sides of road normals
                normals[vertIndex + 4] = -localRight;
                normals[vertIndex + 5] = localRight;
                normals[vertIndex + 6] = -localRight;
                normals[vertIndex + 7] = localRight;

                // Set triangle indices
                if (i < path.NumPoints - 1 || path.isClosedLoop) {
                    for (int j = 0; j < triangleMap.Length; j++) {
                        roadTriangles[triIndex + j] = (vertIndex + triangleMap[j]) % verts.Length;
                        // reverse triangle map for under road so that triangles wind the other way and are visible from underneath
                        underRoadTriangles[triIndex + j] = (vertIndex + triangleMap[triangleMap.Length - 1 - j] + 2) % verts.Length;
                    }
                    for (int j = 0; j < sidesTriangleMap.Length; j++) {
                        sideOfRoadTriangles[triIndex * 2 + j] = (vertIndex + sidesTriangleMap[j]) % verts.Length;
                    }

                }

                vertIndex += 8;
                triIndex += 6;
            }

            mesh.Clear ();
            mesh.vertices = verts;
            mesh.uv = uvs;
            mesh.normals = normals;
            mesh.subMeshCount = 3;
            mesh.SetTriangles (roadTriangles, 0);
            mesh.SetTriangles (underRoadTriangles, 1);
            mesh.SetTriangles (sideOfRoadTriangles, 2);
            mesh.RecalculateBounds ();

            if (!meshHolder.TryGetComponent(out MeshCollider col))
            {
                col = meshHolder.AddComponent<MeshCollider>();
            }

            col.sharedMesh = null;
            col.sharedMesh = mesh;
        }

        void CreateRoadLeftAreaCollider()
        {
            if (meshHolder == null) return;

            if (!meshHolder.TryGetComponent(out Road roadHolder))
            {
                roadHolder = meshHolder.AddComponent<Road>();
            }

            GameObject leftAreaColliderObj = roadHolder.LeftAreaColliderObj;

            if (leftAreaColliderObj != null)
                DestroyImmediate(leftAreaColliderObj);

            if (!isLeftAreaColliderActive) return;

            leftAreaColliderObj = new GameObject("Left Area Collider");
            leftAreaColliderObj.transform.parent = meshHolder.transform;

            leftAreaColliderObj.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            leftAreaColliderObj.transform.localScale = Vector3.one;

            for (int point = 0; point < path.NumPoints - 1; point++)
            {
                GameObject leftAreaColliderMeshObj = new GameObject("Mesh Collider");
                leftAreaColliderMeshObj.transform.parent = leftAreaColliderObj.transform;
                leftAreaColliderMeshObj.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
                leftAreaColliderMeshObj.transform.localScale = Vector3.one;

                float height = 1f;
                Vector3[] verts = new Vector3[16];
                Vector3[] normals = new Vector3[verts.Length];

                int numTris = 2;
                int[] roadTriangles = new int[numTris * 3];
                int[] underRoadTriangles = new int[numTris * 3];
                int[] sideOfRoadTriangles = new int[numTris * 2 * 3];

                int vertIndex = 0;
                int triIndex = 0;

                // Vertices for the top of the road are layed out:
                // 0  1
                // 8  9
                // and so on... So the triangle map 0,8,1 for example, defines a triangle from top left to bottom left to bottom right.
                int[] triangleMap = { 0, 8, 1, 1, 8, 9 };
                int[] sidesTriangleMap = { 4, 6, 14, 12, 4, 14, 5, 15, 7, 13, 15, 5 };

                bool usePathNormals = !(path.space == PathSpace.xyz && flattenSurface);

                for (int i = 0; i < 2; i++)
                {
                    Vector3 localUp = (usePathNormals) ? Vector3.Cross(path.GetTangent(point + i), path.GetNormal(point + i)) : path.up;
                    Vector3 localRight = (usePathNormals) ? path.GetNormal(point + i) : Vector3.Cross(localUp, path.GetTangent(point + i));

                    // Find position to center and left of current path vertex
                    Vector3 vertSideA = path.GetPoint(point + i);
                    Vector3 vertSideB = path.GetPoint(point + i) - localRight * Mathf.Abs(roadWidth);

                    // Add top of road vertices
                    verts[vertIndex + 0] = vertSideA;
                    verts[vertIndex + 1] = vertSideB;
                    // Add bottom of road vertices
                    verts[vertIndex + 2] = vertSideA + localUp * height;
                    verts[vertIndex + 3] = vertSideB + localUp * height;

                    // Duplicate vertices to get flat shading for sides of road
                    verts[vertIndex + 4] = verts[vertIndex + 0];
                    verts[vertIndex + 5] = verts[vertIndex + 1];
                    verts[vertIndex + 6] = verts[vertIndex + 2];
                    verts[vertIndex + 7] = verts[vertIndex + 3];

                    // Top of road normals
                    normals[vertIndex + 0] = localUp;
                    normals[vertIndex + 1] = localUp;
                    // Bottom of road normals
                    normals[vertIndex + 2] = -localUp;
                    normals[vertIndex + 3] = -localUp;
                    // Sides of road normals
                    normals[vertIndex + 4] = -localRight;
                    normals[vertIndex + 5] = localRight;
                    normals[vertIndex + 6] = -localRight;
                    normals[vertIndex + 7] = localRight;

                    // Set triangle indices
                    if (i < 1)
                    {
                        for (int j = 0; j < triangleMap.Length; j++)
                        {
                            roadTriangles[triIndex + j] = (vertIndex + triangleMap[j]) % verts.Length;
                            // reverse triangle map for under road so that triangles wind the other way and are visible from underneath
                            underRoadTriangles[triIndex + j] = (vertIndex + triangleMap[triangleMap.Length - 1 - j] + 2) % verts.Length;
                        }
                        for (int j = 0; j < sidesTriangleMap.Length; j++)
                        {
                            sideOfRoadTriangles[triIndex * 2 + j] = (vertIndex + sidesTriangleMap[j]) % verts.Length;
                        }

                    }

                    vertIndex += 8;
                    triIndex += 6;
                }

                Mesh leftAreaColliderMesh = new Mesh();

                leftAreaColliderMesh.Clear();
                leftAreaColliderMesh.vertices = verts;
                leftAreaColliderMesh.normals = normals;
                leftAreaColliderMesh.subMeshCount = 3;
                leftAreaColliderMesh.SetTriangles(roadTriangles, 0);
                leftAreaColliderMesh.SetTriangles(underRoadTriangles, 1);
                leftAreaColliderMesh.SetTriangles(sideOfRoadTriangles, 2);
                leftAreaColliderMesh.RecalculateBounds();

                if (!leftAreaColliderMeshObj.TryGetComponent(out MeshCollider col))
                {
                    col = leftAreaColliderMeshObj.AddComponent<MeshCollider>();
                }

                col.sharedMesh = leftAreaColliderMesh;
                col.convex = true;
                col.isTrigger = true;
            }

            roadHolder.LeftAreaColliderObj = leftAreaColliderObj;
        }

        void CreateRoadRightAreaCollider()
        {
            if (meshHolder == null) return;


            if (!meshHolder.TryGetComponent(out Road roadHolder))
            {
                roadHolder = meshHolder.AddComponent<Road>();
            }

            GameObject rightAreaColliderObj = roadHolder.RightAreaColliderObj;

            if (rightAreaColliderObj != null)
                DestroyImmediate(rightAreaColliderObj);

            if (!isRightAreaColliderActive) return;

            rightAreaColliderObj = new GameObject("Right Area Collider");
            rightAreaColliderObj.transform.parent = meshHolder.transform;

            rightAreaColliderObj.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            rightAreaColliderObj.transform.localScale = Vector3.one;

            for (int point = 0; point < path.NumPoints - 1; point++)
            {
                GameObject rightAreaColliderMeshObj = new GameObject("Mesh Collider");
                rightAreaColliderMeshObj.transform.parent = rightAreaColliderObj.transform;
                rightAreaColliderMeshObj.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
                rightAreaColliderMeshObj.transform.localScale = Vector3.one;

                float height = 1f;
                Vector3[] verts = new Vector3[16];
                Vector3[] normals = new Vector3[verts.Length];

                int numTris = 2;
                int[] roadTriangles = new int[numTris * 3];
                int[] underRoadTriangles = new int[numTris * 3];
                int[] sideOfRoadTriangles = new int[numTris * 2 * 3];

                int vertIndex = 0;
                int triIndex = 0;

                // Vertices for the top of the road are layed out:
                // 0  1
                // 8  9
                // and so on... So the triangle map 0,8,1 for example, defines a triangle from top left to bottom left to bottom right.
                int[] triangleMap = { 0, 8, 1, 1, 8, 9 };
                int[] sidesTriangleMap = { 4, 6, 14, 12, 4, 14, 5, 15, 7, 13, 15, 5 };

                bool usePathNormals = !(path.space == PathSpace.xyz && flattenSurface);

                for (int i = 0; i < 2; i++)
                {
                    Vector3 localUp = (usePathNormals) ? Vector3.Cross(path.GetTangent(point + i), path.GetNormal(point + i)) : path.up;
                    Vector3 localRight = (usePathNormals) ? path.GetNormal(point + i) : Vector3.Cross(localUp, path.GetTangent(point + i));

                    // Find position to center and left of current path vertex
                    Vector3 vertSideA = path.GetPoint(point + i);
                    Vector3 vertSideB = path.GetPoint(point + i) + localRight * Mathf.Abs(roadWidth);

                    // Add top of road vertices
                    verts[vertIndex + 0] = vertSideA;
                    verts[vertIndex + 1] = vertSideB;
                    // Add bottom of road vertices
                    verts[vertIndex + 2] = vertSideA + localUp * height;
                    verts[vertIndex + 3] = vertSideB + localUp * height;

                    // Duplicate vertices to get flat shading for sides of road
                    verts[vertIndex + 4] = verts[vertIndex + 0];
                    verts[vertIndex + 5] = verts[vertIndex + 1];
                    verts[vertIndex + 6] = verts[vertIndex + 2];
                    verts[vertIndex + 7] = verts[vertIndex + 3];

                    // Top of road normals
                    normals[vertIndex + 0] = localUp;
                    normals[vertIndex + 1] = localUp;
                    // Bottom of road normals
                    normals[vertIndex + 2] = -localUp;
                    normals[vertIndex + 3] = -localUp;
                    // Sides of road normals
                    normals[vertIndex + 4] = -localRight;
                    normals[vertIndex + 5] = localRight;
                    normals[vertIndex + 6] = -localRight;
                    normals[vertIndex + 7] = localRight;

                    // Set triangle indices
                    if (i < 1)
                    {
                        for (int j = 0; j < triangleMap.Length; j++)
                        {
                            roadTriangles[triIndex + j] = (vertIndex + triangleMap[j]) % verts.Length;
                            // reverse triangle map for under road so that triangles wind the other way and are visible from underneath
                            underRoadTriangles[triIndex + j] = (vertIndex + triangleMap[triangleMap.Length - 1 - j] + 2) % verts.Length;
                        }
                        for (int j = 0; j < sidesTriangleMap.Length; j++)
                        {
                            sideOfRoadTriangles[triIndex * 2 + j] = (vertIndex + sidesTriangleMap[j]) % verts.Length;
                        }

                    }

                    vertIndex += 8;
                    triIndex += 6;
                }

                Mesh rightAreaColliderMesh = new Mesh();

                rightAreaColliderMesh.Clear();
                rightAreaColliderMesh.vertices = verts;
                rightAreaColliderMesh.normals = normals;
                rightAreaColliderMesh.subMeshCount = 3;
                rightAreaColliderMesh.SetTriangles(roadTriangles, 0);
                rightAreaColliderMesh.SetTriangles(underRoadTriangles, 1);
                rightAreaColliderMesh.SetTriangles(sideOfRoadTriangles, 2);
                rightAreaColliderMesh.RecalculateBounds();

                if (!rightAreaColliderMeshObj.TryGetComponent(out MeshCollider col))
                {
                    col = rightAreaColliderMeshObj.AddComponent<MeshCollider>();
                }

                col.sharedMesh = rightAreaColliderMesh;
                col.convex = true;
                col.isTrigger = true;
            }

            roadHolder.RightAreaColliderObj = rightAreaColliderObj;
        }

        // Add MeshRenderer and MeshFilter components to this gameobject if not already attached
        void AssignMeshComponents () {

            if (meshHolder == null) {
                meshHolder = new GameObject ("Road Mesh Holder");
                meshHolder.AddComponent<Road>();
            }

            meshHolder.transform.rotation = Quaternion.identity;
            meshHolder.transform.position = Vector3.zero;
            meshHolder.transform.localScale = Vector3.one;

            // Ensure mesh renderer and filter components are assigned
            if (!meshHolder.gameObject.GetComponent<MeshFilter> ()) {
                meshHolder.gameObject.AddComponent<MeshFilter> ();
            }
            if (!meshHolder.GetComponent<MeshRenderer> ()) {
                meshHolder.gameObject.AddComponent<MeshRenderer> ();
            }
            if (!meshHolder.GetComponent<MeshCollider>())
            {
                meshHolder.gameObject.AddComponent<MeshCollider>();
            }

            meshRenderer = meshHolder.GetComponent<MeshRenderer> ();
            meshFilter = meshHolder.GetComponent<MeshFilter> ();
            if (mesh == null) {
                mesh = new Mesh ();
            }
            meshFilter.sharedMesh = mesh;

            if (transform.parent != null)
            {
                meshHolder.transform.parent = transform.parent;
            }
        }

        void AssignMaterials () {
            if (roadMaterial != null && undersideMaterial != null) {
                if (Application.isPlaying)
                {
                    meshRenderer.materials = new Material[] { roadMaterial, undersideMaterial, undersideMaterial };
                    meshRenderer.materials[0].mainTextureScale = new Vector3(1, path.length / 35f);
                    return;
                }

                meshRenderer.sharedMaterials = new Material[] { roadMaterial, undersideMaterial, undersideMaterial };
                meshRenderer.sharedMaterials[0].mainTextureScale = new Vector3(1, textureTiling);

                return;
            }

            if (MonoGlobalVariables.Instance != null)
            {
                MonoGlobalVariables globalInstance = MonoGlobalVariables.Instance;

                if (globalInstance.roadMaterial != null && globalInstance.undersideMaterial != null)
                {
                    if (Application.isPlaying)
                    {
                        meshRenderer.materials = new Material[] { globalInstance.roadMaterial, globalInstance.undersideMaterial, globalInstance.undersideMaterial };
                        meshRenderer.materials[0].mainTextureScale = new Vector3(1, path.length / 35f);

                        return;
                    }

                    meshRenderer.sharedMaterials = new Material[] { globalInstance.roadMaterial, globalInstance.undersideMaterial, globalInstance.undersideMaterial };
                    meshRenderer.sharedMaterials[0].mainTextureScale = new Vector3(1, textureTiling);

                    return;
                }
            }
        }

        void AssignAutoTilingMaterial()
        {
            if (!Application.isPlaying) return;
            if (meshHolder == null) return;

            if (!meshHolder.TryGetComponent(out meshRenderer))
            {
                meshRenderer = meshHolder.gameObject.AddComponent<MeshRenderer>();
            }

            if (roadMaterial != null && undersideMaterial != null)
            {
                meshRenderer.materials = new Material[] { roadMaterial, undersideMaterial, undersideMaterial };
                meshRenderer.materials[0].mainTextureScale = new Vector3(1, path.length / 35f);

                return;
            }

            if (MonoGlobalVariables.Instance != null)
            {
                MonoGlobalVariables globalInstance = MonoGlobalVariables.Instance;

                if (globalInstance.roadMaterial != null && globalInstance.undersideMaterial != null)
                {
                    meshRenderer.materials = new Material[] { globalInstance.roadMaterial, globalInstance.undersideMaterial, globalInstance.undersideMaterial };
                    meshRenderer.materials[0].mainTextureScale = new Vector3(1, path.length / 35f);

                    return;
                }
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (pathCreator == null) return;
            if (pathCreator.path.NumPoints < 2) return;

            for (int i = 0; i < pathCreator.path.NumPoints; i++)
            {
                Vector3 point0 = pathCreator.path.GetPoint(i);

                Gizmos.color = new Color(0, 1, 0, 0.4f);
                Gizmos.DrawLine(point0, point0 + (Vector3.up * 2f));
            }
        }
#endif
    }
}