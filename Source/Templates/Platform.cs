using Godot;
using System;

public partial class Platform : Node2D
{
	[Export]
	public Binary BinaryA { get; set; } = null;

	[Export]
	public Binary BinaryB { get; set; } = null;

	[Export]
	public PathFollow2D Follow { get; set; } = null;

	[Export]
	public float MovingDirection = 0.0f;

	[Export]
	public float MovingScale = 1.0f;

	public override void _Ready()
	{
		BinaryB.OnBinaryValueChanged += (value) => MoveUp();
		BinaryA.OnBinaryValueChanged += (value) => MoveDown();
	}

	public void MoveUp()
	{
		if (BinaryB.On)
		{
			MovingDirection = -1.0f;
			GD.Print("Moving Up");
		}
	}

	public void MoveDown()
	{
		if (BinaryA.On)
		{
			MovingDirection = 1.0f;
			GD.Print("Moving Down");
		}
	}

	public override void _Process(double delta)
	{
		Follow.ProgressRatio += MovingDirection * ((float)delta) * MovingScale;
	}
}
