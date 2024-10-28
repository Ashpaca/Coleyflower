using Godot;
using System;

public partial class Shooter : Node2D
{
	bool fellaClicked = false;
	Area2D mouseArea;
	Fella shootee;
	float shootPower = 5;

	public override void _Ready()
	{
		mouseArea = GetNode<Area2D>("Mouse Area");
	}

	public override void _Process(double delta)
	{
		GlobalPosition = GetGlobalMousePosition();

		if (Input.IsActionJustPressed("mouse_1"))
		{
			shootee = FindShootee();
		}

		if (Input.IsActionJustReleased("mouse_1") && shootee != null)
		{
			ShootShooter();
		}
	}

    private Fella FindShootee()
	{
		foreach (PhysicsBody2D body in mouseArea.GetOverlappingBodies())
		{
			if (body is Fella)
			{
				return (Fella)body;
			}
		}
		return null;
	}

	private void ShootShooter()
	{
		Vector2 pullBackDistance = shootee.GlobalPosition - GlobalPosition;
		shootee.ApplyCentralImpulse(pullBackDistance * shootPower);
	}
}
