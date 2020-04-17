using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
#if POCO1
namespace PocoSyntax
{
    public class PocoAccessorDeclarationSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoAccessorDeclarationSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoAccessorDeclarationSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoAccessorDeclarationSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoAccessorDeclarationSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoAccessorDeclarationSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoAccessorDeclarationSyntaxCollection(IList<PocoAccessorDeclarationSyntax> initList)
        {
            ((List<PocoAccessorDeclarationSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoAccessorDeclarationSyntax>();
    }

    public class PocoAccessorDeclarationSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoAttributeListSyntaxCollection AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxTokenList Modifiers
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Keyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoBlockSyntax Body
        {
            get;
            set;
        }
    }

    public class PocoAccessorListSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoAccessorListSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoAccessorListSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoAccessorListSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoAccessorListSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoAccessorListSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoAccessorListSyntaxCollection(IList<PocoAccessorListSyntax> initList)
        {
            ((List<PocoAccessorListSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoAccessorListSyntax>();
    }

    public class PocoAccessorListSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenBraceToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoAccessorDeclarationSyntaxCollection Accessors
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseBraceToken
        {
            get;
            set;
        }
    }

    public class PocoAliasQualifiedNameSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoAliasQualifiedNameSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoAliasQualifiedNameSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoAliasQualifiedNameSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoAliasQualifiedNameSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoAliasQualifiedNameSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoAliasQualifiedNameSyntaxCollection(IList<PocoAliasQualifiedNameSyntax> initList)
        {
            ((List<PocoAliasQualifiedNameSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoAliasQualifiedNameSyntax>();
    }

    public class PocoAliasQualifiedNameSyntax : PocoNameSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoIdentifierNameSyntax Alias
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken ColonColonToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSimpleNameSyntax Name
        {
            get;
            set;
        }
    }

    public class PocoAnonymousFunctionExpressionSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoAnonymousFunctionExpressionSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoAnonymousFunctionExpressionSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoAnonymousFunctionExpressionSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoAnonymousFunctionExpressionSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoAnonymousFunctionExpressionSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoAnonymousFunctionExpressionSyntaxCollection(IList<PocoAnonymousFunctionExpressionSyntax> initList)
        {
            ((List<PocoAnonymousFunctionExpressionSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoAnonymousFunctionExpressionSyntax>();
    }

    public class PocoAnonymousFunctionExpressionSyntax : PocoExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken AsyncKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoBlockSyntax Block
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax ExpressionBody
        {
            get;
            set;
        }
    }

    public class PocoAnonymousMethodExpressionSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoAnonymousMethodExpressionSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoAnonymousMethodExpressionSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoAnonymousMethodExpressionSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoAnonymousMethodExpressionSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoAnonymousMethodExpressionSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoAnonymousMethodExpressionSyntaxCollection(IList<PocoAnonymousMethodExpressionSyntax> initList)
        {
            ((List<PocoAnonymousMethodExpressionSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoAnonymousMethodExpressionSyntax>();
    }

    public class PocoAnonymousMethodExpressionSyntax : PocoAnonymousFunctionExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken AsyncKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken DelegateKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoParameterListSyntax ParameterList
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoBlockSyntax Block
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoExpressionSyntax ExpressionBody
        {
            get;
            set;
        }
    }

    public class PocoAnonymousObjectCreationExpressionSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoAnonymousObjectCreationExpressionSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoAnonymousObjectCreationExpressionSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoAnonymousObjectCreationExpressionSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoAnonymousObjectCreationExpressionSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoAnonymousObjectCreationExpressionSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoAnonymousObjectCreationExpressionSyntaxCollection(IList<PocoAnonymousObjectCreationExpressionSyntax> initList)
        {
            ((List<PocoAnonymousObjectCreationExpressionSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoAnonymousObjectCreationExpressionSyntax>();
    }

    public class PocoAnonymousObjectCreationExpressionSyntax : PocoExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken NewKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenBraceToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoAnonymousObjectMemberDeclaratorSyntaxCollection Initializers
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseBraceToken
        {
            get;
            set;
        }
    }

    public class PocoAnonymousObjectMemberDeclaratorSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoAnonymousObjectMemberDeclaratorSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoAnonymousObjectMemberDeclaratorSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoAnonymousObjectMemberDeclaratorSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoAnonymousObjectMemberDeclaratorSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoAnonymousObjectMemberDeclaratorSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoAnonymousObjectMemberDeclaratorSyntaxCollection(IList<PocoAnonymousObjectMemberDeclaratorSyntax> initList)
        {
            ((List<PocoAnonymousObjectMemberDeclaratorSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoAnonymousObjectMemberDeclaratorSyntax>();
    }

    public class PocoAnonymousObjectMemberDeclaratorSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoNameEqualsSyntax NameEquals
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Expression
        {
            get;
            set;
        }
    }

    public class PocoArgumentListSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoArgumentListSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoArgumentListSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoArgumentListSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoArgumentListSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoArgumentListSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoArgumentListSyntaxCollection(IList<PocoArgumentListSyntax> initList)
        {
            ((List<PocoArgumentListSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoArgumentListSyntax>();
    }

    public class PocoArgumentListSyntax : PocoBaseArgumentListSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenParenToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoArgumentSyntaxCollection Arguments
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseParenToken
        {
            get;
            set;
        }
    }

    public class PocoArgumentSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoArgumentSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoArgumentSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoArgumentSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoArgumentSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoArgumentSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoArgumentSyntaxCollection(IList<PocoArgumentSyntax> initList)
        {
            ((List<PocoArgumentSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoArgumentSyntax>();
    }

    public class PocoArgumentSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoNameColonSyntax NameColon
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken RefKindKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Expression
        {
            get;
            set;
        }
    }

    public class PocoArrayCreationExpressionSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoArrayCreationExpressionSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoArrayCreationExpressionSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoArrayCreationExpressionSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoArrayCreationExpressionSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoArrayCreationExpressionSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoArrayCreationExpressionSyntaxCollection(IList<PocoArrayCreationExpressionSyntax> initList)
        {
            ((List<PocoArrayCreationExpressionSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoArrayCreationExpressionSyntax>();
    }

    public class PocoArrayCreationExpressionSyntax : PocoExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken NewKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoArrayTypeSyntax Type
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoInitializerExpressionSyntax Initializer
        {
            get;
            set;
        }
    }

    public class PocoArrayRankSpecifierSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoArrayRankSpecifierSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoArrayRankSpecifierSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoArrayRankSpecifierSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoArrayRankSpecifierSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoArrayRankSpecifierSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoArrayRankSpecifierSyntaxCollection(IList<PocoArrayRankSpecifierSyntax> initList)
        {
            ((List<PocoArrayRankSpecifierSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoArrayRankSpecifierSyntax>();
    }

    public class PocoArrayRankSpecifierSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenBracketToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntaxCollection Sizes
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseBracketToken
        {
            get;
            set;
        }
    }

    public class PocoArrayTypeSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoArrayTypeSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoArrayTypeSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoArrayTypeSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoArrayTypeSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoArrayTypeSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoArrayTypeSyntaxCollection(IList<PocoArrayTypeSyntax> initList)
        {
            ((List<PocoArrayTypeSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoArrayTypeSyntax>();
    }

    public class PocoArrayTypeSyntax : PocoTypeSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoTypeSyntax ElementType
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoArrayRankSpecifierSyntaxCollection RankSpecifiers
        {
            get;
            set;
        }
    }

    public class PocoArrowExpressionClauseSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoArrowExpressionClauseSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoArrowExpressionClauseSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoArrowExpressionClauseSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoArrowExpressionClauseSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoArrowExpressionClauseSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoArrowExpressionClauseSyntaxCollection(IList<PocoArrowExpressionClauseSyntax> initList)
        {
            ((List<PocoArrowExpressionClauseSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoArrowExpressionClauseSyntax>();
    }

    public class PocoArrowExpressionClauseSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken ArrowToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Expression
        {
            get;
            set;
        }
    }

    public class PocoAssignmentExpressionSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoAssignmentExpressionSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoAssignmentExpressionSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoAssignmentExpressionSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoAssignmentExpressionSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoAssignmentExpressionSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoAssignmentExpressionSyntaxCollection(IList<PocoAssignmentExpressionSyntax> initList)
        {
            ((List<PocoAssignmentExpressionSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoAssignmentExpressionSyntax>();
    }

    public class PocoAssignmentExpressionSyntax : PocoExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Left
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OperatorToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Right
        {
            get;
            set;
        }
    }

    public class PocoAttributeArgumentListSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoAttributeArgumentListSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoAttributeArgumentListSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoAttributeArgumentListSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoAttributeArgumentListSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoAttributeArgumentListSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoAttributeArgumentListSyntaxCollection(IList<PocoAttributeArgumentListSyntax> initList)
        {
            ((List<PocoAttributeArgumentListSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoAttributeArgumentListSyntax>();
    }

    public class PocoAttributeArgumentListSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenParenToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoAttributeArgumentSyntaxCollection Arguments
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseParenToken
        {
            get;
            set;
        }
    }

    public class PocoAttributeArgumentSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoAttributeArgumentSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoAttributeArgumentSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoAttributeArgumentSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoAttributeArgumentSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoAttributeArgumentSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoAttributeArgumentSyntaxCollection(IList<PocoAttributeArgumentSyntax> initList)
        {
            ((List<PocoAttributeArgumentSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoAttributeArgumentSyntax>();
    }

    public class PocoAttributeArgumentSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Expression
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoNameEqualsSyntax NameEquals
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoNameColonSyntax NameColon
        {
            get;
            set;
        }
    }

    public class PocoAttributeListSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoAttributeListSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoAttributeListSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoAttributeListSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoAttributeListSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoAttributeListSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoAttributeListSyntaxCollection(IList<PocoAttributeListSyntax> initList)
        {
            ((List<PocoAttributeListSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoAttributeListSyntax>();
    }

    public class PocoAttributeListSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenBracketToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoAttributeTargetSpecifierSyntax Target
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoAttributeSyntaxCollection Attributes
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseBracketToken
        {
            get;
            set;
        }
    }

    public class PocoAttributeSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoAttributeSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoAttributeSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoAttributeSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoAttributeSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoAttributeSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoAttributeSyntaxCollection(IList<PocoAttributeSyntax> initList)
        {
            ((List<PocoAttributeSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoAttributeSyntax>();
    }

    public class PocoAttributeSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoNameSyntax Name
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoAttributeArgumentListSyntax ArgumentList
        {
            get;
            set;
        }
    }

    public class PocoAttributeTargetSpecifierSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoAttributeTargetSpecifierSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoAttributeTargetSpecifierSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoAttributeTargetSpecifierSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoAttributeTargetSpecifierSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoAttributeTargetSpecifierSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoAttributeTargetSpecifierSyntaxCollection(IList<PocoAttributeTargetSpecifierSyntax> initList)
        {
            ((List<PocoAttributeTargetSpecifierSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoAttributeTargetSpecifierSyntax>();
    }

    public class PocoAttributeTargetSpecifierSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Identifier
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken ColonToken
        {
            get;
            set;
        }
    }

    public class PocoAwaitExpressionSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoAwaitExpressionSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoAwaitExpressionSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoAwaitExpressionSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoAwaitExpressionSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoAwaitExpressionSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoAwaitExpressionSyntaxCollection(IList<PocoAwaitExpressionSyntax> initList)
        {
            ((List<PocoAwaitExpressionSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoAwaitExpressionSyntax>();
    }

    public class PocoAwaitExpressionSyntax : PocoExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken AwaitKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Expression
        {
            get;
            set;
        }
    }

    public class PocoBadDirectiveTriviaSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoBadDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoBadDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoBadDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoBadDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoBadDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoBadDirectiveTriviaSyntaxCollection(IList<PocoBadDirectiveTriviaSyntax> initList)
        {
            ((List<PocoBadDirectiveTriviaSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoBadDirectiveTriviaSyntax>();
    }

    public class PocoBadDirectiveTriviaSyntax : PocoDirectiveTriviaSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken HashToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Identifier
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken EndOfDirectiveToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override bool IsActive
        {
            get;
            set;
        }
    }

    public class PocoBaseArgumentListSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoBaseArgumentListSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoBaseArgumentListSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoBaseArgumentListSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoBaseArgumentListSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoBaseArgumentListSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoBaseArgumentListSyntaxCollection(IList<PocoBaseArgumentListSyntax> initList)
        {
            ((List<PocoBaseArgumentListSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoBaseArgumentListSyntax>();
    }

    public class PocoBaseArgumentListSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoArgumentSyntaxCollection Arguments
        {
            get;
            set;
        }
    }

    public class PocoBaseCrefParameterListSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoBaseCrefParameterListSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoBaseCrefParameterListSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoBaseCrefParameterListSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoBaseCrefParameterListSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoBaseCrefParameterListSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoBaseCrefParameterListSyntaxCollection(IList<PocoBaseCrefParameterListSyntax> initList)
        {
            ((List<PocoBaseCrefParameterListSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoBaseCrefParameterListSyntax>();
    }

    public class PocoBaseCrefParameterListSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoCrefParameterSyntaxCollection Parameters
        {
            get;
            set;
        }
    }

    public class PocoBaseExpressionSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoBaseExpressionSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoBaseExpressionSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoBaseExpressionSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoBaseExpressionSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoBaseExpressionSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoBaseExpressionSyntaxCollection(IList<PocoBaseExpressionSyntax> initList)
        {
            ((List<PocoBaseExpressionSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoBaseExpressionSyntax>();
    }

    public class PocoBaseExpressionSyntax : PocoInstanceExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Token
        {
            get;
            set;
        }
    }

    public class PocoBaseFieldDeclarationSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoBaseFieldDeclarationSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoBaseFieldDeclarationSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoBaseFieldDeclarationSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoBaseFieldDeclarationSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoBaseFieldDeclarationSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoBaseFieldDeclarationSyntaxCollection(IList<PocoBaseFieldDeclarationSyntax> initList)
        {
            ((List<PocoBaseFieldDeclarationSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoBaseFieldDeclarationSyntax>();
    }

    public class PocoBaseFieldDeclarationSyntax : PocoMemberDeclarationSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoVariableDeclarationSyntax Declaration
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken SemicolonToken
        {
            get;
            set;
        }
    }

    public class PocoBaseListSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoBaseListSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoBaseListSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoBaseListSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoBaseListSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoBaseListSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoBaseListSyntaxCollection(IList<PocoBaseListSyntax> initList)
        {
            ((List<PocoBaseListSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoBaseListSyntax>();
    }

    public class PocoBaseListSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken ColonToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoBaseTypeSyntaxCollection Types
        {
            get;
            set;
        }
    }

    public class PocoBaseMethodDeclarationSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoBaseMethodDeclarationSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoBaseMethodDeclarationSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoBaseMethodDeclarationSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoBaseMethodDeclarationSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoBaseMethodDeclarationSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoBaseMethodDeclarationSyntaxCollection(IList<PocoBaseMethodDeclarationSyntax> initList)
        {
            ((List<PocoBaseMethodDeclarationSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoBaseMethodDeclarationSyntax>();
    }

    public class PocoBaseMethodDeclarationSyntax : PocoMemberDeclarationSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoParameterListSyntax ParameterList
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoBlockSyntax Body
        {
            get;
            set;
        }
    }

    public class PocoBaseParameterListSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoBaseParameterListSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoBaseParameterListSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoBaseParameterListSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoBaseParameterListSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoBaseParameterListSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoBaseParameterListSyntaxCollection(IList<PocoBaseParameterListSyntax> initList)
        {
            ((List<PocoBaseParameterListSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoBaseParameterListSyntax>();
    }

    public class PocoBaseParameterListSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoParameterSyntaxCollection Parameters
        {
            get;
            set;
        }
    }

    public class PocoBasePropertyDeclarationSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoBasePropertyDeclarationSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoBasePropertyDeclarationSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoBasePropertyDeclarationSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoBasePropertyDeclarationSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoBasePropertyDeclarationSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoBasePropertyDeclarationSyntaxCollection(IList<PocoBasePropertyDeclarationSyntax> initList)
        {
            ((List<PocoBasePropertyDeclarationSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoBasePropertyDeclarationSyntax>();
    }

    public class PocoBasePropertyDeclarationSyntax : PocoMemberDeclarationSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoTypeSyntax Type
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExplicitInterfaceSpecifierSyntax ExplicitInterfaceSpecifier
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoAccessorListSyntax AccessorList
        {
            get;
            set;
        }
    }

    public class PocoBaseTypeDeclarationSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoBaseTypeDeclarationSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoBaseTypeDeclarationSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoBaseTypeDeclarationSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoBaseTypeDeclarationSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoBaseTypeDeclarationSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoBaseTypeDeclarationSyntaxCollection(IList<PocoBaseTypeDeclarationSyntax> initList)
        {
            ((List<PocoBaseTypeDeclarationSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoBaseTypeDeclarationSyntax>();
    }

    public class PocoBaseTypeDeclarationSyntax : PocoMemberDeclarationSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Identifier
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoBaseListSyntax BaseList
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenBraceToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseBraceToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken SemicolonToken
        {
            get;
            set;
        }
    }

    public class PocoBaseTypeSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoBaseTypeSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoBaseTypeSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoBaseTypeSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoBaseTypeSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoBaseTypeSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoBaseTypeSyntaxCollection(IList<PocoBaseTypeSyntax> initList)
        {
            ((List<PocoBaseTypeSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoBaseTypeSyntax>();
    }

    public class PocoBaseTypeSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoTypeSyntax Type
        {
            get;
            set;
        }
    }

    public class PocoBinaryExpressionSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoBinaryExpressionSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoBinaryExpressionSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoBinaryExpressionSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoBinaryExpressionSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoBinaryExpressionSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoBinaryExpressionSyntaxCollection(IList<PocoBinaryExpressionSyntax> initList)
        {
            ((List<PocoBinaryExpressionSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoBinaryExpressionSyntax>();
    }

    public class PocoBinaryExpressionSyntax : PocoExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Left
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OperatorToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Right
        {
            get;
            set;
        }
    }

    public class PocoBlockSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoBlockSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoBlockSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoBlockSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoBlockSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoBlockSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoBlockSyntaxCollection(IList<PocoBlockSyntax> initList)
        {
            ((List<PocoBlockSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoBlockSyntax>();
    }

    public class PocoBlockSyntax : PocoStatementSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoAttributeListSyntaxCollection AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenBraceToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoStatementSyntaxCollection Statements
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseBraceToken
        {
            get;
            set;
        }
    }

    public class PocoBracketedArgumentListSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoBracketedArgumentListSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoBracketedArgumentListSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoBracketedArgumentListSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoBracketedArgumentListSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoBracketedArgumentListSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoBracketedArgumentListSyntaxCollection(IList<PocoBracketedArgumentListSyntax> initList)
        {
            ((List<PocoBracketedArgumentListSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoBracketedArgumentListSyntax>();
    }

    public class PocoBracketedArgumentListSyntax : PocoBaseArgumentListSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenBracketToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoArgumentSyntaxCollection Arguments
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseBracketToken
        {
            get;
            set;
        }
    }

    public class PocoBracketedParameterListSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoBracketedParameterListSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoBracketedParameterListSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoBracketedParameterListSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoBracketedParameterListSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoBracketedParameterListSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoBracketedParameterListSyntaxCollection(IList<PocoBracketedParameterListSyntax> initList)
        {
            ((List<PocoBracketedParameterListSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoBracketedParameterListSyntax>();
    }

    public class PocoBracketedParameterListSyntax : PocoBaseParameterListSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenBracketToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoParameterSyntaxCollection Parameters
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseBracketToken
        {
            get;
            set;
        }
    }

    public class PocoBranchingDirectiveTriviaSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoBranchingDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoBranchingDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoBranchingDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoBranchingDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoBranchingDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoBranchingDirectiveTriviaSyntaxCollection(IList<PocoBranchingDirectiveTriviaSyntax> initList)
        {
            ((List<PocoBranchingDirectiveTriviaSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoBranchingDirectiveTriviaSyntax>();
    }

    public class PocoBranchingDirectiveTriviaSyntax : PocoDirectiveTriviaSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual bool BranchTaken
        {
            get;
            set;
        }
    }

    public class PocoBreakStatementSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoBreakStatementSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoBreakStatementSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoBreakStatementSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoBreakStatementSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoBreakStatementSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoBreakStatementSyntaxCollection(IList<PocoBreakStatementSyntax> initList)
        {
            ((List<PocoBreakStatementSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoBreakStatementSyntax>();
    }

    public class PocoBreakStatementSyntax : PocoStatementSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoAttributeListSyntaxCollection AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken BreakKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken SemicolonToken
        {
            get;
            set;
        }
    }

    public class PocoCasePatternSwitchLabelSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoCasePatternSwitchLabelSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoCasePatternSwitchLabelSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoCasePatternSwitchLabelSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoCasePatternSwitchLabelSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoCasePatternSwitchLabelSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoCasePatternSwitchLabelSyntaxCollection(IList<PocoCasePatternSwitchLabelSyntax> initList)
        {
            ((List<PocoCasePatternSwitchLabelSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoCasePatternSwitchLabelSyntax>();
    }

    public class PocoCasePatternSwitchLabelSyntax : PocoSwitchLabelSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken Keyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoPatternSyntax Pattern
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoWhenClauseSyntax WhenClause
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken ColonToken
        {
            get;
            set;
        }
    }

    public class PocoCaseSwitchLabelSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoCaseSwitchLabelSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoCaseSwitchLabelSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoCaseSwitchLabelSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoCaseSwitchLabelSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoCaseSwitchLabelSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoCaseSwitchLabelSyntaxCollection(IList<PocoCaseSwitchLabelSyntax> initList)
        {
            ((List<PocoCaseSwitchLabelSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoCaseSwitchLabelSyntax>();
    }

    public class PocoCaseSwitchLabelSyntax : PocoSwitchLabelSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken Keyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Value
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken ColonToken
        {
            get;
            set;
        }
    }

    public class PocoCastExpressionSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoCastExpressionSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoCastExpressionSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoCastExpressionSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoCastExpressionSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoCastExpressionSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoCastExpressionSyntaxCollection(IList<PocoCastExpressionSyntax> initList)
        {
            ((List<PocoCastExpressionSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoCastExpressionSyntax>();
    }

    public class PocoCastExpressionSyntax : PocoExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenParenToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoTypeSyntax Type
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseParenToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Expression
        {
            get;
            set;
        }
    }

    public class PocoCatchClauseSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoCatchClauseSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoCatchClauseSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoCatchClauseSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoCatchClauseSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoCatchClauseSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoCatchClauseSyntaxCollection(IList<PocoCatchClauseSyntax> initList)
        {
            ((List<PocoCatchClauseSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoCatchClauseSyntax>();
    }

    public class PocoCatchClauseSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CatchKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoCatchDeclarationSyntax Declaration
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoCatchFilterClauseSyntax Filter
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoBlockSyntax Block
        {
            get;
            set;
        }
    }

    public class PocoCatchDeclarationSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoCatchDeclarationSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoCatchDeclarationSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoCatchDeclarationSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoCatchDeclarationSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoCatchDeclarationSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoCatchDeclarationSyntaxCollection(IList<PocoCatchDeclarationSyntax> initList)
        {
            ((List<PocoCatchDeclarationSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoCatchDeclarationSyntax>();
    }

    public class PocoCatchDeclarationSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenParenToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoTypeSyntax Type
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Identifier
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseParenToken
        {
            get;
            set;
        }
    }

    public class PocoCatchFilterClauseSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoCatchFilterClauseSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoCatchFilterClauseSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoCatchFilterClauseSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoCatchFilterClauseSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoCatchFilterClauseSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoCatchFilterClauseSyntaxCollection(IList<PocoCatchFilterClauseSyntax> initList)
        {
            ((List<PocoCatchFilterClauseSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoCatchFilterClauseSyntax>();
    }

    public class PocoCatchFilterClauseSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken WhenKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenParenToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax FilterExpression
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseParenToken
        {
            get;
            set;
        }
    }

    public class PocoCheckedExpressionSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoCheckedExpressionSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoCheckedExpressionSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoCheckedExpressionSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoCheckedExpressionSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoCheckedExpressionSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoCheckedExpressionSyntaxCollection(IList<PocoCheckedExpressionSyntax> initList)
        {
            ((List<PocoCheckedExpressionSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoCheckedExpressionSyntax>();
    }

    public class PocoCheckedExpressionSyntax : PocoExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Keyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenParenToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Expression
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseParenToken
        {
            get;
            set;
        }
    }

    public class PocoCheckedStatementSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoCheckedStatementSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoCheckedStatementSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoCheckedStatementSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoCheckedStatementSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoCheckedStatementSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoCheckedStatementSyntaxCollection(IList<PocoCheckedStatementSyntax> initList)
        {
            ((List<PocoCheckedStatementSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoCheckedStatementSyntax>();
    }

    public class PocoCheckedStatementSyntax : PocoStatementSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoAttributeListSyntaxCollection AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Keyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoBlockSyntax Block
        {
            get;
            set;
        }
    }

    public class PocoClassDeclarationSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoClassDeclarationSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoClassDeclarationSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoClassDeclarationSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoClassDeclarationSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoClassDeclarationSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoClassDeclarationSyntaxCollection(IList<PocoClassDeclarationSyntax> initList)
        {
            ((List<PocoClassDeclarationSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoClassDeclarationSyntax>();
    }

    public class PocoClassDeclarationSyntax : PocoTypeDeclarationSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoAttributeListSyntaxCollection AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxTokenList Modifiers
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken Keyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken Identifier
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoTypeParameterListSyntax TypeParameterList
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoBaseListSyntax BaseList
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoTypeParameterConstraintClauseSyntaxCollection ConstraintClauses
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken OpenBraceToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoMemberDeclarationSyntaxCollection Members
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken CloseBraceToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken SemicolonToken
        {
            get;
            set;
        }
    }

    public class PocoClassOrStructConstraintSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoClassOrStructConstraintSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoClassOrStructConstraintSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoClassOrStructConstraintSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoClassOrStructConstraintSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoClassOrStructConstraintSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoClassOrStructConstraintSyntaxCollection(IList<PocoClassOrStructConstraintSyntax> initList)
        {
            ((List<PocoClassOrStructConstraintSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoClassOrStructConstraintSyntax>();
    }

    public class PocoClassOrStructConstraintSyntax : PocoTypeParameterConstraintSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken ClassOrStructKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken QuestionToken
        {
            get;
            set;
        }
    }

    public class PocoCommonForEachStatementSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoCommonForEachStatementSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoCommonForEachStatementSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoCommonForEachStatementSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoCommonForEachStatementSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoCommonForEachStatementSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoCommonForEachStatementSyntaxCollection(IList<PocoCommonForEachStatementSyntax> initList)
        {
            ((List<PocoCommonForEachStatementSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoCommonForEachStatementSyntax>();
    }

    public class PocoCommonForEachStatementSyntax : PocoStatementSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken AwaitKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken ForEachKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenParenToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken InKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Expression
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseParenToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoStatementSyntax Statement
        {
            get;
            set;
        }
    }

    public class PocoCompilationUnitSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoCompilationUnitSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoCompilationUnitSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoCompilationUnitSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoCompilationUnitSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoCompilationUnitSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoCompilationUnitSyntaxCollection(IList<PocoCompilationUnitSyntax> initList)
        {
            ((List<PocoCompilationUnitSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoCompilationUnitSyntax>();
    }

    public class PocoCompilationUnitSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExternAliasDirectiveSyntaxCollection Externs
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoUsingDirectiveSyntaxCollection Usings
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoAttributeListSyntaxCollection AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoMemberDeclarationSyntaxCollection Members
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken EndOfFileToken
        {
            get;
            set;
        }
    }

    public class PocoConditionalAccessExpressionSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoConditionalAccessExpressionSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoConditionalAccessExpressionSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoConditionalAccessExpressionSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoConditionalAccessExpressionSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoConditionalAccessExpressionSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoConditionalAccessExpressionSyntaxCollection(IList<PocoConditionalAccessExpressionSyntax> initList)
        {
            ((List<PocoConditionalAccessExpressionSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoConditionalAccessExpressionSyntax>();
    }

    public class PocoConditionalAccessExpressionSyntax : PocoExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Expression
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OperatorToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax WhenNotNull
        {
            get;
            set;
        }
    }

    public class PocoConditionalDirectiveTriviaSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoConditionalDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoConditionalDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoConditionalDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoConditionalDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoConditionalDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoConditionalDirectiveTriviaSyntaxCollection(IList<PocoConditionalDirectiveTriviaSyntax> initList)
        {
            ((List<PocoConditionalDirectiveTriviaSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoConditionalDirectiveTriviaSyntax>();
    }

    public class PocoConditionalDirectiveTriviaSyntax : PocoBranchingDirectiveTriviaSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Condition
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual bool ConditionValue
        {
            get;
            set;
        }
    }

    public class PocoConditionalExpressionSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoConditionalExpressionSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoConditionalExpressionSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoConditionalExpressionSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoConditionalExpressionSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoConditionalExpressionSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoConditionalExpressionSyntaxCollection(IList<PocoConditionalExpressionSyntax> initList)
        {
            ((List<PocoConditionalExpressionSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoConditionalExpressionSyntax>();
    }

    public class PocoConditionalExpressionSyntax : PocoExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Condition
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken QuestionToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax WhenTrue
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken ColonToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax WhenFalse
        {
            get;
            set;
        }
    }

    public class PocoConstantPatternSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoConstantPatternSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoConstantPatternSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoConstantPatternSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoConstantPatternSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoConstantPatternSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoConstantPatternSyntaxCollection(IList<PocoConstantPatternSyntax> initList)
        {
            ((List<PocoConstantPatternSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoConstantPatternSyntax>();
    }

    public class PocoConstantPatternSyntax : PocoPatternSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Expression
        {
            get;
            set;
        }
    }

    public class PocoConstructorConstraintSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoConstructorConstraintSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoConstructorConstraintSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoConstructorConstraintSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoConstructorConstraintSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoConstructorConstraintSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoConstructorConstraintSyntaxCollection(IList<PocoConstructorConstraintSyntax> initList)
        {
            ((List<PocoConstructorConstraintSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoConstructorConstraintSyntax>();
    }

    public class PocoConstructorConstraintSyntax : PocoTypeParameterConstraintSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken NewKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenParenToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseParenToken
        {
            get;
            set;
        }
    }

    public class PocoConstructorDeclarationSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoConstructorDeclarationSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoConstructorDeclarationSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoConstructorDeclarationSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoConstructorDeclarationSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoConstructorDeclarationSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoConstructorDeclarationSyntaxCollection(IList<PocoConstructorDeclarationSyntax> initList)
        {
            ((List<PocoConstructorDeclarationSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoConstructorDeclarationSyntax>();
    }

    public class PocoConstructorDeclarationSyntax : PocoBaseMethodDeclarationSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoAttributeListSyntaxCollection AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxTokenList Modifiers
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Identifier
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoParameterListSyntax ParameterList
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoConstructorInitializerSyntax Initializer
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoBlockSyntax Body
        {
            get;
            set;
        }
    }

    public class PocoConstructorInitializerSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoConstructorInitializerSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoConstructorInitializerSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoConstructorInitializerSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoConstructorInitializerSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoConstructorInitializerSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoConstructorInitializerSyntaxCollection(IList<PocoConstructorInitializerSyntax> initList)
        {
            ((List<PocoConstructorInitializerSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoConstructorInitializerSyntax>();
    }

    public class PocoConstructorInitializerSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken ColonToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken ThisOrBaseKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoArgumentListSyntax ArgumentList
        {
            get;
            set;
        }
    }

    public class PocoContinueStatementSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoContinueStatementSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoContinueStatementSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoContinueStatementSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoContinueStatementSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoContinueStatementSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoContinueStatementSyntaxCollection(IList<PocoContinueStatementSyntax> initList)
        {
            ((List<PocoContinueStatementSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoContinueStatementSyntax>();
    }

    public class PocoContinueStatementSyntax : PocoStatementSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoAttributeListSyntaxCollection AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken ContinueKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken SemicolonToken
        {
            get;
            set;
        }
    }

    public class PocoConversionOperatorDeclarationSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoConversionOperatorDeclarationSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoConversionOperatorDeclarationSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoConversionOperatorDeclarationSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoConversionOperatorDeclarationSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoConversionOperatorDeclarationSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoConversionOperatorDeclarationSyntaxCollection(IList<PocoConversionOperatorDeclarationSyntax> initList)
        {
            ((List<PocoConversionOperatorDeclarationSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoConversionOperatorDeclarationSyntax>();
    }

    public class PocoConversionOperatorDeclarationSyntax : PocoBaseMethodDeclarationSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoAttributeListSyntaxCollection AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxTokenList Modifiers
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken ImplicitOrExplicitKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OperatorKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoTypeSyntax Type
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoParameterListSyntax ParameterList
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoBlockSyntax Body
        {
            get;
            set;
        }
    }

    public class PocoConversionOperatorMemberCrefSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoConversionOperatorMemberCrefSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoConversionOperatorMemberCrefSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoConversionOperatorMemberCrefSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoConversionOperatorMemberCrefSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoConversionOperatorMemberCrefSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoConversionOperatorMemberCrefSyntaxCollection(IList<PocoConversionOperatorMemberCrefSyntax> initList)
        {
            ((List<PocoConversionOperatorMemberCrefSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoConversionOperatorMemberCrefSyntax>();
    }

    public class PocoConversionOperatorMemberCrefSyntax : PocoMemberCrefSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken ImplicitOrExplicitKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OperatorKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoTypeSyntax Type
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoCrefParameterListSyntax Parameters
        {
            get;
            set;
        }
    }

    public class PocoCrefBracketedParameterListSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoCrefBracketedParameterListSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoCrefBracketedParameterListSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoCrefBracketedParameterListSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoCrefBracketedParameterListSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoCrefBracketedParameterListSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoCrefBracketedParameterListSyntaxCollection(IList<PocoCrefBracketedParameterListSyntax> initList)
        {
            ((List<PocoCrefBracketedParameterListSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoCrefBracketedParameterListSyntax>();
    }

    public class PocoCrefBracketedParameterListSyntax : PocoBaseCrefParameterListSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenBracketToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoCrefParameterSyntaxCollection Parameters
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseBracketToken
        {
            get;
            set;
        }
    }

    public class PocoCrefParameterListSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoCrefParameterListSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoCrefParameterListSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoCrefParameterListSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoCrefParameterListSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoCrefParameterListSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoCrefParameterListSyntaxCollection(IList<PocoCrefParameterListSyntax> initList)
        {
            ((List<PocoCrefParameterListSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoCrefParameterListSyntax>();
    }

    public class PocoCrefParameterListSyntax : PocoBaseCrefParameterListSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenParenToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoCrefParameterSyntaxCollection Parameters
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseParenToken
        {
            get;
            set;
        }
    }

    public class PocoCrefParameterSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoCrefParameterSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoCrefParameterSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoCrefParameterSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoCrefParameterSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoCrefParameterSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoCrefParameterSyntaxCollection(IList<PocoCrefParameterSyntax> initList)
        {
            ((List<PocoCrefParameterSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoCrefParameterSyntax>();
    }

    public class PocoCrefParameterSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken RefKindKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoTypeSyntax Type
        {
            get;
            set;
        }
    }

    public class PocoCrefSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoCrefSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoCrefSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoCrefSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoCrefSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoCrefSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoCrefSyntaxCollection(IList<PocoCrefSyntax> initList)
        {
            ((List<PocoCrefSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoCrefSyntax>();
    }

    public class PocoCrefSyntax : PocoCSharpSyntaxNode
    {
    }

    public class PocoCSharpSyntaxNodeCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoCSharpSyntaxNode)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoCSharpSyntaxNode)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoCSharpSyntaxNode)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoCSharpSyntaxNode)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoCSharpSyntaxNode)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoCSharpSyntaxNodeCollection(IList<PocoCSharpSyntaxNode> initList)
        {
            ((List<PocoCSharpSyntaxNode>)_list).AddRange(initList);
        }

        IList _list = new List<PocoCSharpSyntaxNode>();
    }

    public class PocoCSharpSyntaxNode
    {
    }

    public class PocoDeclarationExpressionSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoDeclarationExpressionSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoDeclarationExpressionSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoDeclarationExpressionSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoDeclarationExpressionSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoDeclarationExpressionSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoDeclarationExpressionSyntaxCollection(IList<PocoDeclarationExpressionSyntax> initList)
        {
            ((List<PocoDeclarationExpressionSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoDeclarationExpressionSyntax>();
    }

    public class PocoDeclarationExpressionSyntax : PocoExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoTypeSyntax Type
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoVariableDesignationSyntax Designation
        {
            get;
            set;
        }
    }

    public class PocoDeclarationPatternSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoDeclarationPatternSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoDeclarationPatternSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoDeclarationPatternSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoDeclarationPatternSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoDeclarationPatternSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoDeclarationPatternSyntaxCollection(IList<PocoDeclarationPatternSyntax> initList)
        {
            ((List<PocoDeclarationPatternSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoDeclarationPatternSyntax>();
    }

    public class PocoDeclarationPatternSyntax : PocoPatternSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoTypeSyntax Type
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoVariableDesignationSyntax Designation
        {
            get;
            set;
        }
    }

    public class PocoDefaultExpressionSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoDefaultExpressionSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoDefaultExpressionSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoDefaultExpressionSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoDefaultExpressionSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoDefaultExpressionSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoDefaultExpressionSyntaxCollection(IList<PocoDefaultExpressionSyntax> initList)
        {
            ((List<PocoDefaultExpressionSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoDefaultExpressionSyntax>();
    }

    public class PocoDefaultExpressionSyntax : PocoExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Keyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenParenToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoTypeSyntax Type
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseParenToken
        {
            get;
            set;
        }
    }

    public class PocoDefaultSwitchLabelSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoDefaultSwitchLabelSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoDefaultSwitchLabelSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoDefaultSwitchLabelSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoDefaultSwitchLabelSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoDefaultSwitchLabelSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoDefaultSwitchLabelSyntaxCollection(IList<PocoDefaultSwitchLabelSyntax> initList)
        {
            ((List<PocoDefaultSwitchLabelSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoDefaultSwitchLabelSyntax>();
    }

    public class PocoDefaultSwitchLabelSyntax : PocoSwitchLabelSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken Keyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken ColonToken
        {
            get;
            set;
        }
    }

    public class PocoDefineDirectiveTriviaSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoDefineDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoDefineDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoDefineDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoDefineDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoDefineDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoDefineDirectiveTriviaSyntaxCollection(IList<PocoDefineDirectiveTriviaSyntax> initList)
        {
            ((List<PocoDefineDirectiveTriviaSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoDefineDirectiveTriviaSyntax>();
    }

    public class PocoDefineDirectiveTriviaSyntax : PocoDirectiveTriviaSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken HashToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken DefineKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Name
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken EndOfDirectiveToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override bool IsActive
        {
            get;
            set;
        }
    }

    public class PocoDelegateDeclarationSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoDelegateDeclarationSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoDelegateDeclarationSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoDelegateDeclarationSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoDelegateDeclarationSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoDelegateDeclarationSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoDelegateDeclarationSyntaxCollection(IList<PocoDelegateDeclarationSyntax> initList)
        {
            ((List<PocoDelegateDeclarationSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoDelegateDeclarationSyntax>();
    }

    public class PocoDelegateDeclarationSyntax : PocoMemberDeclarationSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoAttributeListSyntaxCollection AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxTokenList Modifiers
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken DelegateKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoTypeSyntax ReturnType
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Identifier
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoTypeParameterListSyntax TypeParameterList
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoParameterListSyntax ParameterList
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoTypeParameterConstraintClauseSyntaxCollection ConstraintClauses
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken SemicolonToken
        {
            get;
            set;
        }
    }

    public class PocoDestructorDeclarationSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoDestructorDeclarationSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoDestructorDeclarationSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoDestructorDeclarationSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoDestructorDeclarationSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoDestructorDeclarationSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoDestructorDeclarationSyntaxCollection(IList<PocoDestructorDeclarationSyntax> initList)
        {
            ((List<PocoDestructorDeclarationSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoDestructorDeclarationSyntax>();
    }

    public class PocoDestructorDeclarationSyntax : PocoBaseMethodDeclarationSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoAttributeListSyntaxCollection AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxTokenList Modifiers
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken TildeToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Identifier
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoParameterListSyntax ParameterList
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoBlockSyntax Body
        {
            get;
            set;
        }
    }

    public class PocoDirectiveTriviaSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoDirectiveTriviaSyntaxCollection(IList<PocoDirectiveTriviaSyntax> initList)
        {
            ((List<PocoDirectiveTriviaSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoDirectiveTriviaSyntax>();
    }

    public class PocoDirectiveTriviaSyntax : PocoStructuredTriviaSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual bool IsActive
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken HashToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken EndOfDirectiveToken
        {
            get;
            set;
        }
    }

    public class PocoDiscardDesignationSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoDiscardDesignationSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoDiscardDesignationSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoDiscardDesignationSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoDiscardDesignationSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoDiscardDesignationSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoDiscardDesignationSyntaxCollection(IList<PocoDiscardDesignationSyntax> initList)
        {
            ((List<PocoDiscardDesignationSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoDiscardDesignationSyntax>();
    }

    public class PocoDiscardDesignationSyntax : PocoVariableDesignationSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken UnderscoreToken
        {
            get;
            set;
        }
    }

    public class PocoDiscardPatternSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoDiscardPatternSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoDiscardPatternSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoDiscardPatternSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoDiscardPatternSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoDiscardPatternSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoDiscardPatternSyntaxCollection(IList<PocoDiscardPatternSyntax> initList)
        {
            ((List<PocoDiscardPatternSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoDiscardPatternSyntax>();
    }

    public class PocoDiscardPatternSyntax : PocoPatternSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken UnderscoreToken
        {
            get;
            set;
        }
    }

    public class PocoDocumentationCommentTriviaSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoDocumentationCommentTriviaSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoDocumentationCommentTriviaSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoDocumentationCommentTriviaSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoDocumentationCommentTriviaSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoDocumentationCommentTriviaSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoDocumentationCommentTriviaSyntaxCollection(IList<PocoDocumentationCommentTriviaSyntax> initList)
        {
            ((List<PocoDocumentationCommentTriviaSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoDocumentationCommentTriviaSyntax>();
    }

    public class PocoDocumentationCommentTriviaSyntax : PocoStructuredTriviaSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoXmlNodeSyntaxCollection Content
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken EndOfComment
        {
            get;
            set;
        }
    }

    public class PocoDoStatementSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoDoStatementSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoDoStatementSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoDoStatementSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoDoStatementSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoDoStatementSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoDoStatementSyntaxCollection(IList<PocoDoStatementSyntax> initList)
        {
            ((List<PocoDoStatementSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoDoStatementSyntax>();
    }

    public class PocoDoStatementSyntax : PocoStatementSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoAttributeListSyntaxCollection AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken DoKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoStatementSyntax Statement
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken WhileKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenParenToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Condition
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseParenToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken SemicolonToken
        {
            get;
            set;
        }
    }

    public class PocoElementAccessExpressionSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoElementAccessExpressionSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoElementAccessExpressionSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoElementAccessExpressionSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoElementAccessExpressionSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoElementAccessExpressionSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoElementAccessExpressionSyntaxCollection(IList<PocoElementAccessExpressionSyntax> initList)
        {
            ((List<PocoElementAccessExpressionSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoElementAccessExpressionSyntax>();
    }

    public class PocoElementAccessExpressionSyntax : PocoExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Expression
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoBracketedArgumentListSyntax ArgumentList
        {
            get;
            set;
        }
    }

    public class PocoElementBindingExpressionSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoElementBindingExpressionSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoElementBindingExpressionSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoElementBindingExpressionSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoElementBindingExpressionSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoElementBindingExpressionSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoElementBindingExpressionSyntaxCollection(IList<PocoElementBindingExpressionSyntax> initList)
        {
            ((List<PocoElementBindingExpressionSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoElementBindingExpressionSyntax>();
    }

    public class PocoElementBindingExpressionSyntax : PocoExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoBracketedArgumentListSyntax ArgumentList
        {
            get;
            set;
        }
    }

    public class PocoElifDirectiveTriviaSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoElifDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoElifDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoElifDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoElifDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoElifDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoElifDirectiveTriviaSyntaxCollection(IList<PocoElifDirectiveTriviaSyntax> initList)
        {
            ((List<PocoElifDirectiveTriviaSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoElifDirectiveTriviaSyntax>();
    }

    public class PocoElifDirectiveTriviaSyntax : PocoConditionalDirectiveTriviaSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken HashToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken ElifKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoExpressionSyntax Condition
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken EndOfDirectiveToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override bool IsActive
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override bool BranchTaken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override bool ConditionValue
        {
            get;
            set;
        }
    }

    public class PocoElseClauseSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoElseClauseSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoElseClauseSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoElseClauseSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoElseClauseSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoElseClauseSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoElseClauseSyntaxCollection(IList<PocoElseClauseSyntax> initList)
        {
            ((List<PocoElseClauseSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoElseClauseSyntax>();
    }

    public class PocoElseClauseSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken ElseKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoStatementSyntax Statement
        {
            get;
            set;
        }
    }

    public class PocoElseDirectiveTriviaSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoElseDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoElseDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoElseDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoElseDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoElseDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoElseDirectiveTriviaSyntaxCollection(IList<PocoElseDirectiveTriviaSyntax> initList)
        {
            ((List<PocoElseDirectiveTriviaSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoElseDirectiveTriviaSyntax>();
    }

    public class PocoElseDirectiveTriviaSyntax : PocoBranchingDirectiveTriviaSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken HashToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken ElseKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken EndOfDirectiveToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override bool IsActive
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override bool BranchTaken
        {
            get;
            set;
        }
    }

    public class PocoEmptyStatementSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoEmptyStatementSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoEmptyStatementSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoEmptyStatementSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoEmptyStatementSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoEmptyStatementSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoEmptyStatementSyntaxCollection(IList<PocoEmptyStatementSyntax> initList)
        {
            ((List<PocoEmptyStatementSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoEmptyStatementSyntax>();
    }

    public class PocoEmptyStatementSyntax : PocoStatementSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoAttributeListSyntaxCollection AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken SemicolonToken
        {
            get;
            set;
        }
    }

    public class PocoEndIfDirectiveTriviaSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoEndIfDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoEndIfDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoEndIfDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoEndIfDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoEndIfDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoEndIfDirectiveTriviaSyntaxCollection(IList<PocoEndIfDirectiveTriviaSyntax> initList)
        {
            ((List<PocoEndIfDirectiveTriviaSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoEndIfDirectiveTriviaSyntax>();
    }

    public class PocoEndIfDirectiveTriviaSyntax : PocoDirectiveTriviaSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken HashToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken EndIfKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken EndOfDirectiveToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override bool IsActive
        {
            get;
            set;
        }
    }

    public class PocoEndRegionDirectiveTriviaSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoEndRegionDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoEndRegionDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoEndRegionDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoEndRegionDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoEndRegionDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoEndRegionDirectiveTriviaSyntaxCollection(IList<PocoEndRegionDirectiveTriviaSyntax> initList)
        {
            ((List<PocoEndRegionDirectiveTriviaSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoEndRegionDirectiveTriviaSyntax>();
    }

    public class PocoEndRegionDirectiveTriviaSyntax : PocoDirectiveTriviaSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken HashToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken EndRegionKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken EndOfDirectiveToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override bool IsActive
        {
            get;
            set;
        }
    }

    public class PocoEnumDeclarationSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoEnumDeclarationSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoEnumDeclarationSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoEnumDeclarationSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoEnumDeclarationSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoEnumDeclarationSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoEnumDeclarationSyntaxCollection(IList<PocoEnumDeclarationSyntax> initList)
        {
            ((List<PocoEnumDeclarationSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoEnumDeclarationSyntax>();
    }

    public class PocoEnumDeclarationSyntax : PocoBaseTypeDeclarationSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoAttributeListSyntaxCollection AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxTokenList Modifiers
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken EnumKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken Identifier
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoBaseListSyntax BaseList
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken OpenBraceToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoEnumMemberDeclarationSyntaxCollection Members
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken CloseBraceToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken SemicolonToken
        {
            get;
            set;
        }
    }

    public class PocoEnumMemberDeclarationSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoEnumMemberDeclarationSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoEnumMemberDeclarationSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoEnumMemberDeclarationSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoEnumMemberDeclarationSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoEnumMemberDeclarationSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoEnumMemberDeclarationSyntaxCollection(IList<PocoEnumMemberDeclarationSyntax> initList)
        {
            ((List<PocoEnumMemberDeclarationSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoEnumMemberDeclarationSyntax>();
    }

    public class PocoEnumMemberDeclarationSyntax : PocoMemberDeclarationSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoAttributeListSyntaxCollection AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxTokenList Modifiers
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Identifier
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoEqualsValueClauseSyntax EqualsValue
        {
            get;
            set;
        }
    }

    public class PocoEqualsValueClauseSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoEqualsValueClauseSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoEqualsValueClauseSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoEqualsValueClauseSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoEqualsValueClauseSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoEqualsValueClauseSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoEqualsValueClauseSyntaxCollection(IList<PocoEqualsValueClauseSyntax> initList)
        {
            ((List<PocoEqualsValueClauseSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoEqualsValueClauseSyntax>();
    }

    public class PocoEqualsValueClauseSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken EqualsToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Value
        {
            get;
            set;
        }
    }

    public class PocoErrorDirectiveTriviaSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoErrorDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoErrorDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoErrorDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoErrorDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoErrorDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoErrorDirectiveTriviaSyntaxCollection(IList<PocoErrorDirectiveTriviaSyntax> initList)
        {
            ((List<PocoErrorDirectiveTriviaSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoErrorDirectiveTriviaSyntax>();
    }

    public class PocoErrorDirectiveTriviaSyntax : PocoDirectiveTriviaSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken HashToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken ErrorKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken EndOfDirectiveToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override bool IsActive
        {
            get;
            set;
        }
    }

    public class PocoEventDeclarationSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoEventDeclarationSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoEventDeclarationSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoEventDeclarationSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoEventDeclarationSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoEventDeclarationSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoEventDeclarationSyntaxCollection(IList<PocoEventDeclarationSyntax> initList)
        {
            ((List<PocoEventDeclarationSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoEventDeclarationSyntax>();
    }

    public class PocoEventDeclarationSyntax : PocoBasePropertyDeclarationSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoAttributeListSyntaxCollection AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxTokenList Modifiers
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken EventKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoTypeSyntax Type
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoExplicitInterfaceSpecifierSyntax ExplicitInterfaceSpecifier
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Identifier
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoAccessorListSyntax AccessorList
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken SemicolonToken
        {
            get;
            set;
        }
    }

    public class PocoEventFieldDeclarationSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoEventFieldDeclarationSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoEventFieldDeclarationSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoEventFieldDeclarationSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoEventFieldDeclarationSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoEventFieldDeclarationSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoEventFieldDeclarationSyntaxCollection(IList<PocoEventFieldDeclarationSyntax> initList)
        {
            ((List<PocoEventFieldDeclarationSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoEventFieldDeclarationSyntax>();
    }

    public class PocoEventFieldDeclarationSyntax : PocoBaseFieldDeclarationSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoAttributeListSyntaxCollection AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxTokenList Modifiers
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken EventKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoVariableDeclarationSyntax Declaration
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken SemicolonToken
        {
            get;
            set;
        }
    }

    public class PocoExplicitInterfaceSpecifierSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoExplicitInterfaceSpecifierSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoExplicitInterfaceSpecifierSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoExplicitInterfaceSpecifierSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoExplicitInterfaceSpecifierSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoExplicitInterfaceSpecifierSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoExplicitInterfaceSpecifierSyntaxCollection(IList<PocoExplicitInterfaceSpecifierSyntax> initList)
        {
            ((List<PocoExplicitInterfaceSpecifierSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoExplicitInterfaceSpecifierSyntax>();
    }

    public class PocoExplicitInterfaceSpecifierSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoNameSyntax Name
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken DotToken
        {
            get;
            set;
        }
    }

    public class PocoExpressionStatementSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoExpressionStatementSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoExpressionStatementSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoExpressionStatementSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoExpressionStatementSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoExpressionStatementSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoExpressionStatementSyntaxCollection(IList<PocoExpressionStatementSyntax> initList)
        {
            ((List<PocoExpressionStatementSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoExpressionStatementSyntax>();
    }

    public class PocoExpressionStatementSyntax : PocoStatementSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoAttributeListSyntaxCollection AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Expression
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken SemicolonToken
        {
            get;
            set;
        }
    }

    public class PocoExpressionSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoExpressionSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoExpressionSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoExpressionSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoExpressionSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoExpressionSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoExpressionSyntaxCollection(IList<PocoExpressionSyntax> initList)
        {
            ((List<PocoExpressionSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoExpressionSyntax>();
    }

    public class PocoExpressionSyntax : PocoCSharpSyntaxNode
    {
    }

    public class PocoExternAliasDirectiveSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoExternAliasDirectiveSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoExternAliasDirectiveSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoExternAliasDirectiveSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoExternAliasDirectiveSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoExternAliasDirectiveSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoExternAliasDirectiveSyntaxCollection(IList<PocoExternAliasDirectiveSyntax> initList)
        {
            ((List<PocoExternAliasDirectiveSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoExternAliasDirectiveSyntax>();
    }

    public class PocoExternAliasDirectiveSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken ExternKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken AliasKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Identifier
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken SemicolonToken
        {
            get;
            set;
        }
    }

    public class PocoFieldDeclarationSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoFieldDeclarationSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoFieldDeclarationSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoFieldDeclarationSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoFieldDeclarationSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoFieldDeclarationSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoFieldDeclarationSyntaxCollection(IList<PocoFieldDeclarationSyntax> initList)
        {
            ((List<PocoFieldDeclarationSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoFieldDeclarationSyntax>();
    }

    public class PocoFieldDeclarationSyntax : PocoBaseFieldDeclarationSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoAttributeListSyntaxCollection AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxTokenList Modifiers
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoVariableDeclarationSyntax Declaration
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken SemicolonToken
        {
            get;
            set;
        }
    }

    public class PocoFinallyClauseSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoFinallyClauseSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoFinallyClauseSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoFinallyClauseSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoFinallyClauseSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoFinallyClauseSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoFinallyClauseSyntaxCollection(IList<PocoFinallyClauseSyntax> initList)
        {
            ((List<PocoFinallyClauseSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoFinallyClauseSyntax>();
    }

    public class PocoFinallyClauseSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken FinallyKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoBlockSyntax Block
        {
            get;
            set;
        }
    }

    public class PocoFixedStatementSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoFixedStatementSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoFixedStatementSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoFixedStatementSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoFixedStatementSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoFixedStatementSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoFixedStatementSyntaxCollection(IList<PocoFixedStatementSyntax> initList)
        {
            ((List<PocoFixedStatementSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoFixedStatementSyntax>();
    }

    public class PocoFixedStatementSyntax : PocoStatementSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoAttributeListSyntaxCollection AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken FixedKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenParenToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoVariableDeclarationSyntax Declaration
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseParenToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoStatementSyntax Statement
        {
            get;
            set;
        }
    }

    public class PocoForEachStatementSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoForEachStatementSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoForEachStatementSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoForEachStatementSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoForEachStatementSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoForEachStatementSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoForEachStatementSyntaxCollection(IList<PocoForEachStatementSyntax> initList)
        {
            ((List<PocoForEachStatementSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoForEachStatementSyntax>();
    }

    public class PocoForEachStatementSyntax : PocoCommonForEachStatementSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoAttributeListSyntaxCollection AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken AwaitKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken ForEachKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken OpenParenToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoTypeSyntax Type
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Identifier
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken InKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoExpressionSyntax Expression
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken CloseParenToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoStatementSyntax Statement
        {
            get;
            set;
        }
    }

    public class PocoForEachVariableStatementSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoForEachVariableStatementSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoForEachVariableStatementSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoForEachVariableStatementSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoForEachVariableStatementSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoForEachVariableStatementSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoForEachVariableStatementSyntaxCollection(IList<PocoForEachVariableStatementSyntax> initList)
        {
            ((List<PocoForEachVariableStatementSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoForEachVariableStatementSyntax>();
    }

    public class PocoForEachVariableStatementSyntax : PocoCommonForEachStatementSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoAttributeListSyntaxCollection AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken AwaitKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken ForEachKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken OpenParenToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Variable
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken InKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoExpressionSyntax Expression
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken CloseParenToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoStatementSyntax Statement
        {
            get;
            set;
        }
    }

    public class PocoForStatementSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoForStatementSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoForStatementSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoForStatementSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoForStatementSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoForStatementSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoForStatementSyntaxCollection(IList<PocoForStatementSyntax> initList)
        {
            ((List<PocoForStatementSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoForStatementSyntax>();
    }

    public class PocoForStatementSyntax : PocoStatementSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoAttributeListSyntaxCollection AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken ForKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenParenToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken FirstSemicolonToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Condition
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken SecondSemicolonToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntaxCollection Incrementors
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseParenToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoStatementSyntax Statement
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoVariableDeclarationSyntax Declaration
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntaxCollection Initializers
        {
            get;
            set;
        }
    }

    public class PocoFromClauseSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoFromClauseSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoFromClauseSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoFromClauseSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoFromClauseSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoFromClauseSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoFromClauseSyntaxCollection(IList<PocoFromClauseSyntax> initList)
        {
            ((List<PocoFromClauseSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoFromClauseSyntax>();
    }

    public class PocoFromClauseSyntax : PocoQueryClauseSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken FromKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoTypeSyntax Type
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Identifier
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken InKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Expression
        {
            get;
            set;
        }
    }

    public class PocoGenericNameSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoGenericNameSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoGenericNameSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoGenericNameSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoGenericNameSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoGenericNameSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoGenericNameSyntaxCollection(IList<PocoGenericNameSyntax> initList)
        {
            ((List<PocoGenericNameSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoGenericNameSyntax>();
    }

    public class PocoGenericNameSyntax : PocoSimpleNameSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken Identifier
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoTypeArgumentListSyntax TypeArgumentList
        {
            get;
            set;
        }
    }

    public class PocoGlobalStatementSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoGlobalStatementSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoGlobalStatementSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoGlobalStatementSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoGlobalStatementSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoGlobalStatementSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoGlobalStatementSyntaxCollection(IList<PocoGlobalStatementSyntax> initList)
        {
            ((List<PocoGlobalStatementSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoGlobalStatementSyntax>();
    }

    public class PocoGlobalStatementSyntax : PocoMemberDeclarationSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoAttributeListSyntaxCollection AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxTokenList Modifiers
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoStatementSyntax Statement
        {
            get;
            set;
        }
    }

    public class PocoGotoStatementSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoGotoStatementSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoGotoStatementSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoGotoStatementSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoGotoStatementSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoGotoStatementSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoGotoStatementSyntaxCollection(IList<PocoGotoStatementSyntax> initList)
        {
            ((List<PocoGotoStatementSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoGotoStatementSyntax>();
    }

    public class PocoGotoStatementSyntax : PocoStatementSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoAttributeListSyntaxCollection AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken GotoKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CaseOrDefaultKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Expression
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken SemicolonToken
        {
            get;
            set;
        }
    }

    public class PocoGroupClauseSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoGroupClauseSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoGroupClauseSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoGroupClauseSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoGroupClauseSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoGroupClauseSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoGroupClauseSyntaxCollection(IList<PocoGroupClauseSyntax> initList)
        {
            ((List<PocoGroupClauseSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoGroupClauseSyntax>();
    }

    public class PocoGroupClauseSyntax : PocoSelectOrGroupClauseSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken GroupKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax GroupExpression
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken ByKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax ByExpression
        {
            get;
            set;
        }
    }

    public class PocoIdentifierNameSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoIdentifierNameSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoIdentifierNameSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoIdentifierNameSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoIdentifierNameSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoIdentifierNameSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoIdentifierNameSyntaxCollection(IList<PocoIdentifierNameSyntax> initList)
        {
            ((List<PocoIdentifierNameSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoIdentifierNameSyntax>();
    }

    public class PocoIdentifierNameSyntax : PocoSimpleNameSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken Identifier
        {
            get;
            set;
        }
    }

    public class PocoIfDirectiveTriviaSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoIfDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoIfDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoIfDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoIfDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoIfDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoIfDirectiveTriviaSyntaxCollection(IList<PocoIfDirectiveTriviaSyntax> initList)
        {
            ((List<PocoIfDirectiveTriviaSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoIfDirectiveTriviaSyntax>();
    }

    public class PocoIfDirectiveTriviaSyntax : PocoConditionalDirectiveTriviaSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken HashToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken IfKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoExpressionSyntax Condition
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken EndOfDirectiveToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override bool IsActive
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override bool BranchTaken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override bool ConditionValue
        {
            get;
            set;
        }
    }

    public class PocoIfStatementSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoIfStatementSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoIfStatementSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoIfStatementSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoIfStatementSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoIfStatementSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoIfStatementSyntaxCollection(IList<PocoIfStatementSyntax> initList)
        {
            ((List<PocoIfStatementSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoIfStatementSyntax>();
    }

    public class PocoIfStatementSyntax : PocoStatementSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoAttributeListSyntaxCollection AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken IfKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenParenToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Condition
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseParenToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoStatementSyntax Statement
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoElseClauseSyntax Else
        {
            get;
            set;
        }
    }

    public class PocoImplicitArrayCreationExpressionSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoImplicitArrayCreationExpressionSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoImplicitArrayCreationExpressionSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoImplicitArrayCreationExpressionSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoImplicitArrayCreationExpressionSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoImplicitArrayCreationExpressionSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoImplicitArrayCreationExpressionSyntaxCollection(IList<PocoImplicitArrayCreationExpressionSyntax> initList)
        {
            ((List<PocoImplicitArrayCreationExpressionSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoImplicitArrayCreationExpressionSyntax>();
    }

    public class PocoImplicitArrayCreationExpressionSyntax : PocoExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken NewKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenBracketToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxTokenList Commas
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseBracketToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoInitializerExpressionSyntax Initializer
        {
            get;
            set;
        }
    }

    public class PocoImplicitElementAccessSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoImplicitElementAccessSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoImplicitElementAccessSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoImplicitElementAccessSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoImplicitElementAccessSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoImplicitElementAccessSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoImplicitElementAccessSyntaxCollection(IList<PocoImplicitElementAccessSyntax> initList)
        {
            ((List<PocoImplicitElementAccessSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoImplicitElementAccessSyntax>();
    }

    public class PocoImplicitElementAccessSyntax : PocoExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoBracketedArgumentListSyntax ArgumentList
        {
            get;
            set;
        }
    }

    public class PocoImplicitStackAllocArrayCreationExpressionSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoImplicitStackAllocArrayCreationExpressionSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoImplicitStackAllocArrayCreationExpressionSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoImplicitStackAllocArrayCreationExpressionSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoImplicitStackAllocArrayCreationExpressionSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoImplicitStackAllocArrayCreationExpressionSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoImplicitStackAllocArrayCreationExpressionSyntaxCollection(IList<PocoImplicitStackAllocArrayCreationExpressionSyntax> initList)
        {
            ((List<PocoImplicitStackAllocArrayCreationExpressionSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoImplicitStackAllocArrayCreationExpressionSyntax>();
    }

    public class PocoImplicitStackAllocArrayCreationExpressionSyntax : PocoExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken StackAllocKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenBracketToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseBracketToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoInitializerExpressionSyntax Initializer
        {
            get;
            set;
        }
    }

    public class PocoIncompleteMemberSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoIncompleteMemberSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoIncompleteMemberSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoIncompleteMemberSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoIncompleteMemberSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoIncompleteMemberSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoIncompleteMemberSyntaxCollection(IList<PocoIncompleteMemberSyntax> initList)
        {
            ((List<PocoIncompleteMemberSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoIncompleteMemberSyntax>();
    }

    public class PocoIncompleteMemberSyntax : PocoMemberDeclarationSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoAttributeListSyntaxCollection AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxTokenList Modifiers
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoTypeSyntax Type
        {
            get;
            set;
        }
    }

    public class PocoIndexerDeclarationSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoIndexerDeclarationSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoIndexerDeclarationSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoIndexerDeclarationSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoIndexerDeclarationSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoIndexerDeclarationSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoIndexerDeclarationSyntaxCollection(IList<PocoIndexerDeclarationSyntax> initList)
        {
            ((List<PocoIndexerDeclarationSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoIndexerDeclarationSyntax>();
    }

    public class PocoIndexerDeclarationSyntax : PocoBasePropertyDeclarationSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoAttributeListSyntaxCollection AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxTokenList Modifiers
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoTypeSyntax Type
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoExplicitInterfaceSpecifierSyntax ExplicitInterfaceSpecifier
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken ThisKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoBracketedParameterListSyntax ParameterList
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoAccessorListSyntax AccessorList
        {
            get;
            set;
        }
    }

    public class PocoIndexerMemberCrefSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoIndexerMemberCrefSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoIndexerMemberCrefSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoIndexerMemberCrefSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoIndexerMemberCrefSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoIndexerMemberCrefSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoIndexerMemberCrefSyntaxCollection(IList<PocoIndexerMemberCrefSyntax> initList)
        {
            ((List<PocoIndexerMemberCrefSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoIndexerMemberCrefSyntax>();
    }

    public class PocoIndexerMemberCrefSyntax : PocoMemberCrefSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken ThisKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoCrefBracketedParameterListSyntax Parameters
        {
            get;
            set;
        }
    }

    public class PocoInitializerExpressionSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoInitializerExpressionSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoInitializerExpressionSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoInitializerExpressionSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoInitializerExpressionSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoInitializerExpressionSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoInitializerExpressionSyntaxCollection(IList<PocoInitializerExpressionSyntax> initList)
        {
            ((List<PocoInitializerExpressionSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoInitializerExpressionSyntax>();
    }

    public class PocoInitializerExpressionSyntax : PocoExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenBraceToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntaxCollection Expressions
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseBraceToken
        {
            get;
            set;
        }
    }

    public class PocoInstanceExpressionSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoInstanceExpressionSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoInstanceExpressionSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoInstanceExpressionSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoInstanceExpressionSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoInstanceExpressionSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoInstanceExpressionSyntaxCollection(IList<PocoInstanceExpressionSyntax> initList)
        {
            ((List<PocoInstanceExpressionSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoInstanceExpressionSyntax>();
    }

    public class PocoInstanceExpressionSyntax : PocoExpressionSyntax
    {
    }

    public class PocoInterfaceDeclarationSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoInterfaceDeclarationSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoInterfaceDeclarationSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoInterfaceDeclarationSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoInterfaceDeclarationSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoInterfaceDeclarationSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoInterfaceDeclarationSyntaxCollection(IList<PocoInterfaceDeclarationSyntax> initList)
        {
            ((List<PocoInterfaceDeclarationSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoInterfaceDeclarationSyntax>();
    }

    public class PocoInterfaceDeclarationSyntax : PocoTypeDeclarationSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoAttributeListSyntaxCollection AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxTokenList Modifiers
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken Keyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken Identifier
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoTypeParameterListSyntax TypeParameterList
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoBaseListSyntax BaseList
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoTypeParameterConstraintClauseSyntaxCollection ConstraintClauses
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken OpenBraceToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoMemberDeclarationSyntaxCollection Members
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken CloseBraceToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken SemicolonToken
        {
            get;
            set;
        }
    }

    public class PocoInterpolatedStringContentSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoInterpolatedStringContentSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoInterpolatedStringContentSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoInterpolatedStringContentSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoInterpolatedStringContentSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoInterpolatedStringContentSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoInterpolatedStringContentSyntaxCollection(IList<PocoInterpolatedStringContentSyntax> initList)
        {
            ((List<PocoInterpolatedStringContentSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoInterpolatedStringContentSyntax>();
    }

    public class PocoInterpolatedStringContentSyntax : PocoCSharpSyntaxNode
    {
    }

    public class PocoInterpolatedStringExpressionSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoInterpolatedStringExpressionSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoInterpolatedStringExpressionSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoInterpolatedStringExpressionSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoInterpolatedStringExpressionSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoInterpolatedStringExpressionSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoInterpolatedStringExpressionSyntaxCollection(IList<PocoInterpolatedStringExpressionSyntax> initList)
        {
            ((List<PocoInterpolatedStringExpressionSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoInterpolatedStringExpressionSyntax>();
    }

    public class PocoInterpolatedStringExpressionSyntax : PocoExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken StringStartToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoInterpolatedStringContentSyntaxCollection Contents
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken StringEndToken
        {
            get;
            set;
        }
    }

    public class PocoInterpolatedStringTextSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoInterpolatedStringTextSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoInterpolatedStringTextSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoInterpolatedStringTextSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoInterpolatedStringTextSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoInterpolatedStringTextSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoInterpolatedStringTextSyntaxCollection(IList<PocoInterpolatedStringTextSyntax> initList)
        {
            ((List<PocoInterpolatedStringTextSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoInterpolatedStringTextSyntax>();
    }

    public class PocoInterpolatedStringTextSyntax : PocoInterpolatedStringContentSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken TextToken
        {
            get;
            set;
        }
    }

    public class PocoInterpolationAlignmentClauseSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoInterpolationAlignmentClauseSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoInterpolationAlignmentClauseSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoInterpolationAlignmentClauseSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoInterpolationAlignmentClauseSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoInterpolationAlignmentClauseSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoInterpolationAlignmentClauseSyntaxCollection(IList<PocoInterpolationAlignmentClauseSyntax> initList)
        {
            ((List<PocoInterpolationAlignmentClauseSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoInterpolationAlignmentClauseSyntax>();
    }

    public class PocoInterpolationAlignmentClauseSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CommaToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Value
        {
            get;
            set;
        }
    }

    public class PocoInterpolationFormatClauseSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoInterpolationFormatClauseSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoInterpolationFormatClauseSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoInterpolationFormatClauseSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoInterpolationFormatClauseSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoInterpolationFormatClauseSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoInterpolationFormatClauseSyntaxCollection(IList<PocoInterpolationFormatClauseSyntax> initList)
        {
            ((List<PocoInterpolationFormatClauseSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoInterpolationFormatClauseSyntax>();
    }

    public class PocoInterpolationFormatClauseSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken ColonToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken FormatStringToken
        {
            get;
            set;
        }
    }

    public class PocoInterpolationSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoInterpolationSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoInterpolationSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoInterpolationSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoInterpolationSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoInterpolationSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoInterpolationSyntaxCollection(IList<PocoInterpolationSyntax> initList)
        {
            ((List<PocoInterpolationSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoInterpolationSyntax>();
    }

    public class PocoInterpolationSyntax : PocoInterpolatedStringContentSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenBraceToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Expression
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoInterpolationAlignmentClauseSyntax AlignmentClause
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoInterpolationFormatClauseSyntax FormatClause
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseBraceToken
        {
            get;
            set;
        }
    }

    public class PocoInvocationExpressionSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoInvocationExpressionSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoInvocationExpressionSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoInvocationExpressionSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoInvocationExpressionSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoInvocationExpressionSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoInvocationExpressionSyntaxCollection(IList<PocoInvocationExpressionSyntax> initList)
        {
            ((List<PocoInvocationExpressionSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoInvocationExpressionSyntax>();
    }

    public class PocoInvocationExpressionSyntax : PocoExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Expression
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoArgumentListSyntax ArgumentList
        {
            get;
            set;
        }
    }

    public class PocoIsPatternExpressionSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoIsPatternExpressionSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoIsPatternExpressionSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoIsPatternExpressionSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoIsPatternExpressionSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoIsPatternExpressionSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoIsPatternExpressionSyntaxCollection(IList<PocoIsPatternExpressionSyntax> initList)
        {
            ((List<PocoIsPatternExpressionSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoIsPatternExpressionSyntax>();
    }

    public class PocoIsPatternExpressionSyntax : PocoExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Expression
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken IsKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoPatternSyntax Pattern
        {
            get;
            set;
        }
    }

    public class PocoJoinClauseSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoJoinClauseSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoJoinClauseSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoJoinClauseSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoJoinClauseSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoJoinClauseSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoJoinClauseSyntaxCollection(IList<PocoJoinClauseSyntax> initList)
        {
            ((List<PocoJoinClauseSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoJoinClauseSyntax>();
    }

    public class PocoJoinClauseSyntax : PocoQueryClauseSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken JoinKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoTypeSyntax Type
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Identifier
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken InKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax InExpression
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OnKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax LeftExpression
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken EqualsKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax RightExpression
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoJoinIntoClauseSyntax Into
        {
            get;
            set;
        }
    }

    public class PocoJoinIntoClauseSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoJoinIntoClauseSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoJoinIntoClauseSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoJoinIntoClauseSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoJoinIntoClauseSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoJoinIntoClauseSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoJoinIntoClauseSyntaxCollection(IList<PocoJoinIntoClauseSyntax> initList)
        {
            ((List<PocoJoinIntoClauseSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoJoinIntoClauseSyntax>();
    }

    public class PocoJoinIntoClauseSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken IntoKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Identifier
        {
            get;
            set;
        }
    }

    public class PocoLabeledStatementSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoLabeledStatementSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoLabeledStatementSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoLabeledStatementSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoLabeledStatementSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoLabeledStatementSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoLabeledStatementSyntaxCollection(IList<PocoLabeledStatementSyntax> initList)
        {
            ((List<PocoLabeledStatementSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoLabeledStatementSyntax>();
    }

    public class PocoLabeledStatementSyntax : PocoStatementSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoAttributeListSyntaxCollection AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Identifier
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken ColonToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoStatementSyntax Statement
        {
            get;
            set;
        }
    }

    public class PocoLambdaExpressionSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoLambdaExpressionSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoLambdaExpressionSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoLambdaExpressionSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoLambdaExpressionSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoLambdaExpressionSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoLambdaExpressionSyntaxCollection(IList<PocoLambdaExpressionSyntax> initList)
        {
            ((List<PocoLambdaExpressionSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoLambdaExpressionSyntax>();
    }

    public class PocoLambdaExpressionSyntax : PocoAnonymousFunctionExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken ArrowToken
        {
            get;
            set;
        }
    }

    public class PocoLetClauseSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoLetClauseSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoLetClauseSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoLetClauseSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoLetClauseSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoLetClauseSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoLetClauseSyntaxCollection(IList<PocoLetClauseSyntax> initList)
        {
            ((List<PocoLetClauseSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoLetClauseSyntax>();
    }

    public class PocoLetClauseSyntax : PocoQueryClauseSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken LetKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Identifier
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken EqualsToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Expression
        {
            get;
            set;
        }
    }

    public class PocoLineDirectiveTriviaSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoLineDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoLineDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoLineDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoLineDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoLineDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoLineDirectiveTriviaSyntaxCollection(IList<PocoLineDirectiveTriviaSyntax> initList)
        {
            ((List<PocoLineDirectiveTriviaSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoLineDirectiveTriviaSyntax>();
    }

    public class PocoLineDirectiveTriviaSyntax : PocoDirectiveTriviaSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken HashToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken LineKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Line
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken File
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken EndOfDirectiveToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override bool IsActive
        {
            get;
            set;
        }
    }

    public class PocoLiteralExpressionSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoLiteralExpressionSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoLiteralExpressionSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoLiteralExpressionSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoLiteralExpressionSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoLiteralExpressionSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoLiteralExpressionSyntaxCollection(IList<PocoLiteralExpressionSyntax> initList)
        {
            ((List<PocoLiteralExpressionSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoLiteralExpressionSyntax>();
    }

    public class PocoLiteralExpressionSyntax : PocoExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Token
        {
            get;
            set;
        }
    }

    public class PocoLoadDirectiveTriviaSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoLoadDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoLoadDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoLoadDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoLoadDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoLoadDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoLoadDirectiveTriviaSyntaxCollection(IList<PocoLoadDirectiveTriviaSyntax> initList)
        {
            ((List<PocoLoadDirectiveTriviaSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoLoadDirectiveTriviaSyntax>();
    }

    public class PocoLoadDirectiveTriviaSyntax : PocoDirectiveTriviaSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken HashToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken LoadKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken File
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken EndOfDirectiveToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override bool IsActive
        {
            get;
            set;
        }
    }

    public class PocoLocalDeclarationStatementSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoLocalDeclarationStatementSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoLocalDeclarationStatementSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoLocalDeclarationStatementSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoLocalDeclarationStatementSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoLocalDeclarationStatementSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoLocalDeclarationStatementSyntaxCollection(IList<PocoLocalDeclarationStatementSyntax> initList)
        {
            ((List<PocoLocalDeclarationStatementSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoLocalDeclarationStatementSyntax>();
    }

    public class PocoLocalDeclarationStatementSyntax : PocoStatementSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoAttributeListSyntaxCollection AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken AwaitKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken UsingKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxTokenList Modifiers
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoVariableDeclarationSyntax Declaration
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken SemicolonToken
        {
            get;
            set;
        }
    }

    public class PocoLocalFunctionStatementSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoLocalFunctionStatementSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoLocalFunctionStatementSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoLocalFunctionStatementSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoLocalFunctionStatementSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoLocalFunctionStatementSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoLocalFunctionStatementSyntaxCollection(IList<PocoLocalFunctionStatementSyntax> initList)
        {
            ((List<PocoLocalFunctionStatementSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoLocalFunctionStatementSyntax>();
    }

    public class PocoLocalFunctionStatementSyntax : PocoStatementSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoAttributeListSyntaxCollection AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxTokenList Modifiers
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoTypeSyntax ReturnType
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Identifier
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoTypeParameterListSyntax TypeParameterList
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoParameterListSyntax ParameterList
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoTypeParameterConstraintClauseSyntaxCollection ConstraintClauses
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoBlockSyntax Body
        {
            get;
            set;
        }
    }

    public class PocoLockStatementSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoLockStatementSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoLockStatementSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoLockStatementSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoLockStatementSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoLockStatementSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoLockStatementSyntaxCollection(IList<PocoLockStatementSyntax> initList)
        {
            ((List<PocoLockStatementSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoLockStatementSyntax>();
    }

    public class PocoLockStatementSyntax : PocoStatementSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoAttributeListSyntaxCollection AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken LockKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenParenToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Expression
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseParenToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoStatementSyntax Statement
        {
            get;
            set;
        }
    }

    public class PocoMakeRefExpressionSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoMakeRefExpressionSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoMakeRefExpressionSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoMakeRefExpressionSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoMakeRefExpressionSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoMakeRefExpressionSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoMakeRefExpressionSyntaxCollection(IList<PocoMakeRefExpressionSyntax> initList)
        {
            ((List<PocoMakeRefExpressionSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoMakeRefExpressionSyntax>();
    }

    public class PocoMakeRefExpressionSyntax : PocoExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Keyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenParenToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Expression
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseParenToken
        {
            get;
            set;
        }
    }

    public class PocoMemberAccessExpressionSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoMemberAccessExpressionSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoMemberAccessExpressionSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoMemberAccessExpressionSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoMemberAccessExpressionSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoMemberAccessExpressionSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoMemberAccessExpressionSyntaxCollection(IList<PocoMemberAccessExpressionSyntax> initList)
        {
            ((List<PocoMemberAccessExpressionSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoMemberAccessExpressionSyntax>();
    }

    public class PocoMemberAccessExpressionSyntax : PocoExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Expression
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OperatorToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSimpleNameSyntax Name
        {
            get;
            set;
        }
    }

    public class PocoMemberBindingExpressionSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoMemberBindingExpressionSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoMemberBindingExpressionSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoMemberBindingExpressionSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoMemberBindingExpressionSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoMemberBindingExpressionSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoMemberBindingExpressionSyntaxCollection(IList<PocoMemberBindingExpressionSyntax> initList)
        {
            ((List<PocoMemberBindingExpressionSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoMemberBindingExpressionSyntax>();
    }

    public class PocoMemberBindingExpressionSyntax : PocoExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OperatorToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSimpleNameSyntax Name
        {
            get;
            set;
        }
    }

    public class PocoMemberCrefSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoMemberCrefSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoMemberCrefSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoMemberCrefSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoMemberCrefSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoMemberCrefSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoMemberCrefSyntaxCollection(IList<PocoMemberCrefSyntax> initList)
        {
            ((List<PocoMemberCrefSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoMemberCrefSyntax>();
    }

    public class PocoMemberCrefSyntax : PocoCrefSyntax
    {
    }

    public class PocoMemberDeclarationSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoMemberDeclarationSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoMemberDeclarationSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoMemberDeclarationSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoMemberDeclarationSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoMemberDeclarationSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoMemberDeclarationSyntaxCollection(IList<PocoMemberDeclarationSyntax> initList)
        {
            ((List<PocoMemberDeclarationSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoMemberDeclarationSyntax>();
    }

    public class PocoMemberDeclarationSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoAttributeListSyntaxCollection AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxTokenList Modifiers
        {
            get;
            set;
        }
    }

    public class PocoMethodDeclarationSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoMethodDeclarationSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoMethodDeclarationSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoMethodDeclarationSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoMethodDeclarationSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoMethodDeclarationSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoMethodDeclarationSyntaxCollection(IList<PocoMethodDeclarationSyntax> initList)
        {
            ((List<PocoMethodDeclarationSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoMethodDeclarationSyntax>();
    }

    public class PocoMethodDeclarationSyntax : PocoBaseMethodDeclarationSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoAttributeListSyntaxCollection AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxTokenList Modifiers
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoTypeSyntax ReturnType
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExplicitInterfaceSpecifierSyntax ExplicitInterfaceSpecifier
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Identifier
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoTypeParameterListSyntax TypeParameterList
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoParameterListSyntax ParameterList
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoTypeParameterConstraintClauseSyntaxCollection ConstraintClauses
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoBlockSyntax Body
        {
            get;
            set;
        }
    }

    public class PocoNameColonSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoNameColonSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoNameColonSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoNameColonSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoNameColonSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoNameColonSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoNameColonSyntaxCollection(IList<PocoNameColonSyntax> initList)
        {
            ((List<PocoNameColonSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoNameColonSyntax>();
    }

    public class PocoNameColonSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoIdentifierNameSyntax Name
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken ColonToken
        {
            get;
            set;
        }
    }

    public class PocoNameEqualsSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoNameEqualsSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoNameEqualsSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoNameEqualsSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoNameEqualsSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoNameEqualsSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoNameEqualsSyntaxCollection(IList<PocoNameEqualsSyntax> initList)
        {
            ((List<PocoNameEqualsSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoNameEqualsSyntax>();
    }

    public class PocoNameEqualsSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoIdentifierNameSyntax Name
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken EqualsToken
        {
            get;
            set;
        }
    }

    public class PocoNameMemberCrefSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoNameMemberCrefSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoNameMemberCrefSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoNameMemberCrefSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoNameMemberCrefSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoNameMemberCrefSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoNameMemberCrefSyntaxCollection(IList<PocoNameMemberCrefSyntax> initList)
        {
            ((List<PocoNameMemberCrefSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoNameMemberCrefSyntax>();
    }

    public class PocoNameMemberCrefSyntax : PocoMemberCrefSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoTypeSyntax Name
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoCrefParameterListSyntax Parameters
        {
            get;
            set;
        }
    }

    public class PocoNamespaceDeclarationSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoNamespaceDeclarationSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoNamespaceDeclarationSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoNamespaceDeclarationSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoNamespaceDeclarationSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoNamespaceDeclarationSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoNamespaceDeclarationSyntaxCollection(IList<PocoNamespaceDeclarationSyntax> initList)
        {
            ((List<PocoNamespaceDeclarationSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoNamespaceDeclarationSyntax>();
    }

    public class PocoNamespaceDeclarationSyntax : PocoMemberDeclarationSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoAttributeListSyntaxCollection AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxTokenList Modifiers
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken NamespaceKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoNameSyntax Name
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenBraceToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExternAliasDirectiveSyntaxCollection Externs
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoUsingDirectiveSyntaxCollection Usings
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoMemberDeclarationSyntaxCollection Members
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseBraceToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken SemicolonToken
        {
            get;
            set;
        }
    }

    public class PocoNameSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoNameSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoNameSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoNameSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoNameSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoNameSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoNameSyntaxCollection(IList<PocoNameSyntax> initList)
        {
            ((List<PocoNameSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoNameSyntax>();
    }

    public class PocoNameSyntax : PocoTypeSyntax
    {
    }

    public class PocoNullableDirectiveTriviaSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoNullableDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoNullableDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoNullableDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoNullableDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoNullableDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoNullableDirectiveTriviaSyntaxCollection(IList<PocoNullableDirectiveTriviaSyntax> initList)
        {
            ((List<PocoNullableDirectiveTriviaSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoNullableDirectiveTriviaSyntax>();
    }

    public class PocoNullableDirectiveTriviaSyntax : PocoDirectiveTriviaSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken HashToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken NullableKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken SettingToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken TargetToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken EndOfDirectiveToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override bool IsActive
        {
            get;
            set;
        }
    }

    public class PocoNullableTypeSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoNullableTypeSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoNullableTypeSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoNullableTypeSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoNullableTypeSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoNullableTypeSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoNullableTypeSyntaxCollection(IList<PocoNullableTypeSyntax> initList)
        {
            ((List<PocoNullableTypeSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoNullableTypeSyntax>();
    }

    public class PocoNullableTypeSyntax : PocoTypeSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoTypeSyntax ElementType
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken QuestionToken
        {
            get;
            set;
        }
    }

    public class PocoObjectCreationExpressionSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoObjectCreationExpressionSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoObjectCreationExpressionSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoObjectCreationExpressionSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoObjectCreationExpressionSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoObjectCreationExpressionSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoObjectCreationExpressionSyntaxCollection(IList<PocoObjectCreationExpressionSyntax> initList)
        {
            ((List<PocoObjectCreationExpressionSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoObjectCreationExpressionSyntax>();
    }

    public class PocoObjectCreationExpressionSyntax : PocoExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken NewKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoTypeSyntax Type
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoArgumentListSyntax ArgumentList
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoInitializerExpressionSyntax Initializer
        {
            get;
            set;
        }
    }

    public class PocoOmittedArraySizeExpressionSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoOmittedArraySizeExpressionSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoOmittedArraySizeExpressionSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoOmittedArraySizeExpressionSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoOmittedArraySizeExpressionSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoOmittedArraySizeExpressionSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoOmittedArraySizeExpressionSyntaxCollection(IList<PocoOmittedArraySizeExpressionSyntax> initList)
        {
            ((List<PocoOmittedArraySizeExpressionSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoOmittedArraySizeExpressionSyntax>();
    }

    public class PocoOmittedArraySizeExpressionSyntax : PocoExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OmittedArraySizeExpressionToken
        {
            get;
            set;
        }
    }

    public class PocoOmittedTypeArgumentSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoOmittedTypeArgumentSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoOmittedTypeArgumentSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoOmittedTypeArgumentSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoOmittedTypeArgumentSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoOmittedTypeArgumentSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoOmittedTypeArgumentSyntaxCollection(IList<PocoOmittedTypeArgumentSyntax> initList)
        {
            ((List<PocoOmittedTypeArgumentSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoOmittedTypeArgumentSyntax>();
    }

    public class PocoOmittedTypeArgumentSyntax : PocoTypeSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OmittedTypeArgumentToken
        {
            get;
            set;
        }
    }

    public class PocoOperatorDeclarationSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoOperatorDeclarationSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoOperatorDeclarationSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoOperatorDeclarationSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoOperatorDeclarationSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoOperatorDeclarationSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoOperatorDeclarationSyntaxCollection(IList<PocoOperatorDeclarationSyntax> initList)
        {
            ((List<PocoOperatorDeclarationSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoOperatorDeclarationSyntax>();
    }

    public class PocoOperatorDeclarationSyntax : PocoBaseMethodDeclarationSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoAttributeListSyntaxCollection AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxTokenList Modifiers
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoTypeSyntax ReturnType
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OperatorKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OperatorToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoParameterListSyntax ParameterList
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoBlockSyntax Body
        {
            get;
            set;
        }
    }

    public class PocoOperatorMemberCrefSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoOperatorMemberCrefSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoOperatorMemberCrefSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoOperatorMemberCrefSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoOperatorMemberCrefSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoOperatorMemberCrefSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoOperatorMemberCrefSyntaxCollection(IList<PocoOperatorMemberCrefSyntax> initList)
        {
            ((List<PocoOperatorMemberCrefSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoOperatorMemberCrefSyntax>();
    }

    public class PocoOperatorMemberCrefSyntax : PocoMemberCrefSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OperatorKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OperatorToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoCrefParameterListSyntax Parameters
        {
            get;
            set;
        }
    }

    public class PocoOrderByClauseSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoOrderByClauseSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoOrderByClauseSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoOrderByClauseSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoOrderByClauseSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoOrderByClauseSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoOrderByClauseSyntaxCollection(IList<PocoOrderByClauseSyntax> initList)
        {
            ((List<PocoOrderByClauseSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoOrderByClauseSyntax>();
    }

    public class PocoOrderByClauseSyntax : PocoQueryClauseSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OrderByKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoOrderingSyntaxCollection Orderings
        {
            get;
            set;
        }
    }

    public class PocoOrderingSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoOrderingSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoOrderingSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoOrderingSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoOrderingSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoOrderingSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoOrderingSyntaxCollection(IList<PocoOrderingSyntax> initList)
        {
            ((List<PocoOrderingSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoOrderingSyntax>();
    }

    public class PocoOrderingSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Expression
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken AscendingOrDescendingKeyword
        {
            get;
            set;
        }
    }

    public class PocoParameterListSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoParameterListSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoParameterListSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoParameterListSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoParameterListSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoParameterListSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoParameterListSyntaxCollection(IList<PocoParameterListSyntax> initList)
        {
            ((List<PocoParameterListSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoParameterListSyntax>();
    }

    public class PocoParameterListSyntax : PocoBaseParameterListSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenParenToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoParameterSyntaxCollection Parameters
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseParenToken
        {
            get;
            set;
        }
    }

    public class PocoParameterSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoParameterSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoParameterSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoParameterSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoParameterSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoParameterSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoParameterSyntaxCollection(IList<PocoParameterSyntax> initList)
        {
            ((List<PocoParameterSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoParameterSyntax>();
    }

    public class PocoParameterSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoAttributeListSyntaxCollection AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxTokenList Modifiers
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoTypeSyntax Type
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Identifier
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoEqualsValueClauseSyntax Default
        {
            get;
            set;
        }
    }

    public class PocoParenthesizedExpressionSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoParenthesizedExpressionSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoParenthesizedExpressionSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoParenthesizedExpressionSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoParenthesizedExpressionSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoParenthesizedExpressionSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoParenthesizedExpressionSyntaxCollection(IList<PocoParenthesizedExpressionSyntax> initList)
        {
            ((List<PocoParenthesizedExpressionSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoParenthesizedExpressionSyntax>();
    }

    public class PocoParenthesizedExpressionSyntax : PocoExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenParenToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Expression
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseParenToken
        {
            get;
            set;
        }
    }

    public class PocoParenthesizedLambdaExpressionSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoParenthesizedLambdaExpressionSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoParenthesizedLambdaExpressionSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoParenthesizedLambdaExpressionSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoParenthesizedLambdaExpressionSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoParenthesizedLambdaExpressionSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoParenthesizedLambdaExpressionSyntaxCollection(IList<PocoParenthesizedLambdaExpressionSyntax> initList)
        {
            ((List<PocoParenthesizedLambdaExpressionSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoParenthesizedLambdaExpressionSyntax>();
    }

    public class PocoParenthesizedLambdaExpressionSyntax : PocoLambdaExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken AsyncKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoParameterListSyntax ParameterList
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken ArrowToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoBlockSyntax Block
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoExpressionSyntax ExpressionBody
        {
            get;
            set;
        }
    }

    public class PocoParenthesizedVariableDesignationSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoParenthesizedVariableDesignationSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoParenthesizedVariableDesignationSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoParenthesizedVariableDesignationSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoParenthesizedVariableDesignationSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoParenthesizedVariableDesignationSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoParenthesizedVariableDesignationSyntaxCollection(IList<PocoParenthesizedVariableDesignationSyntax> initList)
        {
            ((List<PocoParenthesizedVariableDesignationSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoParenthesizedVariableDesignationSyntax>();
    }

    public class PocoParenthesizedVariableDesignationSyntax : PocoVariableDesignationSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenParenToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoVariableDesignationSyntaxCollection Variables
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseParenToken
        {
            get;
            set;
        }
    }

    public class PocoPatternSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoPatternSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoPatternSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoPatternSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoPatternSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoPatternSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoPatternSyntaxCollection(IList<PocoPatternSyntax> initList)
        {
            ((List<PocoPatternSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoPatternSyntax>();
    }

    public class PocoPatternSyntax : PocoCSharpSyntaxNode
    {
    }

    public class PocoPointerTypeSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoPointerTypeSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoPointerTypeSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoPointerTypeSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoPointerTypeSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoPointerTypeSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoPointerTypeSyntaxCollection(IList<PocoPointerTypeSyntax> initList)
        {
            ((List<PocoPointerTypeSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoPointerTypeSyntax>();
    }

    public class PocoPointerTypeSyntax : PocoTypeSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoTypeSyntax ElementType
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken AsteriskToken
        {
            get;
            set;
        }
    }

    public class PocoPositionalPatternClauseSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoPositionalPatternClauseSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoPositionalPatternClauseSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoPositionalPatternClauseSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoPositionalPatternClauseSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoPositionalPatternClauseSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoPositionalPatternClauseSyntaxCollection(IList<PocoPositionalPatternClauseSyntax> initList)
        {
            ((List<PocoPositionalPatternClauseSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoPositionalPatternClauseSyntax>();
    }

    public class PocoPositionalPatternClauseSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenParenToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSubpatternSyntaxCollection Subpatterns
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseParenToken
        {
            get;
            set;
        }
    }

    public class PocoPostfixUnaryExpressionSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoPostfixUnaryExpressionSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoPostfixUnaryExpressionSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoPostfixUnaryExpressionSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoPostfixUnaryExpressionSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoPostfixUnaryExpressionSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoPostfixUnaryExpressionSyntaxCollection(IList<PocoPostfixUnaryExpressionSyntax> initList)
        {
            ((List<PocoPostfixUnaryExpressionSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoPostfixUnaryExpressionSyntax>();
    }

    public class PocoPostfixUnaryExpressionSyntax : PocoExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Operand
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OperatorToken
        {
            get;
            set;
        }
    }

    public class PocoPragmaChecksumDirectiveTriviaSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoPragmaChecksumDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoPragmaChecksumDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoPragmaChecksumDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoPragmaChecksumDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoPragmaChecksumDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoPragmaChecksumDirectiveTriviaSyntaxCollection(IList<PocoPragmaChecksumDirectiveTriviaSyntax> initList)
        {
            ((List<PocoPragmaChecksumDirectiveTriviaSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoPragmaChecksumDirectiveTriviaSyntax>();
    }

    public class PocoPragmaChecksumDirectiveTriviaSyntax : PocoDirectiveTriviaSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken HashToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken PragmaKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken ChecksumKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken File
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Guid
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Bytes
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken EndOfDirectiveToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override bool IsActive
        {
            get;
            set;
        }
    }

    public class PocoPragmaWarningDirectiveTriviaSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoPragmaWarningDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoPragmaWarningDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoPragmaWarningDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoPragmaWarningDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoPragmaWarningDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoPragmaWarningDirectiveTriviaSyntaxCollection(IList<PocoPragmaWarningDirectiveTriviaSyntax> initList)
        {
            ((List<PocoPragmaWarningDirectiveTriviaSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoPragmaWarningDirectiveTriviaSyntax>();
    }

    public class PocoPragmaWarningDirectiveTriviaSyntax : PocoDirectiveTriviaSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken HashToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken PragmaKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken WarningKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken DisableOrRestoreKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntaxCollection ErrorCodes
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken EndOfDirectiveToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override bool IsActive
        {
            get;
            set;
        }
    }

    public class PocoPredefinedTypeSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoPredefinedTypeSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoPredefinedTypeSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoPredefinedTypeSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoPredefinedTypeSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoPredefinedTypeSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoPredefinedTypeSyntaxCollection(IList<PocoPredefinedTypeSyntax> initList)
        {
            ((List<PocoPredefinedTypeSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoPredefinedTypeSyntax>();
    }

    public class PocoPredefinedTypeSyntax : PocoTypeSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Keyword
        {
            get;
            set;
        }
    }

    public class PocoPrefixUnaryExpressionSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoPrefixUnaryExpressionSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoPrefixUnaryExpressionSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoPrefixUnaryExpressionSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoPrefixUnaryExpressionSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoPrefixUnaryExpressionSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoPrefixUnaryExpressionSyntaxCollection(IList<PocoPrefixUnaryExpressionSyntax> initList)
        {
            ((List<PocoPrefixUnaryExpressionSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoPrefixUnaryExpressionSyntax>();
    }

    public class PocoPrefixUnaryExpressionSyntax : PocoExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OperatorToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Operand
        {
            get;
            set;
        }
    }

    public class PocoPropertyDeclarationSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoPropertyDeclarationSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoPropertyDeclarationSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoPropertyDeclarationSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoPropertyDeclarationSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoPropertyDeclarationSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoPropertyDeclarationSyntaxCollection(IList<PocoPropertyDeclarationSyntax> initList)
        {
            ((List<PocoPropertyDeclarationSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoPropertyDeclarationSyntax>();
    }

    public class PocoPropertyDeclarationSyntax : PocoBasePropertyDeclarationSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoAttributeListSyntaxCollection AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxTokenList Modifiers
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoTypeSyntax Type
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoExplicitInterfaceSpecifierSyntax ExplicitInterfaceSpecifier
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Identifier
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoAccessorListSyntax AccessorList
        {
            get;
            set;
        }
    }

    public class PocoPropertyPatternClauseSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoPropertyPatternClauseSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoPropertyPatternClauseSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoPropertyPatternClauseSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoPropertyPatternClauseSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoPropertyPatternClauseSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoPropertyPatternClauseSyntaxCollection(IList<PocoPropertyPatternClauseSyntax> initList)
        {
            ((List<PocoPropertyPatternClauseSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoPropertyPatternClauseSyntax>();
    }

    public class PocoPropertyPatternClauseSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenBraceToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSubpatternSyntaxCollection Subpatterns
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseBraceToken
        {
            get;
            set;
        }
    }

    public class PocoQualifiedCrefSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoQualifiedCrefSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoQualifiedCrefSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoQualifiedCrefSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoQualifiedCrefSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoQualifiedCrefSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoQualifiedCrefSyntaxCollection(IList<PocoQualifiedCrefSyntax> initList)
        {
            ((List<PocoQualifiedCrefSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoQualifiedCrefSyntax>();
    }

    public class PocoQualifiedCrefSyntax : PocoCrefSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoTypeSyntax Container
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken DotToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoMemberCrefSyntax Member
        {
            get;
            set;
        }
    }

    public class PocoQualifiedNameSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoQualifiedNameSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoQualifiedNameSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoQualifiedNameSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoQualifiedNameSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoQualifiedNameSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoQualifiedNameSyntaxCollection(IList<PocoQualifiedNameSyntax> initList)
        {
            ((List<PocoQualifiedNameSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoQualifiedNameSyntax>();
    }

    public class PocoQualifiedNameSyntax : PocoNameSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoNameSyntax Left
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken DotToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSimpleNameSyntax Right
        {
            get;
            set;
        }
    }

    public class PocoQueryBodySyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoQueryBodySyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoQueryBodySyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoQueryBodySyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoQueryBodySyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoQueryBodySyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoQueryBodySyntaxCollection(IList<PocoQueryBodySyntax> initList)
        {
            ((List<PocoQueryBodySyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoQueryBodySyntax>();
    }

    public class PocoQueryBodySyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoQueryClauseSyntaxCollection Clauses
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSelectOrGroupClauseSyntax SelectOrGroup
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoQueryContinuationSyntax Continuation
        {
            get;
            set;
        }
    }

    public class PocoQueryClauseSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoQueryClauseSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoQueryClauseSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoQueryClauseSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoQueryClauseSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoQueryClauseSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoQueryClauseSyntaxCollection(IList<PocoQueryClauseSyntax> initList)
        {
            ((List<PocoQueryClauseSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoQueryClauseSyntax>();
    }

    public class PocoQueryClauseSyntax : PocoCSharpSyntaxNode
    {
    }

    public class PocoQueryContinuationSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoQueryContinuationSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoQueryContinuationSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoQueryContinuationSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoQueryContinuationSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoQueryContinuationSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoQueryContinuationSyntaxCollection(IList<PocoQueryContinuationSyntax> initList)
        {
            ((List<PocoQueryContinuationSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoQueryContinuationSyntax>();
    }

    public class PocoQueryContinuationSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken IntoKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Identifier
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoQueryBodySyntax Body
        {
            get;
            set;
        }
    }

    public class PocoQueryExpressionSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoQueryExpressionSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoQueryExpressionSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoQueryExpressionSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoQueryExpressionSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoQueryExpressionSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoQueryExpressionSyntaxCollection(IList<PocoQueryExpressionSyntax> initList)
        {
            ((List<PocoQueryExpressionSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoQueryExpressionSyntax>();
    }

    public class PocoQueryExpressionSyntax : PocoExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoFromClauseSyntax FromClause
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoQueryBodySyntax Body
        {
            get;
            set;
        }
    }

    public class PocoRangeExpressionSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoRangeExpressionSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoRangeExpressionSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoRangeExpressionSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoRangeExpressionSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoRangeExpressionSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoRangeExpressionSyntaxCollection(IList<PocoRangeExpressionSyntax> initList)
        {
            ((List<PocoRangeExpressionSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoRangeExpressionSyntax>();
    }

    public class PocoRangeExpressionSyntax : PocoExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax LeftOperand
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OperatorToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax RightOperand
        {
            get;
            set;
        }
    }

    public class PocoRecursivePatternSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoRecursivePatternSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoRecursivePatternSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoRecursivePatternSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoRecursivePatternSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoRecursivePatternSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoRecursivePatternSyntaxCollection(IList<PocoRecursivePatternSyntax> initList)
        {
            ((List<PocoRecursivePatternSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoRecursivePatternSyntax>();
    }

    public class PocoRecursivePatternSyntax : PocoPatternSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoTypeSyntax Type
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoPositionalPatternClauseSyntax PositionalPatternClause
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoPropertyPatternClauseSyntax PropertyPatternClause
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoVariableDesignationSyntax Designation
        {
            get;
            set;
        }
    }

    public class PocoReferenceDirectiveTriviaSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoReferenceDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoReferenceDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoReferenceDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoReferenceDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoReferenceDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoReferenceDirectiveTriviaSyntaxCollection(IList<PocoReferenceDirectiveTriviaSyntax> initList)
        {
            ((List<PocoReferenceDirectiveTriviaSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoReferenceDirectiveTriviaSyntax>();
    }

    public class PocoReferenceDirectiveTriviaSyntax : PocoDirectiveTriviaSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken HashToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken ReferenceKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken File
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken EndOfDirectiveToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override bool IsActive
        {
            get;
            set;
        }
    }

    public class PocoRefExpressionSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoRefExpressionSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoRefExpressionSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoRefExpressionSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoRefExpressionSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoRefExpressionSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoRefExpressionSyntaxCollection(IList<PocoRefExpressionSyntax> initList)
        {
            ((List<PocoRefExpressionSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoRefExpressionSyntax>();
    }

    public class PocoRefExpressionSyntax : PocoExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken RefKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Expression
        {
            get;
            set;
        }
    }

    public class PocoRefTypeExpressionSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoRefTypeExpressionSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoRefTypeExpressionSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoRefTypeExpressionSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoRefTypeExpressionSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoRefTypeExpressionSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoRefTypeExpressionSyntaxCollection(IList<PocoRefTypeExpressionSyntax> initList)
        {
            ((List<PocoRefTypeExpressionSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoRefTypeExpressionSyntax>();
    }

    public class PocoRefTypeExpressionSyntax : PocoExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Keyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenParenToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Expression
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseParenToken
        {
            get;
            set;
        }
    }

    public class PocoRefTypeSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoRefTypeSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoRefTypeSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoRefTypeSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoRefTypeSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoRefTypeSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoRefTypeSyntaxCollection(IList<PocoRefTypeSyntax> initList)
        {
            ((List<PocoRefTypeSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoRefTypeSyntax>();
    }

    public class PocoRefTypeSyntax : PocoTypeSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken RefKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken ReadOnlyKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoTypeSyntax Type
        {
            get;
            set;
        }
    }

    public class PocoRefValueExpressionSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoRefValueExpressionSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoRefValueExpressionSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoRefValueExpressionSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoRefValueExpressionSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoRefValueExpressionSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoRefValueExpressionSyntaxCollection(IList<PocoRefValueExpressionSyntax> initList)
        {
            ((List<PocoRefValueExpressionSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoRefValueExpressionSyntax>();
    }

    public class PocoRefValueExpressionSyntax : PocoExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Keyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenParenToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Expression
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Comma
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoTypeSyntax Type
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseParenToken
        {
            get;
            set;
        }
    }

    public class PocoRegionDirectiveTriviaSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoRegionDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoRegionDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoRegionDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoRegionDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoRegionDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoRegionDirectiveTriviaSyntaxCollection(IList<PocoRegionDirectiveTriviaSyntax> initList)
        {
            ((List<PocoRegionDirectiveTriviaSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoRegionDirectiveTriviaSyntax>();
    }

    public class PocoRegionDirectiveTriviaSyntax : PocoDirectiveTriviaSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken HashToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken RegionKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken EndOfDirectiveToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override bool IsActive
        {
            get;
            set;
        }
    }

    public class PocoReturnStatementSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoReturnStatementSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoReturnStatementSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoReturnStatementSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoReturnStatementSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoReturnStatementSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoReturnStatementSyntaxCollection(IList<PocoReturnStatementSyntax> initList)
        {
            ((List<PocoReturnStatementSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoReturnStatementSyntax>();
    }

    public class PocoReturnStatementSyntax : PocoStatementSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoAttributeListSyntaxCollection AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken ReturnKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Expression
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken SemicolonToken
        {
            get;
            set;
        }
    }

    public class PocoSelectClauseSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoSelectClauseSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoSelectClauseSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoSelectClauseSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoSelectClauseSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoSelectClauseSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoSelectClauseSyntaxCollection(IList<PocoSelectClauseSyntax> initList)
        {
            ((List<PocoSelectClauseSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoSelectClauseSyntax>();
    }

    public class PocoSelectClauseSyntax : PocoSelectOrGroupClauseSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken SelectKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Expression
        {
            get;
            set;
        }
    }

    public class PocoSelectOrGroupClauseSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoSelectOrGroupClauseSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoSelectOrGroupClauseSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoSelectOrGroupClauseSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoSelectOrGroupClauseSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoSelectOrGroupClauseSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoSelectOrGroupClauseSyntaxCollection(IList<PocoSelectOrGroupClauseSyntax> initList)
        {
            ((List<PocoSelectOrGroupClauseSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoSelectOrGroupClauseSyntax>();
    }

    public class PocoSelectOrGroupClauseSyntax : PocoCSharpSyntaxNode
    {
    }

    public class PocoShebangDirectiveTriviaSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoShebangDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoShebangDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoShebangDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoShebangDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoShebangDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoShebangDirectiveTriviaSyntaxCollection(IList<PocoShebangDirectiveTriviaSyntax> initList)
        {
            ((List<PocoShebangDirectiveTriviaSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoShebangDirectiveTriviaSyntax>();
    }

    public class PocoShebangDirectiveTriviaSyntax : PocoDirectiveTriviaSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken HashToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken ExclamationToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken EndOfDirectiveToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override bool IsActive
        {
            get;
            set;
        }
    }

    public class PocoSimpleBaseTypeSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoSimpleBaseTypeSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoSimpleBaseTypeSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoSimpleBaseTypeSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoSimpleBaseTypeSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoSimpleBaseTypeSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoSimpleBaseTypeSyntaxCollection(IList<PocoSimpleBaseTypeSyntax> initList)
        {
            ((List<PocoSimpleBaseTypeSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoSimpleBaseTypeSyntax>();
    }

    public class PocoSimpleBaseTypeSyntax : PocoBaseTypeSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoTypeSyntax Type
        {
            get;
            set;
        }
    }

    public class PocoSimpleLambdaExpressionSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoSimpleLambdaExpressionSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoSimpleLambdaExpressionSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoSimpleLambdaExpressionSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoSimpleLambdaExpressionSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoSimpleLambdaExpressionSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoSimpleLambdaExpressionSyntaxCollection(IList<PocoSimpleLambdaExpressionSyntax> initList)
        {
            ((List<PocoSimpleLambdaExpressionSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoSimpleLambdaExpressionSyntax>();
    }

    public class PocoSimpleLambdaExpressionSyntax : PocoLambdaExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken AsyncKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoParameterSyntax Parameter
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken ArrowToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoBlockSyntax Block
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoExpressionSyntax ExpressionBody
        {
            get;
            set;
        }
    }

    public class PocoSimpleNameSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoSimpleNameSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoSimpleNameSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoSimpleNameSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoSimpleNameSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoSimpleNameSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoSimpleNameSyntaxCollection(IList<PocoSimpleNameSyntax> initList)
        {
            ((List<PocoSimpleNameSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoSimpleNameSyntax>();
    }

    public class PocoSimpleNameSyntax : PocoNameSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Identifier
        {
            get;
            set;
        }
    }

    public class PocoSingleVariableDesignationSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoSingleVariableDesignationSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoSingleVariableDesignationSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoSingleVariableDesignationSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoSingleVariableDesignationSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoSingleVariableDesignationSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoSingleVariableDesignationSyntaxCollection(IList<PocoSingleVariableDesignationSyntax> initList)
        {
            ((List<PocoSingleVariableDesignationSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoSingleVariableDesignationSyntax>();
    }

    public class PocoSingleVariableDesignationSyntax : PocoVariableDesignationSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Identifier
        {
            get;
            set;
        }
    }

    public class PocoSizeOfExpressionSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoSizeOfExpressionSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoSizeOfExpressionSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoSizeOfExpressionSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoSizeOfExpressionSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoSizeOfExpressionSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoSizeOfExpressionSyntaxCollection(IList<PocoSizeOfExpressionSyntax> initList)
        {
            ((List<PocoSizeOfExpressionSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoSizeOfExpressionSyntax>();
    }

    public class PocoSizeOfExpressionSyntax : PocoExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Keyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenParenToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoTypeSyntax Type
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseParenToken
        {
            get;
            set;
        }
    }

    public class PocoSkippedTokensTriviaSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoSkippedTokensTriviaSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoSkippedTokensTriviaSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoSkippedTokensTriviaSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoSkippedTokensTriviaSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoSkippedTokensTriviaSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoSkippedTokensTriviaSyntaxCollection(IList<PocoSkippedTokensTriviaSyntax> initList)
        {
            ((List<PocoSkippedTokensTriviaSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoSkippedTokensTriviaSyntax>();
    }

    public class PocoSkippedTokensTriviaSyntax : PocoStructuredTriviaSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxTokenList Tokens
        {
            get;
            set;
        }
    }

    public class PocoStackAllocArrayCreationExpressionSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoStackAllocArrayCreationExpressionSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoStackAllocArrayCreationExpressionSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoStackAllocArrayCreationExpressionSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoStackAllocArrayCreationExpressionSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoStackAllocArrayCreationExpressionSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoStackAllocArrayCreationExpressionSyntaxCollection(IList<PocoStackAllocArrayCreationExpressionSyntax> initList)
        {
            ((List<PocoStackAllocArrayCreationExpressionSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoStackAllocArrayCreationExpressionSyntax>();
    }

    public class PocoStackAllocArrayCreationExpressionSyntax : PocoExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken StackAllocKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoTypeSyntax Type
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoInitializerExpressionSyntax Initializer
        {
            get;
            set;
        }
    }

    public class PocoStatementSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoStatementSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoStatementSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoStatementSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoStatementSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoStatementSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoStatementSyntaxCollection(IList<PocoStatementSyntax> initList)
        {
            ((List<PocoStatementSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoStatementSyntax>();
    }

    public class PocoStatementSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoAttributeListSyntaxCollection AttributeLists
        {
            get;
            set;
        }
    }

    public class PocoStructDeclarationSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoStructDeclarationSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoStructDeclarationSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoStructDeclarationSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoStructDeclarationSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoStructDeclarationSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoStructDeclarationSyntaxCollection(IList<PocoStructDeclarationSyntax> initList)
        {
            ((List<PocoStructDeclarationSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoStructDeclarationSyntax>();
    }

    public class PocoStructDeclarationSyntax : PocoTypeDeclarationSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoAttributeListSyntaxCollection AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxTokenList Modifiers
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken Keyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken Identifier
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoTypeParameterListSyntax TypeParameterList
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoBaseListSyntax BaseList
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoTypeParameterConstraintClauseSyntaxCollection ConstraintClauses
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken OpenBraceToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoMemberDeclarationSyntaxCollection Members
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken CloseBraceToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken SemicolonToken
        {
            get;
            set;
        }
    }

    public class PocoStructuredTriviaSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoStructuredTriviaSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoStructuredTriviaSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoStructuredTriviaSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoStructuredTriviaSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoStructuredTriviaSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoStructuredTriviaSyntaxCollection(IList<PocoStructuredTriviaSyntax> initList)
        {
            ((List<PocoStructuredTriviaSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoStructuredTriviaSyntax>();
    }

    public class PocoStructuredTriviaSyntax : PocoCSharpSyntaxNode
    {
    }

    public class PocoSubpatternSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoSubpatternSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoSubpatternSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoSubpatternSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoSubpatternSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoSubpatternSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoSubpatternSyntaxCollection(IList<PocoSubpatternSyntax> initList)
        {
            ((List<PocoSubpatternSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoSubpatternSyntax>();
    }

    public class PocoSubpatternSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoNameColonSyntax NameColon
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoPatternSyntax Pattern
        {
            get;
            set;
        }
    }

    public class PocoSwitchExpressionArmSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoSwitchExpressionArmSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoSwitchExpressionArmSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoSwitchExpressionArmSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoSwitchExpressionArmSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoSwitchExpressionArmSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoSwitchExpressionArmSyntaxCollection(IList<PocoSwitchExpressionArmSyntax> initList)
        {
            ((List<PocoSwitchExpressionArmSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoSwitchExpressionArmSyntax>();
    }

    public class PocoSwitchExpressionArmSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoPatternSyntax Pattern
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoWhenClauseSyntax WhenClause
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken EqualsGreaterThanToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Expression
        {
            get;
            set;
        }
    }

    public class PocoSwitchExpressionSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoSwitchExpressionSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoSwitchExpressionSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoSwitchExpressionSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoSwitchExpressionSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoSwitchExpressionSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoSwitchExpressionSyntaxCollection(IList<PocoSwitchExpressionSyntax> initList)
        {
            ((List<PocoSwitchExpressionSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoSwitchExpressionSyntax>();
    }

    public class PocoSwitchExpressionSyntax : PocoExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax GoverningExpression
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken SwitchKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenBraceToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSwitchExpressionArmSyntaxCollection Arms
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseBraceToken
        {
            get;
            set;
        }
    }

    public class PocoSwitchLabelSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoSwitchLabelSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoSwitchLabelSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoSwitchLabelSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoSwitchLabelSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoSwitchLabelSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoSwitchLabelSyntaxCollection(IList<PocoSwitchLabelSyntax> initList)
        {
            ((List<PocoSwitchLabelSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoSwitchLabelSyntax>();
    }

    public class PocoSwitchLabelSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Keyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken ColonToken
        {
            get;
            set;
        }
    }

    public class PocoSwitchSectionSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoSwitchSectionSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoSwitchSectionSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoSwitchSectionSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoSwitchSectionSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoSwitchSectionSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoSwitchSectionSyntaxCollection(IList<PocoSwitchSectionSyntax> initList)
        {
            ((List<PocoSwitchSectionSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoSwitchSectionSyntax>();
    }

    public class PocoSwitchSectionSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSwitchLabelSyntaxCollection Labels
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoStatementSyntaxCollection Statements
        {
            get;
            set;
        }
    }

    public class PocoSwitchStatementSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoSwitchStatementSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoSwitchStatementSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoSwitchStatementSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoSwitchStatementSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoSwitchStatementSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoSwitchStatementSyntaxCollection(IList<PocoSwitchStatementSyntax> initList)
        {
            ((List<PocoSwitchStatementSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoSwitchStatementSyntax>();
    }

    public class PocoSwitchStatementSyntax : PocoStatementSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoAttributeListSyntaxCollection AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken SwitchKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenParenToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Expression
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseParenToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenBraceToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSwitchSectionSyntaxCollection Sections
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseBraceToken
        {
            get;
            set;
        }
    }

    public class PocoThisExpressionSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoThisExpressionSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoThisExpressionSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoThisExpressionSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoThisExpressionSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoThisExpressionSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoThisExpressionSyntaxCollection(IList<PocoThisExpressionSyntax> initList)
        {
            ((List<PocoThisExpressionSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoThisExpressionSyntax>();
    }

    public class PocoThisExpressionSyntax : PocoInstanceExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Token
        {
            get;
            set;
        }
    }

    public class PocoThrowExpressionSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoThrowExpressionSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoThrowExpressionSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoThrowExpressionSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoThrowExpressionSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoThrowExpressionSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoThrowExpressionSyntaxCollection(IList<PocoThrowExpressionSyntax> initList)
        {
            ((List<PocoThrowExpressionSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoThrowExpressionSyntax>();
    }

    public class PocoThrowExpressionSyntax : PocoExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken ThrowKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Expression
        {
            get;
            set;
        }
    }

    public class PocoThrowStatementSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoThrowStatementSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoThrowStatementSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoThrowStatementSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoThrowStatementSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoThrowStatementSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoThrowStatementSyntaxCollection(IList<PocoThrowStatementSyntax> initList)
        {
            ((List<PocoThrowStatementSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoThrowStatementSyntax>();
    }

    public class PocoThrowStatementSyntax : PocoStatementSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoAttributeListSyntaxCollection AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken ThrowKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Expression
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken SemicolonToken
        {
            get;
            set;
        }
    }

    public class PocoTryStatementSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoTryStatementSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoTryStatementSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoTryStatementSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoTryStatementSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoTryStatementSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoTryStatementSyntaxCollection(IList<PocoTryStatementSyntax> initList)
        {
            ((List<PocoTryStatementSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoTryStatementSyntax>();
    }

    public class PocoTryStatementSyntax : PocoStatementSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoAttributeListSyntaxCollection AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken TryKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoBlockSyntax Block
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoCatchClauseSyntaxCollection Catches
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoFinallyClauseSyntax Finally
        {
            get;
            set;
        }
    }

    public class PocoTupleElementSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoTupleElementSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoTupleElementSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoTupleElementSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoTupleElementSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoTupleElementSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoTupleElementSyntaxCollection(IList<PocoTupleElementSyntax> initList)
        {
            ((List<PocoTupleElementSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoTupleElementSyntax>();
    }

    public class PocoTupleElementSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoTypeSyntax Type
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Identifier
        {
            get;
            set;
        }
    }

    public class PocoTupleExpressionSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoTupleExpressionSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoTupleExpressionSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoTupleExpressionSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoTupleExpressionSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoTupleExpressionSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoTupleExpressionSyntaxCollection(IList<PocoTupleExpressionSyntax> initList)
        {
            ((List<PocoTupleExpressionSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoTupleExpressionSyntax>();
    }

    public class PocoTupleExpressionSyntax : PocoExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenParenToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoArgumentSyntaxCollection Arguments
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseParenToken
        {
            get;
            set;
        }
    }

    public class PocoTupleTypeSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoTupleTypeSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoTupleTypeSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoTupleTypeSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoTupleTypeSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoTupleTypeSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoTupleTypeSyntaxCollection(IList<PocoTupleTypeSyntax> initList)
        {
            ((List<PocoTupleTypeSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoTupleTypeSyntax>();
    }

    public class PocoTupleTypeSyntax : PocoTypeSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenParenToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoTupleElementSyntaxCollection Elements
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseParenToken
        {
            get;
            set;
        }
    }

    public class PocoTypeArgumentListSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoTypeArgumentListSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoTypeArgumentListSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoTypeArgumentListSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoTypeArgumentListSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoTypeArgumentListSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoTypeArgumentListSyntaxCollection(IList<PocoTypeArgumentListSyntax> initList)
        {
            ((List<PocoTypeArgumentListSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoTypeArgumentListSyntax>();
    }

    public class PocoTypeArgumentListSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken LessThanToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoTypeSyntaxCollection Arguments
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken GreaterThanToken
        {
            get;
            set;
        }
    }

    public class PocoTypeConstraintSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoTypeConstraintSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoTypeConstraintSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoTypeConstraintSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoTypeConstraintSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoTypeConstraintSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoTypeConstraintSyntaxCollection(IList<PocoTypeConstraintSyntax> initList)
        {
            ((List<PocoTypeConstraintSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoTypeConstraintSyntax>();
    }

    public class PocoTypeConstraintSyntax : PocoTypeParameterConstraintSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoTypeSyntax Type
        {
            get;
            set;
        }
    }

    public class PocoTypeCrefSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoTypeCrefSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoTypeCrefSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoTypeCrefSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoTypeCrefSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoTypeCrefSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoTypeCrefSyntaxCollection(IList<PocoTypeCrefSyntax> initList)
        {
            ((List<PocoTypeCrefSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoTypeCrefSyntax>();
    }

    public class PocoTypeCrefSyntax : PocoCrefSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoTypeSyntax Type
        {
            get;
            set;
        }
    }

    public class PocoTypeDeclarationSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoTypeDeclarationSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoTypeDeclarationSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoTypeDeclarationSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoTypeDeclarationSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoTypeDeclarationSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoTypeDeclarationSyntaxCollection(IList<PocoTypeDeclarationSyntax> initList)
        {
            ((List<PocoTypeDeclarationSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoTypeDeclarationSyntax>();
    }

    public class PocoTypeDeclarationSyntax : PocoBaseTypeDeclarationSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Keyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoTypeParameterListSyntax TypeParameterList
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoTypeParameterConstraintClauseSyntaxCollection ConstraintClauses
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoMemberDeclarationSyntaxCollection Members
        {
            get;
            set;
        }
    }

    public class PocoTypeOfExpressionSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoTypeOfExpressionSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoTypeOfExpressionSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoTypeOfExpressionSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoTypeOfExpressionSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoTypeOfExpressionSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoTypeOfExpressionSyntaxCollection(IList<PocoTypeOfExpressionSyntax> initList)
        {
            ((List<PocoTypeOfExpressionSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoTypeOfExpressionSyntax>();
    }

    public class PocoTypeOfExpressionSyntax : PocoExpressionSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Keyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenParenToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoTypeSyntax Type
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseParenToken
        {
            get;
            set;
        }
    }

    public class PocoTypeParameterConstraintClauseSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoTypeParameterConstraintClauseSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoTypeParameterConstraintClauseSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoTypeParameterConstraintClauseSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoTypeParameterConstraintClauseSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoTypeParameterConstraintClauseSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoTypeParameterConstraintClauseSyntaxCollection(IList<PocoTypeParameterConstraintClauseSyntax> initList)
        {
            ((List<PocoTypeParameterConstraintClauseSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoTypeParameterConstraintClauseSyntax>();
    }

    public class PocoTypeParameterConstraintClauseSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken WhereKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoIdentifierNameSyntax Name
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken ColonToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoTypeParameterConstraintSyntaxCollection Constraints
        {
            get;
            set;
        }
    }

    public class PocoTypeParameterConstraintSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoTypeParameterConstraintSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoTypeParameterConstraintSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoTypeParameterConstraintSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoTypeParameterConstraintSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoTypeParameterConstraintSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoTypeParameterConstraintSyntaxCollection(IList<PocoTypeParameterConstraintSyntax> initList)
        {
            ((List<PocoTypeParameterConstraintSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoTypeParameterConstraintSyntax>();
    }

    public class PocoTypeParameterConstraintSyntax : PocoCSharpSyntaxNode
    {
    }

    public class PocoTypeParameterListSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoTypeParameterListSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoTypeParameterListSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoTypeParameterListSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoTypeParameterListSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoTypeParameterListSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoTypeParameterListSyntaxCollection(IList<PocoTypeParameterListSyntax> initList)
        {
            ((List<PocoTypeParameterListSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoTypeParameterListSyntax>();
    }

    public class PocoTypeParameterListSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken LessThanToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoTypeParameterSyntaxCollection Parameters
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken GreaterThanToken
        {
            get;
            set;
        }
    }

    public class PocoTypeParameterSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoTypeParameterSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoTypeParameterSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoTypeParameterSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoTypeParameterSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoTypeParameterSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoTypeParameterSyntaxCollection(IList<PocoTypeParameterSyntax> initList)
        {
            ((List<PocoTypeParameterSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoTypeParameterSyntax>();
    }

    public class PocoTypeParameterSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoAttributeListSyntaxCollection AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken VarianceKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Identifier
        {
            get;
            set;
        }
    }

    public class PocoTypeSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoTypeSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoTypeSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoTypeSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoTypeSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoTypeSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoTypeSyntaxCollection(IList<PocoTypeSyntax> initList)
        {
            ((List<PocoTypeSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoTypeSyntax>();
    }

    public class PocoTypeSyntax : PocoExpressionSyntax
    {
    }

    public class PocoUndefDirectiveTriviaSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoUndefDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoUndefDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoUndefDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoUndefDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoUndefDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoUndefDirectiveTriviaSyntaxCollection(IList<PocoUndefDirectiveTriviaSyntax> initList)
        {
            ((List<PocoUndefDirectiveTriviaSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoUndefDirectiveTriviaSyntax>();
    }

    public class PocoUndefDirectiveTriviaSyntax : PocoDirectiveTriviaSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken HashToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken UndefKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Name
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken EndOfDirectiveToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override bool IsActive
        {
            get;
            set;
        }
    }

    public class PocoUnsafeStatementSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoUnsafeStatementSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoUnsafeStatementSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoUnsafeStatementSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoUnsafeStatementSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoUnsafeStatementSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoUnsafeStatementSyntaxCollection(IList<PocoUnsafeStatementSyntax> initList)
        {
            ((List<PocoUnsafeStatementSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoUnsafeStatementSyntax>();
    }

    public class PocoUnsafeStatementSyntax : PocoStatementSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoAttributeListSyntaxCollection AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken UnsafeKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoBlockSyntax Block
        {
            get;
            set;
        }
    }

    public class PocoUsingDirectiveSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoUsingDirectiveSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoUsingDirectiveSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoUsingDirectiveSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoUsingDirectiveSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoUsingDirectiveSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoUsingDirectiveSyntaxCollection(IList<PocoUsingDirectiveSyntax> initList)
        {
            ((List<PocoUsingDirectiveSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoUsingDirectiveSyntax>();
    }

    public class PocoUsingDirectiveSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken UsingKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoNameSyntax Name
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken SemicolonToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken StaticKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoNameEqualsSyntax Alias
        {
            get;
            set;
        }
    }

    public class PocoUsingStatementSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoUsingStatementSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoUsingStatementSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoUsingStatementSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoUsingStatementSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoUsingStatementSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoUsingStatementSyntaxCollection(IList<PocoUsingStatementSyntax> initList)
        {
            ((List<PocoUsingStatementSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoUsingStatementSyntax>();
    }

    public class PocoUsingStatementSyntax : PocoStatementSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoAttributeListSyntaxCollection AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken AwaitKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken UsingKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenParenToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseParenToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoStatementSyntax Statement
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoVariableDeclarationSyntax Declaration
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Expression
        {
            get;
            set;
        }
    }

    public class PocoVariableDeclarationSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoVariableDeclarationSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoVariableDeclarationSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoVariableDeclarationSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoVariableDeclarationSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoVariableDeclarationSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoVariableDeclarationSyntaxCollection(IList<PocoVariableDeclarationSyntax> initList)
        {
            ((List<PocoVariableDeclarationSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoVariableDeclarationSyntax>();
    }

    public class PocoVariableDeclarationSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoTypeSyntax Type
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoVariableDeclaratorSyntaxCollection Variables
        {
            get;
            set;
        }
    }

    public class PocoVariableDeclaratorSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoVariableDeclaratorSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoVariableDeclaratorSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoVariableDeclaratorSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoVariableDeclaratorSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoVariableDeclaratorSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoVariableDeclaratorSyntaxCollection(IList<PocoVariableDeclaratorSyntax> initList)
        {
            ((List<PocoVariableDeclaratorSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoVariableDeclaratorSyntax>();
    }

    public class PocoVariableDeclaratorSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Identifier
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoBracketedArgumentListSyntax ArgumentList
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoEqualsValueClauseSyntax Initializer
        {
            get;
            set;
        }
    }

    public class PocoVariableDesignationSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoVariableDesignationSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoVariableDesignationSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoVariableDesignationSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoVariableDesignationSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoVariableDesignationSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoVariableDesignationSyntaxCollection(IList<PocoVariableDesignationSyntax> initList)
        {
            ((List<PocoVariableDesignationSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoVariableDesignationSyntax>();
    }

    public class PocoVariableDesignationSyntax : PocoCSharpSyntaxNode
    {
    }

    public class PocoVarPatternSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoVarPatternSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoVarPatternSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoVarPatternSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoVarPatternSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoVarPatternSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoVarPatternSyntaxCollection(IList<PocoVarPatternSyntax> initList)
        {
            ((List<PocoVarPatternSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoVarPatternSyntax>();
    }

    public class PocoVarPatternSyntax : PocoPatternSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken VarKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoVariableDesignationSyntax Designation
        {
            get;
            set;
        }
    }

    public class PocoWarningDirectiveTriviaSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoWarningDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoWarningDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoWarningDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoWarningDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoWarningDirectiveTriviaSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoWarningDirectiveTriviaSyntaxCollection(IList<PocoWarningDirectiveTriviaSyntax> initList)
        {
            ((List<PocoWarningDirectiveTriviaSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoWarningDirectiveTriviaSyntax>();
    }

    public class PocoWarningDirectiveTriviaSyntax : PocoDirectiveTriviaSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken HashToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken WarningKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken EndOfDirectiveToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override bool IsActive
        {
            get;
            set;
        }
    }

    public class PocoWhenClauseSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoWhenClauseSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoWhenClauseSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoWhenClauseSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoWhenClauseSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoWhenClauseSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoWhenClauseSyntaxCollection(IList<PocoWhenClauseSyntax> initList)
        {
            ((List<PocoWhenClauseSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoWhenClauseSyntax>();
    }

    public class PocoWhenClauseSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken WhenKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Condition
        {
            get;
            set;
        }
    }

    public class PocoWhereClauseSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoWhereClauseSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoWhereClauseSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoWhereClauseSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoWhereClauseSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoWhereClauseSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoWhereClauseSyntaxCollection(IList<PocoWhereClauseSyntax> initList)
        {
            ((List<PocoWhereClauseSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoWhereClauseSyntax>();
    }

    public class PocoWhereClauseSyntax : PocoQueryClauseSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken WhereKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Condition
        {
            get;
            set;
        }
    }

    public class PocoWhileStatementSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoWhileStatementSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoWhileStatementSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoWhileStatementSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoWhileStatementSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoWhileStatementSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoWhileStatementSyntaxCollection(IList<PocoWhileStatementSyntax> initList)
        {
            ((List<PocoWhileStatementSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoWhileStatementSyntax>();
    }

    public class PocoWhileStatementSyntax : PocoStatementSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoAttributeListSyntaxCollection AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken WhileKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken OpenParenToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Condition
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken CloseParenToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoStatementSyntax Statement
        {
            get;
            set;
        }
    }

    public class PocoXmlAttributeSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoXmlAttributeSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoXmlAttributeSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoXmlAttributeSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoXmlAttributeSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoXmlAttributeSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoXmlAttributeSyntaxCollection(IList<PocoXmlAttributeSyntax> initList)
        {
            ((List<PocoXmlAttributeSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoXmlAttributeSyntax>();
    }

    public class PocoXmlAttributeSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoXmlNameSyntax Name
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken EqualsToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken StartQuoteToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken EndQuoteToken
        {
            get;
            set;
        }
    }

    public class PocoXmlCDataSectionSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoXmlCDataSectionSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoXmlCDataSectionSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoXmlCDataSectionSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoXmlCDataSectionSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoXmlCDataSectionSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoXmlCDataSectionSyntaxCollection(IList<PocoXmlCDataSectionSyntax> initList)
        {
            ((List<PocoXmlCDataSectionSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoXmlCDataSectionSyntax>();
    }

    public class PocoXmlCDataSectionSyntax : PocoXmlNodeSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken StartCDataToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxTokenList TextTokens
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken EndCDataToken
        {
            get;
            set;
        }
    }

    public class PocoXmlCommentSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoXmlCommentSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoXmlCommentSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoXmlCommentSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoXmlCommentSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoXmlCommentSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoXmlCommentSyntaxCollection(IList<PocoXmlCommentSyntax> initList)
        {
            ((List<PocoXmlCommentSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoXmlCommentSyntax>();
    }

    public class PocoXmlCommentSyntax : PocoXmlNodeSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken LessThanExclamationMinusMinusToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxTokenList TextTokens
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken MinusMinusGreaterThanToken
        {
            get;
            set;
        }
    }

    public class PocoXmlCrefAttributeSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoXmlCrefAttributeSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoXmlCrefAttributeSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoXmlCrefAttributeSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoXmlCrefAttributeSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoXmlCrefAttributeSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoXmlCrefAttributeSyntaxCollection(IList<PocoXmlCrefAttributeSyntax> initList)
        {
            ((List<PocoXmlCrefAttributeSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoXmlCrefAttributeSyntax>();
    }

    public class PocoXmlCrefAttributeSyntax : PocoXmlAttributeSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoXmlNameSyntax Name
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken EqualsToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken StartQuoteToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoCrefSyntax Cref
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken EndQuoteToken
        {
            get;
            set;
        }
    }

    public class PocoXmlElementEndTagSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoXmlElementEndTagSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoXmlElementEndTagSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoXmlElementEndTagSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoXmlElementEndTagSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoXmlElementEndTagSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoXmlElementEndTagSyntaxCollection(IList<PocoXmlElementEndTagSyntax> initList)
        {
            ((List<PocoXmlElementEndTagSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoXmlElementEndTagSyntax>();
    }

    public class PocoXmlElementEndTagSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken LessThanSlashToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoXmlNameSyntax Name
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken GreaterThanToken
        {
            get;
            set;
        }
    }

    public class PocoXmlElementStartTagSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoXmlElementStartTagSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoXmlElementStartTagSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoXmlElementStartTagSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoXmlElementStartTagSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoXmlElementStartTagSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoXmlElementStartTagSyntaxCollection(IList<PocoXmlElementStartTagSyntax> initList)
        {
            ((List<PocoXmlElementStartTagSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoXmlElementStartTagSyntax>();
    }

    public class PocoXmlElementStartTagSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken LessThanToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoXmlNameSyntax Name
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoXmlAttributeSyntaxCollection Attributes
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken GreaterThanToken
        {
            get;
            set;
        }
    }

    public class PocoXmlElementSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoXmlElementSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoXmlElementSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoXmlElementSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoXmlElementSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoXmlElementSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoXmlElementSyntaxCollection(IList<PocoXmlElementSyntax> initList)
        {
            ((List<PocoXmlElementSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoXmlElementSyntax>();
    }

    public class PocoXmlElementSyntax : PocoXmlNodeSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoXmlElementStartTagSyntax StartTag
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoXmlNodeSyntaxCollection Content
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoXmlElementEndTagSyntax EndTag
        {
            get;
            set;
        }
    }

    public class PocoXmlEmptyElementSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoXmlEmptyElementSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoXmlEmptyElementSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoXmlEmptyElementSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoXmlEmptyElementSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoXmlEmptyElementSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoXmlEmptyElementSyntaxCollection(IList<PocoXmlEmptyElementSyntax> initList)
        {
            ((List<PocoXmlEmptyElementSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoXmlEmptyElementSyntax>();
    }

    public class PocoXmlEmptyElementSyntax : PocoXmlNodeSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken LessThanToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoXmlNameSyntax Name
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoXmlAttributeSyntaxCollection Attributes
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken SlashGreaterThanToken
        {
            get;
            set;
        }
    }

    public class PocoXmlNameAttributeSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoXmlNameAttributeSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoXmlNameAttributeSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoXmlNameAttributeSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoXmlNameAttributeSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoXmlNameAttributeSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoXmlNameAttributeSyntaxCollection(IList<PocoXmlNameAttributeSyntax> initList)
        {
            ((List<PocoXmlNameAttributeSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoXmlNameAttributeSyntax>();
    }

    public class PocoXmlNameAttributeSyntax : PocoXmlAttributeSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoXmlNameSyntax Name
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken EqualsToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken StartQuoteToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoIdentifierNameSyntax Identifier
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken EndQuoteToken
        {
            get;
            set;
        }
    }

    public class PocoXmlNameSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoXmlNameSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoXmlNameSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoXmlNameSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoXmlNameSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoXmlNameSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoXmlNameSyntaxCollection(IList<PocoXmlNameSyntax> initList)
        {
            ((List<PocoXmlNameSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoXmlNameSyntax>();
    }

    public class PocoXmlNameSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoXmlPrefixSyntax Prefix
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken LocalName
        {
            get;
            set;
        }
    }

    public class PocoXmlNodeSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoXmlNodeSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoXmlNodeSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoXmlNodeSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoXmlNodeSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoXmlNodeSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoXmlNodeSyntaxCollection(IList<PocoXmlNodeSyntax> initList)
        {
            ((List<PocoXmlNodeSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoXmlNodeSyntax>();
    }

    public class PocoXmlNodeSyntax : PocoCSharpSyntaxNode
    {
    }

    public class PocoXmlPrefixSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoXmlPrefixSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoXmlPrefixSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoXmlPrefixSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoXmlPrefixSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoXmlPrefixSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoXmlPrefixSyntaxCollection(IList<PocoXmlPrefixSyntax> initList)
        {
            ((List<PocoXmlPrefixSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoXmlPrefixSyntax>();
    }

    public class PocoXmlPrefixSyntax : PocoCSharpSyntaxNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken Prefix
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken ColonToken
        {
            get;
            set;
        }
    }

    public class PocoXmlProcessingInstructionSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoXmlProcessingInstructionSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoXmlProcessingInstructionSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoXmlProcessingInstructionSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoXmlProcessingInstructionSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoXmlProcessingInstructionSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoXmlProcessingInstructionSyntaxCollection(IList<PocoXmlProcessingInstructionSyntax> initList)
        {
            ((List<PocoXmlProcessingInstructionSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoXmlProcessingInstructionSyntax>();
    }

    public class PocoXmlProcessingInstructionSyntax : PocoXmlNodeSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken StartProcessingInstructionToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoXmlNameSyntax Name
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxTokenList TextTokens
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken EndProcessingInstructionToken
        {
            get;
            set;
        }
    }

    public class PocoXmlTextAttributeSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoXmlTextAttributeSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoXmlTextAttributeSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoXmlTextAttributeSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoXmlTextAttributeSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoXmlTextAttributeSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoXmlTextAttributeSyntaxCollection(IList<PocoXmlTextAttributeSyntax> initList)
        {
            ((List<PocoXmlTextAttributeSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoXmlTextAttributeSyntax>();
    }

    public class PocoXmlTextAttributeSyntax : PocoXmlAttributeSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoXmlNameSyntax Name
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken EqualsToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken StartQuoteToken
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxTokenList TextTokens
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoSyntaxToken EndQuoteToken
        {
            get;
            set;
        }
    }

    public class PocoXmlTextSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoXmlTextSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoXmlTextSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoXmlTextSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoXmlTextSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoXmlTextSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoXmlTextSyntaxCollection(IList<PocoXmlTextSyntax> initList)
        {
            ((List<PocoXmlTextSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoXmlTextSyntax>();
    }

    public class PocoXmlTextSyntax : PocoXmlNodeSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxTokenList TextTokens
        {
            get;
            set;
        }
    }

    public class PocoYieldStatementSyntaxCollection : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoYieldStatementSyntax)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoYieldStatementSyntax)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoYieldStatementSyntax)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoYieldStatementSyntax)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoYieldStatementSyntax)value);
        // System.Collections.IList
        public void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32 Count => _list.Count;
        public Object SyncRoot => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        public PocoYieldStatementSyntaxCollection(IList<PocoYieldStatementSyntax> initList)
        {
            ((List<PocoYieldStatementSyntax>)_list).AddRange(initList);
        }

        IList _list = new List<PocoYieldStatementSyntax>();
    }

    public class PocoYieldStatementSyntax : PocoStatementSyntax
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public override PocoAttributeListSyntaxCollection AttributeLists
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken YieldKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken ReturnOrBreakKeyword
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoExpressionSyntax Expression
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public virtual PocoSyntaxToken SemicolonToken
        {
            get;
            set;
        }
    }
}
#endif