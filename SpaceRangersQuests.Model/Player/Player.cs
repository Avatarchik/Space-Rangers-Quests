using System;
using System.Collections.Generic;
using System.Linq;
using SpaceRangersQuests.Model.Entity;
using SpaceRangersQuests.Model.Utils;

namespace SpaceRangersQuests.Model.Player
{
    public class Player
    {
        private readonly Dictionary<int, Location> _idToLocation = new Dictionary<int, Location>();
        private readonly Dictionary<int, float> _parameterIndexToValue = new Dictionary<int, float>();
        /// <summary>
        /// Видимость параметра
        /// </summary>
        private readonly Dictionary<int, bool> _parameterIndexToVisibility = new Dictionary<int, bool>();
        /// <summary>
        /// Индекс параметра к его использованию
        /// </summary>
        private readonly Dictionary<int, bool> _parameterIndexToUse = new Dictionary<int, bool>();

        public Player()
        { }

        public Quest Quest { get; private set; }

        public Location CurrentLocation { get; private set; }

        public void Play(Quest quest)
        {
            Quest = quest;
        }

        private void resetQuest()
        {
            _idToLocation.Clear();
            _parameterIndexToVisibility.Clear();
            _parameterIndexToValue.Clear();
            _parameterIndexToUse.Clear();

            Enumerable.Range(0, Quest.Parameters.Count)
                .ForEach(index => _parameterIndexToVisibility.Add(index, true));

            // Заполняем флаги используемости параметров
            Enumerable.Range(0, Quest.Parameters.Count)
                .ForEach(index => _parameterIndexToUse.Add(index, Quest.Parameters[index].active));


            Quest.Locations.ToDictionary(key => key.id, value => value)
                .ForEach(v => _idToLocation.Add(v.Key, v.Value));

            var startLocation = Quest.Locations.Where(v => v.start).ToList();
            SetLocation(startLocation.Single());

            checkTransitions();
            //reduceTransitions();
        }

        private void SetLocation(Location location)
        {
            CurrentLocation = location;
            var oldParametersValue = _parameterIndexToValue.ToDictionary(key => key.Key, value => value.Value);
            location.modifiers.ForEach((modifier, index) => ApplyModifier(index, modifier, oldParametersValue));
        }

        private void ApplyModifier(int index, Modifier modifier, Dictionary<int, float> oldParametersValue)
        {
            if (!_parameterIndexToUse[index])
                return;

            if (modifier.visibility != VisibilityType.VISIBILITY_NO_CHANGE)
                _parameterIndexToVisibility[index] = modifier.visibility == VisibilityType.VISIBILITY_SHOW;

            var modifierValue = modifier.value;
            var value = 0f;
            var oldValue = oldParametersValue[index];

            switch (modifier.Operation)
            {
                // Установить значение
                case OperationType.OPERATION_ASSIGN:
                    value = modifier.value;
                    break;
                // Изменить значение
                case OperationType.OPERATION_CHANGE:
                    value = oldValue + modifierValue;
                    break;
                case OperationType.OPERATION_PERCENT:
                    value = oldValue + (modifierValue * oldValue) / 100f;
                    break;
                case OperationType.OPERATION_EXPRESSION:
                    value = eval(modifier.expression, oldParametersValue);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            value = Math.Max(Quest.Parameters[index].min, value);
            value = Math.Min(Quest.Parameters[index].max, value);
            _parameterIndexToValue[index] = value;
        }

        private void checkTransitions()
        {
            // ID локации
            var locationId = CurrentLocation.id;
            // Получаем все переходы с локации
            var transactions = Quest.Transitions.Where(v => v.@from == locationId).ToList();
            transactions = transactions.Where(t => CheckTransition(t)).ToList();
        }

        public bool CheckTransition(Transition transition)
        {
            if (transition.passCount == 0)
                return true;
            if (transition.alwaysVisible)
                return true;

            return false;
        }

        /// <summary>
        /// Обработать выражение
        /// </summary>
        /// <param name="modifierExpression"></param>
        /// <param name="oldParametersValue"></param>
        /// <returns></returns>
        private float eval(bool modifierExpression, Dictionary<int, float> oldParametersValue)
        {
            throw new NotImplementedException();
        }
    }
}
