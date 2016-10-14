/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 12.06.2016 23:54:11
*/

using System;
using System.Collections;
using System.Diagnostics;

using Nano3.Collection;
namespace Nano3.Diagnostics
{
    public class N3Stopwatch : IStopwatch, IEnumerable
    {
        private Stopwatch sw = new Stopwatch();
        private CycleStorage<TimeSpan> _resultStorage;

        public int ResultCount
        {
            get { return _resultStorage.Count; }
        }
        public TimeSpan this[int resultIndex]
        {
            get { return _resultStorage[resultIndex]; }
        }

        public TimeSpan Elapsed
        {
            get { return _resultStorage[_resultStorage.Count - 1]; }
        }

        public bool IsRunning
        {
            get { return sw.IsRunning; }
        }

        public N3Stopwatch(int storedResultCount)
        {
            if (storedResultCount <= 1) { _resultStorage = new CycleStorage<TimeSpan>(1); }
            else { _resultStorage = new CycleStorage<TimeSpan>(storedResultCount); }
        }

        public void StartNew()
        {
            if (sw.IsRunning) { sw.Stop(); }
            sw.Reset();
            sw.Start();
        }

        public void Reset()
        {
            sw.Reset();
        }

        public void Start()
        {
            if (!sw.IsRunning){ sw.Start(); }
        }

        public void Stop()
        {
            sw.Stop();
            _resultStorage.Add(sw.Elapsed);
        }

        public void Clear()
        {
            _resultStorage.Clear();
            _resultStorage = null;
        }

        public IEnumerator GetEnumerator()
        {
            return _resultStorage.GetEnumerator();
        }
    }
}
