using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SlimDX;
using SlimDX.Direct3D9;

namespace SB3Utility
{
	public interface IImported
	{
		List<ImportedFrame> FrameList { get; }
		List<ImportedMesh> MeshList { get; }
		List<ImportedMaterial> MaterialList { get; }
		List<ImportedTexture> TextureList { get; }
		List<ImportedAnimation> AnimationList { get; }
		List<ImportedMorph> MorphList { get; }
	}

	public class ImportedFrame : ObjChildren<ImportedFrame>, IObjChild
	{
		public string Name { get; set; }
		public Matrix Matrix { get; set; }

		public dynamic Parent { get; set; }
	}

	public class ImportedMesh
	{
		public string Name { get; set; }
		public List<ImportedSubmesh> SubmeshList { get; set; }
		public List<ImportedBone> BoneList { get; set; }
	}

	public class ImportedSubmesh
	{
		public List<ImportedVertex> VertexList { get; set; }
		public List<ImportedFace> FaceList { get; set; }
		public string Material { get; set; }
		public int Index { get; set; }
		public bool WorldCoords { get; set; }
		public bool Visible { get; set; }
	}

	public class ImportedVertex
	{
		public Vector3 Position { get; set; }
		public float[] Weights { get; set; }
		public byte[] BoneIndices { get; set; }
		public Vector3 Normal { get; set; }
		public float[] UV { get; set; }
	}

	public class ImportedVertexWithColour : ImportedVertex
	{
		public Color4 Colour { get; set; }
	}

	public class ImportedFace
	{
		public int[] VertexIndices { get; set; }
	}

	public class ImportedBone
	{
		public string Name { get; set; }
		public Matrix Matrix { get; set; }
	}

	public class ImportedMaterial
	{
		public string Name { get; set; }
		public Color4 Diffuse { get; set; }
		public Color4 Ambient { get; set; }
		public Color4 Specular { get; set; }
		public Color4 Emissive { get; set; }
		public float Power { get; set; }
		public string[] Textures { get; set; }
	}

	public class ImportedTexture
	{
		public string Name { get; set; }
		public byte[] Data { get; set; }
		public bool isCompressed { get; protected set; }

		public ImportedTexture()
		{
		}

		public ImportedTexture(string path) : this(File.OpenRead(path), Path.GetFileName(path)) { }

		public ImportedTexture(Stream stream, string name)
		{
			Name = name;
			using (BinaryReader reader = new BinaryReader(stream))
			{
				Data = reader.ReadToEnd();
			}

			if (Path.GetExtension(name).ToUpper() == ".TGA")
			{
				isCompressed = Data[2] == 0x0A;
			}
		}

		public Texture ToTexture(Device device)
		{
			if (!isCompressed)
			{
				return Texture.FromMemory(device, Data);
			}

			int width = BitConverter.ToInt16(Data, 8 + 4);
			int height = BitConverter.ToInt16(Data, 8 + 6);
			int bpp = Data[8 + 8];

			int bytesPerPixel = bpp / 8;
			int total = width * height * bytesPerPixel;

			byte[] uncompressedTGA = new byte[18 + total + 26];
			Array.Copy(Data, uncompressedTGA, 18);
			uncompressedTGA[2] = 0x02;

			int srcIdx = 18, dstIdx = 18;
			for (int end = 18 + total; dstIdx < end; )
			{
				byte packetHdr = Data[srcIdx++];
				int packetType = packetHdr & (1 << 7);

				if (packetType != 0)
				{
					int repeat = (packetHdr & ~(1 << 7)) + 1;
					for (int j = 0; j < repeat; j++)
					{
						Array.Copy(Data, srcIdx, uncompressedTGA, dstIdx, bytesPerPixel);
						dstIdx += bytesPerPixel;
					}
					srcIdx += bytesPerPixel;
				}
				else
				{
					int len = ((packetHdr & ~(1 << 7)) + 1) * bytesPerPixel;
					Array.Copy(Data, srcIdx, uncompressedTGA, dstIdx, len);
					srcIdx += len;
					dstIdx += len;
				}
			}

			Array.Copy(Data, srcIdx, uncompressedTGA, dstIdx, 26);
			return Texture.FromMemory(device, uncompressedTGA);
		}
	}

	public interface ImportedAnimation
	{
		// List<dynamic : ImportedAnimationTrack> TrackList { get; set; }
	}

	public abstract class ImportedAnimationTrackContainer<TrackType> : ImportedAnimation where TrackType : ImportedAnimationTrack
	{
		public List<TrackType> TrackList { get; set; }
	}

	public class ImportedKeyframedAnimation : ImportedAnimationTrackContainer<ImportedAnimationKeyframedTrack> { }

	public class ImportedSampledAnimation : ImportedAnimationTrackContainer<ImportedAnimationSampledTrack> { }

	public abstract class ImportedAnimationTrack
	{
		public string Name { get; set; }
	}

	public class ImportedAnimationKeyframedTrack : ImportedAnimationTrack
	{
		public ImportedAnimationKeyframe[] Keyframes { get; set; }
	}

	public class ImportedAnimationKeyframe
	{
		public Vector3 Scaling { get; set; }
		public Quaternion Rotation { get; set; }
		public Vector3 Translation { get; set; }
	}

	public class ImportedAnimationSampledTrack : ImportedAnimationTrack
	{
		public Vector3?[] Scalings;
		public Quaternion?[] Rotations;
		public Vector3?[] Translations;
	}

	public class ImportedMorph
	{
		/// <summary>
		/// Target mesh name
		/// </summary>
		public string Name { get; set; }
		public List<ImportedMorphKeyframe> KeyframeList { get; set; }
		public List<ushort> MorphedVertexIndices { get; set; }
	}

	public class ImportedMorphKeyframe
	{
		/// <summary>
		/// Blend shape name
		/// </summary>
		public string Name { get; set; }
		public List<ImportedVertex> VertexList { get; set; }
	}

	public static class ImportedHelpers
	{
		public static ImportedFrame FindFrame(String name, ImportedFrame root)
		{
			ImportedFrame frame = root;
			if ((frame != null) && (frame.Name == name))
			{
				return frame;
			}

			for (int i = 0; i < root.Count; i++)
			{
				if ((frame = FindFrame(name, root[i])) != null)
				{
					return frame;
				}
			}

			return null;
		}

		public static ImportedMesh FindMesh(String frameName, List<ImportedMesh> importedMeshList)
		{
			foreach (ImportedMesh mesh in importedMeshList)
			{
				if (mesh.Name == frameName)
				{
					return mesh;
				}
			}

			return null;
		}

		public static ImportedMaterial FindMaterial(String name, List<ImportedMaterial> importedMats)
		{
			foreach (ImportedMaterial mat in importedMats)
			{
				if (mat.Name == name)
				{
					return mat;
				}
			}

			return null;
		}

		public static ImportedTexture FindTexture(string name, List<ImportedTexture> importedTextureList)
		{
			if (name == null || name == string.Empty)
			{
				return null;
			}

			foreach (ImportedTexture tex in importedTextureList)
			{
				if (tex.Name == name)
				{
					return tex;
				}
			}

			return null;
		}
	}
}
