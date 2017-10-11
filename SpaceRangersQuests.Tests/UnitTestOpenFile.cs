using System;
using System.IO;
using System.Linq;
using SpaceRangersQuests.Model;
using SpaceRangersQuests.Model.Entity;
using SpaceRangersQuests.Model.Player;
using SpaceRangersQuests.Model.Utils;
using Xunit;

namespace SpaceRangersQuests.Tests
{
    public class UnitTestOpenFile
    {
        private static readonly string RootTestDirectory = UnitTestParameters.RootTestDirectory;

        public Quest ReadQuest(string fileName)
        {
            Quest quest = null;
            using (var stream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                quest = FileQuest.Load(stream);
                var streamPos = stream.Position;
                Assert.Equal(streamPos, stream.Length);
            }
            return quest;
        }

        [Fact]
        public void TestReadQuest()
        {
            var files = Directory.GetFiles(RootTestDirectory, "*.qm", SearchOption.AllDirectories);
            files.ForEach(v => ReadQuest(v));
        }

        [Fact]
        public void TestUnknowParameters()
        {
            var files = Directory.GetFiles(RootTestDirectory, "*.qm", SearchOption.AllDirectories);
            files.ForEach(v => UnknowParameters(v));
        }

        public void UnknowParameters(string filePath)
        {
            // Поиск неизвестных для попытки привязать
            var quest = ReadQuest(filePath);
            var paramsUnknow = quest.Parameters.Where(v => v.UnknownValues.GetValue<int>(0) != 0).ToList();
            Assert.True(paramsUnknow.Count == 0);

            paramsUnknow = quest.Parameters.Where(v => !v.UnknownValues.GetValue<byte[]>(1).All(b => b == 0)).ToList();
            Assert.True(paramsUnknow.Count == 0);

            var locationsUnknow = quest.Locations.Where(loc => loc.modifiers.Any(v => v.UnknownValues.GetValue<int>(0) != 0)).ToList();
            Assert.True(paramsUnknow.Count == 0);

        }

        /// <summary>
        /// Поиск использования Transition.units
        /// </summary>
        [Fact]
        public void TransitionsUnits()
        {
            var files = Directory.GetFiles(RootTestDirectory, "*.qm", SearchOption.AllDirectories);
            files.ForEach(v => TransitionsUnits(v));
        }

        /// <summary>
        /// Поиск использования Transition.units
        /// </summary>
        /// <param name="filePath">Имя файла</param>
        public void TransitionsUnits(string filePath)
        {
            var quest = ReadQuest(filePath);
            var units = quest.Transitions.Where(v => v.modifiers.Any(m => m.units)).ToList();
            if (units.Count != 0)
                throw new Exception();
        }
    }
}
