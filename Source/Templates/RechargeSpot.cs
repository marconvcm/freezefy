using Godot;
using System;

public partial class RechargeSpot : Node2D
{
   private Player player;

   [Export]
   public float RechargeRate { get; set; } = 10.0f;

   [Export(PropertyHint.Enum, "Health, Energy, Cold")]
   public string RechargeType { get; set; } = "Cold";

   [Export]
   public Area2D Area { get; set; }

   public override void _Ready()
   {
      Area.BodyEntered += OnBodyEntered;
      Area.BodyExited += OnBodyExited;
   }

   private void OnBodyEntered(Node2D body)
   {
      GD.Print($"OnBodyEntered: isPlayer: {body is Player}`");
      if (body is Player player)
      {
         player.ColdPoints.CooldownRate = RechargeRate;
      }
   }

   private void OnBodyExited(Node2D body)
   {
      GD.Print($"OnBodyExited: isPlayer: {body is Player}`");
      if (body is Player player)
      {
         player.ColdPoints.CooldownRate = 0;
      }
   }
}
