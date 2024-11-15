using Godot;
using System;

public partial class Disc : RigidBody2D
{
	[Export]
	public float radius { get; set; }

	public override void _PhysicsProcess(double delta)
	{	
		ApplyCentralForce(-LinearVelocity * 2);
	}
}
