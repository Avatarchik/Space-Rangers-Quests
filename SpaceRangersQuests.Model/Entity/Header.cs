using SpaceRangersQuests.Model.Utils;

namespace SpaceRangersQuests.Model.Entity
{
    public class Header
    {
        public Header()
        {
            UnknownValues = new UnknownValues();
        }

        public int CountParameters
        {
            get
            {
                if (Magic == 0x423a35d4)
                    return 96;
                if (Magic == 0x423a35d3)
                    return 48;
                if (Magic == 0x423a35d2)
                    return 24;
                return -1;
            }
        }

        public int Version
        {
            get
            {
                var version = Magic ^ 0x423a35d0;
                return (CountParameters < 0 || version > 4)
                    ? -1 
                    : version;
            }
        }

        public UnknownValues UnknownValues { get;private set; }

        public int Magic { get; set; }
        /// <summary>
        /// Раса дающая квест
        /// </summary>
        public Race race;

        /// <summary>
        /// Закончить после завершения квеста
        /// (не возвращаться на планету квестодателя)
        /// </summary>
        public bool doneImmediately;
        /// <summary>
        /// На чьей планете дадут квест
        /// </summary>
        public Race planetRaces;
        /// <summary>
        /// Тип игрока
        /// </summary>
        public PlayerType playerTypes;
        /// <summary>
        /// Раса игрока
        /// </summary>
        public Race playerRaces;
        /// <summary>
        /// Как изменится отношение к игроку
        /// </summary>
        public int relation;
        public int pixelWidth;
        public int pixelHeight;
        public HeaderSizeHorizontal sizeHorizontal;
        public HeaderSizeVertical sizeVertical;
        /// <summary>
        /// Число переходов
        /// </summary>
        public int TransactionCount;
        /// <summary>
        /// Сложность квеста
        /// </summary>
        public int difficulty;
    };
}