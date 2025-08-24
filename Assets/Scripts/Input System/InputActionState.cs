public class InputActionState
{
    public bool Pressed { get; set; }
    public bool Released { get; set; }
    public bool Holding { get; set; }

    public void ResetFrameStates()
    {
        Pressed = false;
        Released = false;
    }
}
