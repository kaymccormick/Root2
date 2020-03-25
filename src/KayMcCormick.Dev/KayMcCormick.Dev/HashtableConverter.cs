﻿#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ProjInterface
// HashtableConverter.cs
// 
// 2020-03-20-3:58 AM
// 
// ---
#endregion
using System ;
using System.Collections ;
using System.Text.Json ;
using System.Text.Json.Serialization ;

namespace KayMcCormick.Dev
{
    /// <summary>
    /// 
    /// </summary>
    public class HashtableConverter : JsonConverter < Hashtable >
    {
        #region Overrides of JsonConverter<Hashtable>
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="typeToConvert"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public override Hashtable Read (
            ref Utf8JsonReader    reader
          , Type                  typeToConvert
          , JsonSerializerOptions options
        )
        {
            return null ;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="options"></param>
        public override void Write (
            Utf8JsonWriter        writer
          , Hashtable             value
          , JsonSerializerOptions options
        )
        {
            writer.WriteStartObject ( ) ;
            foreach ( var q in value.Keys )
            {
                writer.WritePropertyName ( q.ToString ( ) ) ;
                JsonSerializer.Serialize ( writer , value[ q ] , options ) ;
            }

            writer.WriteEndObject ( ) ;
        }
        #endregion
    }
}