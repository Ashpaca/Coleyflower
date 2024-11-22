using Godot;
using System;

public partial class SpiceyPepper : Disc
{
	const int MAX_FLAMES = 20;
	PackedScene flameScene = GD.Load<PackedScene>("res://flame.tscn");
	double timeScinceLastFlame = 0;
	Node2D[] createdFlames;
	int numberOfFlames = 0;

	public override void _Ready()
	{
		base._Ready();
		createdFlames = new Node2D[MAX_FLAMES];
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		timeScinceLastFlame += delta;
		if (timeScinceLastFlame >= .1 && numberOfFlames < MAX_FLAMES)
		{
			timeScinceLastFlame = 0;
			Node2D flameInstance = flameScene.Instantiate<Node2D>();
        	flameInstance.Position = GlobalPosition;
			createdFlames[numberOfFlames] = flameInstance;
			numberOfFlames++;
        	GetTree().Root.AddChild(flameInstance);
		}

		if (numberOfFlames >= MAX_FLAMES)
		{
			for (int i = 0; i < MAX_FLAMES; i++)
			{
				createdFlames[i].QueueFree();
			}
			numberOfFlames = 0;
		}
	}
}
