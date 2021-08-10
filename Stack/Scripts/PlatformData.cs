using Godot;
using System;

public static class PlatformData
{
	public static readonly Vector3[] vertices =
	{
		new Vector3(-1, 0, -1),
		new Vector3(1, 0, -1),
		new Vector3(1, .2f, -1),
		new Vector3(-1, .2f, -1),
		new Vector3(-1, 0, 1),
		new Vector3(1, 0, 1),
		new Vector3(1, .2f, 1),
		new Vector3(-1, .2f, 1),

	};

	public static readonly int[,] triangles =
	{
		//front, back, top, bottom, left, right
		{3, 7, 2, 6}, // Top Face
		{0, 3, 1, 2}, // Front Face
		{5, 6, 4, 7}, // Back Face
		{1, 5, 0, 4}, // Bottom Face
		{4, 7, 0, 3}, // Left Face
		{1, 2, 5, 6} // Right Face
	};

	public static readonly Color[] color = {new Color(.8705f, .7254f, .5254f), new Color(.9294f, .3019f, .4313f), new Color(.8588f, .4235f, .8666f)};

	public static SpatialMaterial defaultMat
	{
		get
		{
			SpatialMaterial tempMaterial = new SpatialMaterial();
			tempMaterial.Metallic = 0;
			tempMaterial.MetallicSpecular = 0;
			tempMaterial.Roughness = 1;

			return tempMaterial;
		}
	}

	public static SpatialMaterial material1
	{
		get
		{
			SpatialMaterial tempMaterial = defaultMat;
			tempMaterial.AlbedoColor = color[0];
			return tempMaterial;
		}
	}
	public static SpatialMaterial material2
	{
		get
		{
			SpatialMaterial tempMaterial = defaultMat;
			tempMaterial.AlbedoColor = color[1];
			return tempMaterial;
		}
	}
	public static SpatialMaterial material3
	{
		get
		{
			SpatialMaterial tempMaterial = defaultMat;
			tempMaterial.AlbedoColor = color[2];
			return tempMaterial;
		}
	}
}
