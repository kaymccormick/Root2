using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using RibbonLib.Model;

namespace AnalysisControls
{
    public class RibbonModelTabCollection : IList, ICollection, IEnumerable
    {
        private IList _listImplementation = new List<RibbonModelTab>();
        public IEnumerator GetEnumerator()
        {
            return _listImplementation.GetEnumerator();
        }

        public void CopyTo(Array array, int index)
        {
            _listImplementation.CopyTo(array, index);
        }

        public int Count
        {
            get { return _listImplementation.Count; }
        }

        public bool IsSynchronized
        {
            get { return _listImplementation.IsSynchronized; }
        }

        public object SyncRoot
        {
            get { return _listImplementation.SyncRoot; }
        }

        public int Add(object? value)
        {
            return _listImplementation.Add(value);
        }

        public void Clear()
        {
            _listImplementation.Clear();
        }

        public bool Contains(object? value)
        {
            return _listImplementation.Contains(value);
        }

        public int IndexOf(object? value)
        {
            return _listImplementation.IndexOf(value);
        }

        public void Insert(int index, object? value)
        {
            _listImplementation.Insert(index, value);
        }

        public void Remove(object? value)
        {
            _listImplementation.Remove(value);
        }

        public void RemoveAt(int index)
        {
            _listImplementation.RemoveAt(index);
        }

        public bool IsFixedSize
        {
            get { return _listImplementation.IsFixedSize; }
        }

        public bool IsReadOnly
        {
            get { return _listImplementation.IsReadOnly; }
        }

        public object? this[int index]
        {
            get { return _listImplementation[index]; }
            set { _listImplementation[index] = value; }
        }
    }
}