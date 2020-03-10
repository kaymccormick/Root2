﻿#region header
// Kay McCormick (mccor)
// 
// WpfApp2
// WpfApp2
// PropInfo.cs
// 
// 2020-02-10-7:38 PM
//
// ---
#endregion
namespace WpfApp2
{
    /// <summary></summary>
    /// <autogeneratedoc />
    /// TODO Edit XML Comment Template for PropInfo
    public class PropInfo
    {
        private LogProperty _logProperty ;

        /// <summary>Initializes a new instance of the <see cref="System.Object" /> class.</summary>
        public PropInfo ( LogProperty logProperty )
        {
            _logProperty = logProperty ;
            Name = logProperty.Name ;
        }


        public PropInfo ( ) {
        }

        public int Count { get ; set ; }

        // ReSharper disable once UnusedMember.Global
        public LogProperty LogProperty
        {
            get => _logProperty ;
            set
            {
                if ( _logProperty != value )
                {
                    Name = value?.Name ;
                }

                _logProperty = value;
            }

        }

        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public string Name { get ; set ; }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString ( ) { return $"{nameof ( Count )}: {Count}" ; }
    }
}