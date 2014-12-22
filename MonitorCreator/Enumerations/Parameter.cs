using System.ComponentModel;

namespace MonitorCreator.Enumerations
{
    /// <summary>
    /// ��������� ������.
    /// </summary>
    public enum Parameter
    {
        [Description("������ ��������")]
        BodyWidth,

        [Description("������ ��������")]
        BodyHeight,

        [Description("������� ��������")]
        BodyThickness,

        [Description("������ �����")]
        LegWidth,

        [Description("������ �����")]
        LegHeight,

        [Description("������� �����")]
        LegThickness,

        [Description("������ ���������")]
        StandWidth,

        [Description("����� ���������")]
        StandLength,

        [Description("������� ���������")]
        StandThickness,

        [Description("������� �����")]
        BorderThickness,
    }
}