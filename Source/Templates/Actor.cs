using System;
using System.Threading.Tasks;
using Godot;

public partial class Actor : CharacterBody2D
{
   private PackedScene BulletTemplate = GD.Load<PackedScene>("res://Templates/Bullet.tscn");

   public const float GRAVITY = 9.81f;

   public const float GOLDEN_RATIO = 1.61803398875f;

   [Export]
   public float Meter { get; set; } = 48.0f; // 1 meter = 16 pixels

   [Export]
   public float WalkingModifier { get; set; } = GOLDEN_RATIO; // 1.5 m/s

   [Export]
   public float RunningModifier { get; set; } = GOLDEN_RATIO * 2.5f; // 4.5 m/s

   [Export]
   public float JumpHeight { get; set; } = 20.0f; // 12 meters

   [Export]
   public int JumpMax { get; set; } = 2;

   [Export]
   public float FireCost { get; set; }

   [Export]
   public float DashCost { get; set; }

   [Export]
   public float KnockbackForce { get; set; } = 80.0f;

   [Export]
   public float KnockbackDuration { get; set; } = 1.0f;

   [Export]
   public Vector2 KnockbackDirection { get; set; } = Vector2.Zero;

   [Export]
   public float KnockbackTimer { get; set; } = 0.0f;

   [Export]
   public StatPlugin HealthPoints { get; set; }

   [Export]
   public StatPlugin SkillPoints { get; set; }

   [Export]
   public CollisionShape2D HitBox { get; set; }

   [Export]
   public Node2D Mount { get; set; }

   [Export]
   public Area2D HitArea { get; set; }

   [Export]
   public float GravityScale { get; set; } = 1.0f;

   [Export]
   public float LadderSpeedScale { get; set; } = 1.0f;

   [Export]
   public bool IsLadderDetected { get; set; } = false;

   [Export]
   public Node2D CurrentTarget { get; set; } = null;

   [Export]
   public AudioStreamPlayer2D fireSound { get; set; }

   [Export]
   public AudioStreamPlayer2D slashSound { get; set; }

   [Export]
   public AudioStreamPlayer2D landingSound { get; set; }

   public float Gravity { get { return GRAVITY * GravityScale; } }

   public Vector2 Direction { get; set; } = Vector2.Zero;

   public ActorState CurrentState { get; set; } = ActorState.Idle;

   public bool IsStopped { get; set; } = false;

   public Vector2 FaceDirection
   {
	  get
	  {
		 return sprite.FlipH ? Vector2.Left : Vector2.Right;
	  }
   }


   public float WalkingSpeed
   {
	  get { return Meter * WalkingModifier; }
   }

   public float RunningSpeed
   {
	  get { return Meter * RunningModifier; }
   }

   public float JumpSpeed
   {
	  get { return JumpHeight * Meter; }
   }

   public int JumpCount { get; set; } = 0;

   public float DashRefresh { get; set; } = -1.0f;

   public float AttackRefresh { get; set; } = -1.0f;

   private Vector2 AttackPosition { get; set; } = Vector2.Zero;

   public ActorState LastState { get; private set; }
   public bool IsVerticalMovementEnabled { get; set; }

   [Export]
   public AnimatedSprite2D sprite;

   [Export]
   public AnimatedSprite2D attackSprite;

   [Export]
   public CollisionShape2D shape;

   [Export]
   public PointLight2D light;

   [Export]
   public Marker2D hook;

   [Export]
   public Area2D Sensor;

   public virtual Vector2 GetInputDirection() => Vector2.Zero;

   private Bullet SpawnBullet() => BulletTemplate.Instantiate<Bullet>();

   internal Godot.Collections.Dictionary<string, Variant> defaultValuesBag = new Godot.Collections.Dictionary<string, Variant>();

   public override void _Ready()
   {
	  AttackPosition = attackSprite.Position;
	  SpawnBullet().QueueFree();
	  HealthPoints.ValueChanged += OnTakeDamage;
	  HitArea.BodyEntered += (body) =>
	  {
		 if (body is Actor actor && body != this)
		 {
			actor.Damage(15.0f, Position);
		 }
	  };
   }

   public void Jump()
   {
	  JumpCount++;
	  if (JumpCount >= JumpMax) { return; }
	  ToggleLadder(false);
	  Velocity = new Vector2(Velocity.X, -JumpSpeed);
   }

   public void Dash()
   {
	  if (DashRefresh > 0.0f || SkillPoints.CurrentValue < DashCost) { return; }
	  DashRefresh = 1.8f;
	  SkillPoints.Subtract(DashCost);
   }

   public void Attack()
   {
	  if (AttackRefresh > 0.0f || CurrentState.IsDashing()) { return; }
	  AttackRefresh = 1.0f;
	  slashSound.Play();
   }

   public void Knockback(Vector2 sourcePosition)
   {
	  Vector2 direction = (GlobalPosition - sourcePosition).Normalized();
	  KnockbackDirection = direction * KnockbackForce;
	  KnockbackTimer = KnockbackDuration;
   }

   public void VerticalAction()
   {
	  if (IsLadderDetected && CurrentTarget != null)
	  {
		 IsVerticalMovementEnabled = true;
		 GlobalPosition = new Vector2(CurrentTarget.GlobalPosition.X, Position.Y + Direction.Y);
	  }
   }

   public void Fire()
   {
	  if (SkillPoints.CurrentValue > FireCost)
	  {
		 Bullet bullet = SpawnBullet();
		 bullet.Direction = sprite.FlipH ? Vector2.Left : Vector2.Right;
		 bullet.Position = hook.GlobalPosition;
		 bullet.Creator = this;
		 GetParent().AddChild(bullet);
		 if (bullet.Direction == Vector2.Left)
		 {
			bullet.FlipLeft();
		 }
		 SkillPoints.Subtract(FireCost);
		 fireSound.Play();
	  }
   }
   public void Damage(float damage, Vector2 direction)
   {
	  Knockback(direction);
	  HealthPoints.Subtract(damage);
   }

   private void UpdateAnimation()
   {
	  sprite.FlipH = Direction != Vector2.Zero ? Direction.X < 0 : sprite.FlipH;

	  if (CurrentState.KeepAtFrame >= 0)
	  {
		 sprite.Stop();
		 sprite.Animation = CurrentState.Animation;
		 sprite.Frame = CurrentState.KeepAtFrame;
	  }
	  else
	  {
		 sprite.Play(CurrentState.Animation);
	  }

	  if (CurrentState.IsAttacking())
	  {
		 if (sprite.FlipH)
		 {
			attackSprite.Position = AttackPosition * new Vector2(-1, 1);
			attackSprite.FlipH = true;
		 }
		 else
		 {
			attackSprite.Position = AttackPosition * new Vector2(1, 1);
			attackSprite.FlipH = false;
		 }
		 attackSprite.Play();
	  }
	  else
	  {
		 attackSprite.Stop();
	  }
   }

   private void UpdateState()
   {
	  LastState = CurrentState;

	  if (IsOnFloor())
	  {
		 if (Direction == Vector2.Zero)
		 {
			CurrentState = ActorState.Idle;
		 }
		 else
		 {
			CurrentState = ActorState.Running;
		 }

		 // Reset jump count when on floor
		 JumpCount = 0;
	  }
	  else
	  {
		 if (Velocity.Y < 0)
		 {
			CurrentState = ActorState.Jumping;
		 }
		 else
		 {
			CurrentState = ActorState.Falling;
		 }
	  }

	  if (DashRefresh > 0.0f)
	  {
		 DashRefresh -= 0.1f;
		 CurrentState = ActorState.Dashing;
	  }

	  if (AttackRefresh > 0.0f)
	  {
		 CurrentState = ActorState.Attacking;
		 HitBox.Disabled = attackSprite.Frame > 3;
		 if (attackSprite.IsPlaying() && attackSprite.Frame > 4)
		 {
			AttackRefresh = -1.0f;
			HitBox.Disabled = true;
		 }
	  }

	  if (KnockbackTimer > 0.0f)
	  {
		 KnockbackTimer -= 0.1f;
	  }

	  if (IsVerticalMovementEnabled && IsOnFloor() && Direction.Y > 0 && CurrentTarget.GlobalPosition.Y < GlobalPosition.Y)
	  {
		 ToggleLadder(false);
	  }
   }

   public override void _PhysicsProcess(double delta)
   {
	  Direction = GetInputDirection();
	  PhysicsUpdate(delta);
	  UpdateState();
	  UpdateAnimation();
	  Velocity = GetNextVelocity(delta);
	  PlaySound();
	  MoveAndSlide();
   }

   private void PlaySound()
   {
	  if (LastState.IsFalling() && IsOnFloor() && !landingSound.IsPlaying())
	  {
		 landingSound.Play();
	  }
   }

   private void OnTakeDamage(float value)
   {
	  if (HealthPoints.IsEmpty)
	  {
		 QueueFree();
	  }
   }

   private Vector2 GetNextVelocity(double delta)
   {
	  if (IsStopped)
	  {
		 return Vector2.Zero;
	  }

	  Vector2 velocity = Velocity;

	  if (Direction == Vector2.Zero && KnockbackTimer <= 0.0f)
	  {
		 velocity = new Vector2(0, velocity.Y);
	  }
	  else
	  {
		 velocity = new Vector2(Direction.X * RunningSpeed, velocity.Y);
	  }

	  if (CurrentState.IsAttacking() && IsOnFloor())
	  {
		 velocity = new Vector2(Direction.X * (RunningSpeed / 2), velocity.Y);
	  }

	  if (CurrentState.IsDashing())
	  {
		 velocity = new Vector2(FaceDirection.X * RunningSpeed * 3.0f * DashRefresh, 0);
	  }

	  if (KnockbackTimer > 0.0f)
	  {
		 velocity = new Vector2(KnockbackDirection.X, velocity.Y);
	  }

	  if (!IsOnFloor())
	  {
		 velocity = new Vector2(velocity.X, velocity.Y + (Gravity * 1.2f));
	  }

	  if (IsVerticalMovementEnabled)
	  {
		 velocity = new Vector2(0, Direction.Y * RunningSpeed * LadderSpeedScale);
	  }

	  return velocity;
   }

   public void ToggleLadder(bool toggle, Node2D target = null)
   {
	  CurrentTarget = target;

	  if (toggle)
	  {
		 IsLadderDetected = true;
	  }
	  else
	  {
		 IsLadderDetected = false;
		 IsVerticalMovementEnabled = false;
	  }
   }

   public virtual void PhysicsUpdate(double delta)
   {
	  // Override this method to add custom physics
   }
}
