using System;
using SpaceRangersQuests.Model.Utils;

namespace SpaceRangersQuests.Model.Entity
{
    public class Modifier
    {
        public Modifier()
        {
            UnknownValues = new UnknownValues();
        }

        public UnknownValues UnknownValues { get; private set; }

        public int rangeFrom;
        public int rangeTo;
        public int value;

        public VisibilityType visibility;

        /// <summary>
        /// По умолчанию true, если не стоят остальные значения
        /// </summary>
        public bool units;
        /// <summary>
        /// Процент
        /// </summary>
        public bool percent;
        /// <summary>
        /// Установить значение
        /// </summary>
        public bool assign;
        /// <summary>
        /// Выражение
        /// </summary>
        public bool expression;

        public OperationType Operation
        {
            get
            {
                if (percent)
                    return OperationType.OPERATION_PERCENT;
                else if (assign)
                    return OperationType.OPERATION_ASSIGN;
                else if (expression)
                    return OperationType.OPERATION_EXPRESSION;
                else if(units)
                    return OperationType.OPERATION_CHANGE;
                else // Т.к. в последних версиях true о_О
                    return OperationType.OPERATION_CHANGE;
                throw new Exception();
            }
        }


        public BoolLengthString expressionString;
        /// <summary>
        /// Для видимости
        /// </summary>
        public AcceptValue acceptValue;
        /// <summary>
        /// Для видимости
        /// </summary>
        public ModValue modValue;
        public BoolLengthString unknowString;
    };
}