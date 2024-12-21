using System;

public struct ActorState
{
   public static readonly ActorState Idle = new ActorState { Animation = "default" };

   public static readonly ActorState Running = new ActorState { Animation = "run" };

   public static readonly ActorState Jumping = new ActorState { Animation = "jump", KeepAtFrame = 0 };

   public static readonly ActorState DoubleJumping = new ActorState { Animation = "djump", KeepAtFrame = 1 };

   public static readonly ActorState Falling = new ActorState { Animation = "jump", KeepAtFrame = 1 };

   public static readonly ActorState Attacking = new ActorState { Animation = "attack" };

   public static readonly ActorState Shooting = new ActorState { Animation = "attack" };

   public static readonly ActorState Dashing = new ActorState { Animation = "dash", KeepAtFrame = 1 };

   public static readonly ActorState Death = new ActorState { Animation = "death", KeepAtFrame = 5, PlayOnce = true };

   public string Animation = "idle";

   public int KeepAtFrame = -1;

   public bool PlayOnce = false;

   public ActorState() { }

   public bool IsDashing() => Animation == Dashing.Animation && KeepAtFrame == Dashing.KeepAtFrame;

   public bool IsAttacking() => Animation == Attacking.Animation;

   public bool IsShooting() => Animation == Shooting.Animation;

   public bool IsIdle() => Animation == Idle.Animation;

   public bool IsFalling() => Animation == Falling.Animation && KeepAtFrame == Falling.KeepAtFrame;

   public bool IsDeath() => Animation == Death.Animation;
}