using Silk.NET.Maths;
using System.Numerics;

namespace MeshSimpler.Core.Extensions
{
    static class Vector4Extensions
    {
        public static Vector3 ToVector3(this in Vector4 vec) =>
            new Vector3(vec.X, vec.Y, vec.Z);
        public static Vector3D<double> ToVector3D(this in Vector4D<double> vec) =>
            new Vector3D<double>(vec.X, vec.Y, vec.Z);
        public static Vector3D<float> ToVector3D(this in Vector4D<float> vec) =>
            new Vector3D<float>(vec.X, vec.Y, vec.Z);
    }
}
