using Silk.NET.Maths;
using System;
using System.Numerics;

namespace MeshSimpler.Core.Extensions
{
    static class Vector3Extensions
    {
        public static Vector3 Add(this in Vector3 vec, float value) =>
            new Vector3(vec.X + value, vec.Y + value, vec.Z + value);

        public static Vector3D<double> Add(this in Vector3D<double> vec, double value)
            => new Vector3D<double>(vec.X + value, vec.Y + value, vec.Z + value);
        public static Vector3D<float> Add(this in Vector3D<float> vec, float value)
            => new Vector3D<float>(vec.X + value, vec.Y + value, vec.Z + value); 
    }
}
