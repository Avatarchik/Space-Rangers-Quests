using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SpaceRangersQuests.Model;
using SpaceRangersQuests.Model.Entity;
using SpaceRangersQuests.Model.Player;
using Xunit;

namespace SpaceRangersQuests.Tests
{
    public class UnitTestPlay
    {
        private static readonly string RootTestDirectory = UnitTestParameters.RootTestDirectory;

        [Fact]
        public void TestPlay()
        {
            var filePath = RootTestDirectory + @"\s_ranger_q\Fishing3.qm";
            Quest quest = null;
            using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                quest = FileQuest.Load(stream);
                var streamPos = stream.Position;
                if (streamPos != stream.Length)
                    throw new Exception("Not all Read!");
            }

            var player = new Player();
            player.Play(quest);
        }
    }
}
