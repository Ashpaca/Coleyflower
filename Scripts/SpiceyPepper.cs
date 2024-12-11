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

		// if startFlaming is true it will spawn a flameInstance every few seconds until a maximum number is reached
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

	/**
	* Lets the SpiceyPepper know it should start spawning flames behind it. And sets how long the
	* flames stick around. Currently set to 2 rounds.
	*/
	public override void OnFlickEffects()
	{
		startFlaming = true;
		flameRoundTimer = 2;
	}

	/**
	* If the SpiceyPepper is spawning flames, it stops it. Decrements the flame round timer, and
	* deletes all the spawned flames if the timer reaches 0.
	*/
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
