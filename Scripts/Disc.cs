using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Disc : RigidBody2D
{
	[Export]
	public float radius { get; set; }
	[Export]
	public int health { get; set; }
	[Export]
	public int damage { get; set; }
	[Export]
	public bool player { get; set; }
	[Export]
	public int numberOfFlicks { get; set; }
    // There will be more 

	List<Disc> discCollisions = new List<Disc>();
	List<Disc> handledDiscCollisions = new List<Disc>();

    public override void _PhysicsProcess(double delta)
	{	
		// Adds drag to the Disc
		ApplyCentralForce(-LinearVelocity * 2);

		// May need to change this, but the idea is to stop discs sooner if they are moving really slowly
		if (LinearVelocity.LengthSquared() < 100)
		{
			ApplyCentralForce(-LinearVelocity * 100);
		}

		CheckCollisons();
	}

	private void CheckCollisons()
	{	
		List<Disc> collisions = new List<Disc>();
		foreach (Node2D body in GetCollidingBodies())
		{
			if (body is Disc disc)
			{
				collisions.Add(disc);
			}
		}

		handledDiscCollisions = handledDiscCollisions.Intersect(collisions).ToList<Disc>();
		discCollisions = discCollisions.Union(collisions.Except(handledDiscCollisions).ToList<Disc>()).ToList<Disc>();
	}

	/**
	* Return: a list containing all the Discs this is currently colliding with not including Discs
	*	that are still in contact with this that have already been handled
	* This function should be called by the GameStateController, at which point all returned Discs
	*	will be considered handled.
	*/
	public List<Disc> HandleCollisons()
	{
		discCollisions = discCollisions.Except(handledDiscCollisions).ToList<Disc>();
		handledDiscCollisions = handledDiscCollisions.Union(discCollisions).ToList<Disc>();
		return discCollisions;
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
