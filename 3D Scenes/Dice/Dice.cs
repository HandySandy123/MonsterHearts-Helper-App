using Godot;
using System;
using System.Collections.Generic;
using Vector3 = Godot.Vector3;

public partial class Dice : Node3D
{
	[Export] private RayCast3D[] rays = new RayCast3D[6];
	[Export] private Vector3 angularVelocity = Vector3.One;
	[Export] private float linearVelocity = 1.0f;
	[Export] private double waitTime = 0.5f;
	[Export] private float rotationWeight;

	private DiceRollButton _rollButton;
	private readonly Random _random = new Random();
	private Vector3 _pos, _oldPos;
	private DiceToss _diceToss;
	
 	public RigidBody3D DiceRigidBody;
	public bool IsMoving;
	public int DiceResult;

	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_diceToss = GetParent<DiceToss>();
		_rollButton = _diceToss.rollButton;
		
		DiceRigidBody = this.GetNode<RigidBody3D>("DiceRigidBody");
		GD.Print(DiceRigidBody.ToString());
		DiceRigidBody.ApplyImpulse(Vector3.Forward * linearVelocity, Position);
		
		_pos = DiceRigidBody.GlobalPosition;
		_oldPos = DiceRigidBody.GlobalPosition;
		
		rotationWeight = (float) _random.NextDouble();
		
		foreach (RayCast3D ray in rays)
		{
			ray.CollideWithBodies = true;
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		_pos = DiceRigidBody.Position;
		CheckForMovement();
	}

	private void CheckForMovement()
	{
		if (_oldPos == _pos)
		{
			IsMoving = false;
		}
		else
		{
			IsMoving = true;
		}

		_oldPos = _pos;
	}

	public int GetDiceRoll()
	{
		for (int i = 0; i < rays.Length; i++)
		{
			if (rays[i].IsColliding())
			{
				return 7 - (i + 1);
			}
		}
		return -1;
	}
	public override void _PhysicsProcess(double delta) 
	{
		angularVelocity.X *= (float) Mathf.LerpAngle(1, 0, rotationWeight);
		angularVelocity.Y *= (float) Mathf.LerpAngle(1, 0, rotationWeight);
		angularVelocity.Z *= (float) Mathf.LerpAngle(1, 0, rotationWeight);
	}
}
