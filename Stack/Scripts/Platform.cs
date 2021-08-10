using Godot;
using System;
using System.Collections.Generic;

public class Platform : MeshInstance
{
	[Export] public bool isActive = false;
	[Export] public bool inMenu = false;

	public int randomMaterial;
	public int randomAxis;
	public Platform last;
	public Vector3[] vertices;
	public Vector3[] newVerts = new Vector3[0];
	public float speed = 2f;
	public int score = 1;
	
	public const float maxMoveDistance = 3f;

	private Random random = new Random();
	private Vector3 velocity;
	private Camera cam;
	private PackedScene prefab = GD.Load<PackedScene>("res://Platform.tscn");
	private Label labelScore;

	public override void _Ready()
	{
		if(!inMenu)
		{
			cam = GetNode<Camera>("../Camera");
			labelScore = GetNode<Label>("../Overlay/Score");
		}
		
		if (isActive)
		{
			MeshInstance lastMesh = GetNode<MeshInstance>("../Base");
			last = lastMesh as Platform;
		}
		
		CalculateRandoms();
		RebuildMesh(PlatformData.vertices);
	}

	public override void _PhysicsProcess(float delta)
	{
		if (isActive)
		{
			if (Mathf.Abs(Translation.x) >= maxMoveDistance || Mathf.Abs(Translation.z) >= maxMoveDistance)
			{
				velocity.x *= -1;
				velocity.z *= -1;
			}
			
			Translate(velocity * delta);
			
			GD.Print(velocity * delta);
			

			if (Input.IsActionJustPressed("Click") && !inMenu)
			{
				isActive = false;
				
				cam.Translate(new Vector3(0, .2f, 0));

				Vector3[] otherVerts = last.vertices;
				Vector3[] newVerts;

				if (last.newVerts.Length == 0)
				{
					newVerts = PlatformData.vertices.Clone() as Vector3[];
				}
				else
				{
					newVerts = last.newVerts.Clone() as Vector3[];
				}

				if (randomAxis == 0)
				{
					float currentX = Translation.x;
					float lastX = last.Translation.x;

					if (vertices[0].x + currentX < otherVerts[0].x + lastX)
					{
						newVerts[0].x = otherVerts[0].x - currentX + lastX;
						newVerts[3].x = otherVerts[0].x - currentX + lastX;
						newVerts[7].x = otherVerts[0].x - currentX + lastX;
						newVerts[4].x = otherVerts[0].x - currentX + lastX;
					}

					if (vertices[3].x + currentX > otherVerts[3].x + lastX)
					{
						newVerts[1].x = otherVerts[3].x - currentX + lastX;
						newVerts[2].x = otherVerts[3].x - currentX + lastX;
						newVerts[5].x = otherVerts[3].x - currentX + lastX;
						newVerts[6].x = otherVerts[3].x - currentX + lastX;
					}

					if (vertices[3].x + currentX < otherVerts[0].x + lastX ||
						vertices[0].x + currentX > otherVerts[3].x + lastX)
					{
						GetTree().ChangeScene("res://Menu.tscn");
					}
				}
				else
				{
					float currentZ = Translation.z;
					float lastZ = last.Translation.z;

					if (vertices[0].z + currentZ < otherVerts[0].z + lastZ)
					{
						newVerts[0].z = otherVerts[0].z - currentZ + lastZ;
						newVerts[1].z = otherVerts[0].z - currentZ + lastZ;
						newVerts[2].z = otherVerts[0].z - currentZ + lastZ;
						newVerts[3].z = otherVerts[0].z - currentZ + lastZ;
					}

					if (vertices[3].z + currentZ > otherVerts[3].z + lastZ)
					{
						newVerts[4].z = otherVerts[3].z - currentZ + lastZ;
						newVerts[5].z = otherVerts[3].z - currentZ + lastZ;
						newVerts[6].z = otherVerts[3].z - currentZ + lastZ;
						newVerts[7].z = otherVerts[3].z - currentZ + lastZ;
					}

					if (vertices[3].z + currentZ < otherVerts[0].z + lastZ ||
						vertices[0].z + currentZ > otherVerts[3].z + lastZ)
					{
						GetTree().ChangeScene("res://Menu.tscn");
					}
				}

				this.newVerts = newVerts;
				
				RebuildMesh(newVerts);
				
				MeshInstance instance = prefab.Instance() as MeshInstance;
				GetParent().AddChild(instance);
				
				Platform instanceScript = instance as Platform;
				instanceScript.isActive = false;
				
				instance.Translate(new Vector3(0, .2f, 0) + Translation);
				instanceScript.last = this;
				instanceScript.speed = speed + .2f;
				
				instanceScript.CalculateRandoms();
				instanceScript.RebuildMesh(newVerts);

				labelScore.Text = score.ToString();

				instanceScript.score = score + 1;

				instanceScript.isActive = true;
			}
		}
	}

	public void RebuildMesh(Vector3[] vertTemplate)
	{
		List<Vector3> tempVertices = new List<Vector3>();
		
		SurfaceTool st = new SurfaceTool();
		
		st.Begin(Mesh.PrimitiveType.Triangles);
		switch (randomMaterial)
		{
			case 0:
				st.SetMaterial(PlatformData.material1);
				break;
			case 1:
				st.SetMaterial(PlatformData.material2);
				break;
			case 2:
				st.SetMaterial(PlatformData.material3);
				break;
		}

		int index = 0;
		
		for (int i = 0; i < 6; i++)
		{
			st.AddVertex(vertTemplate[PlatformData.triangles[i, 0]]);
			st.AddVertex(vertTemplate[PlatformData.triangles[i, 1]]);
			st.AddVertex(vertTemplate[PlatformData.triangles[i, 2]]);
			st.AddVertex(vertTemplate[PlatformData.triangles[i, 3]]);
			
			tempVertices.Add(vertTemplate[PlatformData.triangles[i, 0]]);
			tempVertices.Add(vertTemplate[PlatformData.triangles[i, 1]]);
			tempVertices.Add(vertTemplate[PlatformData.triangles[i, 2]]);
			tempVertices.Add(vertTemplate[PlatformData.triangles[i, 3]]);
			
			st.AddIndex(index + 2);
			st.AddIndex(index + 1);
			st.AddIndex(index + 0);
			st.AddIndex(index + 3);
			st.AddIndex(index + 1);
			st.AddIndex(index + 2);

			index += 4;
		}
		
		st.GenerateNormals();
		Mesh = st.Commit();
		
		vertices = tempVertices.ToArray();
	}

	public void CalculateRandoms()
	{
		randomMaterial = random.Next(0, 3);
		randomAxis = random.Next(0, 2);

		if (last != null)
		{
			while (randomMaterial == last.randomMaterial)
			{
				randomMaterial = random.Next(0, 3);
			}

			while (randomAxis == last.randomAxis)
			{
				randomAxis = random.Next(0, 2);
			}
			
			
		}

		if (randomAxis == 0)
		{
			velocity.z = 0;
			velocity.x = speed;
		}
		else
		{
			velocity.x = 0;
			velocity.z = speed;
		}
	}
}
