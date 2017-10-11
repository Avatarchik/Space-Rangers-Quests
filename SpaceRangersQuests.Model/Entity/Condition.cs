using System;
using System.Collections.Generic;

namespace SpaceRangersQuests.Model.Entity
{
    [Obsolete("", true)]
    public class Condition
    {
        public Condition()
        {
            values = new List<int>();
            multiples = new List<uint>();
        }

        public UInt32 param;

        public Int32 rangeFrom;
        public Int32 rangeTo;

        public bool includeValues;
        public bool includeMultiples;
        public IList<Int32> values { get; private set; }
        public IList<UInt32> multiples { get; private set; }
    };
}