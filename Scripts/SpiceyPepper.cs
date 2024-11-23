using Godot;
using System;

public partial class SpiceyPepper : Disc
{
	const int MAX_FLAMES = 10;
	PackedScene flameScene = GD.Load<PackedScene>("res://flame.tscn");
	double timeSinceLastFlame = 0;
	Node2D[] createdFlames;
	int numberOfFlames = 0;

	bool startFlaming = false;
	int flameRoundTimer = 2;

	public override void _Ready()
	{
		base._Ready();
		createdFlames = new Node2D[MAX_FLAMES];
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);

		if (startFlaming)
		{
			timeSinceLastFlame += delta;
			if (timeSinceLastFlame >= 20/LinearVelocity.Length() && numberOfFlames < MAX_FLAMES)
			{
				timeSinceLastFlame = 0;
				Node2D flameInstance = flameScene.Instantiate<Node2D>();
        		flameInstance.Position = GlobalPosition;
				createdFlames[numberOfFlames] = flameInstance;
				numberOfFlames++;
        		GetTree().Root.AddChild(flameInstance);
			}
			if (numberOfFlames >= MAX_FLAMES)
			{
				startFlaming = false;
			}
		}
	}

	public override void OnFlickEffects()
	{
		startFlaming = true;
		flameRoundTimer = 2;
	}

	public override void EndTurn()
	{
		flameRoundTimer -= 1;
		startFlaming = false;
		if (flameRoundTimer == 0)
		{
			for (int i = 0; i < numberOfFlames; i++)
			{
				createdFlames[i].QueueFree();
			}
			numberOfFlames = 0;
		}
	}
}
