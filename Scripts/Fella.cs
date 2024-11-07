using Godot;
using System;

public partial class Fella : RigidBody2D
{
	public override void _PhysicsProcess(double delta)
	{	
		ApplyCentralForce(-LinearVelocity * 2);
	}
}
