using Godot;
using System;

public partial class Intro : Control
{
    public override void _UnhandledInput(InputEvent @event)
    {
        if(@event is InputEventKey keyEvent && keyEvent.Pressed)
        {
            GetNode<AnimationPlayer>("Animation").Play("intro");
            GetNode<AudioStreamPlayer>("BackgroundAudio").Play(0);
        }
    }
}
