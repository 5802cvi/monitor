using System.ComponentModel;

namespace MonitorCreator.Enumerations
{
    /// <summary>
    /// Параметры модели.
    /// </summary>
    public enum Parameter
    {
        [Description("Ширина монитора")]
        BodyWidth,

        [Description("Высота монитора")]
        BodyHeight,

        [Description("Толщина монитора")]
        BodyThickness,

        [Description("Ширина ножки")]
        LegWidth,

        [Description("Высота ножки")]
        LegHeight,

        [Description("Толщина ножки")]
        LegThickness,

        [Description("Ширина подставки")]
        StandWidth,

        [Description("Длина подставки")]
        StandLength,

        [Description("Толщина подставки")]
        StandThickness,

        [Description("Толщина рамок")]
        BorderThickness,
    }
}