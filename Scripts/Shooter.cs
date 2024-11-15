using Godot;
using System;
using System.Reflection;

public partial class Shooter : Node2D
{
	bool fellaClicked = false;
	Area2D mouseArea;
	Sprite2D shotPointer;
	Fella shootee;
	float shootPower = 5;
	float theta;

	public override void _Ready()
	{
		mouseArea = GetNode<Area2D>("Mouse Area");
		shotPointer = GetNode<Sprite2D>("ShotPointer");
    }

	public override void _Process(double delta)
	{
		GlobalPosition = GetGlobalMousePosition();

		if (Input.IsActionJustPressed("mouse_1"))
		{
			shootee = FindShootee();
		}

		if (Input.IsActionPressed("mouse_1") && shootee != null)
		{
			shotPointer.Visible = true;
			theta = Mathf.Atan2(shootee.GlobalPosition.Y - GlobalPosition.Y, shootee.GlobalPosition.X - GlobalPosition.X);
			shotPointer.Rotation = theta;
			shotPointer.GlobalPosition = shootee.GlobalPosition;
		}

		if (Input.IsActionJustReleased("mouse_1") && shootee != null)
		{
			ShootShooter();
			shotPointer.Visible = false;
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
