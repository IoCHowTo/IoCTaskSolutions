﻿using Microsoft.Practices.Unity;

namespace UnityBuildUp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                args = new[] { "10", "+", "20", "+", "30" };
            }

            using (var container = new UnityContainer())
            {
                Bootstrapper.SetupContainer(container);

                var calculator = container.Resolve<ICalculator>();
                int result = calculator.Evaluate(args);

                var resultWriter = container.BuildUp(new ConsoleAndFileResultWriter("output.txt"));

                resultWriter.WriteResult(result);
            }
        }
    }
}
