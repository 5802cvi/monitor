using System.Drawing;
using System.Collections.Generic;
using MonitorCreator.Enumerations;

namespace MonitorCreator
{
    /// <summary>
    /// Содержит параметры модели.
    /// </summary>
    public class ModelParameters
    {
        //TODO:
        /// <summary>
        /// Словарь параметров.
        /// </summary>
        public Dictionary<Parameter, ParameterData> Parameters { get; private set; }

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public ModelParameters()
        {
            Initialize();
        }

        /// <summary>
        /// Инициализирует переменные.
        /// </summary>
        private void Initialize()
        {
            Parameters = new Dictionary<Parameter, ParameterData>
                {
                    {Parameter.BodyWidth, new ParameterData(Parameter.BodyWidth.ToString(), 28, new PointF(10, 45))},
                    {Parameter.BodyHeight, new ParameterData(Parameter.BodyHeight.ToString(), 15, new PointF(10, 30))},
                    {
                        Parameter.BodyThickness,
                        new ParameterData(Parameter.BodyThickness.ToString(), 0.6f, new PointF(0.5f, 1.5f))
                    },
                    {Parameter.LegWidth, new ParameterData(Parameter.LegWidth.ToString(), 5, new PointF(2, 6))},
                    {Parameter.LegHeight, new ParameterData(Parameter.LegHeight.ToString(), 6, new PointF(2, 7))},
                    {
                        Parameter.LegThickness,
                        new ParameterData(Parameter.LegThickness.ToString(), 0.5f, new PointF(0.4f, 0.6f))
                    },
                    {Parameter.StandWidth, new ParameterData(Parameter.StandWidth.ToString(), 16, new PointF(6, 20))},
                    {Parameter.StandLength, new ParameterData(Parameter.StandLength.ToString(), 6, new PointF(4, 10))},
                    {
                        Parameter.StandThickness,
                        new ParameterData(Parameter.StandThickness.ToString(), 0.5f, new PointF(0.5f, 2))
                    },
                    {
                        Parameter.BorderThickness,
                        new ParameterData(Parameter.BorderThickness.ToString(), 0.8f, new PointF(0.2f, 2))
                    },
                };
        }

        /// <summary>
        /// Проверяет корректность введенных данных.
        /// </summary>
        /// <param name="parameters">Словарь параметров для проверки.</param>
        /// <returns>Список ошибок.</returns>
        public List<string> CheckData(Dictionary<Parameter, ParameterData> parameters)
        {
            var errorList = new List<string>();

            foreach (KeyValuePair<Parameter, ParameterData> parameter in parameters)
            {
                switch (parameter.Key)
                {
                    case Parameter.BodyWidth:
                        {
                            SetMaxValue(Parameter.LegWidth, parameter.Value.Value - 1);
                            SetMaxValue(Parameter.BorderThickness, parameter.Value.Value/2 - 0.5f);
                        }
                        break;

                    case Parameter.BodyThickness:
                        {
                            SetMaxValue(Parameter.LegThickness, parameter.Value.Value);
                        }
                        break;

                    case Parameter.LegWidth:
                        {
                            SetMinValue(Parameter.StandWidth, parameter.Value.Value + 1);
                        }
                        break;

                    case Parameter.LegHeight:
                        {
                            SetMaxValue(Parameter.StandThickness, parameter.Value.Value - 1);
                        }
                        break;

                    case Parameter.LegThickness:
                        {
                            SetMinValue(Parameter.StandLength, parameter.Value.Value);
                        }
                        break;
                }

                var value = parameter.Value.Value;
                var validValue = GetValidValue(parameter.Key);

                if (validValue == null) continue;

                if (!(value >= validValue.RangeValue.X && value <= validValue.RangeValue.Y))
                {
                    errorList.Add("Значение параметра '" + parameter.Value.Description +
                                  "', должно лежать в диапазоне от " + validValue.RangeValue.X + " до " +
                                  validValue.RangeValue.Y + ".\n");
                }

                if (value == 0)
                {
                    errorList.Add("Значение параметра '" + parameter.Value.Description +
                                  "',не должно быть равным 0\n");
                }
            }

            return errorList;
        }

        /// <summary>
        /// Возвращает допустимые значения.
        /// </summary>
        /// <param name="parameter">Параметр.</param>
        /// <returns>Допустимое значение.</returns>
        private ParameterData GetValidValue(Parameter parameter)
        {
            if (Parameters.ContainsKey(parameter))
            {
                return Parameters[parameter];
            }

            return null;
        }

        /// <summary>
        /// Задает новое максимальное значение параметра.
        /// </summary>
        /// <param name="parameter">Параметр.</param>
        /// <param name="maxValue">Новое значение.</param>
        private void SetMaxValue(Parameter parameter, float maxValue)
        {
            if (Parameters.ContainsKey(parameter))
            {
                var currentParameter = Parameters[parameter];
                Parameters[parameter] = new ParameterData(currentParameter.Name,
                                                          new PointF(currentParameter.RangeValue.X, maxValue));
            }
        }

        /// <summary>
        /// Задает новое минимальное значение параметра.
        /// </summary>
        /// <param name="parameter">Параметр.</param>
        /// <param name="minValue">Новое значение.</param>
        private void SetMinValue(Parameter parameter, float minValue)
        {
            if (Parameters.ContainsKey(parameter))
            {
                var currentParameter = Parameters[parameter];
                Parameters[parameter] = new ParameterData(currentParameter.Name,
                                                          new PointF(minValue, currentParameter.RangeValue.Y));
            }
        }

        /// <summary>
        /// Задает новый диапазон значений параметра.
        /// </summary>
        /// <param name="parameter">Параметр.</param>
        /// <param name="minValue">Минимальное значение.</param>
        /// <param name="maxValue">Максимальное значение.</param>
        private void SetRange(Parameter parameter, float minValue, float maxValue)
        {
            if (Parameters.ContainsKey(parameter))
            {
                var currentParameter = Parameters[parameter];
                Parameters[parameter] = new ParameterData(currentParameter.Name,
                                                          new PointF(minValue, maxValue));
            }
        }
    }
}