using System.Collections.Generic;
using System.Diagnostics;
using SpaceRangersQuests.Model.Utils;

namespace SpaceRangersQuests.Model.Entity
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class Parameter
    {
        public Parameter()
        {
            ranges = new List<Range>();
        }

        public UnknownValues UnknownValues = new UnknownValues();

        /// <summary>
        /// ����������� ��������, ������������
        /// </summary>
        public int min;
        /// <summary>
        /// ������������ ��������, ������������
        /// </summary>
        public int max;
        /// <summary>
        /// ������� ��������
        /// </summary>
        public int mid;
        /// <summary>
        /// ��� ���������
        /// </summary>
        public ParameterType parameterType;
        public int unknow1;
        public bool showOnZero;
        public bool minCritical;
        /// <summary>
        /// ������������ �� ���������
        /// </summary>
        public bool active;
        public byte rangeCount;
        public bool money;
        //public byte unknow2[3];
        public BoolLengthString name;
        public IList<Range> ranges;
        public BoolLengthString critText;
        // o_O Tekstom znachenie
        public BoolLengthString starText;

        private string DebuggerDisplay
        {
            get { return name.Text; }
        }
    };
}