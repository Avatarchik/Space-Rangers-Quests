using System.Collections.Generic;
using SpaceRangersQuests.Model.Utils;

namespace SpaceRangersQuests.Model.Entity
{
    public class Quest
    {
        public Quest()
        {
            Parameters = new List<Parameter>();
            Locations = new List<Location>();
            Transitions = new List<Transition>();
            UnknownValues = new UnknownValues();
        }

        public UnknownValues UnknownValues { get; private set; }

        public Header header;

        public IList<Parameter> Parameters { get; private set; }

        public BoolLengthString toStar;
        public BoolLengthString parsec;
        public BoolLengthString artefact;
        public BoolLengthString toPlanet;
        public BoolLengthString date;
        public BoolLengthString money;
        public BoolLengthString fromPlanet;
        public BoolLengthString fromStar;
        public BoolLengthString ranger;

        public int locationCount;
        public int transitionCount;

        public BoolLengthString winnerText;
        public BoolLengthString descriptionText;
        public BoolLengthString unknownText;
        
        public IList<Location> Locations { get; private set; }
        public IList<Transition> Transitions { get; private set; }
    }
}