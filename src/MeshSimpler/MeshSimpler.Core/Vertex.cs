using Silk.NET.Maths;
using System.Numerics;
using System.Runtime.InteropServices;


namespace MeshSimpler.Core
{
	[StructLayout(LayoutKind.Sequential)]
	struct Vertex
	{
		public Vector3D<double> Position;
		public Vector3D<double> Normal;
		public Vector2D<double> TexCoord;
		public int MaterialIndex;

		public Vertex(Vector3D<double> position, Vector3D<double> normal, Vector2D<double> texCoord, int materialIndex) =>
			(Position, Normal, TexCoord, MaterialIndex) = (position, normal, texCoord, materialIndex);

		public override bool Equals(object obj) => base.Equals(obj);
		public bool Equals(in Vertex p) =>
			Position == p.Position && Normal == p.Normal && TexCoord == p.TexCoord && MaterialIndex == p.MaterialIndex;
		public override int GetHashCode() =>
			(Position, Normal, TexCoord, MaterialIndex).GetHashCode();
		public static bool operator ==(in Vertex lhs, in Vertex rhs) => lhs.Equals(rhs);
		public static bool operator !=(in Vertex lhs, in Vertex rhs) => !(lhs == rhs);
	}
}
