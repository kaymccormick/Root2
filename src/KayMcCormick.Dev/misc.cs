     using System;
using System.Collections;
using System.Collections.Generic;

public class PocoSyntaxTokenList : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoSyntaxToken)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoSyntaxToken)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoSyntaxToken)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoSyntaxToken)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoSyntaxToken)value);
        // System.Collections.IList
        public void    RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly            => _list.IsReadOnly;
        public Boolean IsFixedSize           => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void    CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32   Count          => _list.Count;
        public Object  SyncRoot       => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        IList              _list = new List<PocoSyntaxToken>();
    }

    public class PocoSyntaxToken
    {
        public int RawKind { get; set; }

        public string Kind { get; set; }

        public object Value { get; set; }

        public string ValueText { get; set; }
    }
