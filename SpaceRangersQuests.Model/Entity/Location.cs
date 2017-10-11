using System;
using System.Collections.Generic;
using SpaceRangersQuests.Model.Utils;

namespace SpaceRangersQuests.Model.Entity
{
    public class Location
    {
        public const int CountDescriptions = 10;

        public Location(int countParameters)
        {
            CountParameters = countParameters;
            modifiers = new List<Modifier>(countParameters);
            descriptions = new List<BoolLengthString>(CountDescriptions);
            UnknownValues = new UnknownValues();
        }

        public int CountParameters { get; private set; }

        public UnknownValues UnknownValues { get; private set; }

        public int day;
        public int x;
        public int y;
        public int id;

        public bool start;
        public bool success;
        public bool fail;
        public bool death;
        public bool empty;

        public LocationType LocationType
        {
            get
            {
                if (fail && death)
                    return LocationType.LOCATION_DEATH;
                if (fail)
                    return LocationType.LOCATION_FAIL;
                if (success)
                    return LocationType.LOCATION_SUCCESS;
                if (empty)
                    return LocationType.LOCATION_EMPTY;
                if (start)
                    return LocationType.LOCATION_START;

                if(start || empty || success || fail || death)
                    throw new Exception("LocationType");

                return LocationType.LOCATION_NORMAL;

                /*
                if ((start && success) || (start && fail) || (success && fail))
                    return LocationType.LOCATION_NORMAL;
                else if (!start && !success && !fail)
                    return LocationType.LOCATION_NORMAL;
                else if (start)
                    return LocationType.LOCATION_START;
                else if (success)
                    return LocationType.LOCATION_SUCCESS;
                else if (fail)
                    return LocationType.LOCATION_FAIL;
                else if (death)
                    return LocationType.LOCATION_FAIL;
                throw new Exception();
                */
            }
        }

        // [totalParamCount]
        public IList<Modifier> modifiers;
        // Описания локации 10 о_О
        public IList<BoolLengthString> descriptions { get; private set; }

        public byte descriptionExpression;
        public int unknow1;
        public BoolLengthString unknow2;
        public BoolLengthString unknow3;
        public BoolLengthString expression;

    };
}