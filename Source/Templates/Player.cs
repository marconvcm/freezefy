using Godot;
using System;

public partial class Player : Actor
{
   [Export]
   public Marker2D topLeftLimit;

   [Export]
   public Marker2D bottomRightLimit;

   [Export]
   public TweenPlugin HintTweenPlugin;

   [Export]
   public TextFragment textFragment;

   [Export]
   public Camera2D camera;

   [Export]
   public StatPlugin ColdPoints;

   [Export]
   public AudioStreamPlayer2D pickupSound;

   [Export]
   public FlashPlugin HealthFlashPlugin;

   [Export]
   public TextureRect FrostOverlay;

   private float lastDamageTime = 0f;
   private const float DAMAGE_INTERVAL = 1.0f; // 1 second interval
   private bool isFullyFrozen = false;
   private const float FROST_DAMAGE = 0.01f; // 1% damage per tick

   public override Vector2 GetInputDirection() => IsStopped ? Vector2.Zero : InputManager.GetDirection();

   public override void _Ready()
   {
	  base._Ready();
	  camera.LimitLeft = (int)topLeftLimit.Position.X;
	  camera.LimitTop = (int)topLeftLimit.Position.Y;
	  camera.LimitRight = (int)bottomRightLimit.Position.X;
	  camera.LimitBottom = (int)bottomRightLimit.Position.Y;

	  // Simple frost overlay setup
	  FrostOverlay.Modulate = new Color(1, 1, 1, 0);
	  FrostOverlay.SetAnchorsPreset(Control.LayoutPreset.FullRect, true);  // true to keep margins
	  FrostOverlay.ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize;
	  FrostOverlay.StretchMode = TextureRect.StretchModeEnum.KeepAspectCovered;
	  
	  ColdPoints.ValueChanged += UpdateFrostEffect;
	  GD.Print("Frost overlay setup complete");

	  Sensor.BodyEntered += (body) =>
	  {
		 if (body is Attach artillery)
		 {
			HintTweenPlugin.Play();
			this.Mount = artillery;
		 }
	  };

	  Sensor.BodyExited += (body) =>
	  {
		 if (body is Attach artillery)
		 {
			HintTweenPlugin.Stop();
			this.Mount = null;
		 }
	  };

	  Sensor.AreaEntered += (area) =>
	  {

	  };

	  Sensor.AreaExited += (area) =>
	  {

	  };
   }

   private void UpdateFrostEffect(float coldValue)
   {
	  float alpha = Mathf.Clamp(coldValue / ColdPoints.MaxValue, 0, 1);
	  FrostOverlay.Modulate = new Color(1, 1, 1, alpha);
	  isFullyFrozen = alpha >= 1.0f;
	  GD.Print($"Frost alpha: {alpha}, Is fully frozen: {isFullyFrozen}, Health: {HealthPoints.CurrentValue}");
   }

   public override void _Input(InputEvent @event)
   {
	  if (Mount is MountInterface mount && mount.IsMounted)
	  {
		 if (InputManager.IsAux2ActionJustPressed())
		 {
			mount.Unmount();
			this.OnUnmount();
		 }
		 else
		 {
			mount.HandlerOwnerInput(@event);
		 }
	  }
	  else
	  {
		 if (InputManager.IsAux2ActionJustPressed())
		 {
			if (Mount is Attach artillery)
			{
			   artillery.Mount(this);
			   this.OnMount();
			}
		 }

		 if (InputManager.IsSecondaryActionJustPressed())
		 {
			Fire();
		 }

		 if (InputManager.IsMainActionJustPressed())
		 {
			Jump();
		 }

		 if (InputManager.IsRightModifierActionJustPressed())
		 {
			Dash();
		 }

		 if (InputManager.IsAux1ActionJustPressed())
		 {
			Attack();
		 }
	  }
   }

   public override void PhysicsUpdate(double delta)
   {
	  if (Direction.Y != 0)
	  {
		 VerticalAction();
	  }
   }

   public override void _Process(double delta)
   {
	  if (isFullyFrozen)
	  {
		 lastDamageTime += (float)delta;
		 if (lastDamageTime >= DAMAGE_INTERVAL)
		 {
			HealthPoints.Apply(-FROST_DAMAGE);
			GD.Print($"Applied frost damage. Health: {HealthPoints.CurrentValue}");
			lastDamageTime = 0f;
		 }
	  }
	  else
	  {
		 lastDamageTime = 0f;  // Reset timer when not fully frozen
	  }
   }

   private void OnMount()
   {
	  defaultValuesBag["Offset"] = this.camera.Offset;
	  this.camera.Offset = new Vector2(100f, 0);
	  IsStopped = true;
   }

   private void OnUnmount()
   {
	  this.camera.Offset = (Vector2)defaultValuesBag["Offset"];
	  IsStopped = false;
   }

   public void Collect(PickableResource pickableResource)
   {
	  if (pickableResource is ConsumableResource consumableResource)
	  {
		 switch (consumableResource.Type)
		 {
			case "Health":
			   ApplyHealth(consumableResource);
			   break;
			case "Heat":
			   ApplyHeat(consumableResource);
			   break;
		 }
		 pickupSound.Play();
		 HealthFlashPlugin.Flash();
	  }
   }

   private void ApplyHealth(ConsumableResource consumableResource)
   {
	  HealthPoints.Apply(consumableResource.Value);
   }

   private void ApplyHeat(ConsumableResource consumableResource)
   {
	  ColdPoints.Apply(-consumableResource.Value);
   }
}
