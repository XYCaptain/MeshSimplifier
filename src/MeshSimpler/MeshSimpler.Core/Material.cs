using Silk.NET.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MeshSimpler.Core
{
	[StructLayout(LayoutKind.Sequential, Pack = 16)]
	struct Material
	{
		// Note: vec3 and vec4 gets aligned on 16 bytes in Vulkan shaders.
		// Base material
		public readonly Vector4D<double> Diffuse;
		public readonly int DiffuseTextureId;

		// Metal fuzziness
		public readonly float Fuzziness;

		// Dielectric refraction index
		public readonly float RefractionIndex;

		// Which material are we dealing with
		public readonly MaterialModel Model;

		public Material(in Vector4D<double> diffuse, int diffuseTextureId, float fuzziness, float refractionIndex, MaterialModel model) =>
			(Diffuse, DiffuseTextureId, Fuzziness, RefractionIndex, Model) = (diffuse, diffuseTextureId, fuzziness, refractionIndex, model);

		public static Material Lambertian(in Vector3D<double>  diffuse, int textureId = -1) =>
			new Material(new Vector4D<double>(diffuse, 1), textureId, 0.0f, 0.0f, MaterialModel.Lambertian);

		public static Material Metallic(in Vector3D<double> diffuse, float fuzziness, int textureId = -1) =>
			new Material(new Vector4D<double>(diffuse, 1), textureId, fuzziness, 0.0f, MaterialModel.Metallic);

		public static Material Dielectric(float refractionIndex, int textureId = -1) =>
			new Material(new Vector4D<double>(0.7f, 0.7f, 1.0f, 1), textureId, 0.0f, refractionIndex, MaterialModel.Dielectric);

		public static Material Isotropic(in Vector3D<double> diffuse, int textureId = -1) =>
			new Material(new Vector4D<double>(diffuse, 1), textureId, 0.0f, 0.0f, MaterialModel.Isotropic);

		public static Material DiffuseLight(in Vector3D<double> diffuse, int textureId = -1) =>
			new Material(new Vector4D<double>(diffuse, 1), textureId, 0.0f, 0.0f, MaterialModel.DiffuseLight);

		[Flags]
		public enum MaterialModel : uint
		{
			Lambertian = 0,
			Metallic = 1,
			Dielectric = 2,
			Isotropic = 3,
			DiffuseLight = 4
		};
	};
}
