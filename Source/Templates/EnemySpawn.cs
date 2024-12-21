using Godot;
using System;

public partial class EnemySpawn : Node2D
{
   [Export] NodePath playerPath;

   [Export] float SpawnRate = 1.0f;
   
   [Export] int EnemyCount = 5;

   PackedScene mushroomScene = GD.Load<PackedScene>("res://Templates/Monster.tscn");

   Timer timer = new Timer();

   public override void _Ready()
   {
      AddChild(timer);
      timer.Timeout += () => SpawnEnemy();
      timer.WaitTime = SpawnRate;
      timer.OneShot = false;
      timer.Start();
   }

   private void SpawnEnemy()
   {
      if (GetChildren().Count >= EnemyCount)
      {
         return;
      }

      var enemy = mushroomScene.Instantiate<Minion>();

      enemy.GlobalPosition = GlobalPosition;
      enemy.PlayerPath = GetNode<Node2D>(playerPath).GetPath();
      enemy.IsFireAvailable = false;

      AddChild(enemy);

   }
}
