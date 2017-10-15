using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using SpaceRangersQuests.Model.Entity;
using SpaceRangersQuests.Model.Utils;

namespace SpaceRangersQuests.Model.Player
{
    public class Player : ObservableObject
    {
        /// <summary>
        /// Идентефикатор локации
        /// </summary>
        private readonly Dictionary<int, Location> _idToLocation = new Dictionary<int, Location>();
        /// <summary>
        /// Текущее значения параметров
        /// </summary>
        private readonly Dictionary<int, float> _parameterIndexToValue = new Dictionary<int, float>();
        /// <summary>
        /// Видимость параметра
        /// </summary>
        private readonly Dictionary<int, bool> _parameterIndexToVisibility = new Dictionary<int, bool>();
        /// <summary>
        /// Индекс параметра к его использованию
        /// </summary>
        private readonly Dictionary<int, bool> _parameterIndexToUse = new Dictionary<int, bool>();

        private readonly Dictionary<int, int> _transitionCounts = new Dictionary<int, int>();

        private readonly List<Transition> _alwaysVisibleTransitions = new List<Transition>();
        private readonly Dictionary<int, int> _locationDescriptionsCount = new Dictionary<int, int>();
        private string _locationText;

        public Player()
        {
            PossibleTransitions = new ObservableCollection<Transition>();
        }

        public IList<Transition> PossibleTransitions { get; private set; }

        /// <summary>
        /// Установленный квест
        /// </summary>
        public Quest Quest { get; private set; }
        /// <summary>
        /// Текущая локация квеста
        /// </summary>
        public Location CurrentLocation { get; private set; }

        public Transition CurrentTransition { get; private set; }

        public string LocationText
        {
            get { return _locationText; }
            private set
            {
                if (value == _locationText)
                    return;
                _locationText = value;
                OnPropertyChanged(nameof(LocationText));
            }
        }


        /// <summary>
        /// Проиграть квест
        /// </summary>
        /// <param name="quest"></param>
        public void Play(Quest quest)
        {
            Quest = quest;
            resetQuest();
        }

        /// <summary>
        /// Сбрасываем квест
        /// </summary>
        private void resetQuest()
        {
            _idToLocation.Clear();
            _parameterIndexToVisibility.Clear();
            _parameterIndexToValue.Clear();
            _parameterIndexToUse.Clear();
            _transitionCounts.Clear();

            Enumerable.Range(0, Quest.Parameters.Count)
                .ForEach(index => _parameterIndexToVisibility.Add(index, true));

            // Заполняем флаги используемости параметров
            Enumerable.Range(0, Quest.Parameters.Count)
                .ForEach(index => _parameterIndexToUse.Add(index, Quest.Parameters[index].active));

            // Устанавливаем стартовые значения
            _parameterIndexToUse.Where(v => v.Value)
                .Select(v => v.Key)
                .ForEach(v => _parameterIndexToValue[v] = Token.eval(Quest.Parameters[v].starText, _parameterIndexToValue));

            Quest.Locations.ToDictionary(location => location.id, location => location)
                .ForEach(v => _idToLocation.Add(v.Key, v.Value));

            var startLocation = Quest.Locations.Single(v => v.start);
            SetLocation(startLocation);

            //checkTransitions();
            //reduceTransitions();
        }

        private void SetLocation(Location location)
        {
            CurrentLocation = location;
            var oldParametersValue = _parameterIndexToValue.ToDictionary(key => key.Key, value => value.Value);
            location.modifiers.ForEach((modifier, index) => ApplyModifier(index, modifier, oldParametersValue));


            checkTransitions();
            ReduceTransition(CurrentLocation);

            if (CurrentLocation.LocationType != LocationType.LOCATION_EMPTY
                || CurrentLocation.descriptions.Count != 0)
            {
                if (CurrentLocation.descriptionExpression && CurrentLocation.expression.Length != 0)
                {
                    var t = (int)Token.eval(CurrentLocation.expression, _parameterIndexToValue);
                    //FIXME: Bug or feature?
                    if (t == 0)
                        t = 1;
                    if ((t < 1) || (t > 10) || (CurrentLocation.descriptions[t - 1].Length == 0))
                    {
                        Console.WriteLine(
                            $"Invalid location description selection ({t}) in location {CurrentLocation.id}");
                        LocationText = "";
                    }
                    else
                    {
                        LocationText = substituteValues(CurrentLocation.descriptions[t - 1]);
                    }
                }
                else
                {
                    var value = 0;
                    int descriptionIndex = 0;
                    var allDescriptions = CurrentLocation.descriptions.Where(v => v.Length > 0).ToList();
                    if (!_locationDescriptionsCount.TryGetValue(CurrentLocation.id, out descriptionIndex))
                    {
                        _locationDescriptionsCount.Add(CurrentLocation.id, 0);
                        descriptionIndex = 0;
                    }
                    else
                    {
                        descriptionIndex = (++descriptionIndex) % allDescriptions.Count;
                        _locationDescriptionsCount[CurrentLocation.id] = descriptionIndex;
                    }
                    LocationText = substituteValues(allDescriptions[descriptionIndex]);
                }
            }

            Console.WriteLine($"QuestPlayer: {CurrentLocation.id}");

            var currentLocationTransactions = Quest.Transitions.Where(v => v.from == CurrentLocation.id);
            if (PossibleTransitions.Count == 1
                && PossibleTransitions.Single().title.Length == 0
                && CurrentTransition.description.Length == 0)
            {
                StartTransition(PossibleTransitions.Single());
            }
            else if (CurrentLocation.LocationType == LocationType.LOCATION_SUCCESS)
                throw new Exception($"Complite: {LocationText}");
            else if (CurrentLocation.LocationType == LocationType.LOCATION_FAIL)
                throw new Exception($"Fail: {LocationText}");
            else if (CurrentLocation.LocationType == LocationType.LOCATION_DEATH)
                throw new Exception($"Death: {LocationText}");
            else if (!CheckCriticalParameters())
                throw new Exception($"Param: {LocationText}");
        }

        private bool CheckCriticalParameters()
        {
            foreach (var parameterValue in _parameterIndexToValue)
            {
                var parameter = Quest.Parameters[parameterValue.Key];
                if (parameter.parameterType == ParameterType.PARAMETER_NORMAL)
                    continue;

                var crit =
                    (parameter.minCritical && parameterValue.Value <= parameter.min)
                    || (!parameter.minCritical && parameterValue.Value >= parameter.max);

                if (crit)
                {
                    switch (parameter.parameterType)
                    {
                        case ParameterType.PARAMETER_FAIL:
                            throw new Exception($"FAIL: {substituteValues(parameter.critText)}");
                            break;
                        case ParameterType.PARAMETER_SUCCESS:
                            throw new Exception($"SUCCESS: {substituteValues(parameter.critText)}");
                            break;
                        case ParameterType.PARAMETER_DEATH:
                            throw new Exception($"DEATH: {substituteValues(parameter.critText)}");
                            break;
                    }
                    return false;
                }
            }
            return true;
        }

        public void StartTransition(Transition transition)
        {
            CurrentTransition = transition;

            var oldParametersValue = _parameterIndexToValue.ToDictionary(key => key.Key, value => value.Value);
            transition.modifiers.ForEach((modifier, index) => ApplyModifier(index, modifier, oldParametersValue));

            Console.WriteLine($"QuestPlayer: P{CurrentTransition.id}");

            LocationText = substituteValues(CurrentTransition.description);

            if (Quest.Locations.Single(v => v.id == CurrentTransition.to).empty)
            {
                FinishTransition();
            }
            else if (CurrentTransition.description.Length == 0)
            {
                FinishTransition();
            }
            else
            {
                throw new Exception(substituteValues(CurrentTransition.description));
                //emit(transitionText(d->substituteValues(d->m_currentTransition.description)));
            }
        }

        private void FinishTransition()
        {
            if (!_transitionCounts.ContainsKey(CurrentTransition.id))
            {
                _transitionCounts.Add(CurrentTransition.id, 1);
            }
            else
            {
                _transitionCounts[CurrentTransition.id] += 1;
            }
            SetLocation(Quest.Locations.Single(v => v.id == CurrentTransition.to));
        }

        private void ApplyModifier(int index, Modifier modifier, Dictionary<int, float> oldParametersValue)
        {
            if (!_parameterIndexToUse.ContainsKey(index)
                || !_parameterIndexToUse[index])
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
                    value = Token.eval(modifier.expressionString, oldParametersValue);
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
            PossibleTransitions.Clear();
            _alwaysVisibleTransitions.Clear();

            // ID локации
            var locationId = CurrentLocation.id;
            // Получаем все переходы с локации
            var transitions = Quest.Transitions.Where(v => v.from == locationId).ToList();
            transitions.ForEach(v => CheckTransition(v));

            //ReduceTransition(CurrentLocation);

            //transitions = transitions.Where(t => CheckTransition(t)).ToList();
        }

        private void ReduceTransition(Location currentLocation)
        {
            var rnd = new Random();

            var allTransactions = new Dictionary<string, IList<Transition>>();
            var alwaysVisibleTransitions = new List<Transition>();

            // Группируем транзакции по имени перехода
            foreach (var possibleTransition in PossibleTransitions)
            {
                IList<Transition> transactionList;
                var titleText = string.IsNullOrEmpty(possibleTransition.title.Text)
                    ?""
                    :possibleTransition.title.Text;
                if (!allTransactions.TryGetValue(titleText, out transactionList))
                {
                    transactionList = new List<Transition>();
                    allTransactions.Add(titleText, transactionList);
                }
                transactionList.Add(possibleTransition);
            }

            // Добавляем всегда видимыет транзакции
            foreach (var alwaysVisibleTransition in _alwaysVisibleTransitions)
            {
                var titleText = string.IsNullOrEmpty(alwaysVisibleTransition.title.Text)
                    ? ""
                    : alwaysVisibleTransition.title.Text;
                if (!allTransactions.ContainsKey(alwaysVisibleTransition.title.Text))
                {
                    alwaysVisibleTransitions.Add(alwaysVisibleTransition);
                }
            }

            //var allTransactions = _possibleTransitions.Concat(_alwaysVisibleTransitions).ToList();

            PossibleTransitions.Clear();
            _alwaysVisibleTransitions.Clear();

            alwaysVisibleTransitions.ForEach(v => _alwaysVisibleTransitions.Add(v));

            foreach (var allTransaction in allTransactions)
            {
                if (allTransaction.Value.Count == 1)
                {
                    var transition = allTransaction.Value.Single();
                    var priority = transition.priority;
                    if (priority < 1f)
                    {
                        if (rnd.Next(1000) < (int)(priority * 1000))
                        {
                            PossibleTransitions.Add(transition);
                        }
                    }
                    else
                    {
                        PossibleTransitions.Add(transition);
                    }
                    continue;
                }
                var allRange = allTransaction.Value.Sum(v => v.priority);
                var randValue = rnd.Next((int)(allRange * 1000));
                var countPriority = 0;
                Transition selectTransaction = null;
                foreach (var transition in allTransaction.Value)
                {
                    selectTransaction = transition;
                    countPriority = (int)(transition.priority * 1000);
                    if (countPriority >= randValue)
                        break;
                }
                PossibleTransitions.Add(selectTransaction);
            }
        }

        public bool CheckTransition(Transition transition)
        {
            var condition = _parameterIndexToUse
                .Where(v => v.Value)
                .Select(v => v.Key)
                .All(index => CheckModification(_parameterIndexToValue[index], transition.modifiers[index]));

            var passed = (transition.passCount == 0)
                || !(_transitionCounts.ContainsKey(transition.id) && (_transitionCounts[transition.id] >= transition.passCount));

            if (passed && condition && (transition.globalCondition.Length == 0 ||
                                        (int)Token.eval(transition.globalCondition, _parameterIndexToValue) != 0))
            {
                PossibleTransitions.Add(transition);
            }

            if (transition.alwaysVisible)
            {
                _alwaysVisibleTransitions.Add(transition);
            }

            return false;
        }

        private bool CheckModification(float parameterValue, Modifier modifier)
        {
            if (parameterValue < modifier.rangeFrom || parameterValue > modifier.rangeTo)
                return false;

            if (modifier.IncludeValue.CountIncludeValues > 0)
            {
                var contains = modifier.IncludeValue.IncludeValues.Contains((int)parameterValue);
                if ((modifier.IncludeValue.IncludeType == IncludeValueType.Unaccept && contains)
                    || (modifier.IncludeValue.IncludeType == IncludeValueType.Accept && !contains))
                {
                    return false;
                }
            }
            if (modifier.modValue.CountModValues > 0)
            {
                var isMod = modifier.modValue.ModValues.Any(v => ((int)parameterValue % v) == 0);
                if ((modifier.modValue.ModType == ModValueType.AcceptValue_Unmoded && isMod)
                    || (modifier.modValue.ModType == ModValueType.AcceptValue_Moded && !isMod))
                {
                    return false;
                }
            }
            return true;
        }

        private string substituteValues(BoolLengthString valueString)
        {
            return valueString.Length > 0 ? valueString.Text : "";
        }
    }
}
