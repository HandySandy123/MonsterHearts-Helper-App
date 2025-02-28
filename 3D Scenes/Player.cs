using Godot;
using System;

public partial class Player : Camera3D
{
	[Export] private float movementFactor = 0.2f;

	private Transform3D playerTransform;
	private Vector3 yaw = new Vector3(0, 1, 0);
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		playerTransform = Transform;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Transform = playerTransform;
	}

	public override void _PhysicsProcess(double delta)
	{
		if (Input.IsActionPressed("YawAdd"))
		{
			GD.Print("YawAdd");
			RotateObjectLocal(yaw, movementFactor);
		}
		if (Input.IsActionPressed("YawSubtract"))
		{
			GD.Print("YawSubtract");
			RotateObjectLocal(yaw * -1, movementFactor);
		}
	}

	public override void _Input(InputEvent @event)
	{
		
	}
}
