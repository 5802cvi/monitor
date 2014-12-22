using Kompas6API5;
using System.Drawing;
using Kompas6Constants3D;
using System.Collections.Generic;
using MonitorCreator.Enumerations;

namespace MonitorCreator.ModelParts
{
    /// <summary>
    /// Доска.
    /// </summary>
    public class Stand
    {
        /// <summary>
        /// Строит часть модели.
        /// </summary>
        /// <param name="document3D">3D документ.</param>
        /// <param name="parameters">Параметры модели.</param>
        public void Create(ksDocument3D document3D, Dictionary<Parameter, ParameterData> parameters)
        {
            var standWidth = parameters[Parameter.StandWidth].Value / 2;
            var standLength = parameters[Parameter.StandLength].Value / 2;
            var standHeight = parameters[Parameter.StandThickness].Value;

            var part = (ksPart)document3D.GetPart((short)Part_Type.pNew_Part);
            if (part != null)
            {
                var sketchProperty = new Sketch
                {
                    Shape = ShapeType.Line,
                    Plane = PlaneType.PlaneXOY,
                    NormalValue = standLength,
                    ReverseValue = standLength,
                    Operation = OperationType.BaseExtrusion,
                    DirectionType = Direction_Type.dtBoth,
                    OperationColor = Color.FloralWhite
                };

                sketchProperty.PointsList.Add(new PointF(-standWidth, standHeight));
                sketchProperty.PointsList.Add(new PointF(standWidth, standHeight));
                sketchProperty.PointsList.Add(new PointF(standWidth, 0));
                sketchProperty.PointsList.Add(new PointF(-standWidth, 0));

                sketchProperty.SketchName = "Ножка";
                sketchProperty.CreateNewSketch(part);
            }
        }
    }
}