using Godot;
using System;

public partial class Disc : RigidBody2D
{
	[Export]
	public float radius { get; set; }
	[Export]
	public int health { get; set; }
	[Export]
	public int damage { get; set; }
	// There will be more 

	public override void _PhysicsProcess(double delta)
	{	
		ApplyCentralForce(-LinearVelocity * 2);
	}
}
