using System;
using Godot;

[GlobalClass]
public partial class Attach : StaticBody2D, MountInterface
{
   private PackedScene BulletTemplate = GD.Load<PackedScene>("res://Templates/GunfireBullet.tscn");

   private Bullet SpawnBullet() => BulletTemplate.Instantiate<Bullet>();

   public Actor CurrentOwner { get; private set; }

   public Vector2 Offset => Vector2.Up * 10.0f;

   public Boolean IsFiring { get; private set; }

   public bool IsMounted => CurrentOwner != null;

   [Export]
   public Node2D Axys;

   [Export]
   public RayCast2D RayCast;

   [Export]
   public AnimationPlayer Animation;

   public void CannonFire()
   {

   }

   public void GunFire()
   {
      if (Animation.IsPlaying())
      {
         return;
      }

      Animation.Play("gun-fire");
   }

   public void TriggerCannonFire()
   {
   }

   public void TriggerGunFire()
   {
      Bullet bullet = SpawnBullet();
      bullet.Position = RayCast.GlobalPosition;
      bullet.Creator = this;
      bullet.Rotation = Axys.Rotation;
      bullet.Direction = Vector2.Right.Rotated(Axys.Rotation).Normalized();
      bullet.Speed = 1000;
      GetParent().AddChild(bullet);
      if (bullet.Direction == Vector2.Left)
      {
         bullet.FlipLeft();
      }
   }

   public void Mount(Actor target)
   {
      CurrentOwner = target;
      target.Mount = this;
      target.GlobalPosition = GlobalPosition + Offset;
   }

   public void Unmount()
   {
      CurrentOwner.Mount = null;
      CurrentOwner = null;
      Axys.Rotation = 0;
   }

   public void HandlerOwnerInput(InputEvent @event)
   {
      if (InputManager.IsAux1ActionJustPressed())
      {
         IsFiring = true;
      }

      if (InputManager.IsAux1ActionReleased())
      {
         IsFiring = false;
      }
   }

   public override void _Process(double delta)
   {
      if (IsFiring)
      {
         GunFire();
      }

      if (IsMounted)
      {
         float YValue = Mathf.Clamp(InputManager.GetDirection().Y, -1f, 1f) * (IsFiring ? 0.1f / 4 : 0.1f);
         Axys.Rotation = Mathf.Clamp(Axys.Rotation + YValue, -Mathf.Pi / 4f, Mathf.Pi / 4f);
      }
   }
}
