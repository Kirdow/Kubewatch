
namespace Kubewatch.Enums
{
    public enum EFlipMode
    {
        None,
        Orientation,
        Color
    }

    public static class EFlipModeExt
    {
        public static string GetDisplayString(this EFlipMode mode)
        {
            switch (mode)
            {
            case EFlipMode.Orientation: return "Orientation";
            case EFlipMode.Color: return "Color";
            default: return "Not";
            }
        }
    }
}