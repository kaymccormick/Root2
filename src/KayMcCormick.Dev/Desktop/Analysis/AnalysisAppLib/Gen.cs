using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PocoSyntax
{
    public class PocoSyntaxToken
    {
        public int RawKind { get; set; }

        public string Kind { get; set; }

        public object Value { get; set; }

        public string ValueText { get; set; }
    }
public class PocoSyntaxTokenCollection
        : IList, IEnumerable, ICollection
    {
        public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoSyntaxToken)value);
        public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoSyntaxToken)value);
        public // System.Collections.IList
            void Clear() => _list.Clear();
        public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoSyntaxToken)value);
        public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoSyntaxToken)value);
        public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoSyntaxToken)value);
        public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly  => _list.IsReadOnly;
        public Boolean IsFixedSize => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32   Count          => _list.Count;
        public Object  SyncRoot       => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
        IList _list = new List<PocoSyntaxToken>();
    }

        class PocoCSharpSyntaxNodeCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoCSharpSyntaxNode)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoCSharpSyntaxNode)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoCSharpSyntaxNode)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoCSharpSyntaxNode)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoCSharpSyntaxNode)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoCSharpSyntaxNode>();
        }

        public class PocoCSharpSyntaxNode
        {
        }

        class PocoAccessorDeclarationSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoAccessorDeclarationSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoAccessorDeclarationSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoAccessorDeclarationSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoAccessorDeclarationSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoAccessorDeclarationSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoAccessorDeclarationSyntax>();
        }

        public class PocoAccessorDeclarationSyntax : PocoCSharpSyntaxNode
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax AttributeLists
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual Microsoft.CodeAnalysis.SyntaxToken Modifiers
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken Keyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual BlockSyntax Body
            {
                get;
                set;
            }
        }

        class PocoAccessorListSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoAccessorListSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoAccessorListSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoAccessorListSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoAccessorListSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoAccessorListSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoAccessorListSyntax>();
        }

        public class PocoAccessorListSyntax : PocoCSharpSyntaxNode
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken OpenBraceToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual Microsoft.CodeAnalysis.CSharp.Syntax.AccessorDeclarationSyntax Accessors
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken CloseBraceToken
            {
                get;
                set;
            }
        }

        class PocoAliasQualifiedNameSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoAliasQualifiedNameSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoAliasQualifiedNameSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoAliasQualifiedNameSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoAliasQualifiedNameSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoAliasQualifiedNameSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoAliasQualifiedNameSyntax>();
        }

        public class PocoAliasQualifiedNameSyntax : PocoNameSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual IdentifierNameSyntax Alias
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken ColonColonToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SimpleNameSyntax Name
            {
                get;
                set;
            }
        }

        class PocoAnonymousFunctionExpressionSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoAnonymousFunctionExpressionSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoAnonymousFunctionExpressionSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoAnonymousFunctionExpressionSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoAnonymousFunctionExpressionSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoAnonymousFunctionExpressionSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoAnonymousFunctionExpressionSyntax>();
        }

        public class PocoAnonymousFunctionExpressionSyntax : PocoExpressionSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken AsyncKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual BlockSyntax Block
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual ExpressionSyntax ExpressionBody
            {
                get;
                set;
            }
        }

        class PocoAnonymousMethodExpressionSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoAnonymousMethodExpressionSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoAnonymousMethodExpressionSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoAnonymousMethodExpressionSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoAnonymousMethodExpressionSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoAnonymousMethodExpressionSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoAnonymousMethodExpressionSyntax>();
        }

        public class PocoAnonymousMethodExpressionSyntax : PocoAnonymousFunctionExpressionSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override SyntaxToken AsyncKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken DelegateKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual ParameterListSyntax ParameterList
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override BlockSyntax Block
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override ExpressionSyntax ExpressionBody
            {
                get;
                set;
            }
        }

        class PocoAnonymousObjectCreationExpressionSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoAnonymousObjectCreationExpressionSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoAnonymousObjectCreationExpressionSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoAnonymousObjectCreationExpressionSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoAnonymousObjectCreationExpressionSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoAnonymousObjectCreationExpressionSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoAnonymousObjectCreationExpressionSyntax>();
        }

        public class PocoAnonymousObjectCreationExpressionSyntax : PocoExpressionSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken NewKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken OpenBraceToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual Microsoft.CodeAnalysis.CSharp.Syntax.AnonymousObjectMemberDeclaratorSyntax Initializers
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken CloseBraceToken
            {
                get;
                set;
            }
        }

        class PocoAnonymousObjectMemberDeclaratorSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoAnonymousObjectMemberDeclaratorSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoAnonymousObjectMemberDeclaratorSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoAnonymousObjectMemberDeclaratorSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoAnonymousObjectMemberDeclaratorSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoAnonymousObjectMemberDeclaratorSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoAnonymousObjectMemberDeclaratorSyntax>();
        }

        public class PocoAnonymousObjectMemberDeclaratorSyntax : PocoCSharpSyntaxNode
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual NameEqualsSyntax NameEquals
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual ExpressionSyntax Expression
            {
                get;
                set;
            }
        }

        class PocoArgumentListSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoArgumentListSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoArgumentListSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoArgumentListSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoArgumentListSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoArgumentListSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoArgumentListSyntax>();
        }

        public class PocoArgumentListSyntax : PocoBaseArgumentListSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken OpenParenToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override Microsoft.CodeAnalysis.CSharp.Syntax.ArgumentSyntax Arguments
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken CloseParenToken
            {
                get;
                set;
            }
        }

        class PocoArgumentSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoArgumentSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoArgumentSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoArgumentSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoArgumentSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoArgumentSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoArgumentSyntax>();
        }

        public class PocoArgumentSyntax : PocoCSharpSyntaxNode
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual NameColonSyntax NameColon
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken RefKindKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual ExpressionSyntax Expression
            {
                get;
                set;
            }
        }

        class PocoArrayCreationExpressionSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoArrayCreationExpressionSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoArrayCreationExpressionSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoArrayCreationExpressionSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoArrayCreationExpressionSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoArrayCreationExpressionSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoArrayCreationExpressionSyntax>();
        }

        public class PocoArrayCreationExpressionSyntax : PocoExpressionSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken NewKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual ArrayTypeSyntax Type
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual InitializerExpressionSyntax Initializer
            {
                get;
                set;
            }
        }

        class PocoArrayRankSpecifierSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoArrayRankSpecifierSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoArrayRankSpecifierSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoArrayRankSpecifierSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoArrayRankSpecifierSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoArrayRankSpecifierSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoArrayRankSpecifierSyntax>();
        }

        public class PocoArrayRankSpecifierSyntax : PocoCSharpSyntaxNode
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken OpenBracketToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionSyntax Sizes
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken CloseBracketToken
            {
                get;
                set;
            }
        }

        class PocoArrayTypeSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoArrayTypeSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoArrayTypeSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoArrayTypeSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoArrayTypeSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoArrayTypeSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoArrayTypeSyntax>();
        }

        public class PocoArrayTypeSyntax : PocoTypeSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual TypeSyntax ElementType
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual Microsoft.CodeAnalysis.CSharp.Syntax.ArrayRankSpecifierSyntax RankSpecifiers
            {
                get;
                set;
            }
        }

        class PocoArrowExpressionClauseSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoArrowExpressionClauseSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoArrowExpressionClauseSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoArrowExpressionClauseSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoArrowExpressionClauseSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoArrowExpressionClauseSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoArrowExpressionClauseSyntax>();
        }

        public class PocoArrowExpressionClauseSyntax : PocoCSharpSyntaxNode
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken ArrowToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual ExpressionSyntax Expression
            {
                get;
                set;
            }
        }

        class PocoAssignmentExpressionSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoAssignmentExpressionSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoAssignmentExpressionSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoAssignmentExpressionSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoAssignmentExpressionSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoAssignmentExpressionSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoAssignmentExpressionSyntax>();
        }

        public class PocoAssignmentExpressionSyntax : PocoExpressionSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual ExpressionSyntax Left
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken OperatorToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual ExpressionSyntax Right
            {
                get;
                set;
            }
        }

        class PocoAttributeArgumentListSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoAttributeArgumentListSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoAttributeArgumentListSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoAttributeArgumentListSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoAttributeArgumentListSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoAttributeArgumentListSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoAttributeArgumentListSyntax>();
        }

        public class PocoAttributeArgumentListSyntax : PocoCSharpSyntaxNode
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken OpenParenToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual Microsoft.CodeAnalysis.CSharp.Syntax.AttributeArgumentSyntax Arguments
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken CloseParenToken
            {
                get;
                set;
            }
        }

        class PocoAttributeArgumentSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoAttributeArgumentSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoAttributeArgumentSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoAttributeArgumentSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoAttributeArgumentSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoAttributeArgumentSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoAttributeArgumentSyntax>();
        }

        public class PocoAttributeArgumentSyntax : PocoCSharpSyntaxNode
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual ExpressionSyntax Expression
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual NameEqualsSyntax NameEquals
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual NameColonSyntax NameColon
            {
                get;
                set;
            }
        }

        class PocoAttributeListSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoAttributeListSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoAttributeListSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoAttributeListSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoAttributeListSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoAttributeListSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoAttributeListSyntax>();
        }

        public class PocoAttributeListSyntax : PocoCSharpSyntaxNode
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken OpenBracketToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual AttributeTargetSpecifierSyntax Target
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual Microsoft.CodeAnalysis.CSharp.Syntax.AttributeSyntax Attributes
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken CloseBracketToken
            {
                get;
                set;
            }
        }

        class PocoAttributeSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoAttributeSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoAttributeSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoAttributeSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoAttributeSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoAttributeSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoAttributeSyntax>();
        }

        public class PocoAttributeSyntax : PocoCSharpSyntaxNode
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual NameSyntax Name
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual AttributeArgumentListSyntax ArgumentList
            {
                get;
                set;
            }
        }

        class PocoAttributeTargetSpecifierSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoAttributeTargetSpecifierSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoAttributeTargetSpecifierSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoAttributeTargetSpecifierSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoAttributeTargetSpecifierSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoAttributeTargetSpecifierSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoAttributeTargetSpecifierSyntax>();
        }

        public class PocoAttributeTargetSpecifierSyntax : PocoCSharpSyntaxNode
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken Identifier
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken ColonToken
            {
                get;
                set;
            }
        }

        class PocoAwaitExpressionSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoAwaitExpressionSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoAwaitExpressionSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoAwaitExpressionSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoAwaitExpressionSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoAwaitExpressionSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoAwaitExpressionSyntax>();
        }

        public class PocoAwaitExpressionSyntax : PocoExpressionSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken AwaitKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual ExpressionSyntax Expression
            {
                get;
                set;
            }
        }

        class PocoBadDirectiveTriviaSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoBadDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoBadDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoBadDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoBadDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoBadDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoBadDirectiveTriviaSyntax>();
        }

        public class PocoBadDirectiveTriviaSyntax : PocoDirectiveTriviaSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override SyntaxToken HashToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken Identifier
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override SyntaxToken EndOfDirectiveToken
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

        class PocoBaseArgumentListSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoBaseArgumentListSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoBaseArgumentListSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoBaseArgumentListSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoBaseArgumentListSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoBaseArgumentListSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoBaseArgumentListSyntax>();
        }

        public class PocoBaseArgumentListSyntax : PocoCSharpSyntaxNode
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual Microsoft.CodeAnalysis.CSharp.Syntax.ArgumentSyntax Arguments
            {
                get;
                set;
            }
        }

        class PocoBaseCrefParameterListSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoBaseCrefParameterListSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoBaseCrefParameterListSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoBaseCrefParameterListSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoBaseCrefParameterListSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoBaseCrefParameterListSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoBaseCrefParameterListSyntax>();
        }

        public class PocoBaseCrefParameterListSyntax : PocoCSharpSyntaxNode
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual Microsoft.CodeAnalysis.CSharp.Syntax.CrefParameterSyntax Parameters
            {
                get;
                set;
            }
        }

        class PocoBaseExpressionSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoBaseExpressionSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoBaseExpressionSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoBaseExpressionSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoBaseExpressionSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoBaseExpressionSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoBaseExpressionSyntax>();
        }

        public class PocoBaseExpressionSyntax : PocoInstanceExpressionSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken Token
            {
                get;
                set;
            }
        }

        class PocoBaseFieldDeclarationSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoBaseFieldDeclarationSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoBaseFieldDeclarationSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoBaseFieldDeclarationSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoBaseFieldDeclarationSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoBaseFieldDeclarationSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoBaseFieldDeclarationSyntax>();
        }

        public class PocoBaseFieldDeclarationSyntax : PocoMemberDeclarationSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual VariableDeclarationSyntax Declaration
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken SemicolonToken
            {
                get;
                set;
            }
        }

        class PocoBaseListSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoBaseListSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoBaseListSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoBaseListSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoBaseListSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoBaseListSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoBaseListSyntax>();
        }

        public class PocoBaseListSyntax : PocoCSharpSyntaxNode
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken ColonToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual Microsoft.CodeAnalysis.CSharp.Syntax.BaseTypeSyntax Types
            {
                get;
                set;
            }
        }

        class PocoBaseMethodDeclarationSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoBaseMethodDeclarationSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoBaseMethodDeclarationSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoBaseMethodDeclarationSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoBaseMethodDeclarationSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoBaseMethodDeclarationSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoBaseMethodDeclarationSyntax>();
        }

        public class PocoBaseMethodDeclarationSyntax : PocoMemberDeclarationSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual ParameterListSyntax ParameterList
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual BlockSyntax Body
            {
                get;
                set;
            }
        }

        class PocoBaseParameterListSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoBaseParameterListSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoBaseParameterListSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoBaseParameterListSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoBaseParameterListSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoBaseParameterListSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoBaseParameterListSyntax>();
        }

        public class PocoBaseParameterListSyntax : PocoCSharpSyntaxNode
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual Microsoft.CodeAnalysis.CSharp.Syntax.ParameterSyntax Parameters
            {
                get;
                set;
            }
        }

        class PocoBasePropertyDeclarationSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoBasePropertyDeclarationSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoBasePropertyDeclarationSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoBasePropertyDeclarationSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoBasePropertyDeclarationSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoBasePropertyDeclarationSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoBasePropertyDeclarationSyntax>();
        }

        public class PocoBasePropertyDeclarationSyntax : PocoMemberDeclarationSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual TypeSyntax Type
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual ExplicitInterfaceSpecifierSyntax ExplicitInterfaceSpecifier
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual AccessorListSyntax AccessorList
            {
                get;
                set;
            }
        }

        class PocoBaseTypeDeclarationSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoBaseTypeDeclarationSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoBaseTypeDeclarationSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoBaseTypeDeclarationSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoBaseTypeDeclarationSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoBaseTypeDeclarationSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoBaseTypeDeclarationSyntax>();
        }

        public class PocoBaseTypeDeclarationSyntax : PocoMemberDeclarationSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken Identifier
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual BaseListSyntax BaseList
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken OpenBraceToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken CloseBraceToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken SemicolonToken
            {
                get;
                set;
            }
        }

        class PocoBaseTypeSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoBaseTypeSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoBaseTypeSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoBaseTypeSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoBaseTypeSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoBaseTypeSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoBaseTypeSyntax>();
        }

        public class PocoBaseTypeSyntax : PocoCSharpSyntaxNode
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual TypeSyntax Type
            {
                get;
                set;
            }
        }

        class PocoBinaryExpressionSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoBinaryExpressionSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoBinaryExpressionSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoBinaryExpressionSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoBinaryExpressionSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoBinaryExpressionSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoBinaryExpressionSyntax>();
        }

        public class PocoBinaryExpressionSyntax : PocoExpressionSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual ExpressionSyntax Left
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken OperatorToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual ExpressionSyntax Right
            {
                get;
                set;
            }
        }

        class PocoBlockSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoBlockSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoBlockSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoBlockSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoBlockSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoBlockSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoBlockSyntax>();
        }

        public class PocoBlockSyntax : PocoStatementSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax AttributeLists
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken OpenBraceToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual Microsoft.CodeAnalysis.CSharp.Syntax.StatementSyntax Statements
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken CloseBraceToken
            {
                get;
                set;
            }
        }

        class PocoBracketedArgumentListSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoBracketedArgumentListSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoBracketedArgumentListSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoBracketedArgumentListSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoBracketedArgumentListSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoBracketedArgumentListSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoBracketedArgumentListSyntax>();
        }

        public class PocoBracketedArgumentListSyntax : PocoBaseArgumentListSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken OpenBracketToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override Microsoft.CodeAnalysis.CSharp.Syntax.ArgumentSyntax Arguments
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken CloseBracketToken
            {
                get;
                set;
            }
        }

        class PocoBracketedParameterListSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoBracketedParameterListSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoBracketedParameterListSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoBracketedParameterListSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoBracketedParameterListSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoBracketedParameterListSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoBracketedParameterListSyntax>();
        }

        public class PocoBracketedParameterListSyntax : PocoBaseParameterListSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken OpenBracketToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override Microsoft.CodeAnalysis.CSharp.Syntax.ParameterSyntax Parameters
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken CloseBracketToken
            {
                get;
                set;
            }
        }

        class PocoBranchingDirectiveTriviaSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoBranchingDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoBranchingDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoBranchingDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoBranchingDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoBranchingDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
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

        class PocoBreakStatementSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoBreakStatementSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoBreakStatementSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoBreakStatementSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoBreakStatementSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoBreakStatementSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoBreakStatementSyntax>();
        }

        public class PocoBreakStatementSyntax : PocoStatementSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax AttributeLists
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken BreakKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken SemicolonToken
            {
                get;
                set;
            }
        }

        class PocoCasePatternSwitchLabelSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoCasePatternSwitchLabelSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoCasePatternSwitchLabelSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoCasePatternSwitchLabelSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoCasePatternSwitchLabelSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoCasePatternSwitchLabelSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoCasePatternSwitchLabelSyntax>();
        }

        public class PocoCasePatternSwitchLabelSyntax : PocoSwitchLabelSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override SyntaxToken Keyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual PatternSyntax Pattern
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual WhenClauseSyntax WhenClause
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override SyntaxToken ColonToken
            {
                get;
                set;
            }
        }

        class PocoCaseSwitchLabelSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoCaseSwitchLabelSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoCaseSwitchLabelSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoCaseSwitchLabelSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoCaseSwitchLabelSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoCaseSwitchLabelSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoCaseSwitchLabelSyntax>();
        }

        public class PocoCaseSwitchLabelSyntax : PocoSwitchLabelSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override SyntaxToken Keyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual ExpressionSyntax Value
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override SyntaxToken ColonToken
            {
                get;
                set;
            }
        }

        class PocoCastExpressionSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoCastExpressionSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoCastExpressionSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoCastExpressionSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoCastExpressionSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoCastExpressionSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoCastExpressionSyntax>();
        }

        public class PocoCastExpressionSyntax : PocoExpressionSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken OpenParenToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual TypeSyntax Type
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken CloseParenToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual ExpressionSyntax Expression
            {
                get;
                set;
            }
        }

        class PocoCatchClauseSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoCatchClauseSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoCatchClauseSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoCatchClauseSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoCatchClauseSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoCatchClauseSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoCatchClauseSyntax>();
        }

        public class PocoCatchClauseSyntax : PocoCSharpSyntaxNode
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken CatchKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual CatchDeclarationSyntax Declaration
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual CatchFilterClauseSyntax Filter
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual BlockSyntax Block
            {
                get;
                set;
            }
        }

        class PocoCatchDeclarationSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoCatchDeclarationSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoCatchDeclarationSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoCatchDeclarationSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoCatchDeclarationSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoCatchDeclarationSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoCatchDeclarationSyntax>();
        }

        public class PocoCatchDeclarationSyntax : PocoCSharpSyntaxNode
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken OpenParenToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual TypeSyntax Type
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken Identifier
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken CloseParenToken
            {
                get;
                set;
            }
        }

        class PocoCatchFilterClauseSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoCatchFilterClauseSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoCatchFilterClauseSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoCatchFilterClauseSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoCatchFilterClauseSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoCatchFilterClauseSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoCatchFilterClauseSyntax>();
        }

        public class PocoCatchFilterClauseSyntax : PocoCSharpSyntaxNode
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken WhenKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken OpenParenToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual ExpressionSyntax FilterExpression
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken CloseParenToken
            {
                get;
                set;
            }
        }

        class PocoCheckedExpressionSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoCheckedExpressionSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoCheckedExpressionSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoCheckedExpressionSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoCheckedExpressionSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoCheckedExpressionSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoCheckedExpressionSyntax>();
        }

        public class PocoCheckedExpressionSyntax : PocoExpressionSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken Keyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken OpenParenToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual ExpressionSyntax Expression
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken CloseParenToken
            {
                get;
                set;
            }
        }

        class PocoCheckedStatementSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoCheckedStatementSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoCheckedStatementSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoCheckedStatementSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoCheckedStatementSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoCheckedStatementSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoCheckedStatementSyntax>();
        }

        public class PocoCheckedStatementSyntax : PocoStatementSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax AttributeLists
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken Keyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual BlockSyntax Block
            {
                get;
                set;
            }
        }

        class PocoClassDeclarationSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoClassDeclarationSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoClassDeclarationSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoClassDeclarationSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoClassDeclarationSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoClassDeclarationSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoClassDeclarationSyntax>();
        }

        public class PocoClassDeclarationSyntax : PocoTypeDeclarationSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax AttributeLists
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override Microsoft.CodeAnalysis.SyntaxToken Modifiers
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override SyntaxToken Keyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override SyntaxToken Identifier
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override TypeParameterListSyntax TypeParameterList
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override BaseListSyntax BaseList
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterConstraintClauseSyntax ConstraintClauses
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override SyntaxToken OpenBraceToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax Members
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override SyntaxToken CloseBraceToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override SyntaxToken SemicolonToken
            {
                get;
                set;
            }
        }

        class PocoClassOrStructConstraintSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoClassOrStructConstraintSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoClassOrStructConstraintSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoClassOrStructConstraintSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoClassOrStructConstraintSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoClassOrStructConstraintSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoClassOrStructConstraintSyntax>();
        }

        public class PocoClassOrStructConstraintSyntax : PocoTypeParameterConstraintSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken ClassOrStructKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken QuestionToken
            {
                get;
                set;
            }
        }

        class PocoCommonForEachStatementSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoCommonForEachStatementSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoCommonForEachStatementSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoCommonForEachStatementSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoCommonForEachStatementSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoCommonForEachStatementSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoCommonForEachStatementSyntax>();
        }

        public class PocoCommonForEachStatementSyntax : PocoStatementSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken AwaitKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken ForEachKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken OpenParenToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken InKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual ExpressionSyntax Expression
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken CloseParenToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual StatementSyntax Statement
            {
                get;
                set;
            }
        }

        class PocoCompilationUnitSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoCompilationUnitSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoCompilationUnitSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoCompilationUnitSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoCompilationUnitSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoCompilationUnitSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoCompilationUnitSyntax>();
        }

        public class PocoCompilationUnitSyntax : PocoCSharpSyntaxNode
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual Microsoft.CodeAnalysis.CSharp.Syntax.ExternAliasDirectiveSyntax Externs
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual Microsoft.CodeAnalysis.CSharp.Syntax.UsingDirectiveSyntax Usings
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax AttributeLists
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax Members
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken EndOfFileToken
            {
                get;
                set;
            }
        }

        class PocoConditionalAccessExpressionSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoConditionalAccessExpressionSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoConditionalAccessExpressionSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoConditionalAccessExpressionSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoConditionalAccessExpressionSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoConditionalAccessExpressionSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoConditionalAccessExpressionSyntax>();
        }

        public class PocoConditionalAccessExpressionSyntax : PocoExpressionSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual ExpressionSyntax Expression
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken OperatorToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual ExpressionSyntax WhenNotNull
            {
                get;
                set;
            }
        }

        class PocoConditionalDirectiveTriviaSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoConditionalDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoConditionalDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoConditionalDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoConditionalDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoConditionalDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoConditionalDirectiveTriviaSyntax>();
        }

        public class PocoConditionalDirectiveTriviaSyntax : PocoBranchingDirectiveTriviaSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual ExpressionSyntax Condition
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

        class PocoConditionalExpressionSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoConditionalExpressionSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoConditionalExpressionSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoConditionalExpressionSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoConditionalExpressionSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoConditionalExpressionSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoConditionalExpressionSyntax>();
        }

        public class PocoConditionalExpressionSyntax : PocoExpressionSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual ExpressionSyntax Condition
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken QuestionToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual ExpressionSyntax WhenTrue
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken ColonToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual ExpressionSyntax WhenFalse
            {
                get;
                set;
            }
        }

        class PocoConstantPatternSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoConstantPatternSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoConstantPatternSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoConstantPatternSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoConstantPatternSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoConstantPatternSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoConstantPatternSyntax>();
        }

        public class PocoConstantPatternSyntax : PocoPatternSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual ExpressionSyntax Expression
            {
                get;
                set;
            }
        }

        class PocoConstructorConstraintSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoConstructorConstraintSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoConstructorConstraintSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoConstructorConstraintSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoConstructorConstraintSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoConstructorConstraintSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoConstructorConstraintSyntax>();
        }

        public class PocoConstructorConstraintSyntax : PocoTypeParameterConstraintSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken NewKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken OpenParenToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken CloseParenToken
            {
                get;
                set;
            }
        }

        class PocoConstructorDeclarationSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoConstructorDeclarationSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoConstructorDeclarationSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoConstructorDeclarationSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoConstructorDeclarationSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoConstructorDeclarationSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoConstructorDeclarationSyntax>();
        }

        public class PocoConstructorDeclarationSyntax : PocoBaseMethodDeclarationSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax AttributeLists
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override Microsoft.CodeAnalysis.SyntaxToken Modifiers
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken Identifier
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override ParameterListSyntax ParameterList
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual ConstructorInitializerSyntax Initializer
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override BlockSyntax Body
            {
                get;
                set;
            }
        }

        class PocoConstructorInitializerSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoConstructorInitializerSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoConstructorInitializerSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoConstructorInitializerSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoConstructorInitializerSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoConstructorInitializerSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoConstructorInitializerSyntax>();
        }

        public class PocoConstructorInitializerSyntax : PocoCSharpSyntaxNode
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken ColonToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken ThisOrBaseKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual ArgumentListSyntax ArgumentList
            {
                get;
                set;
            }
        }

        class PocoContinueStatementSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoContinueStatementSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoContinueStatementSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoContinueStatementSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoContinueStatementSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoContinueStatementSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoContinueStatementSyntax>();
        }

        public class PocoContinueStatementSyntax : PocoStatementSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax AttributeLists
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken ContinueKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken SemicolonToken
            {
                get;
                set;
            }
        }

        class PocoConversionOperatorDeclarationSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoConversionOperatorDeclarationSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoConversionOperatorDeclarationSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoConversionOperatorDeclarationSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoConversionOperatorDeclarationSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoConversionOperatorDeclarationSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoConversionOperatorDeclarationSyntax>();
        }

        public class PocoConversionOperatorDeclarationSyntax : PocoBaseMethodDeclarationSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax AttributeLists
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override Microsoft.CodeAnalysis.SyntaxToken Modifiers
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken ImplicitOrExplicitKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken OperatorKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual TypeSyntax Type
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override ParameterListSyntax ParameterList
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override BlockSyntax Body
            {
                get;
                set;
            }
        }

        class PocoConversionOperatorMemberCrefSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoConversionOperatorMemberCrefSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoConversionOperatorMemberCrefSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoConversionOperatorMemberCrefSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoConversionOperatorMemberCrefSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoConversionOperatorMemberCrefSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoConversionOperatorMemberCrefSyntax>();
        }

        public class PocoConversionOperatorMemberCrefSyntax : PocoMemberCrefSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken ImplicitOrExplicitKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken OperatorKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual TypeSyntax Type
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual CrefParameterListSyntax Parameters
            {
                get;
                set;
            }
        }

        class PocoCrefBracketedParameterListSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoCrefBracketedParameterListSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoCrefBracketedParameterListSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoCrefBracketedParameterListSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoCrefBracketedParameterListSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoCrefBracketedParameterListSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoCrefBracketedParameterListSyntax>();
        }

        public class PocoCrefBracketedParameterListSyntax : PocoBaseCrefParameterListSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken OpenBracketToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override Microsoft.CodeAnalysis.CSharp.Syntax.CrefParameterSyntax Parameters
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken CloseBracketToken
            {
                get;
                set;
            }
        }

        class PocoCrefParameterListSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoCrefParameterListSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoCrefParameterListSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoCrefParameterListSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoCrefParameterListSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoCrefParameterListSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoCrefParameterListSyntax>();
        }

        public class PocoCrefParameterListSyntax : PocoBaseCrefParameterListSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken OpenParenToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override Microsoft.CodeAnalysis.CSharp.Syntax.CrefParameterSyntax Parameters
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken CloseParenToken
            {
                get;
                set;
            }
        }

        class PocoCrefParameterSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoCrefParameterSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoCrefParameterSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoCrefParameterSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoCrefParameterSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoCrefParameterSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoCrefParameterSyntax>();
        }

        public class PocoCrefParameterSyntax : PocoCSharpSyntaxNode
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken RefKindKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual TypeSyntax Type
            {
                get;
                set;
            }
        }

        class PocoCrefSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoCrefSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoCrefSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoCrefSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoCrefSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoCrefSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoCrefSyntax>();
        }

        public class PocoCrefSyntax : PocoCSharpSyntaxNode
        {
        }

        class PocoDeclarationExpressionSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoDeclarationExpressionSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoDeclarationExpressionSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoDeclarationExpressionSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoDeclarationExpressionSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoDeclarationExpressionSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoDeclarationExpressionSyntax>();
        }

        public class PocoDeclarationExpressionSyntax : PocoExpressionSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual TypeSyntax Type
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual VariableDesignationSyntax Designation
            {
                get;
                set;
            }
        }

        class PocoDeclarationPatternSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoDeclarationPatternSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoDeclarationPatternSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoDeclarationPatternSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoDeclarationPatternSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoDeclarationPatternSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoDeclarationPatternSyntax>();
        }

        public class PocoDeclarationPatternSyntax : PocoPatternSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual TypeSyntax Type
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual VariableDesignationSyntax Designation
            {
                get;
                set;
            }
        }

        class PocoDefaultExpressionSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoDefaultExpressionSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoDefaultExpressionSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoDefaultExpressionSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoDefaultExpressionSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoDefaultExpressionSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoDefaultExpressionSyntax>();
        }

        public class PocoDefaultExpressionSyntax : PocoExpressionSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken Keyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken OpenParenToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual TypeSyntax Type
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken CloseParenToken
            {
                get;
                set;
            }
        }

        class PocoDefaultSwitchLabelSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoDefaultSwitchLabelSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoDefaultSwitchLabelSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoDefaultSwitchLabelSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoDefaultSwitchLabelSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoDefaultSwitchLabelSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoDefaultSwitchLabelSyntax>();
        }

        public class PocoDefaultSwitchLabelSyntax : PocoSwitchLabelSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override SyntaxToken Keyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override SyntaxToken ColonToken
            {
                get;
                set;
            }
        }

        class PocoDefineDirectiveTriviaSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoDefineDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoDefineDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoDefineDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoDefineDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoDefineDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoDefineDirectiveTriviaSyntax>();
        }

        public class PocoDefineDirectiveTriviaSyntax : PocoDirectiveTriviaSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override SyntaxToken HashToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken DefineKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken Name
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override SyntaxToken EndOfDirectiveToken
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

        class PocoDelegateDeclarationSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoDelegateDeclarationSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoDelegateDeclarationSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoDelegateDeclarationSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoDelegateDeclarationSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoDelegateDeclarationSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoDelegateDeclarationSyntax>();
        }

        public class PocoDelegateDeclarationSyntax : PocoMemberDeclarationSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax AttributeLists
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override Microsoft.CodeAnalysis.SyntaxToken Modifiers
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken DelegateKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual TypeSyntax ReturnType
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken Identifier
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual TypeParameterListSyntax TypeParameterList
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual ParameterListSyntax ParameterList
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterConstraintClauseSyntax ConstraintClauses
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken SemicolonToken
            {
                get;
                set;
            }
        }

        class PocoDestructorDeclarationSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoDestructorDeclarationSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoDestructorDeclarationSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoDestructorDeclarationSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoDestructorDeclarationSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoDestructorDeclarationSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoDestructorDeclarationSyntax>();
        }

        public class PocoDestructorDeclarationSyntax : PocoBaseMethodDeclarationSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax AttributeLists
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override Microsoft.CodeAnalysis.SyntaxToken Modifiers
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken TildeToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken Identifier
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override ParameterListSyntax ParameterList
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override BlockSyntax Body
            {
                get;
                set;
            }
        }

        class PocoDirectiveTriviaSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
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
            public virtual SyntaxToken HashToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken EndOfDirectiveToken
            {
                get;
                set;
            }
        }

        class PocoDiscardDesignationSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoDiscardDesignationSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoDiscardDesignationSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoDiscardDesignationSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoDiscardDesignationSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoDiscardDesignationSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoDiscardDesignationSyntax>();
        }

        public class PocoDiscardDesignationSyntax : PocoVariableDesignationSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken UnderscoreToken
            {
                get;
                set;
            }
        }

        class PocoDiscardPatternSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoDiscardPatternSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoDiscardPatternSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoDiscardPatternSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoDiscardPatternSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoDiscardPatternSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoDiscardPatternSyntax>();
        }

        public class PocoDiscardPatternSyntax : PocoPatternSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken UnderscoreToken
            {
                get;
                set;
            }
        }

        class PocoDocumentationCommentTriviaSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoDocumentationCommentTriviaSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoDocumentationCommentTriviaSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoDocumentationCommentTriviaSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoDocumentationCommentTriviaSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoDocumentationCommentTriviaSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoDocumentationCommentTriviaSyntax>();
        }

        public class PocoDocumentationCommentTriviaSyntax : PocoStructuredTriviaSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual Microsoft.CodeAnalysis.CSharp.Syntax.XmlNodeSyntax Content
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken EndOfComment
            {
                get;
                set;
            }
        }

        class PocoDoStatementSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoDoStatementSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoDoStatementSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoDoStatementSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoDoStatementSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoDoStatementSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoDoStatementSyntax>();
        }

        public class PocoDoStatementSyntax : PocoStatementSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax AttributeLists
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken DoKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual StatementSyntax Statement
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken WhileKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken OpenParenToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual ExpressionSyntax Condition
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken CloseParenToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken SemicolonToken
            {
                get;
                set;
            }
        }

        class PocoElementAccessExpressionSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoElementAccessExpressionSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoElementAccessExpressionSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoElementAccessExpressionSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoElementAccessExpressionSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoElementAccessExpressionSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoElementAccessExpressionSyntax>();
        }

        public class PocoElementAccessExpressionSyntax : PocoExpressionSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual ExpressionSyntax Expression
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual BracketedArgumentListSyntax ArgumentList
            {
                get;
                set;
            }
        }

        class PocoElementBindingExpressionSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoElementBindingExpressionSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoElementBindingExpressionSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoElementBindingExpressionSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoElementBindingExpressionSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoElementBindingExpressionSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoElementBindingExpressionSyntax>();
        }

        public class PocoElementBindingExpressionSyntax : PocoExpressionSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual BracketedArgumentListSyntax ArgumentList
            {
                get;
                set;
            }
        }

        class PocoElifDirectiveTriviaSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoElifDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoElifDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoElifDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoElifDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoElifDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoElifDirectiveTriviaSyntax>();
        }

        public class PocoElifDirectiveTriviaSyntax : PocoConditionalDirectiveTriviaSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override SyntaxToken HashToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken ElifKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override ExpressionSyntax Condition
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override SyntaxToken EndOfDirectiveToken
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

        class PocoElseClauseSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoElseClauseSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoElseClauseSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoElseClauseSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoElseClauseSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoElseClauseSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoElseClauseSyntax>();
        }

        public class PocoElseClauseSyntax : PocoCSharpSyntaxNode
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken ElseKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual StatementSyntax Statement
            {
                get;
                set;
            }
        }

        class PocoElseDirectiveTriviaSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoElseDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoElseDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoElseDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoElseDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoElseDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoElseDirectiveTriviaSyntax>();
        }

        public class PocoElseDirectiveTriviaSyntax : PocoBranchingDirectiveTriviaSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override SyntaxToken HashToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken ElseKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override SyntaxToken EndOfDirectiveToken
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

        class PocoEmptyStatementSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoEmptyStatementSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoEmptyStatementSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoEmptyStatementSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoEmptyStatementSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoEmptyStatementSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoEmptyStatementSyntax>();
        }

        public class PocoEmptyStatementSyntax : PocoStatementSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax AttributeLists
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken SemicolonToken
            {
                get;
                set;
            }
        }

        class PocoEndIfDirectiveTriviaSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoEndIfDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoEndIfDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoEndIfDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoEndIfDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoEndIfDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoEndIfDirectiveTriviaSyntax>();
        }

        public class PocoEndIfDirectiveTriviaSyntax : PocoDirectiveTriviaSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override SyntaxToken HashToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken EndIfKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override SyntaxToken EndOfDirectiveToken
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

        class PocoEndRegionDirectiveTriviaSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoEndRegionDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoEndRegionDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoEndRegionDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoEndRegionDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoEndRegionDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoEndRegionDirectiveTriviaSyntax>();
        }

        public class PocoEndRegionDirectiveTriviaSyntax : PocoDirectiveTriviaSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override SyntaxToken HashToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken EndRegionKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override SyntaxToken EndOfDirectiveToken
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

        class PocoEnumDeclarationSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoEnumDeclarationSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoEnumDeclarationSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoEnumDeclarationSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoEnumDeclarationSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoEnumDeclarationSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoEnumDeclarationSyntax>();
        }

        public class PocoEnumDeclarationSyntax : PocoBaseTypeDeclarationSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax AttributeLists
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override Microsoft.CodeAnalysis.SyntaxToken Modifiers
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken EnumKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override SyntaxToken Identifier
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override BaseListSyntax BaseList
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override SyntaxToken OpenBraceToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual Microsoft.CodeAnalysis.CSharp.Syntax.EnumMemberDeclarationSyntax Members
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override SyntaxToken CloseBraceToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override SyntaxToken SemicolonToken
            {
                get;
                set;
            }
        }

        class PocoEnumMemberDeclarationSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoEnumMemberDeclarationSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoEnumMemberDeclarationSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoEnumMemberDeclarationSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoEnumMemberDeclarationSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoEnumMemberDeclarationSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoEnumMemberDeclarationSyntax>();
        }

        public class PocoEnumMemberDeclarationSyntax : PocoMemberDeclarationSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax AttributeLists
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override Microsoft.CodeAnalysis.SyntaxToken Modifiers
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken Identifier
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual EqualsValueClauseSyntax EqualsValue
            {
                get;
                set;
            }
        }

        class PocoEqualsValueClauseSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoEqualsValueClauseSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoEqualsValueClauseSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoEqualsValueClauseSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoEqualsValueClauseSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoEqualsValueClauseSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoEqualsValueClauseSyntax>();
        }

        public class PocoEqualsValueClauseSyntax : PocoCSharpSyntaxNode
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken EqualsToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual ExpressionSyntax Value
            {
                get;
                set;
            }
        }

        class PocoErrorDirectiveTriviaSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoErrorDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoErrorDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoErrorDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoErrorDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoErrorDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoErrorDirectiveTriviaSyntax>();
        }

        public class PocoErrorDirectiveTriviaSyntax : PocoDirectiveTriviaSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override SyntaxToken HashToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken ErrorKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override SyntaxToken EndOfDirectiveToken
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

        class PocoEventDeclarationSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoEventDeclarationSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoEventDeclarationSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoEventDeclarationSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoEventDeclarationSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoEventDeclarationSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoEventDeclarationSyntax>();
        }

        public class PocoEventDeclarationSyntax : PocoBasePropertyDeclarationSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax AttributeLists
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override Microsoft.CodeAnalysis.SyntaxToken Modifiers
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken EventKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override TypeSyntax Type
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override ExplicitInterfaceSpecifierSyntax ExplicitInterfaceSpecifier
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken Identifier
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override AccessorListSyntax AccessorList
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken SemicolonToken
            {
                get;
                set;
            }
        }

        class PocoEventFieldDeclarationSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoEventFieldDeclarationSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoEventFieldDeclarationSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoEventFieldDeclarationSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoEventFieldDeclarationSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoEventFieldDeclarationSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoEventFieldDeclarationSyntax>();
        }

        public class PocoEventFieldDeclarationSyntax : PocoBaseFieldDeclarationSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax AttributeLists
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override Microsoft.CodeAnalysis.SyntaxToken Modifiers
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken EventKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override VariableDeclarationSyntax Declaration
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override SyntaxToken SemicolonToken
            {
                get;
                set;
            }
        }

        class PocoExplicitInterfaceSpecifierSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoExplicitInterfaceSpecifierSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoExplicitInterfaceSpecifierSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoExplicitInterfaceSpecifierSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoExplicitInterfaceSpecifierSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoExplicitInterfaceSpecifierSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoExplicitInterfaceSpecifierSyntax>();
        }

        public class PocoExplicitInterfaceSpecifierSyntax : PocoCSharpSyntaxNode
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual NameSyntax Name
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken DotToken
            {
                get;
                set;
            }
        }

        class PocoExpressionStatementSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoExpressionStatementSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoExpressionStatementSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoExpressionStatementSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoExpressionStatementSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoExpressionStatementSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoExpressionStatementSyntax>();
        }

        public class PocoExpressionStatementSyntax : PocoStatementSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax AttributeLists
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual ExpressionSyntax Expression
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken SemicolonToken
            {
                get;
                set;
            }
        }

        class PocoExpressionSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoExpressionSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoExpressionSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoExpressionSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoExpressionSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoExpressionSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoExpressionSyntax>();
        }

        public class PocoExpressionSyntax : PocoCSharpSyntaxNode
        {
        }

        class PocoExternAliasDirectiveSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoExternAliasDirectiveSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoExternAliasDirectiveSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoExternAliasDirectiveSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoExternAliasDirectiveSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoExternAliasDirectiveSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoExternAliasDirectiveSyntax>();
        }

        public class PocoExternAliasDirectiveSyntax : PocoCSharpSyntaxNode
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken ExternKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken AliasKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken Identifier
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken SemicolonToken
            {
                get;
                set;
            }
        }

        class PocoFieldDeclarationSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoFieldDeclarationSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoFieldDeclarationSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoFieldDeclarationSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoFieldDeclarationSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoFieldDeclarationSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoFieldDeclarationSyntax>();
        }

        public class PocoFieldDeclarationSyntax : PocoBaseFieldDeclarationSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax AttributeLists
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override Microsoft.CodeAnalysis.SyntaxToken Modifiers
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override VariableDeclarationSyntax Declaration
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override SyntaxToken SemicolonToken
            {
                get;
                set;
            }
        }

        class PocoFinallyClauseSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoFinallyClauseSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoFinallyClauseSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoFinallyClauseSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoFinallyClauseSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoFinallyClauseSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoFinallyClauseSyntax>();
        }

        public class PocoFinallyClauseSyntax : PocoCSharpSyntaxNode
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken FinallyKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual BlockSyntax Block
            {
                get;
                set;
            }
        }

        class PocoFixedStatementSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoFixedStatementSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoFixedStatementSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoFixedStatementSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoFixedStatementSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoFixedStatementSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoFixedStatementSyntax>();
        }

        public class PocoFixedStatementSyntax : PocoStatementSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax AttributeLists
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken FixedKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken OpenParenToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual VariableDeclarationSyntax Declaration
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken CloseParenToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual StatementSyntax Statement
            {
                get;
                set;
            }
        }

        class PocoForEachStatementSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoForEachStatementSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoForEachStatementSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoForEachStatementSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoForEachStatementSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoForEachStatementSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoForEachStatementSyntax>();
        }

        public class PocoForEachStatementSyntax : PocoCommonForEachStatementSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax AttributeLists
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override SyntaxToken AwaitKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override SyntaxToken ForEachKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override SyntaxToken OpenParenToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual TypeSyntax Type
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken Identifier
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override SyntaxToken InKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override ExpressionSyntax Expression
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override SyntaxToken CloseParenToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override StatementSyntax Statement
            {
                get;
                set;
            }
        }

        class PocoForEachVariableStatementSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoForEachVariableStatementSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoForEachVariableStatementSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoForEachVariableStatementSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoForEachVariableStatementSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoForEachVariableStatementSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoForEachVariableStatementSyntax>();
        }

        public class PocoForEachVariableStatementSyntax : PocoCommonForEachStatementSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax AttributeLists
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override SyntaxToken AwaitKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override SyntaxToken ForEachKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override SyntaxToken OpenParenToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual ExpressionSyntax Variable
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override SyntaxToken InKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override ExpressionSyntax Expression
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override SyntaxToken CloseParenToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override StatementSyntax Statement
            {
                get;
                set;
            }
        }

        class PocoForStatementSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoForStatementSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoForStatementSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoForStatementSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoForStatementSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoForStatementSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoForStatementSyntax>();
        }

        public class PocoForStatementSyntax : PocoStatementSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax AttributeLists
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken ForKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken OpenParenToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken FirstSemicolonToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual ExpressionSyntax Condition
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken SecondSemicolonToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionSyntax Incrementors
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken CloseParenToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual StatementSyntax Statement
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual VariableDeclarationSyntax Declaration
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionSyntax Initializers
            {
                get;
                set;
            }
        }

        class PocoFromClauseSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoFromClauseSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoFromClauseSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoFromClauseSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoFromClauseSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoFromClauseSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoFromClauseSyntax>();
        }

        public class PocoFromClauseSyntax : PocoQueryClauseSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken FromKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual TypeSyntax Type
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken Identifier
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken InKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual ExpressionSyntax Expression
            {
                get;
                set;
            }
        }

        class PocoGenericNameSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoGenericNameSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoGenericNameSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoGenericNameSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoGenericNameSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoGenericNameSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoGenericNameSyntax>();
        }

        public class PocoGenericNameSyntax : PocoSimpleNameSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override SyntaxToken Identifier
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual TypeArgumentListSyntax TypeArgumentList
            {
                get;
                set;
            }
        }

        class PocoGlobalStatementSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoGlobalStatementSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoGlobalStatementSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoGlobalStatementSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoGlobalStatementSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoGlobalStatementSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoGlobalStatementSyntax>();
        }

        public class PocoGlobalStatementSyntax : PocoMemberDeclarationSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax AttributeLists
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override Microsoft.CodeAnalysis.SyntaxToken Modifiers
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual StatementSyntax Statement
            {
                get;
                set;
            }
        }

        class PocoGotoStatementSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoGotoStatementSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoGotoStatementSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoGotoStatementSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoGotoStatementSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoGotoStatementSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoGotoStatementSyntax>();
        }

        public class PocoGotoStatementSyntax : PocoStatementSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax AttributeLists
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken GotoKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken CaseOrDefaultKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual ExpressionSyntax Expression
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken SemicolonToken
            {
                get;
                set;
            }
        }

        class PocoGroupClauseSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoGroupClauseSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoGroupClauseSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoGroupClauseSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoGroupClauseSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoGroupClauseSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoGroupClauseSyntax>();
        }

        public class PocoGroupClauseSyntax : PocoSelectOrGroupClauseSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken GroupKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual ExpressionSyntax GroupExpression
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken ByKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual ExpressionSyntax ByExpression
            {
                get;
                set;
            }
        }

        class PocoIdentifierNameSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoIdentifierNameSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoIdentifierNameSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoIdentifierNameSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoIdentifierNameSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoIdentifierNameSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoIdentifierNameSyntax>();
        }

        public class PocoIdentifierNameSyntax : PocoSimpleNameSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override SyntaxToken Identifier
            {
                get;
                set;
            }
        }

        class PocoIfDirectiveTriviaSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoIfDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoIfDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoIfDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoIfDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoIfDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoIfDirectiveTriviaSyntax>();
        }

        public class PocoIfDirectiveTriviaSyntax : PocoConditionalDirectiveTriviaSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override SyntaxToken HashToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken IfKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override ExpressionSyntax Condition
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override SyntaxToken EndOfDirectiveToken
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

        class PocoIfStatementSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoIfStatementSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoIfStatementSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoIfStatementSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoIfStatementSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoIfStatementSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoIfStatementSyntax>();
        }

        public class PocoIfStatementSyntax : PocoStatementSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax AttributeLists
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken IfKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken OpenParenToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual ExpressionSyntax Condition
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken CloseParenToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual StatementSyntax Statement
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual ElseClauseSyntax Else
            {
                get;
                set;
            }
        }

        class PocoImplicitArrayCreationExpressionSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoImplicitArrayCreationExpressionSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoImplicitArrayCreationExpressionSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoImplicitArrayCreationExpressionSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoImplicitArrayCreationExpressionSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoImplicitArrayCreationExpressionSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoImplicitArrayCreationExpressionSyntax>();
        }

        public class PocoImplicitArrayCreationExpressionSyntax : PocoExpressionSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken NewKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken OpenBracketToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual Microsoft.CodeAnalysis.SyntaxToken Commas
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken CloseBracketToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual InitializerExpressionSyntax Initializer
            {
                get;
                set;
            }
        }

        class PocoImplicitElementAccessSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoImplicitElementAccessSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoImplicitElementAccessSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoImplicitElementAccessSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoImplicitElementAccessSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoImplicitElementAccessSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoImplicitElementAccessSyntax>();
        }

        public class PocoImplicitElementAccessSyntax : PocoExpressionSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual BracketedArgumentListSyntax ArgumentList
            {
                get;
                set;
            }
        }

        class PocoImplicitStackAllocArrayCreationExpressionSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoImplicitStackAllocArrayCreationExpressionSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoImplicitStackAllocArrayCreationExpressionSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoImplicitStackAllocArrayCreationExpressionSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoImplicitStackAllocArrayCreationExpressionSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoImplicitStackAllocArrayCreationExpressionSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoImplicitStackAllocArrayCreationExpressionSyntax>();
        }

        public class PocoImplicitStackAllocArrayCreationExpressionSyntax : PocoExpressionSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken StackAllocKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken OpenBracketToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken CloseBracketToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual InitializerExpressionSyntax Initializer
            {
                get;
                set;
            }
        }

        class PocoIncompleteMemberSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoIncompleteMemberSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoIncompleteMemberSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoIncompleteMemberSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoIncompleteMemberSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoIncompleteMemberSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoIncompleteMemberSyntax>();
        }

        public class PocoIncompleteMemberSyntax : PocoMemberDeclarationSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax AttributeLists
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override Microsoft.CodeAnalysis.SyntaxToken Modifiers
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual TypeSyntax Type
            {
                get;
                set;
            }
        }

        class PocoIndexerDeclarationSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoIndexerDeclarationSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoIndexerDeclarationSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoIndexerDeclarationSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoIndexerDeclarationSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoIndexerDeclarationSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoIndexerDeclarationSyntax>();
        }

        public class PocoIndexerDeclarationSyntax : PocoBasePropertyDeclarationSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax AttributeLists
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override Microsoft.CodeAnalysis.SyntaxToken Modifiers
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override TypeSyntax Type
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override ExplicitInterfaceSpecifierSyntax ExplicitInterfaceSpecifier
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken ThisKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual BracketedParameterListSyntax ParameterList
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override AccessorListSyntax AccessorList
            {
                get;
                set;
            }
        }

        class PocoIndexerMemberCrefSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoIndexerMemberCrefSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoIndexerMemberCrefSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoIndexerMemberCrefSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoIndexerMemberCrefSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoIndexerMemberCrefSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoIndexerMemberCrefSyntax>();
        }

        public class PocoIndexerMemberCrefSyntax : PocoMemberCrefSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken ThisKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual CrefBracketedParameterListSyntax Parameters
            {
                get;
                set;
            }
        }

        class PocoInitializerExpressionSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoInitializerExpressionSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoInitializerExpressionSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoInitializerExpressionSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoInitializerExpressionSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoInitializerExpressionSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoInitializerExpressionSyntax>();
        }

        public class PocoInitializerExpressionSyntax : PocoExpressionSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken OpenBraceToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionSyntax Expressions
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken CloseBraceToken
            {
                get;
                set;
            }
        }

        class PocoInstanceExpressionSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoInstanceExpressionSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoInstanceExpressionSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoInstanceExpressionSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoInstanceExpressionSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoInstanceExpressionSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoInstanceExpressionSyntax>();
        }

        public class PocoInstanceExpressionSyntax : PocoExpressionSyntax
        {
        }

        class PocoInterfaceDeclarationSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoInterfaceDeclarationSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoInterfaceDeclarationSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoInterfaceDeclarationSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoInterfaceDeclarationSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoInterfaceDeclarationSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoInterfaceDeclarationSyntax>();
        }

        public class PocoInterfaceDeclarationSyntax : PocoTypeDeclarationSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax AttributeLists
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override Microsoft.CodeAnalysis.SyntaxToken Modifiers
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override SyntaxToken Keyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override SyntaxToken Identifier
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override TypeParameterListSyntax TypeParameterList
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override BaseListSyntax BaseList
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterConstraintClauseSyntax ConstraintClauses
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override SyntaxToken OpenBraceToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax Members
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override SyntaxToken CloseBraceToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override SyntaxToken SemicolonToken
            {
                get;
                set;
            }
        }

        class PocoInterpolatedStringContentSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoInterpolatedStringContentSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoInterpolatedStringContentSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoInterpolatedStringContentSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoInterpolatedStringContentSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoInterpolatedStringContentSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoInterpolatedStringContentSyntax>();
        }

        public class PocoInterpolatedStringContentSyntax : PocoCSharpSyntaxNode
        {
        }

        class PocoInterpolatedStringExpressionSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoInterpolatedStringExpressionSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoInterpolatedStringExpressionSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoInterpolatedStringExpressionSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoInterpolatedStringExpressionSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoInterpolatedStringExpressionSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoInterpolatedStringExpressionSyntax>();
        }

        public class PocoInterpolatedStringExpressionSyntax : PocoExpressionSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken StringStartToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual Microsoft.CodeAnalysis.CSharp.Syntax.InterpolatedStringContentSyntax Contents
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken StringEndToken
            {
                get;
                set;
            }
        }

        class PocoInterpolatedStringTextSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoInterpolatedStringTextSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoInterpolatedStringTextSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoInterpolatedStringTextSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoInterpolatedStringTextSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoInterpolatedStringTextSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoInterpolatedStringTextSyntax>();
        }

        public class PocoInterpolatedStringTextSyntax : PocoInterpolatedStringContentSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken TextToken
            {
                get;
                set;
            }
        }

        class PocoInterpolationAlignmentClauseSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoInterpolationAlignmentClauseSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoInterpolationAlignmentClauseSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoInterpolationAlignmentClauseSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoInterpolationAlignmentClauseSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoInterpolationAlignmentClauseSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoInterpolationAlignmentClauseSyntax>();
        }

        public class PocoInterpolationAlignmentClauseSyntax : PocoCSharpSyntaxNode
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken CommaToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual ExpressionSyntax Value
            {
                get;
                set;
            }
        }

        class PocoInterpolationFormatClauseSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoInterpolationFormatClauseSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoInterpolationFormatClauseSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoInterpolationFormatClauseSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoInterpolationFormatClauseSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoInterpolationFormatClauseSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoInterpolationFormatClauseSyntax>();
        }

        public class PocoInterpolationFormatClauseSyntax : PocoCSharpSyntaxNode
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken ColonToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken FormatStringToken
            {
                get;
                set;
            }
        }

        class PocoInterpolationSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoInterpolationSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoInterpolationSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoInterpolationSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoInterpolationSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoInterpolationSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoInterpolationSyntax>();
        }

        public class PocoInterpolationSyntax : PocoInterpolatedStringContentSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken OpenBraceToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual ExpressionSyntax Expression
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual InterpolationAlignmentClauseSyntax AlignmentClause
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual InterpolationFormatClauseSyntax FormatClause
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken CloseBraceToken
            {
                get;
                set;
            }
        }

        class PocoInvocationExpressionSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoInvocationExpressionSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoInvocationExpressionSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoInvocationExpressionSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoInvocationExpressionSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoInvocationExpressionSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoInvocationExpressionSyntax>();
        }

        public class PocoInvocationExpressionSyntax : PocoExpressionSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual ExpressionSyntax Expression
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual ArgumentListSyntax ArgumentList
            {
                get;
                set;
            }
        }

        class PocoIsPatternExpressionSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoIsPatternExpressionSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoIsPatternExpressionSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoIsPatternExpressionSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoIsPatternExpressionSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoIsPatternExpressionSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoIsPatternExpressionSyntax>();
        }

        public class PocoIsPatternExpressionSyntax : PocoExpressionSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual ExpressionSyntax Expression
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken IsKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual PatternSyntax Pattern
            {
                get;
                set;
            }
        }

        class PocoJoinClauseSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoJoinClauseSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoJoinClauseSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoJoinClauseSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoJoinClauseSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoJoinClauseSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoJoinClauseSyntax>();
        }

        public class PocoJoinClauseSyntax : PocoQueryClauseSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken JoinKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual TypeSyntax Type
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken Identifier
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken InKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual ExpressionSyntax InExpression
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken OnKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual ExpressionSyntax LeftExpression
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken EqualsKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual ExpressionSyntax RightExpression
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual JoinIntoClauseSyntax Into
            {
                get;
                set;
            }
        }

        class PocoJoinIntoClauseSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoJoinIntoClauseSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoJoinIntoClauseSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoJoinIntoClauseSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoJoinIntoClauseSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoJoinIntoClauseSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoJoinIntoClauseSyntax>();
        }

        public class PocoJoinIntoClauseSyntax : PocoCSharpSyntaxNode
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken IntoKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken Identifier
            {
                get;
                set;
            }
        }

        class PocoLabeledStatementSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoLabeledStatementSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoLabeledStatementSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoLabeledStatementSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoLabeledStatementSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoLabeledStatementSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoLabeledStatementSyntax>();
        }

        public class PocoLabeledStatementSyntax : PocoStatementSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax AttributeLists
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken Identifier
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken ColonToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual StatementSyntax Statement
            {
                get;
                set;
            }
        }

        class PocoLambdaExpressionSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoLambdaExpressionSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoLambdaExpressionSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoLambdaExpressionSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoLambdaExpressionSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoLambdaExpressionSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoLambdaExpressionSyntax>();
        }

        public class PocoLambdaExpressionSyntax : PocoAnonymousFunctionExpressionSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken ArrowToken
            {
                get;
                set;
            }
        }

        class PocoLetClauseSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoLetClauseSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoLetClauseSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoLetClauseSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoLetClauseSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoLetClauseSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoLetClauseSyntax>();
        }

        public class PocoLetClauseSyntax : PocoQueryClauseSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken LetKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken Identifier
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken EqualsToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual ExpressionSyntax Expression
            {
                get;
                set;
            }
        }

        class PocoLineDirectiveTriviaSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoLineDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoLineDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoLineDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoLineDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoLineDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoLineDirectiveTriviaSyntax>();
        }

        public class PocoLineDirectiveTriviaSyntax : PocoDirectiveTriviaSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override SyntaxToken HashToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken LineKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken Line
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken File
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override SyntaxToken EndOfDirectiveToken
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

        class PocoLiteralExpressionSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoLiteralExpressionSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoLiteralExpressionSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoLiteralExpressionSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoLiteralExpressionSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoLiteralExpressionSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoLiteralExpressionSyntax>();
        }

        public class PocoLiteralExpressionSyntax : PocoExpressionSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken Token
            {
                get;
                set;
            }
        }

        class PocoLoadDirectiveTriviaSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoLoadDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoLoadDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoLoadDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoLoadDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoLoadDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoLoadDirectiveTriviaSyntax>();
        }

        public class PocoLoadDirectiveTriviaSyntax : PocoDirectiveTriviaSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override SyntaxToken HashToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken LoadKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken File
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override SyntaxToken EndOfDirectiveToken
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

        class PocoLocalDeclarationStatementSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoLocalDeclarationStatementSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoLocalDeclarationStatementSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoLocalDeclarationStatementSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoLocalDeclarationStatementSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoLocalDeclarationStatementSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoLocalDeclarationStatementSyntax>();
        }

        public class PocoLocalDeclarationStatementSyntax : PocoStatementSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax AttributeLists
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken AwaitKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken UsingKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual Microsoft.CodeAnalysis.SyntaxToken Modifiers
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual VariableDeclarationSyntax Declaration
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken SemicolonToken
            {
                get;
                set;
            }
        }

        class PocoLocalFunctionStatementSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoLocalFunctionStatementSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoLocalFunctionStatementSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoLocalFunctionStatementSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoLocalFunctionStatementSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoLocalFunctionStatementSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoLocalFunctionStatementSyntax>();
        }

        public class PocoLocalFunctionStatementSyntax : PocoStatementSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax AttributeLists
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual Microsoft.CodeAnalysis.SyntaxToken Modifiers
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual TypeSyntax ReturnType
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken Identifier
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual TypeParameterListSyntax TypeParameterList
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual ParameterListSyntax ParameterList
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterConstraintClauseSyntax ConstraintClauses
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual BlockSyntax Body
            {
                get;
                set;
            }
        }

        class PocoLockStatementSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoLockStatementSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoLockStatementSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoLockStatementSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoLockStatementSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoLockStatementSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoLockStatementSyntax>();
        }

        public class PocoLockStatementSyntax : PocoStatementSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax AttributeLists
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken LockKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken OpenParenToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual ExpressionSyntax Expression
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken CloseParenToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual StatementSyntax Statement
            {
                get;
                set;
            }
        }

        class PocoMakeRefExpressionSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoMakeRefExpressionSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoMakeRefExpressionSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoMakeRefExpressionSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoMakeRefExpressionSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoMakeRefExpressionSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoMakeRefExpressionSyntax>();
        }

        public class PocoMakeRefExpressionSyntax : PocoExpressionSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken Keyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken OpenParenToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual ExpressionSyntax Expression
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken CloseParenToken
            {
                get;
                set;
            }
        }

        class PocoMemberAccessExpressionSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoMemberAccessExpressionSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoMemberAccessExpressionSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoMemberAccessExpressionSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoMemberAccessExpressionSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoMemberAccessExpressionSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoMemberAccessExpressionSyntax>();
        }

        public class PocoMemberAccessExpressionSyntax : PocoExpressionSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual ExpressionSyntax Expression
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken OperatorToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SimpleNameSyntax Name
            {
                get;
                set;
            }
        }

        class PocoMemberBindingExpressionSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoMemberBindingExpressionSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoMemberBindingExpressionSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoMemberBindingExpressionSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoMemberBindingExpressionSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoMemberBindingExpressionSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoMemberBindingExpressionSyntax>();
        }

        public class PocoMemberBindingExpressionSyntax : PocoExpressionSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken OperatorToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SimpleNameSyntax Name
            {
                get;
                set;
            }
        }

        class PocoMemberCrefSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoMemberCrefSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoMemberCrefSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoMemberCrefSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoMemberCrefSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoMemberCrefSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoMemberCrefSyntax>();
        }

        public class PocoMemberCrefSyntax : PocoCrefSyntax
        {
        }

        class PocoMemberDeclarationSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoMemberDeclarationSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoMemberDeclarationSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoMemberDeclarationSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoMemberDeclarationSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoMemberDeclarationSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoMemberDeclarationSyntax>();
        }

        public class PocoMemberDeclarationSyntax : PocoCSharpSyntaxNode
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax AttributeLists
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual Microsoft.CodeAnalysis.SyntaxToken Modifiers
            {
                get;
                set;
            }
        }

        class PocoMethodDeclarationSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoMethodDeclarationSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoMethodDeclarationSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoMethodDeclarationSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoMethodDeclarationSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoMethodDeclarationSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoMethodDeclarationSyntax>();
        }

        public class PocoMethodDeclarationSyntax : PocoBaseMethodDeclarationSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax AttributeLists
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override Microsoft.CodeAnalysis.SyntaxToken Modifiers
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual TypeSyntax ReturnType
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual ExplicitInterfaceSpecifierSyntax ExplicitInterfaceSpecifier
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken Identifier
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual TypeParameterListSyntax TypeParameterList
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override ParameterListSyntax ParameterList
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterConstraintClauseSyntax ConstraintClauses
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override BlockSyntax Body
            {
                get;
                set;
            }
        }

        class PocoNameColonSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoNameColonSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoNameColonSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoNameColonSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoNameColonSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoNameColonSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoNameColonSyntax>();
        }

        public class PocoNameColonSyntax : PocoCSharpSyntaxNode
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual IdentifierNameSyntax Name
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken ColonToken
            {
                get;
                set;
            }
        }

        class PocoNameEqualsSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoNameEqualsSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoNameEqualsSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoNameEqualsSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoNameEqualsSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoNameEqualsSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoNameEqualsSyntax>();
        }

        public class PocoNameEqualsSyntax : PocoCSharpSyntaxNode
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual IdentifierNameSyntax Name
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken EqualsToken
            {
                get;
                set;
            }
        }

        class PocoNameMemberCrefSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoNameMemberCrefSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoNameMemberCrefSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoNameMemberCrefSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoNameMemberCrefSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoNameMemberCrefSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoNameMemberCrefSyntax>();
        }

        public class PocoNameMemberCrefSyntax : PocoMemberCrefSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual TypeSyntax Name
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual CrefParameterListSyntax Parameters
            {
                get;
                set;
            }
        }

        class PocoNamespaceDeclarationSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoNamespaceDeclarationSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoNamespaceDeclarationSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoNamespaceDeclarationSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoNamespaceDeclarationSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoNamespaceDeclarationSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoNamespaceDeclarationSyntax>();
        }

        public class PocoNamespaceDeclarationSyntax : PocoMemberDeclarationSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax AttributeLists
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override Microsoft.CodeAnalysis.SyntaxToken Modifiers
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken NamespaceKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual NameSyntax Name
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken OpenBraceToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual Microsoft.CodeAnalysis.CSharp.Syntax.ExternAliasDirectiveSyntax Externs
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual Microsoft.CodeAnalysis.CSharp.Syntax.UsingDirectiveSyntax Usings
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax Members
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken CloseBraceToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken SemicolonToken
            {
                get;
                set;
            }
        }

        class PocoNameSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoNameSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoNameSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoNameSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoNameSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoNameSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoNameSyntax>();
        }

        public class PocoNameSyntax : PocoTypeSyntax
        {
        }

        class PocoNullableDirectiveTriviaSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoNullableDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoNullableDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoNullableDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoNullableDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoNullableDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoNullableDirectiveTriviaSyntax>();
        }

        public class PocoNullableDirectiveTriviaSyntax : PocoDirectiveTriviaSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override SyntaxToken HashToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken NullableKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken SettingToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken TargetToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override SyntaxToken EndOfDirectiveToken
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

        class PocoNullableTypeSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoNullableTypeSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoNullableTypeSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoNullableTypeSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoNullableTypeSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoNullableTypeSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoNullableTypeSyntax>();
        }

        public class PocoNullableTypeSyntax : PocoTypeSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual TypeSyntax ElementType
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken QuestionToken
            {
                get;
                set;
            }
        }

        class PocoObjectCreationExpressionSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoObjectCreationExpressionSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoObjectCreationExpressionSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoObjectCreationExpressionSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoObjectCreationExpressionSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoObjectCreationExpressionSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoObjectCreationExpressionSyntax>();
        }

        public class PocoObjectCreationExpressionSyntax : PocoExpressionSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken NewKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual TypeSyntax Type
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual ArgumentListSyntax ArgumentList
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual InitializerExpressionSyntax Initializer
            {
                get;
                set;
            }
        }

        class PocoOmittedArraySizeExpressionSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoOmittedArraySizeExpressionSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoOmittedArraySizeExpressionSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoOmittedArraySizeExpressionSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoOmittedArraySizeExpressionSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoOmittedArraySizeExpressionSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoOmittedArraySizeExpressionSyntax>();
        }

        public class PocoOmittedArraySizeExpressionSyntax : PocoExpressionSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken OmittedArraySizeExpressionToken
            {
                get;
                set;
            }
        }

        class PocoOmittedTypeArgumentSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoOmittedTypeArgumentSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoOmittedTypeArgumentSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoOmittedTypeArgumentSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoOmittedTypeArgumentSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoOmittedTypeArgumentSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoOmittedTypeArgumentSyntax>();
        }

        public class PocoOmittedTypeArgumentSyntax : PocoTypeSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken OmittedTypeArgumentToken
            {
                get;
                set;
            }
        }

        class PocoOperatorDeclarationSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoOperatorDeclarationSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoOperatorDeclarationSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoOperatorDeclarationSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoOperatorDeclarationSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoOperatorDeclarationSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoOperatorDeclarationSyntax>();
        }

        public class PocoOperatorDeclarationSyntax : PocoBaseMethodDeclarationSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax AttributeLists
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override Microsoft.CodeAnalysis.SyntaxToken Modifiers
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual TypeSyntax ReturnType
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken OperatorKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken OperatorToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override ParameterListSyntax ParameterList
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override BlockSyntax Body
            {
                get;
                set;
            }
        }

        class PocoOperatorMemberCrefSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoOperatorMemberCrefSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoOperatorMemberCrefSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoOperatorMemberCrefSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoOperatorMemberCrefSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoOperatorMemberCrefSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoOperatorMemberCrefSyntax>();
        }

        public class PocoOperatorMemberCrefSyntax : PocoMemberCrefSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken OperatorKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken OperatorToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual CrefParameterListSyntax Parameters
            {
                get;
                set;
            }
        }

        class PocoOrderByClauseSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoOrderByClauseSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoOrderByClauseSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoOrderByClauseSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoOrderByClauseSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoOrderByClauseSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoOrderByClauseSyntax>();
        }

        public class PocoOrderByClauseSyntax : PocoQueryClauseSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken OrderByKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual Microsoft.CodeAnalysis.CSharp.Syntax.OrderingSyntax Orderings
            {
                get;
                set;
            }
        }

        class PocoOrderingSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoOrderingSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoOrderingSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoOrderingSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoOrderingSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoOrderingSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoOrderingSyntax>();
        }

        public class PocoOrderingSyntax : PocoCSharpSyntaxNode
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual ExpressionSyntax Expression
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken AscendingOrDescendingKeyword
            {
                get;
                set;
            }
        }

        class PocoParameterListSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoParameterListSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoParameterListSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoParameterListSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoParameterListSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoParameterListSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoParameterListSyntax>();
        }

        public class PocoParameterListSyntax : PocoBaseParameterListSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken OpenParenToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override Microsoft.CodeAnalysis.CSharp.Syntax.ParameterSyntax Parameters
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken CloseParenToken
            {
                get;
                set;
            }
        }

        class PocoParameterSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoParameterSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoParameterSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoParameterSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoParameterSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoParameterSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoParameterSyntax>();
        }

        public class PocoParameterSyntax : PocoCSharpSyntaxNode
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax AttributeLists
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual Microsoft.CodeAnalysis.SyntaxToken Modifiers
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual TypeSyntax Type
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken Identifier
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual EqualsValueClauseSyntax Default
            {
                get;
                set;
            }
        }

        class PocoParenthesizedExpressionSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoParenthesizedExpressionSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoParenthesizedExpressionSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoParenthesizedExpressionSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoParenthesizedExpressionSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoParenthesizedExpressionSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoParenthesizedExpressionSyntax>();
        }

        public class PocoParenthesizedExpressionSyntax : PocoExpressionSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken OpenParenToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual ExpressionSyntax Expression
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken CloseParenToken
            {
                get;
                set;
            }
        }

        class PocoParenthesizedLambdaExpressionSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoParenthesizedLambdaExpressionSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoParenthesizedLambdaExpressionSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoParenthesizedLambdaExpressionSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoParenthesizedLambdaExpressionSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoParenthesizedLambdaExpressionSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoParenthesizedLambdaExpressionSyntax>();
        }

        public class PocoParenthesizedLambdaExpressionSyntax : PocoLambdaExpressionSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override SyntaxToken AsyncKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual ParameterListSyntax ParameterList
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override SyntaxToken ArrowToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override BlockSyntax Block
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override ExpressionSyntax ExpressionBody
            {
                get;
                set;
            }
        }

        class PocoParenthesizedVariableDesignationSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoParenthesizedVariableDesignationSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoParenthesizedVariableDesignationSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoParenthesizedVariableDesignationSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoParenthesizedVariableDesignationSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoParenthesizedVariableDesignationSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoParenthesizedVariableDesignationSyntax>();
        }

        public class PocoParenthesizedVariableDesignationSyntax : PocoVariableDesignationSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken OpenParenToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual Microsoft.CodeAnalysis.CSharp.Syntax.VariableDesignationSyntax Variables
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken CloseParenToken
            {
                get;
                set;
            }
        }

        class PocoPatternSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoPatternSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoPatternSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoPatternSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoPatternSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoPatternSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoPatternSyntax>();
        }

        public class PocoPatternSyntax : PocoCSharpSyntaxNode
        {
        }

        class PocoPointerTypeSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoPointerTypeSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoPointerTypeSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoPointerTypeSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoPointerTypeSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoPointerTypeSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoPointerTypeSyntax>();
        }

        public class PocoPointerTypeSyntax : PocoTypeSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual TypeSyntax ElementType
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken AsteriskToken
            {
                get;
                set;
            }
        }

        class PocoPositionalPatternClauseSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoPositionalPatternClauseSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoPositionalPatternClauseSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoPositionalPatternClauseSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoPositionalPatternClauseSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoPositionalPatternClauseSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoPositionalPatternClauseSyntax>();
        }

        public class PocoPositionalPatternClauseSyntax : PocoCSharpSyntaxNode
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken OpenParenToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual Microsoft.CodeAnalysis.CSharp.Syntax.SubpatternSyntax Subpatterns
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken CloseParenToken
            {
                get;
                set;
            }
        }

        class PocoPostfixUnaryExpressionSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoPostfixUnaryExpressionSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoPostfixUnaryExpressionSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoPostfixUnaryExpressionSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoPostfixUnaryExpressionSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoPostfixUnaryExpressionSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoPostfixUnaryExpressionSyntax>();
        }

        public class PocoPostfixUnaryExpressionSyntax : PocoExpressionSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual ExpressionSyntax Operand
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken OperatorToken
            {
                get;
                set;
            }
        }

        class PocoPragmaChecksumDirectiveTriviaSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoPragmaChecksumDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoPragmaChecksumDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoPragmaChecksumDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoPragmaChecksumDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoPragmaChecksumDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoPragmaChecksumDirectiveTriviaSyntax>();
        }

        public class PocoPragmaChecksumDirectiveTriviaSyntax : PocoDirectiveTriviaSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override SyntaxToken HashToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken PragmaKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken ChecksumKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken File
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken Guid
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken Bytes
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override SyntaxToken EndOfDirectiveToken
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

        class PocoPragmaWarningDirectiveTriviaSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoPragmaWarningDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoPragmaWarningDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoPragmaWarningDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoPragmaWarningDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoPragmaWarningDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoPragmaWarningDirectiveTriviaSyntax>();
        }

        public class PocoPragmaWarningDirectiveTriviaSyntax : PocoDirectiveTriviaSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override SyntaxToken HashToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken PragmaKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken WarningKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken DisableOrRestoreKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionSyntax ErrorCodes
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override SyntaxToken EndOfDirectiveToken
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

        class PocoPredefinedTypeSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoPredefinedTypeSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoPredefinedTypeSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoPredefinedTypeSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoPredefinedTypeSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoPredefinedTypeSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoPredefinedTypeSyntax>();
        }

        public class PocoPredefinedTypeSyntax : PocoTypeSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken Keyword
            {
                get;
                set;
            }
        }

        class PocoPrefixUnaryExpressionSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoPrefixUnaryExpressionSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoPrefixUnaryExpressionSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoPrefixUnaryExpressionSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoPrefixUnaryExpressionSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoPrefixUnaryExpressionSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoPrefixUnaryExpressionSyntax>();
        }

        public class PocoPrefixUnaryExpressionSyntax : PocoExpressionSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken OperatorToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual ExpressionSyntax Operand
            {
                get;
                set;
            }
        }

        class PocoPropertyDeclarationSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoPropertyDeclarationSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoPropertyDeclarationSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoPropertyDeclarationSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoPropertyDeclarationSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoPropertyDeclarationSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoPropertyDeclarationSyntax>();
        }

        public class PocoPropertyDeclarationSyntax : PocoBasePropertyDeclarationSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax AttributeLists
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override Microsoft.CodeAnalysis.SyntaxToken Modifiers
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override TypeSyntax Type
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override ExplicitInterfaceSpecifierSyntax ExplicitInterfaceSpecifier
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken Identifier
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override AccessorListSyntax AccessorList
            {
                get;
                set;
            }
        }

        class PocoPropertyPatternClauseSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoPropertyPatternClauseSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoPropertyPatternClauseSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoPropertyPatternClauseSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoPropertyPatternClauseSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoPropertyPatternClauseSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoPropertyPatternClauseSyntax>();
        }

        public class PocoPropertyPatternClauseSyntax : PocoCSharpSyntaxNode
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken OpenBraceToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual Microsoft.CodeAnalysis.CSharp.Syntax.SubpatternSyntax Subpatterns
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken CloseBraceToken
            {
                get;
                set;
            }
        }

        class PocoQualifiedCrefSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoQualifiedCrefSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoQualifiedCrefSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoQualifiedCrefSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoQualifiedCrefSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoQualifiedCrefSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoQualifiedCrefSyntax>();
        }

        public class PocoQualifiedCrefSyntax : PocoCrefSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual TypeSyntax Container
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken DotToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual MemberCrefSyntax Member
            {
                get;
                set;
            }
        }

        class PocoQualifiedNameSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoQualifiedNameSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoQualifiedNameSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoQualifiedNameSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoQualifiedNameSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoQualifiedNameSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoQualifiedNameSyntax>();
        }

        public class PocoQualifiedNameSyntax : PocoNameSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual NameSyntax Left
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken DotToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SimpleNameSyntax Right
            {
                get;
                set;
            }
        }

        class PocoQueryBodySyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoQueryBodySyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoQueryBodySyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoQueryBodySyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoQueryBodySyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoQueryBodySyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoQueryBodySyntax>();
        }

        public class PocoQueryBodySyntax : PocoCSharpSyntaxNode
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual Microsoft.CodeAnalysis.CSharp.Syntax.QueryClauseSyntax Clauses
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SelectOrGroupClauseSyntax SelectOrGroup
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual QueryContinuationSyntax Continuation
            {
                get;
                set;
            }
        }

        class PocoQueryClauseSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoQueryClauseSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoQueryClauseSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoQueryClauseSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoQueryClauseSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoQueryClauseSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoQueryClauseSyntax>();
        }

        public class PocoQueryClauseSyntax : PocoCSharpSyntaxNode
        {
        }

        class PocoQueryContinuationSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoQueryContinuationSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoQueryContinuationSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoQueryContinuationSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoQueryContinuationSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoQueryContinuationSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoQueryContinuationSyntax>();
        }

        public class PocoQueryContinuationSyntax : PocoCSharpSyntaxNode
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken IntoKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken Identifier
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual QueryBodySyntax Body
            {
                get;
                set;
            }
        }

        class PocoQueryExpressionSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoQueryExpressionSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoQueryExpressionSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoQueryExpressionSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoQueryExpressionSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoQueryExpressionSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoQueryExpressionSyntax>();
        }

        public class PocoQueryExpressionSyntax : PocoExpressionSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual FromClauseSyntax FromClause
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual QueryBodySyntax Body
            {
                get;
                set;
            }
        }

        class PocoRangeExpressionSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoRangeExpressionSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoRangeExpressionSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoRangeExpressionSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoRangeExpressionSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoRangeExpressionSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoRangeExpressionSyntax>();
        }

        public class PocoRangeExpressionSyntax : PocoExpressionSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual ExpressionSyntax LeftOperand
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken OperatorToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual ExpressionSyntax RightOperand
            {
                get;
                set;
            }
        }

        class PocoRecursivePatternSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoRecursivePatternSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoRecursivePatternSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoRecursivePatternSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoRecursivePatternSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoRecursivePatternSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoRecursivePatternSyntax>();
        }

        public class PocoRecursivePatternSyntax : PocoPatternSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual TypeSyntax Type
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual PositionalPatternClauseSyntax PositionalPatternClause
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual PropertyPatternClauseSyntax PropertyPatternClause
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual VariableDesignationSyntax Designation
            {
                get;
                set;
            }
        }

        class PocoReferenceDirectiveTriviaSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoReferenceDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoReferenceDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoReferenceDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoReferenceDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoReferenceDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoReferenceDirectiveTriviaSyntax>();
        }

        public class PocoReferenceDirectiveTriviaSyntax : PocoDirectiveTriviaSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override SyntaxToken HashToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken ReferenceKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken File
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override SyntaxToken EndOfDirectiveToken
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

        class PocoRefExpressionSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoRefExpressionSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoRefExpressionSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoRefExpressionSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoRefExpressionSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoRefExpressionSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoRefExpressionSyntax>();
        }

        public class PocoRefExpressionSyntax : PocoExpressionSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken RefKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual ExpressionSyntax Expression
            {
                get;
                set;
            }
        }

        class PocoRefTypeExpressionSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoRefTypeExpressionSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoRefTypeExpressionSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoRefTypeExpressionSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoRefTypeExpressionSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoRefTypeExpressionSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoRefTypeExpressionSyntax>();
        }

        public class PocoRefTypeExpressionSyntax : PocoExpressionSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken Keyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken OpenParenToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual ExpressionSyntax Expression
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken CloseParenToken
            {
                get;
                set;
            }
        }

        class PocoRefTypeSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoRefTypeSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoRefTypeSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoRefTypeSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoRefTypeSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoRefTypeSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoRefTypeSyntax>();
        }

        public class PocoRefTypeSyntax : PocoTypeSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken RefKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken ReadOnlyKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual TypeSyntax Type
            {
                get;
                set;
            }
        }

        class PocoRefValueExpressionSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoRefValueExpressionSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoRefValueExpressionSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoRefValueExpressionSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoRefValueExpressionSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoRefValueExpressionSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoRefValueExpressionSyntax>();
        }

        public class PocoRefValueExpressionSyntax : PocoExpressionSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken Keyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken OpenParenToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual ExpressionSyntax Expression
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken Comma
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual TypeSyntax Type
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken CloseParenToken
            {
                get;
                set;
            }
        }

        class PocoRegionDirectiveTriviaSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoRegionDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoRegionDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoRegionDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoRegionDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoRegionDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoRegionDirectiveTriviaSyntax>();
        }

        public class PocoRegionDirectiveTriviaSyntax : PocoDirectiveTriviaSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override SyntaxToken HashToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken RegionKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override SyntaxToken EndOfDirectiveToken
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

        class PocoReturnStatementSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoReturnStatementSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoReturnStatementSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoReturnStatementSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoReturnStatementSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoReturnStatementSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoReturnStatementSyntax>();
        }

        public class PocoReturnStatementSyntax : PocoStatementSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax AttributeLists
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken ReturnKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual ExpressionSyntax Expression
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken SemicolonToken
            {
                get;
                set;
            }
        }

        class PocoSelectClauseSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoSelectClauseSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoSelectClauseSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoSelectClauseSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoSelectClauseSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoSelectClauseSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoSelectClauseSyntax>();
        }

        public class PocoSelectClauseSyntax : PocoSelectOrGroupClauseSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken SelectKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual ExpressionSyntax Expression
            {
                get;
                set;
            }
        }

        class PocoSelectOrGroupClauseSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoSelectOrGroupClauseSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoSelectOrGroupClauseSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoSelectOrGroupClauseSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoSelectOrGroupClauseSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoSelectOrGroupClauseSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoSelectOrGroupClauseSyntax>();
        }

        public class PocoSelectOrGroupClauseSyntax : PocoCSharpSyntaxNode
        {
        }

        class PocoShebangDirectiveTriviaSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoShebangDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoShebangDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoShebangDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoShebangDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoShebangDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoShebangDirectiveTriviaSyntax>();
        }

        public class PocoShebangDirectiveTriviaSyntax : PocoDirectiveTriviaSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override SyntaxToken HashToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken ExclamationToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override SyntaxToken EndOfDirectiveToken
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

        class PocoSimpleBaseTypeSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoSimpleBaseTypeSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoSimpleBaseTypeSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoSimpleBaseTypeSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoSimpleBaseTypeSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoSimpleBaseTypeSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoSimpleBaseTypeSyntax>();
        }

        public class PocoSimpleBaseTypeSyntax : PocoBaseTypeSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override TypeSyntax Type
            {
                get;
                set;
            }
        }

        class PocoSimpleLambdaExpressionSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoSimpleLambdaExpressionSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoSimpleLambdaExpressionSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoSimpleLambdaExpressionSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoSimpleLambdaExpressionSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoSimpleLambdaExpressionSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoSimpleLambdaExpressionSyntax>();
        }

        public class PocoSimpleLambdaExpressionSyntax : PocoLambdaExpressionSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override SyntaxToken AsyncKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual ParameterSyntax Parameter
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override SyntaxToken ArrowToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override BlockSyntax Block
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override ExpressionSyntax ExpressionBody
            {
                get;
                set;
            }
        }

        class PocoSimpleNameSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoSimpleNameSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoSimpleNameSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoSimpleNameSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoSimpleNameSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoSimpleNameSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoSimpleNameSyntax>();
        }

        public class PocoSimpleNameSyntax : PocoNameSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken Identifier
            {
                get;
                set;
            }
        }

        class PocoSingleVariableDesignationSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoSingleVariableDesignationSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoSingleVariableDesignationSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoSingleVariableDesignationSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoSingleVariableDesignationSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoSingleVariableDesignationSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoSingleVariableDesignationSyntax>();
        }

        public class PocoSingleVariableDesignationSyntax : PocoVariableDesignationSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken Identifier
            {
                get;
                set;
            }
        }

        class PocoSizeOfExpressionSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoSizeOfExpressionSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoSizeOfExpressionSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoSizeOfExpressionSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoSizeOfExpressionSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoSizeOfExpressionSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoSizeOfExpressionSyntax>();
        }

        public class PocoSizeOfExpressionSyntax : PocoExpressionSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken Keyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken OpenParenToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual TypeSyntax Type
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken CloseParenToken
            {
                get;
                set;
            }
        }

        class PocoSkippedTokensTriviaSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoSkippedTokensTriviaSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoSkippedTokensTriviaSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoSkippedTokensTriviaSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoSkippedTokensTriviaSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoSkippedTokensTriviaSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoSkippedTokensTriviaSyntax>();
        }

        public class PocoSkippedTokensTriviaSyntax : PocoStructuredTriviaSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual Microsoft.CodeAnalysis.SyntaxToken Tokens
            {
                get;
                set;
            }
        }

        class PocoStackAllocArrayCreationExpressionSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoStackAllocArrayCreationExpressionSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoStackAllocArrayCreationExpressionSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoStackAllocArrayCreationExpressionSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoStackAllocArrayCreationExpressionSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoStackAllocArrayCreationExpressionSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoStackAllocArrayCreationExpressionSyntax>();
        }

        public class PocoStackAllocArrayCreationExpressionSyntax : PocoExpressionSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken StackAllocKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual TypeSyntax Type
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual InitializerExpressionSyntax Initializer
            {
                get;
                set;
            }
        }

        class PocoStatementSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoStatementSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoStatementSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoStatementSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoStatementSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoStatementSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoStatementSyntax>();
        }

        public class PocoStatementSyntax : PocoCSharpSyntaxNode
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax AttributeLists
            {
                get;
                set;
            }
        }

        class PocoStructDeclarationSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoStructDeclarationSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoStructDeclarationSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoStructDeclarationSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoStructDeclarationSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoStructDeclarationSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoStructDeclarationSyntax>();
        }

        public class PocoStructDeclarationSyntax : PocoTypeDeclarationSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax AttributeLists
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override Microsoft.CodeAnalysis.SyntaxToken Modifiers
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override SyntaxToken Keyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override SyntaxToken Identifier
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override TypeParameterListSyntax TypeParameterList
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override BaseListSyntax BaseList
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterConstraintClauseSyntax ConstraintClauses
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override SyntaxToken OpenBraceToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax Members
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override SyntaxToken CloseBraceToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override SyntaxToken SemicolonToken
            {
                get;
                set;
            }
        }

        class PocoStructuredTriviaSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoStructuredTriviaSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoStructuredTriviaSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoStructuredTriviaSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoStructuredTriviaSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoStructuredTriviaSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoStructuredTriviaSyntax>();
        }

        public class PocoStructuredTriviaSyntax : PocoCSharpSyntaxNode
        {
        }

        class PocoSubpatternSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoSubpatternSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoSubpatternSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoSubpatternSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoSubpatternSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoSubpatternSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoSubpatternSyntax>();
        }

        public class PocoSubpatternSyntax : PocoCSharpSyntaxNode
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual NameColonSyntax NameColon
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual PatternSyntax Pattern
            {
                get;
                set;
            }
        }

        class PocoSwitchExpressionArmSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoSwitchExpressionArmSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoSwitchExpressionArmSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoSwitchExpressionArmSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoSwitchExpressionArmSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoSwitchExpressionArmSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoSwitchExpressionArmSyntax>();
        }

        public class PocoSwitchExpressionArmSyntax : PocoCSharpSyntaxNode
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual PatternSyntax Pattern
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual WhenClauseSyntax WhenClause
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken EqualsGreaterThanToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual ExpressionSyntax Expression
            {
                get;
                set;
            }
        }

        class PocoSwitchExpressionSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoSwitchExpressionSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoSwitchExpressionSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoSwitchExpressionSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoSwitchExpressionSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoSwitchExpressionSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoSwitchExpressionSyntax>();
        }

        public class PocoSwitchExpressionSyntax : PocoExpressionSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual ExpressionSyntax GoverningExpression
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken SwitchKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken OpenBraceToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual Microsoft.CodeAnalysis.CSharp.Syntax.SwitchExpressionArmSyntax Arms
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken CloseBraceToken
            {
                get;
                set;
            }
        }

        class PocoSwitchLabelSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoSwitchLabelSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoSwitchLabelSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoSwitchLabelSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoSwitchLabelSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoSwitchLabelSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoSwitchLabelSyntax>();
        }

        public class PocoSwitchLabelSyntax : PocoCSharpSyntaxNode
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken Keyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken ColonToken
            {
                get;
                set;
            }
        }

        class PocoSwitchSectionSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoSwitchSectionSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoSwitchSectionSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoSwitchSectionSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoSwitchSectionSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoSwitchSectionSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoSwitchSectionSyntax>();
        }

        public class PocoSwitchSectionSyntax : PocoCSharpSyntaxNode
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual Microsoft.CodeAnalysis.CSharp.Syntax.SwitchLabelSyntax Labels
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual Microsoft.CodeAnalysis.CSharp.Syntax.StatementSyntax Statements
            {
                get;
                set;
            }
        }

        class PocoSwitchStatementSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoSwitchStatementSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoSwitchStatementSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoSwitchStatementSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoSwitchStatementSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoSwitchStatementSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoSwitchStatementSyntax>();
        }

        public class PocoSwitchStatementSyntax : PocoStatementSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax AttributeLists
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken SwitchKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken OpenParenToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual ExpressionSyntax Expression
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken CloseParenToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken OpenBraceToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual Microsoft.CodeAnalysis.CSharp.Syntax.SwitchSectionSyntax Sections
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken CloseBraceToken
            {
                get;
                set;
            }
        }

        class PocoThisExpressionSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoThisExpressionSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoThisExpressionSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoThisExpressionSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoThisExpressionSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoThisExpressionSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoThisExpressionSyntax>();
        }

        public class PocoThisExpressionSyntax : PocoInstanceExpressionSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken Token
            {
                get;
                set;
            }
        }

        class PocoThrowExpressionSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoThrowExpressionSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoThrowExpressionSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoThrowExpressionSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoThrowExpressionSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoThrowExpressionSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoThrowExpressionSyntax>();
        }

        public class PocoThrowExpressionSyntax : PocoExpressionSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken ThrowKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual ExpressionSyntax Expression
            {
                get;
                set;
            }
        }

        class PocoThrowStatementSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoThrowStatementSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoThrowStatementSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoThrowStatementSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoThrowStatementSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoThrowStatementSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoThrowStatementSyntax>();
        }

        public class PocoThrowStatementSyntax : PocoStatementSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax AttributeLists
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken ThrowKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual ExpressionSyntax Expression
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken SemicolonToken
            {
                get;
                set;
            }
        }

        class PocoTryStatementSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoTryStatementSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoTryStatementSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoTryStatementSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoTryStatementSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoTryStatementSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoTryStatementSyntax>();
        }

        public class PocoTryStatementSyntax : PocoStatementSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax AttributeLists
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken TryKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual BlockSyntax Block
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual Microsoft.CodeAnalysis.CSharp.Syntax.CatchClauseSyntax Catches
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual FinallyClauseSyntax Finally
            {
                get;
                set;
            }
        }

        class PocoTupleElementSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoTupleElementSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoTupleElementSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoTupleElementSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoTupleElementSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoTupleElementSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoTupleElementSyntax>();
        }

        public class PocoTupleElementSyntax : PocoCSharpSyntaxNode
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual TypeSyntax Type
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken Identifier
            {
                get;
                set;
            }
        }

        class PocoTupleExpressionSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoTupleExpressionSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoTupleExpressionSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoTupleExpressionSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoTupleExpressionSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoTupleExpressionSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoTupleExpressionSyntax>();
        }

        public class PocoTupleExpressionSyntax : PocoExpressionSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken OpenParenToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual Microsoft.CodeAnalysis.CSharp.Syntax.ArgumentSyntax Arguments
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken CloseParenToken
            {
                get;
                set;
            }
        }

        class PocoTupleTypeSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoTupleTypeSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoTupleTypeSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoTupleTypeSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoTupleTypeSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoTupleTypeSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoTupleTypeSyntax>();
        }

        public class PocoTupleTypeSyntax : PocoTypeSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken OpenParenToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual Microsoft.CodeAnalysis.CSharp.Syntax.TupleElementSyntax Elements
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken CloseParenToken
            {
                get;
                set;
            }
        }

        class PocoTypeArgumentListSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoTypeArgumentListSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoTypeArgumentListSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoTypeArgumentListSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoTypeArgumentListSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoTypeArgumentListSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoTypeArgumentListSyntax>();
        }

        public class PocoTypeArgumentListSyntax : PocoCSharpSyntaxNode
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken LessThanToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax Arguments
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken GreaterThanToken
            {
                get;
                set;
            }
        }

        class PocoTypeConstraintSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoTypeConstraintSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoTypeConstraintSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoTypeConstraintSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoTypeConstraintSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoTypeConstraintSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoTypeConstraintSyntax>();
        }

        public class PocoTypeConstraintSyntax : PocoTypeParameterConstraintSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual TypeSyntax Type
            {
                get;
                set;
            }
        }

        class PocoTypeCrefSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoTypeCrefSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoTypeCrefSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoTypeCrefSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoTypeCrefSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoTypeCrefSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoTypeCrefSyntax>();
        }

        public class PocoTypeCrefSyntax : PocoCrefSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual TypeSyntax Type
            {
                get;
                set;
            }
        }

        class PocoTypeDeclarationSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoTypeDeclarationSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoTypeDeclarationSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoTypeDeclarationSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoTypeDeclarationSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoTypeDeclarationSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoTypeDeclarationSyntax>();
        }

        public class PocoTypeDeclarationSyntax : PocoBaseTypeDeclarationSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken Keyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual TypeParameterListSyntax TypeParameterList
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterConstraintClauseSyntax ConstraintClauses
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax Members
            {
                get;
                set;
            }
        }

        class PocoTypeOfExpressionSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoTypeOfExpressionSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoTypeOfExpressionSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoTypeOfExpressionSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoTypeOfExpressionSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoTypeOfExpressionSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoTypeOfExpressionSyntax>();
        }

        public class PocoTypeOfExpressionSyntax : PocoExpressionSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken Keyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken OpenParenToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual TypeSyntax Type
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken CloseParenToken
            {
                get;
                set;
            }
        }

        class PocoTypeParameterConstraintClauseSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoTypeParameterConstraintClauseSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoTypeParameterConstraintClauseSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoTypeParameterConstraintClauseSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoTypeParameterConstraintClauseSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoTypeParameterConstraintClauseSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoTypeParameterConstraintClauseSyntax>();
        }

        public class PocoTypeParameterConstraintClauseSyntax : PocoCSharpSyntaxNode
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken WhereKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual IdentifierNameSyntax Name
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken ColonToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterConstraintSyntax Constraints
            {
                get;
                set;
            }
        }

        class PocoTypeParameterConstraintSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoTypeParameterConstraintSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoTypeParameterConstraintSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoTypeParameterConstraintSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoTypeParameterConstraintSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoTypeParameterConstraintSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoTypeParameterConstraintSyntax>();
        }

        public class PocoTypeParameterConstraintSyntax : PocoCSharpSyntaxNode
        {
        }

        class PocoTypeParameterListSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoTypeParameterListSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoTypeParameterListSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoTypeParameterListSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoTypeParameterListSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoTypeParameterListSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoTypeParameterListSyntax>();
        }

        public class PocoTypeParameterListSyntax : PocoCSharpSyntaxNode
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken LessThanToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterSyntax Parameters
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken GreaterThanToken
            {
                get;
                set;
            }
        }

        class PocoTypeParameterSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoTypeParameterSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoTypeParameterSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoTypeParameterSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoTypeParameterSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoTypeParameterSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoTypeParameterSyntax>();
        }

        public class PocoTypeParameterSyntax : PocoCSharpSyntaxNode
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax AttributeLists
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken VarianceKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken Identifier
            {
                get;
                set;
            }
        }

        class PocoTypeSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoTypeSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoTypeSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoTypeSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoTypeSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoTypeSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoTypeSyntax>();
        }

        public class PocoTypeSyntax : PocoExpressionSyntax
        {
        }

        class PocoUndefDirectiveTriviaSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoUndefDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoUndefDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoUndefDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoUndefDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoUndefDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoUndefDirectiveTriviaSyntax>();
        }

        public class PocoUndefDirectiveTriviaSyntax : PocoDirectiveTriviaSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override SyntaxToken HashToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken UndefKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken Name
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override SyntaxToken EndOfDirectiveToken
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

        class PocoUnsafeStatementSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoUnsafeStatementSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoUnsafeStatementSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoUnsafeStatementSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoUnsafeStatementSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoUnsafeStatementSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoUnsafeStatementSyntax>();
        }

        public class PocoUnsafeStatementSyntax : PocoStatementSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax AttributeLists
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken UnsafeKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual BlockSyntax Block
            {
                get;
                set;
            }
        }

        class PocoUsingDirectiveSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoUsingDirectiveSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoUsingDirectiveSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoUsingDirectiveSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoUsingDirectiveSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoUsingDirectiveSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoUsingDirectiveSyntax>();
        }

        public class PocoUsingDirectiveSyntax : PocoCSharpSyntaxNode
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken UsingKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual NameSyntax Name
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken SemicolonToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken StaticKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual NameEqualsSyntax Alias
            {
                get;
                set;
            }
        }

        class PocoUsingStatementSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoUsingStatementSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoUsingStatementSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoUsingStatementSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoUsingStatementSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoUsingStatementSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoUsingStatementSyntax>();
        }

        public class PocoUsingStatementSyntax : PocoStatementSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax AttributeLists
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken AwaitKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken UsingKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken OpenParenToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken CloseParenToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual StatementSyntax Statement
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual VariableDeclarationSyntax Declaration
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual ExpressionSyntax Expression
            {
                get;
                set;
            }
        }

        class PocoVariableDeclarationSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoVariableDeclarationSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoVariableDeclarationSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoVariableDeclarationSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoVariableDeclarationSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoVariableDeclarationSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoVariableDeclarationSyntax>();
        }

        public class PocoVariableDeclarationSyntax : PocoCSharpSyntaxNode
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual TypeSyntax Type
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual Microsoft.CodeAnalysis.CSharp.Syntax.VariableDeclaratorSyntax Variables
            {
                get;
                set;
            }
        }

        class PocoVariableDeclaratorSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoVariableDeclaratorSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoVariableDeclaratorSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoVariableDeclaratorSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoVariableDeclaratorSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoVariableDeclaratorSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoVariableDeclaratorSyntax>();
        }

        public class PocoVariableDeclaratorSyntax : PocoCSharpSyntaxNode
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken Identifier
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual BracketedArgumentListSyntax ArgumentList
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual EqualsValueClauseSyntax Initializer
            {
                get;
                set;
            }
        }

        class PocoVariableDesignationSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoVariableDesignationSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoVariableDesignationSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoVariableDesignationSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoVariableDesignationSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoVariableDesignationSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoVariableDesignationSyntax>();
        }

        public class PocoVariableDesignationSyntax : PocoCSharpSyntaxNode
        {
        }

        class PocoVarPatternSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoVarPatternSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoVarPatternSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoVarPatternSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoVarPatternSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoVarPatternSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoVarPatternSyntax>();
        }

        public class PocoVarPatternSyntax : PocoPatternSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken VarKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual VariableDesignationSyntax Designation
            {
                get;
                set;
            }
        }

        class PocoWarningDirectiveTriviaSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoWarningDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoWarningDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoWarningDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoWarningDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoWarningDirectiveTriviaSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoWarningDirectiveTriviaSyntax>();
        }

        public class PocoWarningDirectiveTriviaSyntax : PocoDirectiveTriviaSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override SyntaxToken HashToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken WarningKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override SyntaxToken EndOfDirectiveToken
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

        class PocoWhenClauseSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoWhenClauseSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoWhenClauseSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoWhenClauseSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoWhenClauseSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoWhenClauseSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoWhenClauseSyntax>();
        }

        public class PocoWhenClauseSyntax : PocoCSharpSyntaxNode
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken WhenKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual ExpressionSyntax Condition
            {
                get;
                set;
            }
        }

        class PocoWhereClauseSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoWhereClauseSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoWhereClauseSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoWhereClauseSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoWhereClauseSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoWhereClauseSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoWhereClauseSyntax>();
        }

        public class PocoWhereClauseSyntax : PocoQueryClauseSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken WhereKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual ExpressionSyntax Condition
            {
                get;
                set;
            }
        }

        class PocoWhileStatementSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoWhileStatementSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoWhileStatementSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoWhileStatementSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoWhileStatementSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoWhileStatementSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoWhileStatementSyntax>();
        }

        public class PocoWhileStatementSyntax : PocoStatementSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax AttributeLists
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken WhileKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken OpenParenToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual ExpressionSyntax Condition
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken CloseParenToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual StatementSyntax Statement
            {
                get;
                set;
            }
        }

        class PocoXmlAttributeSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoXmlAttributeSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoXmlAttributeSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoXmlAttributeSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoXmlAttributeSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoXmlAttributeSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoXmlAttributeSyntax>();
        }

        public class PocoXmlAttributeSyntax : PocoCSharpSyntaxNode
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual XmlNameSyntax Name
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken EqualsToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken StartQuoteToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken EndQuoteToken
            {
                get;
                set;
            }
        }

        class PocoXmlCDataSectionSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoXmlCDataSectionSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoXmlCDataSectionSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoXmlCDataSectionSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoXmlCDataSectionSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoXmlCDataSectionSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoXmlCDataSectionSyntax>();
        }

        public class PocoXmlCDataSectionSyntax : PocoXmlNodeSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken StartCDataToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual Microsoft.CodeAnalysis.SyntaxToken TextTokens
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken EndCDataToken
            {
                get;
                set;
            }
        }

        class PocoXmlCommentSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoXmlCommentSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoXmlCommentSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoXmlCommentSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoXmlCommentSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoXmlCommentSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoXmlCommentSyntax>();
        }

        public class PocoXmlCommentSyntax : PocoXmlNodeSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken LessThanExclamationMinusMinusToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual Microsoft.CodeAnalysis.SyntaxToken TextTokens
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken MinusMinusGreaterThanToken
            {
                get;
                set;
            }
        }

        class PocoXmlCrefAttributeSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoXmlCrefAttributeSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoXmlCrefAttributeSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoXmlCrefAttributeSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoXmlCrefAttributeSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoXmlCrefAttributeSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoXmlCrefAttributeSyntax>();
        }

        public class PocoXmlCrefAttributeSyntax : PocoXmlAttributeSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override XmlNameSyntax Name
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override SyntaxToken EqualsToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override SyntaxToken StartQuoteToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual CrefSyntax Cref
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override SyntaxToken EndQuoteToken
            {
                get;
                set;
            }
        }

        class PocoXmlElementEndTagSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoXmlElementEndTagSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoXmlElementEndTagSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoXmlElementEndTagSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoXmlElementEndTagSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoXmlElementEndTagSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoXmlElementEndTagSyntax>();
        }

        public class PocoXmlElementEndTagSyntax : PocoCSharpSyntaxNode
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken LessThanSlashToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual XmlNameSyntax Name
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken GreaterThanToken
            {
                get;
                set;
            }
        }

        class PocoXmlElementStartTagSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoXmlElementStartTagSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoXmlElementStartTagSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoXmlElementStartTagSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoXmlElementStartTagSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoXmlElementStartTagSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoXmlElementStartTagSyntax>();
        }

        public class PocoXmlElementStartTagSyntax : PocoCSharpSyntaxNode
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken LessThanToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual XmlNameSyntax Name
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual Microsoft.CodeAnalysis.CSharp.Syntax.XmlAttributeSyntax Attributes
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken GreaterThanToken
            {
                get;
                set;
            }
        }

        class PocoXmlElementSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoXmlElementSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoXmlElementSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoXmlElementSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoXmlElementSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoXmlElementSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoXmlElementSyntax>();
        }

        public class PocoXmlElementSyntax : PocoXmlNodeSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual XmlElementStartTagSyntax StartTag
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual Microsoft.CodeAnalysis.CSharp.Syntax.XmlNodeSyntax Content
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual XmlElementEndTagSyntax EndTag
            {
                get;
                set;
            }
        }

        class PocoXmlEmptyElementSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoXmlEmptyElementSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoXmlEmptyElementSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoXmlEmptyElementSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoXmlEmptyElementSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoXmlEmptyElementSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoXmlEmptyElementSyntax>();
        }

        public class PocoXmlEmptyElementSyntax : PocoXmlNodeSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken LessThanToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual XmlNameSyntax Name
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual Microsoft.CodeAnalysis.CSharp.Syntax.XmlAttributeSyntax Attributes
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken SlashGreaterThanToken
            {
                get;
                set;
            }
        }

        class PocoXmlNameAttributeSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoXmlNameAttributeSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoXmlNameAttributeSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoXmlNameAttributeSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoXmlNameAttributeSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoXmlNameAttributeSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoXmlNameAttributeSyntax>();
        }

        public class PocoXmlNameAttributeSyntax : PocoXmlAttributeSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override XmlNameSyntax Name
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override SyntaxToken EqualsToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override SyntaxToken StartQuoteToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual IdentifierNameSyntax Identifier
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override SyntaxToken EndQuoteToken
            {
                get;
                set;
            }
        }

        class PocoXmlNameSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoXmlNameSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoXmlNameSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoXmlNameSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoXmlNameSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoXmlNameSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoXmlNameSyntax>();
        }

        public class PocoXmlNameSyntax : PocoCSharpSyntaxNode
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual XmlPrefixSyntax Prefix
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken LocalName
            {
                get;
                set;
            }
        }

        class PocoXmlNodeSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoXmlNodeSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoXmlNodeSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoXmlNodeSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoXmlNodeSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoXmlNodeSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoXmlNodeSyntax>();
        }

        public class PocoXmlNodeSyntax : PocoCSharpSyntaxNode
        {
        }

        class PocoXmlPrefixSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoXmlPrefixSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoXmlPrefixSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoXmlPrefixSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoXmlPrefixSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoXmlPrefixSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoXmlPrefixSyntax>();
        }

        public class PocoXmlPrefixSyntax : PocoCSharpSyntaxNode
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken Prefix
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken ColonToken
            {
                get;
                set;
            }
        }

        class PocoXmlProcessingInstructionSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoXmlProcessingInstructionSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoXmlProcessingInstructionSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoXmlProcessingInstructionSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoXmlProcessingInstructionSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoXmlProcessingInstructionSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoXmlProcessingInstructionSyntax>();
        }

        public class PocoXmlProcessingInstructionSyntax : PocoXmlNodeSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken StartProcessingInstructionToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual XmlNameSyntax Name
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual Microsoft.CodeAnalysis.SyntaxToken TextTokens
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken EndProcessingInstructionToken
            {
                get;
                set;
            }
        }

        class PocoXmlTextAttributeSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoXmlTextAttributeSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoXmlTextAttributeSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoXmlTextAttributeSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoXmlTextAttributeSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoXmlTextAttributeSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoXmlTextAttributeSyntax>();
        }

        public class PocoXmlTextAttributeSyntax : PocoXmlAttributeSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override XmlNameSyntax Name
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override SyntaxToken EqualsToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override SyntaxToken StartQuoteToken
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual Microsoft.CodeAnalysis.SyntaxToken TextTokens
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override SyntaxToken EndQuoteToken
            {
                get;
                set;
            }
        }

        class PocoXmlTextSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoXmlTextSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoXmlTextSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoXmlTextSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoXmlTextSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoXmlTextSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoXmlTextSyntax>();
        }

        public class PocoXmlTextSyntax : PocoXmlNodeSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual Microsoft.CodeAnalysis.SyntaxToken TextTokens
            {
                get;
                set;
            }
        }

        class PocoYieldStatementSyntaxCollection : IList, IEnumerable, ICollection
        {
            public // System.Collections.IList
            Int32 Add(Object value) => _list.Add((PocoYieldStatementSyntax)value);
            public // System.Collections.IList
            Boolean Contains(Object value) => _list.Contains((PocoYieldStatementSyntax)value);
            public // System.Collections.IList
            void Clear() => _list.Clear();
            public // System.Collections.IList
            Int32 IndexOf(Object value) => _list.IndexOf((PocoYieldStatementSyntax)value);
            public // System.Collections.IList
            void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoYieldStatementSyntax)value);
            public // System.Collections.IList
            void Remove(Object value) => _list.Remove((PocoYieldStatementSyntax)value);
            public // System.Collections.IList
            void RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
            public Boolean IsReadOnly => _list.IsReadOnly;
            public Boolean IsFixedSize => _list.IsFixedSize;
            public Object this[Int32 index]
            {
                get => _list[index];
                set => _list[index] = value;
            }

            public // System.Collections.ICollection
            void CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
            public Int32 Count => _list.Count;
            public Object SyncRoot => _list.SyncRoot;
            public Boolean IsSynchronized => _list.IsSynchronized;
            public // System.Collections.IEnumerable
            IEnumerator GetEnumerator() => _list.GetEnumerator();
            IList _list = new List<PocoYieldStatementSyntax>();
        }

        public class PocoYieldStatementSyntax : PocoStatementSyntax
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public override Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax AttributeLists
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken YieldKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken ReturnOrBreakKeyword
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual ExpressionSyntax Expression
            {
                get;
                set;
            }

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
            public virtual SyntaxToken SemicolonToken
            {
                get;
                set;
            }
        }
    }