using Godot;
using System;

public partial class Door : Node2D
{
   [Export]
   public Binary BinarySwitch { get; set; }

   [Export]
   public Sprite2D ON { get; set; }

   [Export]
   public Sprite2D OFF { get; set; }

   [Export]
   public StaticBody2D Body { get; set; }

   public override void _Ready()
   {
      BinarySwitch.OnBinaryValueChanged += (value) =>
      {
         if (value)
         {
            Open();
         }
         else
         {
            Close();
         }
      };
   }

   public void Open()
   {
      ON.Visible = true;
      OFF.Visible = false;
      Body.GetNode<CollisionShape2D>("Shape").Disabled = false;
   }

   public void Close()
   {
      ON.Visible = false;
      OFF.Visible = true;
      Body.GetNode<CollisionShape2D>("Shape").Disabled = true;
   }
}
