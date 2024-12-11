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
		// Adds drag to the Disc
		ApplyCentralForce(-LinearVelocity * 2);
	}

	/**
	* Parameters: Currently None
	* Return: Currently None
	* Parent Disc class has no definition. Each child can implement their own effects when launched
	*/
	public virtual void OnFlickEffects()
	{

	}

	/**
	* Parameters: Currently None
	* Return: Currently None
	* Parent Disc class has no definition. Each child can implement their own effects when a turn
	* 	ends. For example a disc may have an effect that lasts a certain number of turns. Every
	*	disc has this function called each turn.
	*/
	public virtual void EndTurn()
	{

	}
}
