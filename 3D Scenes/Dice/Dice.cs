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
	[Export] private double waitTime = 0.5f;
	[Export] private float rotationWeight;

	private DiceRollutton rollButton;
	private Random random = new Random();
	private Vector3 pos, oldPos;
	private DiceToss diceToss;
	private Timer _timer;
	
 	public RigidBody3D diceRigidBody;
	public bool isMoving;
	public int diceResult;

	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		diceToss = GetParent<DiceToss>();
		rollButton = diceToss.rollButton;
		
		_timer = new Timer();
		_timer.WaitTime = waitTime;
		_timer.Start();
		
		AddChild(_timer);
		
		diceRigidBody = GetNode<RigidBody3D>("DiceRigidBody");
		diceRigidBody.AngularVelocity += angularVelocity;
		diceRigidBody.LinearVelocity += Vector3.One + new Vector3(linearVelocity, 0, 0);
		
		pos = diceRigidBody.GlobalPosition;
		oldPos = diceRigidBody.GlobalPosition;
		
		rotationWeight = (float) random.NextDouble();
		
		foreach (RayCast3D ray in rays)
		{
			ray.CollideWithBodies = true;
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		pos = diceRigidBody.GlobalPosition;
		//GD.Print(isMoving);
		if (_timer.TimeLeft <= 0)
		{
			CheckForMovement();
		}
	}

	private void CheckForMovement()
	{
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
