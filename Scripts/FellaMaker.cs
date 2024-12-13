using Godot;
using System;

public partial class FellaMaker : Node2D
{
	PackedScene FellaScene = GD.Load<PackedScene>("res://Disc.tscn");
	PackedScene SpiceyPepperScene = GD.Load<PackedScene>("res://spicey_pepper.tscn");
	PackedScene[] DiscsToSpawn;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		DiscsToSpawn = new PackedScene[2] {FellaScene, SpiceyPepperScene};
	}

    public Disc Spawn(Vector2 MousePosition, int discNum)
	{
        Disc foo = DiscsToSpawn[discNum].Instantiate<Disc>();
        foo.Position = new Vector2(MousePosition.X, MousePosition.Y);
        GetTree().Root.AddChild(foo);
        return foo;
    }
}
