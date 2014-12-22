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
    public class ModelBody
    {
        /// <summary>
        /// Строит часть модели.
        /// </summary>
        /// <param name="document3D">3D документ.</param>
        /// <param name="parameters">Параметры модели.</param>
        public void Create(ksDocument3D document3D, Dictionary<Parameter, ParameterData> parameters)
        {
            var bodyWidth = parameters[Parameter.BodyWidth].Value/2;
            var bodyHeight = parameters[Parameter.BodyHeight].Value;
            var bodyThickness = parameters[Parameter.BodyThickness].Value/2;
            var legHeight = parameters[Parameter.LegHeight].Value;
            var borderThickness = parameters[Parameter.BorderThickness].Value;

            var part = (ksPart)document3D.GetPart((short)Part_Type.pNew_Part);
            if (part != null)
            {
                var sketchProperty = new Sketch
                {
                    Shape = ShapeType.Line,
                    Plane = PlaneType.PlaneXOY,
                    NormalValue = bodyThickness,
                    ReverseValue = bodyThickness,
                    Operation = OperationType.BaseExtrusion,
                    DirectionType = Direction_Type.dtBoth,
                    OperationColor = Color.FloralWhite
                };

                sketchProperty.PointsList.Add(new PointF(-bodyWidth, legHeight + bodyHeight));
                sketchProperty.PointsList.Add(new PointF(bodyWidth, legHeight + bodyHeight));
                sketchProperty.PointsList.Add(new PointF(bodyWidth, legHeight));
                sketchProperty.PointsList.Add(new PointF(-bodyWidth, legHeight));

                sketchProperty.SketchName = "Монитор";
                sketchProperty.CreateNewSketch(part);

                sketchProperty.AddBreakPoint();
                sketchProperty.PointsList.Add(new PointF(-bodyWidth + borderThickness, legHeight + bodyHeight - borderThickness));
                sketchProperty.PointsList.Add(new PointF(bodyWidth - borderThickness, legHeight + bodyHeight - borderThickness));
                sketchProperty.PointsList.Add(new PointF(bodyWidth - borderThickness, legHeight + borderThickness));
                sketchProperty.PointsList.Add(new PointF(-bodyWidth + borderThickness, legHeight + borderThickness));

                sketchProperty.NormalValue += 0.2f;
                sketchProperty.SketchName = "Рамки";
                sketchProperty.CreateNewSketch(part);

                sketchProperty.OperationColor = Color.Black;
                sketchProperty.NormalValue -= 0.1f;
                sketchProperty.ClearBreakPoint();
                sketchProperty.PointsList.Clear();
                sketchProperty.PointsList.Add(new PointF(-bodyWidth + 0.1f, legHeight + bodyHeight - 0.1f));
                sketchProperty.PointsList.Add(new PointF(bodyWidth - 0.1f, legHeight + bodyHeight - 0.1f));
                sketchProperty.PointsList.Add(new PointF(bodyWidth - 0.1f, legHeight + 0.1f));
                sketchProperty.PointsList.Add(new PointF(-bodyWidth + 0.1f, legHeight + 0.1f));

                sketchProperty.SketchName = "Экран";
                sketchProperty.CreateNewSketch(part);

                if (borderThickness > 0.8)
                {
                    sketchProperty.PointsList.Clear();
                    sketchProperty.PointsList.Add(new PointF(bodyWidth - 0.8f, legHeight + 0.7f));
                    sketchProperty.PointsList.Add(new PointF(bodyWidth - 0.1f, legHeight + 0.7f));
                    sketchProperty.PointsList.Add(new PointF(bodyWidth - 0.1f, legHeight + 0.1f));
                    sketchProperty.PointsList.Add(new PointF(bodyWidth - 0.8f, legHeight + 0.1f));

                    sketchProperty.NormalValue += 0.15f;
                    sketchProperty.SketchName = "Кнопка включения";
                    sketchProperty.CreateNewSketch(part);
                }
            }
        }
    }
}