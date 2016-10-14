/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 06.08.2016 23:41:04
*/

using System;
using System.Text;

namespace Nano3.Diagnostics
{
    public interface IEfficientMeasured
    {
        EfficiencyTimeCounter Efficiency { get; }
    }

    public delegate void StringMessage(object sender, string message);

    public class EfficiencyTimeCounter
    {
        public event StringMessage ETCChangeEvent;

        private string _name;
        public string Name { get { return _name; } }

        private N3Stopwatch _sw;
        public N3Stopwatch LastResultsStorage { get { return _sw; } }

        public bool IsRun { get { return _sw.IsRunning; } }

        private double _totalTime; public double TotalTime { get { return _totalTime; } }
        private long _totalCycles; public long TotalCycles { get { return _totalCycles; } }

        private double _usefulTime; public double UsefulTime { get { return _usefulTime; } }
        private long _usefulCycles; public long UsefulCycles { get { return _usefulCycles; } }

        public double ERatioTime
        {
            get
            {
                return _totalCycles > 0 ? _usefulTime / _totalTime : 0;
            }
        }
        public long ERatioCycles
        {
            get { return _totalCycles > 0 ? _usefulCycles / _totalCycles : 0; }
        }
        public double AwerageTotalTime
        {
            get { return _totalCycles > 0 ? _totalTime / _totalCycles : 0; }
        }
        public double AwerageUsefulTile
        {
            get { return _usefulCycles > 0 ? _usefulTime / _usefulCycles : 0; }
        }

        public EfficiencyTimeCounter(string name, int resultStorageCount = 100)
        {
            _sw = new N3Stopwatch(resultStorageCount);
            _name = name;
        }

        public void Start()
        {
            _sw.StartNew();
        }
        public void Stop(bool isUseful)
        {
            if (_sw.IsRunning)
            {
                _sw.Stop();
                double lastTime = _sw.Elapsed.TotalMilliseconds;
                _totalTime += lastTime; _totalCycles++;
                if (isUseful) { _usefulCycles++; _usefulTime += lastTime; }
                ETCChangeEvent?.Invoke(this, ToString());
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(_name);
            sb.Append(": Total [");
            sb.Append(string.Format("{0:0.00} ms/{1}]", _totalTime, _totalCycles));
            if (_usefulTime != _totalTime)
            {
                sb.Append(", Useful [");
                sb.Append(string.Format("{0:0.00} ms/{1}]", _usefulTime, _usefulCycles));
                sb.Append(", Efficiency [");
                sb.Append(string.Format("{0:0.00} %]", (ERatioTime * 100)));
            }
            return sb.ToString();
        }
    }
}
