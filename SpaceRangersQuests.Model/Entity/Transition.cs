using System.Collections.Generic;
using SpaceRangersQuests.Model.Utils;

namespace SpaceRangersQuests.Model.Entity
{
    public class Transition
    {
        public Transition()
        {
            modifiers = new List<Modifier>();
            UnknownValues = new UnknownValues();
        }

        public UnknownValues UnknownValues { get; private set; }

        public double priority;
        public int day;
        public int id;
        public int from;
        public int to;
        public byte unknow1;
        public bool alwaysVisible;
        public int passCount;
        /// <summary>
        /// Порядок паказа в переходе
        /// </summary>
        public int position;

        public IList<Modifier> modifiers { get; private set; }

        public BoolLengthString globalCondition;
        public BoolLengthString title;
        public BoolLengthString description;
    }
}