using Godot;
using System;

public partial class Ladder : Area2D
{
   public override void _Ready()
   {
      base._Ready();
      BodyEntered += OnBodyEntered;
      BodyExited += OnBodyExited;
   }

   private void OnBodyExited(Node2D body)
   {
      if (body is Actor actor)
      {
         actor.ToggleLadder(false);
        
      }
   }

   private void OnBodyEntered(Node2D body)
   {
      if (body is Actor actor)
      {
         actor.ToggleLadder(true, this);
      }
   }
}
