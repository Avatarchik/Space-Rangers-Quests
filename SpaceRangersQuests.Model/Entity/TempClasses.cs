using System;
using System.Diagnostics;

namespace SpaceRangersQuests.Model.Entity
{
    public enum TypeTurn : int
    {
        TurnUnlim = 1,
        TurnCount = 2
    }

    public enum TypeStruct : int
    {
        DefaultNames = 0,
        Parameters = 1,
    }

    [Flags]
    public enum Race : byte
    {
        RACE_MALOQ = 0x01,
        RACE_PELENG = 0x02,
        RACE_HUMAN = 0x04,
        RACE_FAEYAN = 0x08,
        RACE_GAAL = 0x10,
        RACE_NONE = 0x40,
        RACE_ANY = 0xFF
    }

    [Flags]
    public enum PlayerType : byte
    {
        PLAYER_TRADER = 0x01,
        PLAYER_PIRATE = 0x02,
        PLAYER_WARRIOR = 0x04,
        PLAYER_ANY = 0xFF
    }

    public enum HeaderSizeHorizontal : int
    {
        HeaderSizeHorizontal_VeryLarge = 0x0A,
        HeaderSizeHorizontal_Large = 0x0F,
        HeaderSizeHorizontal_Middle = 0x16,
        HeaderSizeHorizontal_Small = 0x1E
    }

    public enum HeaderSizeVertical : int
    {
        HeaderSizeVertical_VeryLarge = 0x08,
        HeaderSizeVertical_Large = 0x0C,
        HeaderSizeVertical_Middle = 0x12,
        HeaderSizeVertical_Small = 0x18
    }


    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class Range
    {
        /// <summary>
        /// От
        /// </summary>
        public int From;
        /// <summary>
        /// До
        /// </summary>
        public int To;
        public BoolLengthString Text;

        private string DebuggerDisplay
        {
            get { return $"[{From}, {To}] {Text.Text}"; }
        }
    };

    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class BoolLengthString
    {
        public bool Use;
        public int Length;
        public string Text;

        private string DebuggerDisplay
        {
            get { return Use ? $"{Text}" : "{dont use}"; }
        }
    }

    /// <summary>
    /// Тип принимаем/не прием значений
    /// </summary>
    public enum AcceptValueType : byte
    {
        /// <summary>
        /// Не принимаемые
        /// </summary>
        Unaccept = 0,
        /// <summary>
        /// Принимаемые
        /// </summary>
        Accept = 1,
    }
}
