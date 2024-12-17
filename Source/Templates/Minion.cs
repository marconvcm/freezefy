using Godot;

public partial class Minion : Actor
{
   [Export]
   public NodePath PlayerPath { get; set; }

   private Player Player;

   [Export]
   public bool IsFireAvailable = true;

   [Export]
   public bool IsMeleeAvailable = true;

   [Export]
   public bool IsJumpAvailable = true;

   [Export]
   public float AttackRange = 30.0f;
   [Export]
   public float FireRange = 200.0f;

   public override void _Ready()
   {
      base._Ready();
      Player = GetNode<Player>(PlayerPath);
   }

   public override Vector2 GetInputDirection()
   {
      if (Player != null && Player.IsAlive)
      {

         Vector2 directionToPlayer = Player.GlobalPosition - GlobalPosition;
         if (directionToPlayer.Length() <= AttackRange)
         {
            return new Vector2(-Mathf.Sign(directionToPlayer.X), 0);
         }
         return new Vector2(Mathf.Sign(directionToPlayer.X), 0);
      }

      return Vector2.Zero;
   }


   public override void _PhysicsProcess(double delta)
   {
      base._PhysicsProcess(delta);

      if (Player == null || !Player.IsAlive)
      {
         return;
      }

      Vector2 directionToPlayer = Player.GlobalPosition - GlobalPosition;

      if (IsOnWall() && IsJumpAvailable)
      {
         Jump();
      }

      if (directionToPlayer.Length() <= AttackRange && IsOnFloor() && IsMeleeAvailable)
      {
         Attack();
      }
      else if (directionToPlayer.Length() > FireRange && directionToPlayer.Length() > AttackRange && IsFireAvailable)
      {
         Fire();
      }
   }
}
