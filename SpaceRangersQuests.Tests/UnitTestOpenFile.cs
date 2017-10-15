using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using SpaceRangersQuests.Model;
using SpaceRangersQuests.Model.Entity;
using SpaceRangersQuests.Model.Player;
using SpaceRangersQuests.Model.Utils;
#if !NUnitTest
using System.Reflection.Metadata.Ecma335;
using Xunit;
using Xunit.Sdk;
#else
using NUnit.Framework;
#endif

namespace SpaceRangersQuests.Tests
{
#if NUnitTest
    [TestFixture]
#endif
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
                AssertEqual(streamPos, stream.Length);
            }
            return quest;
        }

#if !NUnitTest
        [Fact]
#else
        [Test]
#endif
        public void TestReadQuest()
        {
            var files = Directory.GetFiles(RootTestDirectory, "*.qm", SearchOption.AllDirectories);
            files.ForEach(v => ReadQuest(v));
        }
#if !NUnitTest
        [Fact]
#else
        [Test]
#endif
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
#if !NUnitTest
        [Fact]
#else
        [Test]
#endif
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



        /// <summary>
        /// Поиск использования Transition.units
        /// </summary>
#if !NUnitTest
        [Fact]
#else
        [Test]
#endif
        public void FileExpression()
        {
            var expression = new List<string>();
            var files = Directory.GetFiles(RootTestDirectory, "*.qm", SearchOption.AllDirectories);
            expression.AddRange(files.SelectMany(AllExpression));
            expression = expression.Distinct().ToList();
        }

        public IEnumerable<string> AllExpression(string filePath)
        {
            var quest = ReadQuest(filePath);
            var allExpressions = quest.Parameters
                .Where(v => v.starText.Length > 0)
                .Select(v => v.starText.Text)
                .ToList();
            allExpressions = allExpressions.Concat(quest.Locations
                .Where(v => v.expression.Length > 0)
                .Select(v => v.expression.Text))
                .ToList();
            allExpressions = allExpressions.Concat(quest.Locations
                    .SelectMany(v => v.modifiers)
                    .Where(v => v.expressionString.Length > 0)
                    .Select(v => v.expressionString.Text))
                .ToList();

            foreach (var expression in allExpressions)
            {
                yield return expression;
            }
        }

        private void AssertEqual(long value1, long value2)
        {
#if !NUnitTest
            Assert.Equal(value1, value2);
#else
            Assert.AreEqual(value1, value2);
#endif
        }
    }
}
