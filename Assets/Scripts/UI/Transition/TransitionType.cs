namespace PSEMO.UI
{
    [System.Flags]
    public enum TransitionType
    {
        None  = 0,
        Fade  = 1 << 0,
        Slide = 1 << 1,
        Scale = 1 << 2
    }
}