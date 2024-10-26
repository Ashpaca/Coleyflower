using Godot;
using System;

public partial class FellaMaker : Node2D
{
	PackedScene FellaScene = GD.Load<PackedScene>("res://Fella.tscn");

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        if (Input.IsActionJustPressed("mouse_1"))
        {
            Spawn(GetGlobalMousePosition());
        }
    }

    private void Spawn(Vector2 MousePosition)
	{
        Fella foo = FellaScene.Instantiate<Fella>();
        foo.Position = new Vector2(MousePosition.X, MousePosition.Y);
        GetTree().Root.AddChild(foo);
    }
}
