using MeshSimpler.Core.Sence;
using qem;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using VGltf;

namespace MeshSimpler.Core.Sence
{
    public class QemLoder
    {
        public static void RunSimplifyByLevel(byte[] file, string importtype, string outpath, int lodparmal)
        {
            var model = Model.LoadModel(file, importtype);
            var totalcount = (int)(model.Indices.Count / 3);
            RunSimplify(model, (int)(totalcount * lodparmal / 16f), @$"{outpath}_LOD_{lodparmal}");
        }
        public static void RunSimplifyByLevel(string path, int lodparmal)
        {
            FileInfo fi = new FileInfo(path);
            var outpath = fi.DirectoryName;
            var model = Model.LoadModel(path);
            var totalcount = (int)(model.Indices.Count / 3);
            RunSimplify(model, (int)(totalcount * lodparmal / 16f), @$"{outpath}\{fi.Name.Replace(fi.Extension, "")}_LOD_{lodparmal}");
        }
        public static void RunSimplify(byte[] file, string importtype, string outpath, int maxlodlevel)
        {
            var model = Model.LoadModel(file, importtype);
            var totalcount = (int)(model.Indices.Count / 3);

            if (maxlodlevel > 16)
            {
                maxlodlevel = 16;
            }

            Dictionary<int, float> lodcount = new();
            for (int i = 1; i <= maxlodlevel; i++)
            {
                if (i == 1)
                    lodcount.Add(i, 1f / maxlodlevel * i * 0.1f);
                else if (i == 2)
                    lodcount.Add(i, 1f / maxlodlevel * i * 0.5f);
                else if (i == 3)
                    lodcount.Add(i, 1f / maxlodlevel * i * 0.8f);
                else
                    lodcount.Add(i, 1f / maxlodlevel * i);
            }

            foreach (var item in lodcount)
                RunSimplify(model, (int)(totalcount * item.Value), @$"{outpath}_LOD_{item.Key}");

        }
        public static void RunSimplify(string path, int maxlodlevel)
        {
            FileInfo fi = new FileInfo(path);
            var outpath = fi.DirectoryName;
            var model = Model.LoadModel(path);
            var totalcount = (int)(model.Indices.Count / 3);

            if (maxlodlevel > 16)
            {
                maxlodlevel = 16;
            }

            Dictionary<int, float> lodcount = new();
            for (int i = 1; i <= maxlodlevel; i++)
            {
                if (i == 1)
                    lodcount.Add(i, 1f / maxlodlevel * i * 0.1f);
                else if (i == 2)
                    lodcount.Add(i, 1f / maxlodlevel * i * 0.5f);
                else if (i == 3)
                    lodcount.Add(i, 1f / maxlodlevel * i * 0.8f);
                else
                    lodcount.Add(i, 1f / maxlodlevel * i);
            }

            foreach (var item in lodcount)
                RunSimplify(model, (int)(totalcount * item.Value), @$"{outpath}\{fi.Name.Replace(fi.Extension, "")}_LOD_{item.Key}");

        }
        private static void RunSimplify(Model Model, int targetCount, string outputPath = null, string fileextsion = "glb", string exporttype = "glb2")
        {
            qem.Mesh originalMesh = new qem.Mesh();
            originalMesh.tris = new Triangle[Model.Indices.Count / 3];
            List<Vector> vectors = Model.Vertices.Select(x => new Vector(x.Position.X, x.Position.Y, x.Position.Z)).ToList();

            for (int i = 0; i < Model.Indices.Count / 3; i++)
            {
                originalMesh.tris[i] = new Triangle()
                {
                    v1 = vectors[(int)Model.Indices[i * 3]],
                    v2 = vectors[(int)Model.Indices[i * 3 + 1]],
                    v3 = vectors[(int)Model.Indices[i * 3 + 2]],
                };
            }
            Run(originalMesh, targetCount, $"{outputPath}.{fileextsion}", exporttype);
        }
        private static void Run(Mesh originalMesh, int targetCount, string outputPath, string exporttype)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            PrintMeshInfo("Input", originalMesh.vertexCount, originalMesh.trisCount);
            float t1 = stopwatch.ElapsedMilliseconds;

            Mesh simplifiedMesh = originalMesh.Simplify(targetCount);
            float t2 = stopwatch.ElapsedMilliseconds - t1;

            //STLParser.SaveSTL(simplifiedMesh, outputPath);
            SaveAs(simplifiedMesh, outputPath, exporttype);
            PrintMeshInfo("Output", simplifiedMesh.vertexCount, simplifiedMesh.trisCount);
            float t3 = stopwatch.ElapsedMilliseconds - t2;

            Console.WriteLine();
            Console.WriteLine($"Execution details [ms]." +
                $"\nOverall time: {t1 + t2 + t3}" +
                $"\n STL Loading: {t1}" +
                $"\n QEM Algorithm: {t2}" +
                $"\n STL Saving: {t3}");
        }
        private static void SaveAs(qem.Mesh simplifiedMesh, string outputPath, string exporttype)
        {
            Assimp.Scene sc = new Assimp.Scene();
            sc.Materials.Add(new Assimp.Material());
     
            var aimesh = new Assimp.Mesh();
            for (int i = 0; i < simplifiedMesh.trisCount; i++)
            {
                var v1 = simplifiedMesh.tris[i].v1;
                var v2 = simplifiedMesh.tris[i].v2;
                var v3 = simplifiedMesh.tris[i].v3;

                var aiv1 = new Assimp.Vector3D((float)v1.X, (float)v1.Y, (float)v1.Z);
                var aiv2 = new Assimp.Vector3D((float)v2.X, (float)v2.Y, (float)v2.Z);
                var aiv3 = new Assimp.Vector3D((float)v3.X, (float)v3.Y, (float)v3.Z);

                var face = new Assimp.Face();
                face.Indices.Add(aimesh.Vertices.Count);
                face.Indices.Add(aimesh.Vertices.Count + 1);
                face.Indices.Add(aimesh.Vertices.Count + 2);

                aimesh.Vertices.Add(aiv1);
                aimesh.Vertices.Add(aiv2);
                aimesh.Vertices.Add(aiv3);
                aimesh.Faces.Add(face);
                aimesh.MaterialIndex = 0;
            }

            sc.Meshes.Add(aimesh);
            var node = new Assimp.Node();
            node.MeshIndices.Add(0);
            sc.RootNode = node;

            Assimp.AssimpContext assimpContext = new Assimp.AssimpContext();
            assimpContext.ExportFile(sc, outputPath, exporttype);
        }
        private static void PrintMeshInfo(string header, int verts, int currentTrisCount)
        {
            Console.WriteLine($"| {header} | Vertices: {verts}, Tris: {currentTrisCount}");
        }
        private static void PrintErrorMessage(string exception)
        {
            Console.WriteLine("\nIncorrect input args. Try using [help].\n" +
                $"Details: {exception}");
        }
    }
}