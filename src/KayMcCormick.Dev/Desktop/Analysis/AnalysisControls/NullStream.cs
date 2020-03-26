using System.IO ;

namespace AnalysisControls
{
    public class NullStream : Stream
    {
        public override bool CanRead { get { return true ; } }

        public override bool CanSeek { get { return true ; } }

        public override bool CanWrite { get { return true ; } }

        public override void Flush ( ) { }

        public override long Length { get { return 0 ; } }


        public override long Position { get { return 0 ; } set { } }

        public override int Read ( byte[] buffer , int offset , int count )
        {
            for ( var i = 0 ; i < buffer.Length ; i ++ )
            {
                buffer[ i ] = 0 ;
            }

            return count ;
        }

        public override long Seek ( long offset , SeekOrigin origin ) { return 0 ; }

        public override void SetLength ( long value ) { }

        public override void Write ( byte[] buffer , int offset , int count ) { }
    }
}