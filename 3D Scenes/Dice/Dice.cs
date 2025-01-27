using Godot;
using System;
using System.Collections.Generic;
using Vector3 = Godot.Vector3;

public partial class Dice : Node3D
{
	[Export] private RayCast3D[] rays = new RayCast3D[6];
	[Export] private Vector3 angularVelocity = Vector3.One;
	[Export] private float linearVelocity = 1.0f;
	[Export] private PackedScene table;

	private Vector3 pos;
	private Vector3 oldPos;
	
 	public RigidBody3D diceRigidBody;
    public bool isMoving;

    
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		diceRigidBody = GetNode<RigidBody3D>("DiceRigidBody");
		diceRigidBody.AngularVelocity += angularVelocity;
		diceRigidBody.LinearVelocity += Vector3.One + new Vector3(linearVelocity, 0, 0);
		
		pos = diceRigidBody.GlobalPosition;
		oldPos = diceRigidBody.GlobalPosition;
		
		foreach (RayCast3D ray in rays)
		{
			ray.CollideWithBodies = true;
			
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
		pos = diceRigidBody.GlobalPosition;
		
		if (oldPos == pos)
		{
			isMoving = false;
		}
		else
		{
			isMoving = true;
		}

		oldPos = pos;
	}

	public int GetDiceRoll()
	{
		for (int i = 0; i < rays.Length; i++)
		{
			if (rays[i].IsColliding())
			{
				GD.Print(7 - (i + 1));
				return 7 - (i + 1);
			}
		}
		return -1;
	}
	public override void _PhysicsProcess(double delta) 
	{
		
		
		angularVelocity.X *= (float) Mathf.LerpAngle(1, 0, 0.8);
		angularVelocity.Y *= (float) Mathf.LerpAngle(1, 0, 0.8);
		angularVelocity.Z *= (float) Mathf.LerpAngle(1, 0, 0.8);
	}
}
