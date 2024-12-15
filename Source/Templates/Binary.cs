using Godot;
using System;

public partial class Binary : Node2D
{
	[Export]
	public bool On { get; set; } = false;

	[Export]
	public Binary Next { get; set; } = null;

	[Export]
	public AnimationPlayer AnimationPlayer { get; set; } = null;

	[Export]
	public Area2D ActivationArea { get; set; } = null;

	[Signal]
	public delegate void OnBinaryValueChangedEventHandler(bool value);

	[Export]
	public Node2D Source { get; set; } = null;

	public override void _Ready()
	{
		AnimationPlayer.AnimationFinished += (name) =>
		{
			EmitSignal(nameof(OnBinaryValueChanged), On);

			if (Next is Binary binary && Source is not Binary)
			{
				binary.Toggle(this);
			}
		};

		ActivationArea.AreaEntered += (area) => Trigger(area);
		Toggle();
	}
	public void Trigger(Area2D area)
	{
		if (area.Name == "HitBox")
		{
			Toggle(area);
		}
	}

	public void Toggle(Node2D source = null)
	{
		Source = source ?? this;
		On = !On;
		if (AnimationPlayer != null)
		{
			AnimationPlayer.Play(On ? "ON" : "OFF");
		}
	}
}
