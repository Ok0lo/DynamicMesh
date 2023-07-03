using UnityEngine;
using System.Collections.Generic;

namespace DynamicMesh {

    public struct Face {

        public Face(List<Vector3> vertices, List<int> triangles, List<Vector2> uvs) {
            Vertices = vertices;
            Triangles = triangles;
            Uvs = uvs;
        }

        public List<Vector3> Vertices { get; private set; }
        public List<int> Triangles { get; private set; }
        public List<Vector2> Uvs { get; private set; }

    }

}
