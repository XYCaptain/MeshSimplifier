using MeshSimpler.Core.Extensions;
using Silk.NET.Maths;

namespace MeshSimpler.Core
{
    abstract class Procedural
    {
        public virtual (Vector3D<double>, Vector3D<double>) BoundingBox => default;
    }

    class Sphere : Procedural
    {
        public readonly Vector3D<double> Center;
        public readonly float Radius;

        public Sphere(in Vector3D<double> center, float radius) =>
            (Center, Radius) = (center, radius);

        public override (Vector3D<double>, Vector3D<double>) BoundingBox =>
            (Center.Add(-Radius), Center.Add(Radius));
    }
}
