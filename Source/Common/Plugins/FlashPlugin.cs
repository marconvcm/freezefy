using Godot;

[GlobalClass]
public partial class FlashPlugin : Node
{
 
    [Export] public Material FlashMaterial;

    [Export] public CanvasItem Sprite { get; set; }

    // Export the flash duration
    [Export] public float FlashDuration { get; set; } = 0.2f;

    [Export] public bool IsFlashing { get; set; } = false;

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
        Sprite.Material = FlashMaterial;

        // Start the timer with the flash duration
        _timer.Start(FlashDuration);
        IsFlashing = true;

        // Wait for the timer to timeout
        await ToSignal(_timer, Timer.SignalName.Timeout);
        IsFlashing = false;

        // Reset the sprite's material to its original state
        Sprite.Material = _originalSpriteMaterial;
    }
}
