using Godot;
using System;

public partial class FellaMaker : Node2D
{
	PackedScene FellaScene = GD.Load<PackedScene>("res://Disc.tscn");

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        if (Input.IsActionJustPressed("mouse_2"))
        {
            Spawn(GetGlobalMousePosition());
        }
    }

    private void Spawn(Vector2 MousePosition)
	{
        Disc foo = FellaScene.Instantiate<Disc>();
        foo.Position = new Vector2(MousePosition.X, MousePosition.Y);
        GetTree().Root.AddChild(foo);
    }
}
