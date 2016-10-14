/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 12.06.2016 23:51:49
*/

using System;

namespace Nano3.Diagnostics
{
    public interface IStopwatch
    {
        TimeSpan this[int resultIndex] { get; }
        TimeSpan Elapsed { get; }
        int ResultCount { get; }
        bool IsRunning { get; }

        void StartNew();
        void Start();
        void Stop();
        void Reset();

        void Clear();
    }

    public interface IStopwatched
    {
        IStopwatch StopWatch { get; }
        void SetElapsedResultsCount(int count);
    }
}
