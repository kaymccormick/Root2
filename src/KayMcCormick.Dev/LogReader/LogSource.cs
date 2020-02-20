using System.Runtime.InteropServices.ComTypes ;
using Microsoft.VisualStudio.Shell ;

namespace LogReader
{
    public interface ILogEntryProvider
    {
    }

    // ReSharper disable once UnusedType.Global
    public class Misc
    {
        // ReSharper disable once NotAccessedField.Local
#pragma warning disable IDE0052 // Remove unread private members
        // private readonly OleDataObject o ;
#pragma warning restore IDE0052 // Remove unread private members
        // public Misc ( OleDataObject o ) { this.o = o ; }
    }
    // ReSharper disable once UnusedType.Global
    public abstract class DataObject : IDataObject
    {
        public abstract void GetData ( ref      FORMATETC format , out STGMEDIUM medium ) ;
        public abstract void GetDataHere ( ref  FORMATETC format , ref STGMEDIUM medium ) ;
        public abstract int  QueryGetData ( ref FORMATETC format ) ;

        public abstract int GetCanonicalFormatEtc (
            ref FORMATETC formatIn
          , out FORMATETC formatOut
        ) ;

        public abstract void SetData (
            ref FORMATETC formatIn
          , ref STGMEDIUM medium
          , bool          release
        ) ;

        public abstract IEnumFORMATETC EnumFormatEtc ( DATADIR direction ) ;

        public abstract int DAdvise (
            ref FORMATETC pFormatetc
          , ADVF          advf
          , IAdviseSink   adviseSink
          , out int       connection
        ) ;

        public abstract void DUnadvise ( int                 connection ) ;
        public abstract int  EnumDAdvise ( out IEnumSTATDATA enumAdvise ) ;
    }


    // ReSharper disable once UnusedType.Global
    public class OleDataSourceLogEntryProvider : ILogEntryProvider , IDataObject
    {
        private readonly IDataObject _dataObjectImplementation ;
        public OleDataSourceLogEntryProvider ( IDataObject dataObjectImplementation )
        {
            _dataObjectImplementation = dataObjectImplementation ;
        }

        public void GetData ( ref FORMATETC format , out STGMEDIUM medium )
        {
            _dataObjectImplementation.GetData ( ref format , out medium ) ;
        }


        public void GetDataHere ( ref FORMATETC format , ref STGMEDIUM medium )
        {
            _dataObjectImplementation.GetDataHere ( ref format , ref medium ) ;
        }


        public int QueryGetData ( ref FORMATETC format )
        {
            return _dataObjectImplementation.QueryGetData ( ref format ) ;
        }


        public int GetCanonicalFormatEtc ( ref FORMATETC formatIn , out FORMATETC formatOut )
        {
            return _dataObjectImplementation.GetCanonicalFormatEtc (
                                                                    ref formatIn
                                                                  , out formatOut
                                                                   ) ;
        }


        public void SetData ( ref FORMATETC formatIn , ref STGMEDIUM medium , bool release )
        {
            _dataObjectImplementation.SetData ( ref formatIn , ref medium , release ) ;
        }


        public IEnumFORMATETC EnumFormatEtc ( DATADIR direction )
        {
            return _dataObjectImplementation.EnumFormatEtc ( direction ) ;
        }


        public int DAdvise (
            ref FORMATETC pFormatetc
          , ADVF          advf
          , IAdviseSink   adviseSink
          , out int       connection
        )
        {
            return _dataObjectImplementation.DAdvise (
                                                      ref pFormatetc
                                                    , advf
                                                    , adviseSink
                                                    , out connection
                                                     ) ;
        }


        public void DUnadvise ( int connection )
        {
            _dataObjectImplementation.DUnadvise ( connection ) ;
        }


        public int EnumDAdvise ( out IEnumSTATDATA enumAdvise )
        {
            return _dataObjectImplementation.EnumDAdvise ( out enumAdvise ) ;
        }
    }
}