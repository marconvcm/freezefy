using Godot;

public partial class Pickable : Node2D
{
    [Export]
    public Sprite2D sprite;

    [Export]
    public Area2D area;

    private Vector2 startPosition;
    private Vector2 endPosition;
    private float elapsedTime = 0.0f;
    private float duration = 1.0f; // Time for one cycle of floating

    public override void _Ready()
    {
        startPosition = sprite.Position;
        endPosition = new Vector2(sprite.Position.X, sprite.Position.Y - 5);
    }

    public override void _PhysicsProcess(double delta)
    {
        elapsedTime += (float)delta;

        // Calculate the interpolation factor using PingPong for oscillation
        float t = Mathf.PingPong(elapsedTime / duration, 1.0f);

        // Interpolate the position to create the floating effect
        sprite.Position = startPosition.Lerp(endPosition, t);
    }
}
