using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeshSimpler.Core.Extensions
{
    public static class AssimpExtensions
    {
        public static Assimp.Mesh ToAiMesh(this QEM.Mesh mesh)
        {
            Assimp.Mesh aimesh = new();

            List<QEM.Vector> vectors = mesh.tris.SelectMany(x => new[] { x.v1, x.v2, x.v3 }).ToList();
            aimesh.Vertices.AddRange(vectors.Select(x => new Assimp.Vector3D((float)x.X, (float)x.Y, (float)x.Z)));

            for (int i = 0; i < mesh.trisCount; i++)
            {
                aimesh.Faces.Add(new Assimp.Face(new int[] { i * 3, i * 3 + 1, i * 3 + 2 }));
            }

            return aimesh;
        }

        public static QEM.Mesh ToQemMesh(this Assimp.Mesh mesh)
        {
            List<int> Indices = mesh.Faces.SelectMany(x => x.Indices.Cast<int>()).ToList();
            List<QEM.Vector> vectors = mesh.Vertices.Select(x => new QEM.Vector(x.X, x.Y, x.Z)).ToList();

            QEM.Mesh originalMesh = new QEM.Mesh();
            originalMesh.tris = new QEM.Triangle[Indices.Count / 3];

            for (int i = 0; i < Indices.Count / 3; i++)
            {
                originalMesh.tris[i] = new QEM.Triangle()
                {
                    v1 = vectors[(int)Indices[i * 3]],
                    v2 = vectors[(int)Indices[i * 3 + 1]],
                    v3 = vectors[(int)Indices[i * 3 + 2]],
                };
            }

            return originalMesh;
        }
    }
}
