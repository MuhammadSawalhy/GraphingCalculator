using System;
using System.Collections.Generic;
using MathPackage.Operations;

namespace MathPackage
{
    public static class Derivative
    {
        public static Node CalculateDerivative(Node Node, string MainVariable)
        {
            List<Node> arguments = new List<Node>();
            if (Node.Children != null)
            {
                for (var i = 0; i < Node.Children.Length; i++)
                {
                    arguments.Add(Node.Children[i]);
                }
            }
            else
            {
                arguments = null;
            }
            switch (Node)
            {
                case Add _:
                    return AddDerivative(arguments, MainVariable);
                case Subtract _:
                    return SubtractDerivative(arguments, MainVariable);
                case Multiply _:
                    return MultiplyDerivative(arguments, MainVariable);
                case Divide _:
                    return DivideDerivative(arguments, MainVariable);
                case Power _:
                    return PowerDerivative(arguments, MainVariable);
                case Constant c:
                    return new Constant(0);
                case Variable v:
                    return new Constant(v.Name.Name == MainVariable ? 1 : 0);
                case Log l:
                    return LogDerivative(l, MainVariable);
                default:
                    throw new Exception("This function is not supported yet");
            }
        }
        
        private static Node AddDerivative(IReadOnlyList<Node> arguments, string MainVariable)
        {
            return new Add(
                CalculateDerivative(arguments[0], MainVariable),
                CalculateDerivative(arguments[1], MainVariable)
             );
        }
        private static Node SubtractDerivative(IReadOnlyList<Node> arguments, string MainVariable)
        {
            return new Subtract( 
                CalculateDerivative(arguments[0],MainVariable),
                CalculateDerivative(arguments[1],MainVariable)
             );
        }
        private static Node MultiplyDerivative(IReadOnlyList<Node> arguments, string MainVariable)
        {
            return new Add(

                new Multiply( arguments[0], CalculateDerivative(arguments[1], MainVariable) ),
                new Multiply(arguments[1], CalculateDerivative(arguments[0], MainVariable) )
            );
        }
        private static Node DivideDerivative(IReadOnlyList<Node> arguments, string MainVariable)
        {
            return new Divide(
                new Subtract(
                    new Multiply(
                         arguments[1], CalculateDerivative(arguments[0], MainVariable) ),
                    new Multiply(
                         arguments[0], CalculateDerivative(arguments[1], MainVariable) )
                ),
                new Power( arguments[1],new Constant(2))
            );
        }
        private static Node PowerDerivative(IReadOnlyList<Node> arguments, string MainVariable)
        {
            return new Multiply(

                new Power(arguments[0], arguments[1]),
                new Add(

                    new Divide(

                        new Multiply(
                            arguments[1],
                            CalculateDerivative(arguments[0], MainVariable)
                            ),
                        arguments[0]
                    ),
                    new Multiply(
                        new Log(arguments[0], new Constant(Math.E,"e")),
                        CalculateDerivative(arguments[1], MainVariable)
                    )
                )
            );
        }
        private static Node LogDerivative(Log Node, string MainVariable)
        {
            //    return new Expression(new Multiply(new Node[]
            //    {
            //        new Divide(new Node[]
            //        {
            //            new Constant(1),
            //            new Multiply(new Node[]
            //            {
            //                Node.Children[0],
            //                new Log(new Node[]{Node.Base},new Constant(Math.E,"e"))
            //            })
            //        }),
            //        CalculateDerivative(arguments[0], varName).Root
            //    }));
            return null;
        }
        private static Node SinDerivative(IReadOnlyList<Node> arguments, string MainVariable)
        {
            return new Multiply(
                new Cos(arguments[0]),
                CalculateDerivative(arguments[0], MainVariable)
            );
        }
        private static Node CosDerivative(IReadOnlyList<Node> arguments, string MainVariable)
        {
            return new Neg(
                new Multiply(
                    new Sin(arguments[0]),
                    CalculateDerivative(arguments[0], MainVariable)
                )
            );
        }
        private static Node TanDerivative(IReadOnlyList<Node> arguments, string MainVariable)
        {
            return new Multiply(

                 new Power(
                     new Sec( arguments[0] ),
                     new Constant(2)
                 ),
                 CalculateDerivative(arguments[0], MainVariable)
            );
        }
    }

}