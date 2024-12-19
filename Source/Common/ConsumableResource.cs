using Godot;

[GlobalClass]
public partial class ConsumableResource : PickableResource
{
   [Export(PropertyHint.Enum, "Health, Mana, Heat")]
   public string Type = "Health";

   [Export]
   public float Value = 0.5f;
}
