using Kompas6API5;
using System.Drawing;
using Kompas6Constants3D;
using System.Collections.Generic;
using MonitorCreator.Enumerations;

namespace MonitorCreator
{
    /// <summary>
    /// Свойства эскиза.
    /// </summary>
    public class Sketch
    {
        #region - Переменные -
        //TODO:
        /// <summary>
        /// Список разделителей.
        /// </summary>
        private List<int> _breakPointsList = new List<int>();

        #endregion // Переменные.

        #region - Конструктор -

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public Sketch()
        {
            Initialize();
        }

        #endregion // Конструктор.
        
        #region - Инициализация -

        /// <summary>
        /// Инициализирует переменные.
        /// </summary>
        private void Initialize()
        {
            SketchName = string.Empty;
            PointsList = new List<PointF>();

            _breakPointsList = new List<int>();
        }

        #endregion // Инициализация.
        
        #region - Свойства -

        /// <summary>
        /// Название эскиза.
        /// </summary>
        public string SketchName { get; set; }

        /// <summary>
        /// Значение свойства.
        /// </summary>
        public double NormalValue { get; set; }

        /// <summary>
        /// Значение свойства.
        /// </summary>
        public double ReverseValue { get; set; }

        /// <summary>
        /// Примитив.
        /// </summary>
        public ShapeType Shape { get; set; }

        /// <summary>
        /// Плоскость для рисования.
        /// </summary>
        public PlaneType Plane { get; set; }

        /// <summary>
        /// Цвет операции.
        /// </summary>
        public Color OperationColor { get; set; }

        /// <summary>
        /// Направление каманды.
        /// </summary>
        public Direction_Type DirectionType { get; set; }

        /// <summary>
        /// Список координат фигуры.
        /// </summary>
        public List<PointF> PointsList { get; set; }

        /// <summary>
        /// Список операций.
        /// </summary>
        public OperationType Operation { get; set; }

        #endregion // Свойства.

        #region - Public методы -

        /// <summary>
        /// Добавляет разделитель линий.
        /// </summary>
        public void AddBreakPoint()
        {
            _breakPointsList.Add(PointsList.Count - 1);
        }

        /// <summary>
        /// Очищает список разделителей линий.
        /// </summary>
        public void ClearBreakPoint()
        {
            _breakPointsList.Clear();
        }

        /// <summary>
        /// Создает новый эскиз.
        /// </summary>
        /// <param name="part">Новая деталь.</param>
        public void CreateNewSketch(ksPart part)
        {
            var entitySketch = (ksEntity)part.NewEntity((short)Obj3dType.o3d_sketch);

            if (entitySketch == null) return;

            // Интерфейс свойств эскиза.
            var sketchDef = (ksSketchDefinition)entitySketch.GetDefinition();
            if (sketchDef != null)
            {
                var plane = CreateDefaultPlane(part);

                // Установим плоскость базовой для эскиза.
                sketchDef.SetPlane(plane);

                // 
                if (!string.IsNullOrEmpty(SketchName))
                    entitySketch.name = SketchName;

                // Создадим эскиз.
                entitySketch.Create();

                // Интерфейс редактора эскиза.
                var sketchEdit = (ksDocument2D)sketchDef.BeginEdit();

                switch (Shape)
                {
                    case ShapeType.Line:
                        {
                            DrawLine(sketchEdit);
                        }
                        break;
                }

                // Завершение редактирования эскиза.
                sketchDef.EndEdit();

                switch (Operation)
                {
                    case OperationType.BaseExtrusion:
                        {
                            BaseExtrusion(part, entitySketch);
                        }
                        break;
                }
            }
        }

        #endregion // Public методы.

        #region - Операции 2D -

        /// <summary>
        /// Рисует линию.
        /// </summary>
        /// <param name="sketchEdit">Эскиз для рисования.</param>
        private void DrawLine(ksDocument2D sketchEdit)
        {
            if (PointsList.Count == 0) return;

            var pointList = new List<PointF>();

            for (int i = 0; i < PointsList.Count; i++)
            {
                if (_breakPointsList.Contains(i))
                {
                    pointList.Add(PointsList[i]);

                    DrawLine(sketchEdit, pointList);

                    pointList.Clear();

                    if (i != PointsList.Count - 1)
                    {
                        i++;
                    }
                }

                pointList.Add(PointsList[i]);
            }

            DrawLine(sketchEdit, pointList);
        }

        /// <summary>
        /// Рисует линию.
        /// </summary>
        /// <param name="sketchEdit">Эскиз для рисования.</param>
        /// <param name="pointsList">Координаты линии.</param>
        private void DrawLine(ksDocument2D sketchEdit, List<PointF> pointsList)
        {
            for (int i = 0; i < pointsList.Count - 1; i++)
            {
                sketchEdit.ksLineSeg(pointsList[i].X, pointsList[i].Y, pointsList[i + 1].X, pointsList[i + 1].Y, 1);
            }

            int index = pointsList.Count - 1;
            sketchEdit.ksLineSeg(pointsList[index].X, pointsList[index].Y, pointsList[0].X, pointsList[0].Y, 1);
        }

        #endregion // Операции 2D.

        #region - Операции 3D -

        /// <summary>
        /// Базовая операция выдавливания.
        /// </summary>
        /// <param name="part">Интерфейс детали.</param>
        /// <param name="entitySketch">Эскиз.</param>
        private void BaseExtrusion(ksPart part, ksEntity entitySketch)
        {
            // Построим выдавливанием.
            var entityExtr = (ksEntity)part.NewEntity((short)Obj3dType.o3d_baseExtrusion);
            if (entityExtr != null)
            {
                // Интерфейс свойств базовой операции выдавливания.
                var extrusionDef = (ksBaseExtrusionDefinition)entityExtr.GetDefinition();

                // Интерфейс базовой операции выдавливания.
                if (extrusionDef != null)
                {
                    // Направление выдавливания.
                    extrusionDef.directionType = (short)DirectionType;

                    // 1 - прямое направление,
                    // 2 - строго на глубину,
                    // 3 - расстояние.
                    switch (DirectionType)
                    {
                        case Direction_Type.dtNormal:
                            {
                                extrusionDef.SetSideParam(true, (short)End_Type.etBlind, NormalValue);
                            }
                            break;

                        case Direction_Type.dtReverse:
                            {
                                extrusionDef.SetSideParam(false, (short)End_Type.etBlind, ReverseValue);
                            }
                            break;

                        case Direction_Type.dtBoth:
                            {
                                extrusionDef.SetSideParam(true, (short)End_Type.etBlind, NormalValue);
                                extrusionDef.SetSideParam(false, (short)End_Type.etBlind, ReverseValue);
                            }
                            break;
                    }

                    var colorParam = (ksColorParam)entityExtr.ColorParam();

                    // Задаем цвет операции.
                    colorParam.color = GetKompasColor(OperationColor);

                    // Эскиз операции выдавливания.
                    extrusionDef.SetSketch(entitySketch);

                    // Создать операцию.
                    entityExtr.Create();

                    // Обновить параметры эскиза.
                    entitySketch.Update();

                    // Обновить параметры операции выдавливания.
                    entityExtr.Update();
                }
            }
        }
        
        #endregion // Операции 3D.

        #region - Private методы -

        /// <summary>
        /// Преобразует цвет модели в понятный для Компаса.
        /// </summary>
        /// <param name="color">Цвет модели.</param>
        /// <returns>Значение цвета.</returns>
        private int GetKompasColor(Color color)
        {
            return Color.FromArgb(color.B, color.G, color.R).ToArgb();
        }

        /// <summary>
        /// Возвращает текущую плоскость для рисования.
        /// </summary>
        private short ActivePlane
        {
            get
            {
                var plane = (short)Obj3dType.o3d_planeXOY;

                switch (Plane)
                {
                    case PlaneType.PlaneXOY:
                        plane = (short)Obj3dType.o3d_planeXOY;
                        break;

                    case PlaneType.PlaneXOZ:
                        plane = (short)Obj3dType.o3d_planeXOZ;
                        break;

                    case PlaneType.PlaneYOZ:
                        plane = (short)Obj3dType.o3d_planeYOZ;
                        break;
                }

                return plane;
            }
        }

        /// <summary>
        /// Создает плоскость.
        /// </summary>
        /// <param name="part">Деталь.</param>
        /// <returns>Плоскость.</returns>
        public ksEntity CreateDefaultPlane(ksPart part)
        {
            return (ksEntity)part.GetDefaultEntity(ActivePlane);
        }
        
        #endregion // Private методы.
    }
}