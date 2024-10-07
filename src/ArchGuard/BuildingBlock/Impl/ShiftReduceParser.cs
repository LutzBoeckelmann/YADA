// Copyright (c) Lutz Boeckelmann and Contributors. MIT License - see LICENSE.txt

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using YADA.Analyzer;

namespace YADA.ArchGuard.BuildingBlock
{

    /// <summary>
    /// A shift reduce parser to parse logical expressions similar to the c#
    /// compiler
    /// </summary>
    internal class ShiftReduceParser
    {
        /// <summary>
        /// A stack implementation that stores the last (or uppermost) three items
        /// directly in members for easier access.
        /// </summary>
        private class OwnStack
        {
            private Stack<Symbol> m_Stack = new Stack<Symbol>();
            public Symbol First { get; private set; } = Symbol.Null;
            public Symbol Second { get; private set; } = Symbol.Null;
            public Symbol Third { get; private set; } = Symbol.Null;

            public void Pop(int count = 1)
            {
                for (int i = 0; i < count; i++)
                {
                    Pop();
                }
            }

            public Symbol Pop()
            {
                var result = First;

                First = Second;
                Second = Third;
                if (m_Stack.Count > 0)
                {
                    Third = m_Stack.Pop();
                }
                else
                {
                    Third = Symbol.Null;
                }

                return result;
            }
            public void Push(Symbol symbol)
            {
                if (!Third.IsNull)
                {
                    m_Stack.Push(Third);
                }
                Third = Second;
                Second = First;
                First = symbol;
            }
            
            public Symbol Peek()
            {
                return First;
            }

            public bool IsEmpty => First.IsNull;

            public int Size
            {
                get
                {
                    var result = m_Stack.Count;
                    if (!First.IsNull)
                    {
                        result++;
                    }

                    if (!Second.IsNull)
                    {
                        result++;
                    }

                    if (!Third.IsNull)
                    {
                        result++;
                    }
                    return result;
                }
            }
        }

        private OwnStack m_Stack;
        private Queue<Terminal> input;

        public ILogicalExpression ParseLogicalExpression(List<string> tokens)
        {

            m_Stack = new OwnStack();
            input = new Queue<Terminal>(tokens.Select(i => new Terminal(i)));

            var currentInput = input.Dequeue();

            bool somethingHappen = true;

            while (!currentInput.IsNull || !(m_Stack.Size == 1 && m_Stack.First is Expression))
            {
                somethingHappen = false;
                
                Symbol production = GetProduction(currentInput);
                if (!production.IsNull)
                {
                    m_Stack.Push(production);
                    somethingHappen = true;
                }
                else
                {
                    if (!currentInput.IsNull)
                    {

                        m_Stack.Push(currentInput);
                        somethingHappen = true;

                        if (input.Count > 0)
                        {
                            currentInput = input.Dequeue();
                        }
                        else
                        {
                            currentInput = new EmptyTerminal();
                        }
                    }
                }

                if (!somethingHappen)
                {
                    throw new Exception("Not able to parse input");
                }
            }

            return m_Stack.Peek();
        }

        private Symbol GetProduction(Terminal currentInput)
        {

            if (m_Stack.First.IsTerminal && IsRegEx(m_Stack.First))
            {
                // Reduce: Factor <- REGEX
                var reg = m_Stack.First.Content;
                m_Stack.Pop();

                return new REGEX(reg);
            }

            if (m_Stack.First is Factor && IsNot(m_Stack.Second))
            {
                // Reduce:  Factor <- NOT Factor
                var factor = (Factor)m_Stack.First;
                m_Stack.Pop(2);
                
                return new Not(factor);
            }

            if (IsClosingBrace(m_Stack.First)&& m_Stack.Second is Expression && IsOpeningBrace(m_Stack.Third))
            {
                // Reduce:  Factor <- ( EXPRESSION )
                var expression = (Expression)m_Stack.Second;
                m_Stack.Pop(3);
                
                return new Braces(expression);
            }

            if (m_Stack.First is Term && m_Stack.Second.IsTerminal && IsOr(m_Stack.Second) && m_Stack.Third is Expression && !IsAnd(currentInput))
            {
                // Reduce:  Expression <- Expression AND Term
                var term = (Term)m_Stack.First;
                var expression = (Expression)m_Stack.Third;
                m_Stack.Pop(3);
                return new OR(term, expression);
            }

            if (m_Stack.First is Term && !IsAnd(currentInput))
            {
                // Reduce: Expression<-Term
                var term = (Term)m_Stack.First;
                m_Stack.Pop();
                return new Expression(term);
            }

            if (m_Stack.First is Factor && m_Stack.Second.IsTerminal && IsAnd(m_Stack.Second) && m_Stack.Third is Term)
            {
                // Reduce: Term <- Term OR Factor
                var factor = (Factor)m_Stack.First;
                var term = (Term)m_Stack.Third;
                m_Stack.Pop(3);
                return new AND(factor, term);
            }

            if (m_Stack.First is Factor)
            {
                // Reduce: Term <- Factor
                var factor = (Factor)m_Stack.First;
                m_Stack.Pop();
                return new Term(factor);
            }

            return Symbol.Null;
        }

        private bool IsRegEx(Symbol symbol)
        {
            return symbol.IsTerminal
                && !IsNot(symbol)
                && !IsOpeningBrace(symbol)
                && !IsClosingBrace(symbol)
                && !IsOr(symbol)
                && !IsAnd(symbol);
        }

        private bool IsNot(Symbol symbol)
        {
            return symbol.IsTerminal && symbol.Content.ToUpperInvariant() == "NOT";
        }

        private bool IsOpeningBrace(Symbol symbol)
        {
            return symbol.IsTerminal && symbol.Content == "(";
        }

        private bool IsClosingBrace(Symbol symbol)
        {
            return symbol.IsTerminal && symbol.Content == ")";
        }

        private bool IsOr(Symbol symbol)
        {
            return symbol.IsTerminal && symbol.Content.ToUpperInvariant() == "OR";
        }

        private bool IsAnd(Symbol symbol)
        {
            return symbol.IsTerminal && symbol.Content.ToUpperInvariant() == "AND";
        }

        public class Symbol: ILogicalExpression
        {
            public static Symbol Null = new NullSymbol();
            public virtual bool IsNull => false;
            public virtual bool IsTerminal => false;
            public string Content { get; protected set; }

            public virtual bool Evaluate(ITypeDescription typeDescription)
            {
                throw new NotSupportedException();
            }
        }

        class NullSymbol : Symbol
        {
            public override bool IsNull => true;

            public override bool Evaluate(ITypeDescription typeDescription)
            {
                throw new NotSupportedException();
            }
        }

        class Expression : Symbol
        {
            public Term Term { get; }

            public Expression(Term term)
            {
                Term = term;
            }

            public override bool Evaluate(ITypeDescription typeDescription)
            {
                return Term.Evaluate(typeDescription);
            }
        }

        class OR : Expression
        {
            public Expression Expression { get; }

            public OR(Term term, Expression expression) : base(term)
            {
                Expression = expression;
            }

            public override bool Evaluate(ITypeDescription typeDescription)
            {
                return Term.Evaluate(typeDescription) || Expression.Evaluate(typeDescription);
            }
        }

        class Term : Symbol
        {
            public Factor Factor { get; }
            public Term(Factor factor)
            {
                Factor = factor;
            }

            public override bool Evaluate(ITypeDescription typeDescription)
            {
                return Factor.Evaluate(typeDescription);
            }
        }

        class Factor : Symbol
        {

        }

        class Braces : Factor
        {
            private readonly Expression m_Expression;

            public Braces(Expression expression)
            {
                m_Expression = expression;
            }

            public override bool Evaluate(ITypeDescription typeDescription)
            {
                return m_Expression.Evaluate(typeDescription);
            }
        }

        class REGEX : Factor
        { 
            private readonly Regex m_RegEx;

            public REGEX(string regEx)
            {
                m_RegEx = new Regex(regEx);
            }

            public override bool Evaluate(ITypeDescription typeDescription)
            {
                return m_RegEx.IsMatch(typeDescription.FullName);
            }
        }
       
        class Not : Factor
        {
            public Not(Factor internalFactor)
            {
                InternalFactor = internalFactor;
            }

            public Factor InternalFactor { get; }

            public override bool Evaluate(ITypeDescription typeDescription)
            {
                return !InternalFactor.Evaluate(typeDescription);
            }
        }

        class Terminal : Symbol
        {
            public Terminal(string content)
            {
                Content = content;
            }
            public override bool IsTerminal => true;

            public override string ToString()
            {
                return Content;
            }
        }

        class EmptyTerminal : Terminal
        {
            public override bool IsTerminal => true;
            public override bool IsNull => true;

            public EmptyTerminal() : base("Empty")  {   }
        }

        class AND : Term
        {
            public Term Term { get; }

            public AND(Factor factor, Term term) : base(factor)
            {
                Term = term;
            }

            public override bool Evaluate(ITypeDescription typeDescription)
            {
                return Factor.Evaluate(typeDescription) && Term.Evaluate(typeDescription);
            }
        }
    }
}