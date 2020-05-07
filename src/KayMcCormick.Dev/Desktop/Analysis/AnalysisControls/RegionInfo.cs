using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media.TextFormatting;
using JetBrains.Annotations;
using Microsoft.CodeAnalysis;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class RegionInfo : INotifyPropertyChanged
    {
        private int _offset;
        private int _length;
        private SyntaxToken? _syntaxToken;
        private SyntaxNode _syntaxNode;
        private SyntaxTrivia? _trivia = default;
        private TextRun _textRun;
        private Rect _boundingRect;
        private List<CharacterCell> _characters;

        public int Offset
        {
            get { return _offset; }
            set
            {
                if (value == _offset) return;
                _offset = value;
                OnPropertyChanged();
            }
        }

        public int Length
        {
            get { return _length; }
            set
            {
                if (value == _length) return;
                _length = value;
                OnPropertyChanged();
            }
        }

        public SyntaxToken? SyntaxToken
        {
            get { return _syntaxToken; }
            set
            {
                if (Nullable.Equals(value, _syntaxToken)) return;
                _syntaxToken = value;
                OnPropertyChanged();
            }
        }

        public SyntaxNode SyntaxNode
        {
            get { return _syntaxNode; }
            set
            {
                if (Equals(value, _syntaxNode)) return;
                _syntaxNode = value;
                OnPropertyChanged();
            }
        }

        public SyntaxTrivia TriviaValue => Trivia.GetValueOrDefault();
        public SyntaxTrivia? Trivia
        {
            get { return _trivia; }
            set
            {
                if (Nullable.Equals(value, _trivia)) return;
                _trivia = value;
                OnPropertyChanged();
                OnPropertyChanged("TriviaValue");
            }
        }

        public TextRun TextRun
        {
            get { return _textRun; }
            set
            {
                if (Equals(value, _textRun)) return;
                _textRun = value;
                OnPropertyChanged();
            }
        }

        public Rect BoundingRect
        {
            get { return _boundingRect; }
            set
            {
                if (value.Equals(_boundingRect)) return;
                _boundingRect = value;
                OnPropertyChanged();
            }
        }

        public List<CharacterCell> Characters       
        {
            get { return _characters; }
            set
            {
                if (Equals(value, _characters)) return;
                _characters = value;
                OnPropertyChanged();
            }
        }

        public string Key { get; set; }

        public RegionInfo(TextRun textRun, Rect boundingRect, List<CharacterCell> characters)
        {
            TextRun = textRun;
            BoundingRect = new Rect((int)boundingRect.X, (int)boundingRect.Y, (int)boundingRect.Width, (int)boundingRect.Height);
            Characters = characters;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}