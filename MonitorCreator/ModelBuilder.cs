using System.Windows.Forms;
using Kompas6API5;
using System.Collections.Generic;
using Kompas6Constants3D;
using MonitorCreator.Enumerations;
using MonitorCreator.ModelParts;
using MonitorCreator.Properties;
using System;
using System.Diagnostics;

namespace MonitorCreator
{
    /// <summary>
    /// Содержит методы для построения детали (модели).
    /// </summary>
    public class ModelBuilder
    {
        /// <summary>
        /// Интерфейс объекта КОМПАС.
        /// </summary>
        private readonly KompasObject _kompas;

        /// <summary>
        /// Конструктор с параметром.
        /// </summary>
        /// <param name="kompas">Интерфейс объекта КОМПАС.</param>
        public ModelBuilder(KompasObject kompas)
        {
            _kompas = kompas;
        }

        /// <summary>
        /// Строит модель.
        /// </summary>
        /// <param name="parameters">Параметры модели.</param>
        public void Build(Dictionary<Parameter, ParameterData> parameters)
        {
            //TimeSpan _tSpan;
            //for (int i = 0; i < 10; i++)
            //{
            //var _stopwatch = new Stopwatch();
            //_stopwatch.Start();

            var document3D = (ksDocument3D) _kompas.ActiveDocument3D();

            if (document3D == null)
            {
                var result = MessageBox.Show(Resources.CreateNewDocumentText, Resources.MainWindowTitle,
                                             MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    document3D = (ksDocument3D) _kompas.Document3D();
                    document3D.Create();
                }
                else return;
            }
            else
            {
                var result = MessageBox.Show(Resources.CreateInCurrentDocumentText, Resources.MainWindowTitle,
                                             MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (result == DialogResult.No)
                {
                    document3D = (ksDocument3D) _kompas.Document3D();
                    document3D.Create();
                }

                if (result == DialogResult.Cancel) return;
            }

            ClearDocument(document3D);
            HideAllGeom(document3D);
            SetViewProjection(document3D, 7);

            new ModelBody().Create(document3D, parameters);
            new Leg().Create(document3D, parameters);
            new Stand().Create(document3D, parameters);

            //_stopwatch.Stop();
            //_tSpan = _stopwatch.Elapsed;

            //MessageBox.Show("Время построения: " + _tSpan.TotalSeconds.ToString());
            //}
        }

        /// <summary>
        /// Очищает 3D документ.
        /// </summary>
        private void ClearDocument(ksDocument3D document3D)
        {
            var part = (ksPart)document3D.GetPart((short)Part_Type.pTop_Part);
            var entityCollection = (ksEntityCollection)part.EntityCollection(0);

            for (int i = entityCollection.GetCount() - 1; i > 0; i--)
            {
                var entity = (ksEntity)entityCollection.GetByIndex(i);
                document3D.DeleteObject(entity);
            }
        } 

        /// <summary>
        /// Скрывает все оси и геометрические обозначения.
        /// </summary>
        /// <param name="document3D">3D документ.</param>
        private void HideAllGeom(ksDocument3D document3D)
        {
            if (document3D == null) return;

            document3D.hideAllAuxiliaryGeom = true;
            document3D.hideAllSketches = false;
        }

        /// <summary>
        /// Задает ориентацию.
        /// </summary>
        /// <param name="document3D">3D документ.</param>
        /// <param name="index">Индекс.</param>
        private void SetViewProjection(ksDocument3D document3D, int index)
        {
            if (document3D == null) return;

            //TODO:
            var projectionCollection = document3D.GetViewProjectionCollection() as ksViewProjectionCollection;

            if (projectionCollection == null) return;
            var projection = projectionCollection.Next() as ksViewProjection;

            while (projection != null)
            {
                if (projection.index == index)
                {
                    projection.SetCurrent();
                    break;
                }

                projection = projectionCollection.Next();
            }
        }
    }
}