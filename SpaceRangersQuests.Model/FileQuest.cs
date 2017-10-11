using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SpaceRangersQuests.Model.Entity;
using SpaceRangersQuests.Model.Utils;

namespace SpaceRangersQuests.Model
{
    public static class FileQuest
    {
        public static Quest Load(Stream stream)
        {
            var quest = new Quest();

            quest.header = LoadHeader(stream);

            LoadParameters(stream, quest.header.CountParameters)
                .ForEach(v => quest.Parameters.Add(v));

            quest.toStar = LoadBoolLengthString(stream);
            quest.parsec = LoadBoolLengthString(stream);
            quest.artefact = LoadBoolLengthString(stream);
            quest.toPlanet = LoadBoolLengthString(stream);
            quest.date = LoadBoolLengthString(stream);
            quest.money = LoadBoolLengthString(stream);
            quest.fromPlanet = LoadBoolLengthString(stream);
            quest.fromStar = LoadBoolLengthString(stream);
            quest.ranger = LoadBoolLengthString(stream);

            quest.locationCount = stream.ReadInt32();
            quest.transitionCount = stream.ReadInt32();

            quest.winnerText = LoadBoolLengthString(stream);
            quest.descriptionText = LoadBoolLengthString(stream);
            quest.unknownText = LoadBoolLengthString(stream);

            quest.UnknownValues.Add(quest.unknownText);

            LoadLocations(stream, quest.header.CountParameters, quest.locationCount)
                .ForEach(v => quest.Locations.Add(v));
            LoadTransitions(stream, quest.header.CountParameters, quest.transitionCount)
                .ForEach(v => quest.Transitions.Add(v));
            return quest;
        }

        private static Header LoadHeader(Stream stream)
        {
            var header = new Header();

            header.Magic = stream.ReadInt32();

            if (header.Version == -1)
                throw new NotSupportedException($"Version: {header.Version}. Magic: {header.Magic}.");

            header.UnknownValues.Add(stream.ReadInt32());
            header.race = (Race)stream.ReadByte();
            header.doneImmediately = ReadBool(stream);
            header.UnknownValues.Add(stream.ReadInt32());
            header.planetRaces = (Race)stream.ReadByte();
            header.UnknownValues.Add(stream.ReadInt32());
            header.playerTypes = (PlayerType)stream.ReadByte();
            header.UnknownValues.Add(stream.ReadInt32());
            header.playerRaces = (Race)stream.ReadByte();
            header.relation = stream.ReadInt32();
            header.pixelWidth = stream.ReadInt32();
            header.pixelHeight = stream.ReadInt32();
            header.sizeHorizontal = (HeaderSizeHorizontal)stream.ReadInt32();
            header.sizeVertical = (HeaderSizeVertical)stream.ReadInt32();
            header.UnknownValues.Add(stream.ReadInt32());
            header.TransactionCount = stream.ReadInt32();
            header.difficulty = stream.ReadInt32();
            return header;
        }

        private static IEnumerable<Parameter> LoadParameters(Stream stream, int countParameters)
        {
            return Enumerable.Range(0, countParameters).Select(v => LoadParameter(stream));
        }

        private static Parameter LoadParameter(Stream stream)
        {
            var parameter = new Parameter();
            parameter.min = stream.ReadInt32();
            parameter.max = stream.ReadInt32();
            parameter.mid = stream.ReadInt32();
            parameter.parameterType = (ParameterType)stream.ReadByte();
            parameter.UnknownValues.Add(stream.ReadInt32());
            parameter.showOnZero = ReadBool(stream);
            parameter.minCritical = ReadBool(stream);
            parameter.active = ReadBool(stream);
            parameter.rangeCount = (byte)stream.ReadByte();
            parameter.UnknownValues.Add(Enumerable.Range(0, 3).Select(v => (byte)stream.ReadByte()).ToArray());
            parameter.money = ReadBool(stream);
            parameter.name = LoadBoolLengthString(stream);

            Enumerable.Range(0, parameter.rangeCount).ForEach(v => parameter.ranges.Add(LoadRange(stream)));

            parameter.critText = LoadBoolLengthString(stream);
            parameter.starText = LoadBoolLengthString(stream);

            return parameter;
        }

        private static BoolLengthString LoadBoolLengthString(Stream stream)
        {
            var boolLengthString = new BoolLengthString();

            boolLengthString.Use = stream.ReadInt32() != 0;

            if (!boolLengthString.Use)
                return boolLengthString;

            boolLengthString.Length = stream.ReadInt32();
            var buffer = new byte[boolLengthString.Length * 2];
            stream.Read(buffer, 0, buffer.Length);
            boolLengthString.Text = Encoding.Unicode.GetString(buffer, 0, buffer.Length);

            return boolLengthString;
        }

        private static Range LoadRange(Stream stream)
        {
            var range = new Range();

            range.From = stream.ReadInt32();
            range.To = stream.ReadInt32();
            range.Text = LoadBoolLengthString(stream);

            return range;
        }

        private static IEnumerable<Location> LoadLocations(Stream stream, int countParameters, int locationCount)
        {
            return Enumerable.Range(0, locationCount).Select(v => LoadLocation(stream, countParameters));
        }

        private static Location LoadLocation(Stream stream, int countParameters)
        {
            var location = new Location(countParameters);

            location.day = stream.ReadInt32();
            location.x = stream.ReadInt32();
            location.y = stream.ReadInt32();
            location.id = stream.ReadInt32();

            location.start = ReadBool(stream);
            location.success = ReadBool(stream);
            location.fail = ReadBool(stream);
            location.death = ReadBool(stream);
            location.empty = ReadBool(stream);

            LoadMofidiers(stream, countParameters)
                .ForEach(v => location.modifiers.Add(v));

            Enumerable.Range(0, Location.CountDescriptions)
                .ForEach(v => location.descriptions.Add(LoadBoolLengthString(stream)));

            location.descriptionExpression = (byte)stream.ReadByte();
            location.UnknownValues.Add(stream.ReadInt32());
            location.unknow2 = LoadBoolLengthString(stream);
            location.UnknownValues.Add(location.unknow2);
            location.unknow3 = LoadBoolLengthString(stream);
            location.UnknownValues.Add(location.unknow3);
            location.expression = LoadBoolLengthString(stream);

            return location;
        }

        private static IEnumerable<Modifier> LoadMofidiers(Stream stream, int countParameters)
        {
            return Enumerable.Range(0, countParameters).Select(v => LoadMofidier(stream));
        }

        private static Modifier LoadMofidier(Stream stream)
        {
            var modifier = new Modifier();

            modifier.UnknownValues.Add(stream.ReadInt32());
            modifier.rangeFrom = stream.ReadInt32();
            modifier.rangeTo = stream.ReadInt32();
            modifier.value = stream.ReadInt32();
            modifier.visibility = (VisibilityType)stream.ReadInt32();
            modifier.units = ReadBool(stream);
            modifier.percent = ReadBool(stream);
            modifier.assign = ReadBool(stream);
            modifier.expression = ReadBool(stream);

            modifier.expressionString = LoadBoolLengthString(stream);
            modifier.acceptValue = LoadAcceptValue(stream);
            modifier.modValue = LoadModValue(stream);

            modifier.unknowString = LoadBoolLengthString(stream);
            modifier.UnknownValues.Add(modifier.unknowString);

            return modifier;
        }

        private static ModValue LoadModValue(Stream stream)
        {
            var modValue = new ModValue();

            modValue.CountModValues = stream.ReadInt32();
            modValue.ModType = (ModValueType)stream.ReadByte();

            Enumerable.Range(0, modValue.CountModValues)
                .ForEach(v => modValue.ModValues.Add(stream.ReadInt32()));

            return modValue;
        }

        private static AcceptValue LoadAcceptValue(Stream stream)
        {
            var acceptValue = new AcceptValue();

            acceptValue.CountAcceptValues = stream.ReadInt32();
            acceptValue.AcceptType = (AcceptValueType)stream.ReadByte();

            Enumerable.Range(0, acceptValue.CountAcceptValues)
                .ForEach(v => acceptValue.AcceptValues.Add(stream.ReadInt32()));

            return acceptValue;
        }

        private static IEnumerable<Transition> LoadTransitions(Stream stream, int countParameters, int transitionCount)
        {
            return Enumerable.Range(0, transitionCount).Select(v => LoadTransition(stream, countParameters));
        }

        private static Transition LoadTransition(Stream stream, int countParameters)
        {
            var transition = new Transition();

            transition.priority = stream.ReadDouble();
            transition.day = stream.ReadInt32();
            transition.id = stream.ReadInt32();
            transition.from = stream.ReadInt32();
            transition.to = stream.ReadInt32();

            transition.UnknownValues.Add(stream.ReadByte());
            transition.alwaysVisible = ReadBool(stream);
            transition.passCount = stream.ReadInt32();
            transition.position = stream.ReadInt32();

            LoadMofidiers(stream, countParameters)
                .ForEach(v => transition.modifiers.Add(v));

            transition.globalCondition = LoadBoolLengthString(stream);
            transition.title = LoadBoolLengthString(stream);
            transition.description = LoadBoolLengthString(stream);

            return transition;
        }

        private static bool ReadBool(Stream stream, bool validate = true)
        {
            var byteValue = stream.ReadByte();
            if (byteValue != 0 && byteValue != 1)
                throw new Exception($"byteValue: {byteValue}");
            return byteValue != 0;
        }
    }
}
