using Assimp;
using ComputeSharp;
using MeshSimpler.Core.Sence;
using NUnit.Framework;
using Silk.NET.Maths;
using System;
using System.Collections.Generic;
using System.IO;

namespace MeshSimpler.Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }
        [Test]
        public void TestAssimp()
        {
            var file = System.IO.File.ReadAllBytes(@"C:\Users\Liuxinyu\Desktop\MyQem-master\$ab5a154f-3546-409a-b77d-c593165859a9_LOD_5.glb");
            var file2 = System.IO.File.ReadAllBytes(@"C:\Users\Liuxinyu\Desktop\MyQem-master\$6177ecd5-9098-4a0a-b039-a817cb665549_LOD_3.glb");
            AssimpContext assimpContext = new AssimpContext();

            MemoryStream ms = new MemoryStream(file);
            var sence = assimpContext.ImportFileFromStream(ms, "glb");
            sence.RootNode.Transform = Matrix4x4.FromRotationZ(90f / 180f * MathF.PI);
            var bob = assimpContext.ExportToBlob(sence, "gltb2");
            assimpContext.ExportFile(sence, @$"C:\Users\Liuxinyu\Desktop\MyQem-master\test1.gltf", "gltb2");

            MemoryStream ms2 = new MemoryStream(file2);
            var sence2 = assimpContext.ImportFileFromStream(ms2, "glb");
            sence2.RootNode.Transform = Matrix4x4.FromRotationZ(-0.5f * MathF.PI);
            var bob2 = assimpContext.ExportToBlob(sence2, "gltf2");
            assimpContext.ExportFile(sence2, @$"C:\Users\Liuxinyu\Desktop\MyQem-master\test2.gltf", "gltb2");

            //System.IO.File.WriteAllBytes(@$"C:\Users\Liuxinyu\Desktop\MyQem-master\test1.gltf", bob.Data);
            //System.IO.File.WriteAllBytes(@$"C:\Users\Liuxinyu\Desktop\MyQem-master\test2.gltf", bob2.Data);

            //FileStream fs = new FileStream(@"C:\Users\Liuxinyu\Desktop\MyQem-master\test1.glb", FileMode.OpenOrCreate);
            //bob.ToStream(fs);

            //FileStream fs2 = new FileStream(@"C:\Users\Liuxinyu\Desktop\MyQem-master\test2.glb", FileMode.OpenOrCreate);
            //bob2.ToStream(fs2);

            //var glbbytes = ms1.ToArray();
            //System.IO.File.WriteAllBytes(@$"C:\Users\Liuxinyu\Desktop\MyQem-master\test.glb", bob.Data);
        }
        [Test]
        public void TestI3dm()
        {
            //var mapbox_positions = new List<System.Numerics.Vector3>();

            //mapbox_positions.Add(new System.Numerics.Vector3(-8407346.9596f, 4743739.3031f, 38.29f));
            //mapbox_positions.Add(new System.Numerics.Vector3(-8406181.2949f, 4744924.0771f, 38.29f));

            //var i3dm = new I3dm.Tile.I3dm(mapbox_positions, simplyGlb);
            //i3dm.BatchTableJson = "{\"Height\":[100,101]}";
            //i3dm.FeatureTable.IsEastNorthUp = true;
            //// act
            //var bytes = I3dm.Tile.I3dmWriter.Write(i3dm);

            //var cmpt = CmptReader.Read(cmptfile);
            //Assert.IsTrue(cmpt.CmptHeader.TilesLength == 2);
            //Assert.IsTrue(cmpt.Tiles.Count == 2);
            //Assert.IsTrue(cmpt.Magics.ToArray()[0] == "b3dm");
            //Assert.IsTrue(cmpt.Magics.ToArray()[1] == "i3dm");
            //var b3dm = B3dmReader.ReadB3dm(new MemoryStream(cmpt.Tiles.ToArray()[0]));
            //var i3dm = I3dmReader.Read(new MemoryStream(cmpt.Tiles.ToArray()[1]));
        }
        [Test]
        public void Vector4D_AllocateReadWriteBuffer()
        {
            var m_Tangents = Gpu.Default.AllocateReadWriteBuffer<Vector4D<double>>(1000, AllocationMode.Default);
            Assert.AreEqual(1000, m_Tangents.Length);
            m_Tangents.Dispose();
            Assert.Pass("");
        }
        [Test]
        public void SimplyMesh()
        {
            QemLoder.RunSimplify(@"C:\Users\Liuxinyu\Desktop\MyQem-master\$ab5a154f-3546-409a-b77d-c593165859a9.glb", 10);
        }
        [Test]
        public void SimplyMesh3()
        {
            QemLoder.RunSimplifyByLevel(@"C:\Users\Liuxinyu\Desktop\MyQem-master\$ab5a154f-3546-409a-b77d-c593165859a9.glb", 1);
        }
    }
}