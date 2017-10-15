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
        /// <summary>
        /// „исло возможных переходов по этому пути
        /// 0 - безлимитно
        /// </summary>
        public int passCount;
        /// <summary>
        /// ѕор€док паказа в переходе
        /// 0-9
        /// </summary>
        public int position;

        public IList<Modifier> modifiers { get; private set; }

        public BoolLengthString globalCondition;
        public BoolLengthString title { get; set; }
        public BoolLengthString description;
    }
}