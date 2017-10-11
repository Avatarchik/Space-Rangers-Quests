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
        /// �� ��������� true, ���� �� ����� ��������� ��������
        /// </summary>
        public bool units;
        /// <summary>
        /// �������
        /// </summary>
        public bool percent;
        /// <summary>
        /// ���������� ��������
        /// </summary>
        public bool assign;
        /// <summary>
        /// ���������
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
                else // �.�. � ��������� ������� true �_�
                    return OperationType.OPERATION_CHANGE;
                throw new Exception();
            }
        }


        public BoolLengthString expressionString;
        /// <summary>
        /// ��� ���������
        /// </summary>
        public AcceptValue acceptValue;
        /// <summary>
        /// ��� ���������
        /// </summary>
        public ModValue modValue;
        public BoolLengthString unknowString;
    };
}