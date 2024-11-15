using Godot;
using System;
using System.Reflection;

public partial class Shooter : Node2D
{
	bool fellaClicked = false;
	Area2D mouseArea;
	Sprite2D shotPointer;
	Disc shootee;
	float powerCoefficient = 5;
	float theta;
	float maxPower = 2;
	Vector2 forceVector;

	public override void _Ready()
	{
		mouseArea = GetNode<Area2D>("Mouse Area");
		shotPointer = GetNode<Sprite2D>("ShotPointer");
		shotPointer.Visible = false;
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
			Vector2 pullBackDistance = shootee.GlobalPosition - GlobalPosition;
			float forcePower = Mathf.Clamp(pullBackDistance.Length()/300, 0, maxPower);
			forceVector = pullBackDistance.Normalized() * forcePower * 300;

			shotPointer.Offset = new Vector2(shootee.radius + 10f/(1 + forcePower), 0f);
			shotPointer.Scale = new Vector2(1 + forcePower, 1 + forcePower);
        }

		if (Input.IsActionJustReleased("mouse_1") && shootee != null)
		{
			ShootShooter();
			shotPointer.Visible = false;
		}
	}

    private Disc FindShootee()
	{
		foreach (PhysicsBody2D body in mouseArea.GetOverlappingBodies())
		{
			if (body is Disc)
			{
				return (Disc)body;
			}
		}
		return null;
	}

	private void ShootShooter()
	{
		shootee.ApplyCentralImpulse(forceVector * powerCoefficient);
	}
}
