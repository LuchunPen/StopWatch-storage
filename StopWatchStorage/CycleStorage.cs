/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 13.06.2016 1:32:40
*/

using System;
using System.Collections;
using System.Collections.Generic;

namespace Nano3.Collection
{
    public class CycleStorage<TValue> : IEnumerable
    {
        private int _currentIndex;
        private int _count;

        private TValue[] _items;
        public TValue[] Items { get { return _items; } }

        public int Count { get { return _count; } }
        public TValue this[int index]
        {
            get
            {
                if (_items.Length == _count)
                {
                    return _items[(_currentIndex + index) % _count];
                }
                else { return _items[index % _count]; }
            }
        }

        public CycleStorage(int itemsCount)
        {
            _items = new TValue[itemsCount];
            _currentIndex = 0;
            _count = 0;
        }

        public void Add(TValue item)
        {
            _items[_currentIndex] = item;

            _currentIndex = _currentIndex + 1 == _items.Length ? 0 : _currentIndex + 1;
            if (_count < _items.Length) { _count++; }
        }

        public TValue AddReusable(TValue item)
        {
            TValue result = _items[_currentIndex];
            _items[_currentIndex] = item;

            _currentIndex = _currentIndex + 1 == _items.Length ? 0 : _currentIndex + 1;
            if (_count < _items.Length) { _count++; }

            return result;
        }

        public void Remove(int index)
        {
            if (_items.Length == _count)
            {
                _items[(_currentIndex + index) % _count] = default(TValue);
            }
            else { _items[index % _count] = default(TValue); }
        }

        public void Clear()
        {
            _count = 0;
            _currentIndex = 0;
            Array.Clear(_items, 0, _count);
        }

        public IEnumerator GetEnumerator()
        {
            return new CycleArrayEnumerator(this);
        }

        public struct CycleArrayEnumerator : IEnumerator<TValue>
        {
            private CycleStorage<TValue> _itemArray;
            private int _currIndex;

            public CycleArrayEnumerator(CycleStorage<TValue> array)
            {
                if (array == null) throw new ArgumentNullException(array.ToString());
                _itemArray = array;
                _currIndex = -1;
            }

            public TValue Current
            {
                get { return _itemArray[_currIndex]; }
            }

            object IEnumerator.Current
            {
                get { return _itemArray[_currIndex]; }
            }

            public bool MoveNext()
            {
                if (_currIndex == _itemArray._count - 1) { Reset(); return false; }
                else { _currIndex++; return true; }
            }

            public void Reset()
            {
                _currIndex = -1;
            }

            public void Dispose()
            {
                _itemArray = null;
            }
        }
    }
}
