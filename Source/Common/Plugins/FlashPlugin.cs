using Godot;

[GlobalClass]
public partial class FlashPlugin : Node
{
   // Preload the flash material (you can also expose it instead for flexibility)
    private static readonly Material FLASH_MATERIAL = GD.Load<Material>("res://Assets/Effects/white_flash_material.tres");

    // Export the sprite that will be flashed
    [Export] public CanvasItem Sprite { get; set; }

    // Export the flash duration
    [Export] public float FlashDuration { get; set; } = 0.2f;

    // Store the original sprite material
    private Material _originalSpriteMaterial;

    // Timer instance
    private Timer _timer;

    public override void _Ready()
    {
        // Initialize the timer and add it as a child of this node
        _timer = new Timer();
        AddChild(_timer);

        // Store the original sprite material
        if (Sprite != null)
        {
            _originalSpriteMaterial = Sprite.Material;
        }
    }

    // Function to trigger the flash effect
    public async void Flash()
    {
        if (Sprite == null) return;

        // Set the sprite's material to the flash material
        Sprite.Material = FLASH_MATERIAL;

        // Start the timer with the flash duration
        _timer.Start(FlashDuration);

        // Wait for the timer to timeout
        await ToSignal(_timer, Timer.SignalName.Timeout);

        // Reset the sprite's material to its original state
        Sprite.Material = _originalSpriteMaterial;
    }
}
