using System;
using Godot;

public partial class LVL00 : Node2D
{
   PackedScene mushroomScene = GD.Load<PackedScene>("res://Templates/Mushroom.tscn");

   public override void _UnhandledInput(InputEvent @event)
   {
      if (@event is InputEventMouseButton mouseButton && mouseButton.Pressed)
      {
         SpawnEnemy(mouseButton);
      }
   }

   private void SpawnEnemy(InputEventMouseButton mouseButton)
   {
      var enemy = mushroomScene.Instantiate<Minion>();
      
      enemy.Position = GetLocalMousePosition();
      enemy.PlayerPath = GetNode<Player>("Player").GetPath();
      enemy.IsFireAvailable = false;
      AddChild(enemy);
   }
}
