using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using SpaceRangersQuests.Model.Entity;

namespace SpaceRangersQuests.Model.Player
{
    public static class CharExtensions
    {
        private static List<char> WhiteSpaces = new List<char> { '\t', '\n', '\v', '\f', '\r', ' ' };
        private static List<char> Digits = new List<char> { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

        public static bool IsWhiteSpace(this char chr)
        {
            return WhiteSpaces.Contains(chr);
        }

        public static bool IsDigit(this char chr)
        {
            return Digits.Contains(chr);
        }
    }
    public class Token
    {
        private static UInt32[] PRECEDENCES = new uint[] { 0, 0, 0, 0, 6, 6, 8, 7, 8, 3, 3, 3, 3, 3, 3, 1, 2, 9, 4, 5, 0, 0, 8 };
        private static Random _rnd = new Random();

        public class ValueToken
        {
            public UInt32 id;
            public float from;
            public float to;
            public float number;
        }

        private Token()
        {
            type = TokenType.TOKEN_NONE;
            value = new ValueToken();
            value.from = 0.0f;
            value.to = 0.0f;
            value.number = 0.0f;
            list = new List<float>();
        }

        private Token(float number)
            : this()
        {
            type = TokenType.TOKEN_NUMBER;
            value.number = number;
        }

        private Token(float from, float to)
            : this()
        {
            type = TokenType.TOKEN_RANGE;
            value.from = from;
            value.to = to;
        }

        public enum TokenType
        {
            TOKEN_NONE = 255,
            TOKEN_NUMBER = 0,
            TOKEN_RANGE = 1,
            TOKEN_OPEN_PAR = 2,
            TOKEN_CLOSE_PAR = 3,
            TOKEN_OP_PLUS = 4,
            TOKEN_OP_MINUS = 5,
            TOKEN_OP_DIV = 6,
            TOKEN_OP_MULTIPLY = 7,
            TOKEN_OP_MOD = 8,
            TOKEN_EQUAL = 9,
            TOKEN_MORE = 10,
            TOKEN_LESS = 11,
            TOKEN_MORE_EQUAL = 12,
            TOKEN_LESS_EQUAL = 13,
            TOKEN_NOT_EQUAL = 14,
            TOKEN_OR = 15,
            TOKEN_AND = 16,
            TOKEN_NOT = 17,
            TOKEN_TO = 18,
            TOKEN_IN = 19,
            TOKEN_PARAMETER = 20,
            TOKEN_LIST = 21,
            TOKEN_OP_INT_DIV = 22,
        };

        public TokenType type { get; private set; }

        public ValueToken value { get; private set; }

        public IList<float> list { get; private set; }

        /// <summary>
        /// Обработать выражение
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="oldParametersValue"></param>
        /// <returns></returns>
        public static float eval(BoolLengthString expression, Dictionary<int, float> oldParametersValue)
        {
            return eval(tokenize(expression.Text), oldParametersValue);
        }

        private static float eval(IList<Token> exp, Dictionary<int, float> parameters)
        {
            if (exp.Count == 0)
                return 0;

            IList<Token> opStack = new List<Token>();
            foreach (var t in exp)
            {
                if ((t.type == TokenType.TOKEN_NUMBER) || (t.type == TokenType.TOKEN_RANGE) ||
                    (t.type == TokenType.TOKEN_PARAMETER) || (t.type == TokenType.TOKEN_LIST))
                {
                    opStack.Insert(0, t);
                }
                else
                {
                    Token b = opStack.First();
                    opStack.RemoveAt(0);
                    Token a = opStack.First();
                    opStack.RemoveAt(0);
                    opStack.Insert(0, t.Apply(a, b, parameters));
                }
            }
            Token r = opStack.First();

            if (r.type == TokenType.TOKEN_PARAMETER)
                return parameters[(int)r.value.id];
            else if (r.type == TokenType.TOKEN_RANGE)
                return r.value.from + (_rnd.Next((int)(r.value.to - r.value.from + 1)));
            else if (r.type == TokenType.TOKEN_LIST)
                return r.list[_rnd.Next(r.list.Count)];
            else
                return r.value.number;
        }

        private static IList<Token> tokenize(string expressionText)
        {
            var pos = 0;
            Token prev = null;
            var result = new List<Token>();
            var opStack = new List<Token>();

            var exp = expressionText.Replace(",", ".");

            while (pos < exp.Length)
            {
                if (exp[pos].IsWhiteSpace())
                {
                    pos++;
                    continue;
                }
                Token t = new Token();
                if ((exp[pos].IsDigit())
                    || ((exp[pos] == '-')
                        && (prev.type != TokenType.TOKEN_NUMBER)
                        && (prev.type != TokenType.TOKEN_RANGE)
                        && (prev.type != TokenType.TOKEN_PARAMETER)
                        && (prev.type != TokenType.TOKEN_LIST)
                        && (prev.type != TokenType.TOKEN_CLOSE_PAR)))
                {
                    t = new Token(GetFloat(ref pos, exp));
                }
                else if (exp[pos] == '-')
                {
                    t.type = TokenType.TOKEN_OP_MINUS;
                    pos++;
                }
                else if (exp[pos] == '+')
                {
                    t.type = TokenType.TOKEN_OP_PLUS;
                    pos++;
                }
                else if (exp[pos] == '*')
                {
                    t.type = TokenType.TOKEN_OP_MULTIPLY;
                    pos++;
                }
                else if (exp[pos] == '/')
                {
                    t.type = TokenType.TOKEN_OP_DIV;
                    pos++;
                }
                else if (exp[pos] == '(')
                {
                    t.type = TokenType.TOKEN_OPEN_PAR;
                    pos++;
                }
                else if (exp[pos] == ')')
                {
                    t.type = TokenType.TOKEN_CLOSE_PAR;
                    pos++;
                }
                else if (exp[pos] == '=')
                {
                    t.type = TokenType.TOKEN_EQUAL;
                    pos++;
                }
                else if (exp[pos] == '>')
                {
                    if (exp[pos + 1] == '=')
                    {
                        t.type = TokenType.TOKEN_MORE_EQUAL;
                        pos += 2;
                    }
                    else
                    {
                        t.type = TokenType.TOKEN_MORE;
                        pos++;
                    }
                }
                else if (exp[pos] == '<')
                {
                    if (exp[pos + 1] == '=')
                    {
                        t.type = TokenType.TOKEN_LESS_EQUAL;
                        pos += 2;
                    }
                    else if (exp[pos + 1] == '>')
                    {
                        t.type = TokenType.TOKEN_NOT_EQUAL;
                        pos += 2;
                    }
                    else
                    {
                        t.type = TokenType.TOKEN_LESS;
                        pos++;
                    }
                }
                else if (exp[pos] == '[')
                {
                    if (exp[pos + 1] == 'p')
                    {
                        pos += 2;
                        t.type = TokenType.TOKEN_PARAMETER;
                        t.value.id = (uint)GetInt(ref pos, exp);
                        if (exp[pos] != ']')
                        {
                            Console.WriteLine($"Unclosed parameter token in \"{exp}\"");
                            return new List<Token>();
                        }
                        pos++;
                    }
                    else if ((exp[pos + 1].IsDigit()) || ((exp[pos + 1] == '-')))
                    {
                        pos++;
                        float v1, v2;
                        v1 = GetFloat(ref pos, exp);
                        if (exp[pos] == ']')
                        {
                            t = new Token(v1);
                            pos++;
                        }
                        else if ((exp[pos] == '.') || (exp[pos] == 'h'))
                        {
                            pos++;
                            v2 = GetFloat(ref pos, exp);
                            if (exp[pos] != ']')
                            {
                                Console.WriteLine($"Invalid range token in \"{exp}\" at pos = {pos}");
                                return new List<Token>();
                            }
                            t = new Token(v1, v2);
                            pos++;
                        }
                        else if (exp[pos] == ';')
                        {
                            t.type = TokenType.TOKEN_LIST;
                            t.list.Add(v1);
                            while (exp[pos] == ';')
                            {
                                pos++;
                                t.list.Add(GetFloat(ref pos, exp));
                            }
                            if (exp[pos] != ']')
                            {
                                Console.WriteLine($"Unclosed list token in \"{exp}\"");
                                return new List<Token>();
                            }
                            pos++;
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Invalid token in \"{exp}\"");
                        return new List<Token>();
                    }
                }
                else if (exp.Substring(pos, 3) == "mod")
                {
                    t.type = TokenType.TOKEN_OP_MOD;
                    pos += 3;
                }
                else if (exp.Substring(pos, 3) == "div")
                {
                    t.type = TokenType.TOKEN_OP_INT_DIV;
                    pos += 3;
                }
                else if (exp.Substring(pos, 3) == "and")
                {
                    t.type = TokenType.TOKEN_AND;
                    pos += 3;
                }
                else if (exp.Substring(pos, 3) == "not")
                {
                    t.type = TokenType.TOKEN_NOT;
                    pos += 3;
                }
                else if (exp.Substring(pos, 2) == "or")
                {
                    t.type = TokenType.TOKEN_OR;
                    pos += 2;
                }
                else if (exp.Substring(pos, 2) == "to")
                {
                    t.type = TokenType.TOKEN_TO;
                    pos += 2;
                }
                else if (exp.Substring(pos, 2) == "in")
                {
                    t.type = TokenType.TOKEN_IN;
                    pos += 2;
                }
                else
                {
                    Console.WriteLine($"Invalid token in \"{exp}\" at pos = {pos}");
                    return new List<Token>();
                }

                if ((t.type == TokenType.TOKEN_NUMBER) || (t.type == TokenType.TOKEN_RANGE) ||
                    (t.type == TokenType.TOKEN_PARAMETER) || (t.type == TokenType.TOKEN_LIST))
                {
                    result.Add(t);
                }
                else if (t.type == TokenType.TOKEN_OPEN_PAR)
                {
                    opStack.Insert(0, t);
                }
                else if (t.type == TokenType.TOKEN_CLOSE_PAR)
                {
                    Token t2;
                    do
                    {
                        if (opStack.Count == 0)
                        {
                            Console.WriteLine($"Parenthesis error in \"{exp}\"");
                            return new List<Token>();
                        }
                        t2 = opStack.First();
                        opStack.RemoveAt(0);
                        if (t2.type != TokenType.TOKEN_OPEN_PAR)
                            result.Add(t2);
                    }
                    while (t2.type != TokenType.TOKEN_OPEN_PAR && opStack.Count != 0);
                }
                else
                {
                    if (opStack.Count != 0)
                    {
                        Token o2 = opStack.First();
                        while ((opStack.Count != 0)
                            && (o2.type != TokenType.TOKEN_OPEN_PAR)
                            && (o2.type != TokenType.TOKEN_CLOSE_PAR)
                            && (PRECEDENCES[(int)t.type] <= PRECEDENCES[(int)o2.type]))
                        {
                            result.Add(o2);
                            opStack.RemoveAt(0);
                            if (opStack.Count != 0)
                                o2 = opStack.First();
                        }
                    }
                    opStack.Insert(0, t);
                }

                prev = t;
            }

            while (opStack.Count != 0)
            {
                Token t = opStack.First();
                opStack.RemoveAt(0);
                if (t.type == TokenType.TOKEN_OPEN_PAR)
                {
                    Console.WriteLine($"Parenthesis error in \"{exp}\"");
                    return new List<Token>();
                }
                result.Add(t);
            }
            return result;
        }

        private static float GetFloat(ref int pos, string exp)
        {
            var offset = 1;
            var s = exp[pos + offset];
            var wasPoint = false;
            while (s != '\0' && (s.IsDigit() || s == '-' || (s == '.' && !wasPoint)))
            {
                if (s == '.')
                    wasPoint = true;
                s = exp[pos + ++offset];
            }
            var substring = exp.Substring(pos, offset)
                .Replace(".", ",");
            float res = float.Parse(substring, NumberStyles.AllowDecimalPoint);
            pos += offset;
            return res;
        }

        private static int GetInt(ref int pos, string exp)
        {
            int offset = 0;
            var s = exp[pos + 1];
            while (s != '\0' && (s.IsDigit() || s == '-'))
            {
                s++;
                offset++;
            }
            int res = int.Parse(exp.Substring(pos, offset));
            pos += offset;
            return res;
        }

        private Token Apply(Token a, Token b, Dictionary<int, float> parameters)
        {
            float av, bv;

            if (a.type ==TokenType.TOKEN_RANGE)
                av = a.value.from + (_rnd.Next((int) (a.value.to - a.value.from + 1)));
            else if (a.type == TokenType.TOKEN_LIST)
                av = a.list[_rnd.Next(a.list.Count)];
            else if (a.type == TokenType.TOKEN_PARAMETER)
                av = parameters[(int) a.value.id];
            else
                av = a.value.number;

            if (b.type == TokenType.TOKEN_RANGE)
                bv = b.value.from + (_rnd.Next((int)(b.value.to - b.value.from + 1)));
            else if (b.type == TokenType.TOKEN_LIST)
                bv = b.list[_rnd.Next(b.list.Count)];
            else if (b.type == TokenType.TOKEN_PARAMETER)
                bv = parameters[(int)b.value.id];
            else
                bv = b.value.number;

            switch (type)
            {
                case TokenType.TOKEN_NUMBER:
                case TokenType.TOKEN_RANGE:
                case TokenType.TOKEN_PARAMETER:
                case TokenType.TOKEN_OPEN_PAR:
                case TokenType.TOKEN_CLOSE_PAR:
                case TokenType.TOKEN_NONE:
                case TokenType.TOKEN_LIST:
                    return this;
                case TokenType.TOKEN_OP_PLUS:
                    return new Token(av + bv);
                case TokenType.TOKEN_OP_MINUS:
                    return new Token(av - bv);
                case TokenType.TOKEN_OP_DIV:
                    return new Token(av / bv);
                case TokenType.TOKEN_OP_INT_DIV:
                    return new Token((int)av / (int)bv);
                case TokenType.TOKEN_OP_MULTIPLY:
                    return new Token(av* bv);
                case TokenType.TOKEN_OP_MOD:
                    return new Token(av % bv);
                case TokenType.TOKEN_EQUAL:
                    return new Token(av == bv?1:0);
                case TokenType.TOKEN_MORE:
                    return new Token(av > bv ? 1 : 0);
                case TokenType.TOKEN_LESS:
                    return new Token(av<bv ? 1 : 0);
                case TokenType.TOKEN_MORE_EQUAL:
                    return new Token(av >= bv ? 1 : 0);
                case TokenType.TOKEN_LESS_EQUAL:
                    return new Token(av <= bv ? 1 : 0);
                case TokenType.TOKEN_NOT_EQUAL:
                    return new Token(av != bv ? 1 : 0);
                case TokenType.TOKEN_OR:
                    return new Token(av > 0 || bv > 0 ? 1 : 0);
                case TokenType.TOKEN_AND:
                    return new Token(av > 0 && bv > 0 ? 1 : 0);
                case TokenType.TOKEN_NOT:
                    return new Token(av > 0 ? 0 : 1);
                case TokenType.TOKEN_TO:
                    //FIXME: List?
                    if ((a.type == TokenType.TOKEN_RANGE) && (b.type == TokenType.TOKEN_RANGE))
                    {
                        var min = Math.Min(a.value.from, b.value.from);
                        var max = Math.Max(a.value.to, b.value.to);
                        return new Token(min, max);
                    }
                    else if ((a.type == TokenType.TOKEN_RANGE) && (b.type != TokenType.TOKEN_RANGE))
                    {
                        var min = Math.Min(a.value.from, bv);
                        var max = Math.Max(a.value.to, bv);
                        return new Token(min, max);
                    }
                    else if ((a.type != TokenType.TOKEN_RANGE) && (b.type == TokenType.TOKEN_RANGE))
                    {
                        var min = Math.Min(av, b.value.from);
                        var max = Math.Max(av, b.value.to);
                        return new Token(min, max);
                    }
                    else
                    {
                        float min = Math.Min(av, bv);
                        float max = Math.Max(av, bv);
                        return new Token(min, max);
                    }
                case TokenType.TOKEN_IN:
                    if ((a.type == TokenType.TOKEN_RANGE) && (b.type == TokenType.TOKEN_RANGE))
                    {
                        return new Token((a.value.from >= b.value.from) && (a.value.to <= b.value.to) ? 1 : 0);
                    }
                    else if ((a.type == TokenType.TOKEN_RANGE) && (b.type != TokenType.TOKEN_RANGE))
                    {
                        return new Token((a.value.from <= bv) && (a.value.to >= bv) ? 1 : 0);
                    }
                    else if ((a.type != TokenType.TOKEN_RANGE) && (b.type == TokenType.TOKEN_RANGE))
                    {
                        return new Token((av >= b.value.from) && (av <= b.value.to) ? 1 : 0);
                    }
                    else
                    {
                        return new Token(av == bv ? 1 : 0);
                    }
            }
            throw new Exception();
        }
    }
}