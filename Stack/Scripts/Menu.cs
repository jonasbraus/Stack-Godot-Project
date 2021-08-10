using Godot;
using System;

public class Menu : CanvasLayer
{

	private void _on_Play_pressed()
	{
		GetTree().ChangeScene("res://World.tscn");
	}
	
}

