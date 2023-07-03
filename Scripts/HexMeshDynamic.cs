using UnityEngine;
using System.Collections.Generic;

namespace DynamicMesh {

    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class HexMeshDynamic : MonoBehaviour {

        [SerializeField] private Material _material;
        [SerializeField] private float _innerSize;
        [SerializeField] private float _outerSize;
        [SerializeField] private float _height;
        [SerializeField] private bool _isFlatTopped;
        [SerializeField] private MeshCollider _meshCollider;

        private Mesh _mesh;
        private MeshFilter _meshFilter;
        private MeshRenderer _meshRenderer;

        private List<Face> _faceList;

        public void Awake() {
            GenerateMesh();
        }

        public void GenerateMesh() {
            _meshFilter = GetComponent<MeshFilter>();
            _meshRenderer = GetComponent<MeshRenderer>();

            _mesh = new Mesh();

            _meshRenderer.material = new Material(_material);

            DrawFaces();
            CombineFaces();

            _meshFilter.mesh = _mesh;

            if (_meshCollider != null) {
                _meshCollider.sharedMesh = _mesh;
            }
        }

        private void DrawFaces() {
            _faceList = new List<Face>();

            // Top faces
            for (int point = 0; point < 6; point++) {
                _faceList.Add(CreateFace(_innerSize, _outerSize, _height / 2f, _height / 2f, point));
            }

            if (_height == 0) return;

            // Bottom faces
            for (int point = 0; point < 6; point++) {
                _faceList.Add(CreateFace(_innerSize, _outerSize, -_height / 2f, -_height / 2f, point, true));
            }

            // Outer faces
            for (int point = 0; point < 6; point++) {
                _faceList.Add(CreateFace(_outerSize, _outerSize, _height / 2f, -_height / 2f, point, true));
            }

            // Inner faces
            for (int point = 0; point < 6; point++) {
                _faceList.Add(CreateFace(_innerSize, _innerSize, _height / 2f, -_height / 2f, point));
            }
        }

        private void CombineFaces() {
            var vertices = new List<Vector3>();
            var triangles = new List<int>();
            var uvs = new List<Vector2>();

            for (int i = 0; i < _faceList.Count; i++) {
                vertices.AddRange(_faceList[i].Vertices);
                uvs.AddRange(_faceList[i].Uvs);

                int offset = i * 4;
                foreach (int triangle in _faceList[i].Triangles) {
                    triangles.Add(triangle + offset);
                }
            }

            _mesh.vertices = vertices.ToArray();
            _mesh.triangles = triangles.ToArray();
            _mesh.uv = uvs.ToArray();
            _mesh.RecalculateNormals();
        }

        private Face CreateFace(float innerRad, float outerRad, float heightA, float heightB, int point, bool reverse = false) {
            Vector3 pointA = GetPoint(innerRad, heightB, point);
            Vector3 pointB = GetPoint(innerRad, heightB, point < 5 ? point + 1 : 0);
            Vector3 pointC = GetPoint(outerRad, heightA, point < 5 ? point + 1 : 0);
            Vector3 pointD = GetPoint(outerRad, heightA, point);

            var vertices = new List<Vector3>() { pointA, pointB, pointC, pointD };
            var triangles = new List<int>() { 0, 1, 2, 2, 3, 0 };
            var uvs = new List<Vector2>() { new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1) };
            if (reverse == true) {
                vertices.Reverse();
            }

            return new Face(vertices, triangles, uvs);
        }

        private Vector3 GetPoint(float size, float height, int index) {
            float angleDegris = _isFlatTopped ? 60 * index : 60 * index - 30;
            float angleRadians = Mathf.PI / 180f * angleDegris;
            return new Vector3((size * Mathf.Cos(angleRadians)), height, size * Mathf.Sin(angleRadians));
        }

        public bool IsNull() {
            return _mesh == null;
        }

    }
    
}
