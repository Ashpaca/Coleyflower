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
	}

	/**
	* Return: The first disc found under the mouse pointer. Or null if no Discs found.
	*/
    public Disc FindShootee()
	{
		foreach (PhysicsBody2D body in mouseArea.GetOverlappingBodies())
		{
			if (body is Disc disc && disc.player)
			{
				return disc;
			}
		}
		return null;
	}

	/**
	* Return: Force Vector to apply to the flicked Disc. The actual value applied to the disc is
	*	the local copy of this vector. The returned value is in case the GameStateController needs
	*	to know how much force is going to be applied.
	* Handles displaying the arrow for visualising the shot, and calculating the amount of force
	* that should be applied to the selected disc. Clamps the power between 0 and maxPower.
	*/
	public Vector2 AimShot(Disc shootee)
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
		return forceVector;
	}

	/**
	* Parameter: shootee - the Disc that will be flicked and have a impulse applied to it.
	* Applies the force vector and also hides the aiming arrow.
	*/
	public void ShootShootee(Disc shootee)
	{
		shootee.ApplyCentralImpulse(forceVector * powerCoefficient);
		shotPointer.Visible = false;
	}
}
