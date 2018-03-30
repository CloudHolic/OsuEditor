using System.ComponentModel;

namespace OsuEditor.Models
{
    public enum PlayMode
    {
        [Description("Osu!")]
        Std,
        [Description("Taiko")]
        Taiko,
        [Description("Catch the Beat")]
        Ctb,
        [Description("Osu! Mania")]
        Mania
    }
}
