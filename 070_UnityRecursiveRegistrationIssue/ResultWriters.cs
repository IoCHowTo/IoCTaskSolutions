﻿using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;

namespace UnityRecursiveRegistrationIssue
{
    public interface IResultWriter
    {
        void WriteResult(int value);
        void WriteContext();
    }
    
    public interface IResultWriterPolicy
    {
        void WriteResult(int value);
    }

    /// <summary>
    /// Console result writer (writes the result also into the log)
    /// </summary>
    public class ConsoleResultWriter : IResultWriterPolicy
    {
        private readonly IUnityContainer _container;

        public ConsoleResultWriter(IUnityContainer container)
        {
            _container = container;
        }

        public void WriteResult(int value)
        {
            _container.Resolve<IResultWriter>().WriteContext();
            Console.WriteLine(value);
        }
    }

    /// <summary>
    /// Log result writer currently uses the console for output
    /// </summary>
    public class LogResultWriter : IResultWriterPolicy
    {
        public void WriteResult(int value)
        {
            Console.WriteLine("Log Result: {0}", value);
        }
    }

    public class FileResultWriter : IResultWriterPolicy, IDisposable
    {
        private readonly StreamWriter _output;

        public FileResultWriter()
        {
            _output = File.CreateText("output.txt");
        }

        public void WriteResult(int value)
        {
            _output.WriteLine(value);
        }

        public void Dispose()
        {
            if (_output != null)
            {
                _output.Dispose();
            }

            GC.SuppressFinalize(this);
        }
    }

    /// <summary>
    /// Purpose of this class is to use both result writers
    /// to write the result into the console as well as file
    /// </summary>
    public class ConsolidatedResultWriter : IResultWriter
    {
        private readonly IEnumerable<IResultWriterPolicy> _writers;

        public ConsolidatedResultWriter(IResultWriterPolicy[] writers)
        {
            _writers = writers;
        }

        public void WriteResult(int value)
        {
            _writers.ForEach(w => w.WriteResult(value));
        }

        public void WriteContext()
        {
            Console.WriteLine("Calculation context");
        }
    }
}