using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Mihailik.InternetExplorer
{
    internal sealed class PluggableProtocolResponseOutputStream : Stream
    {
        readonly List<byte[]> buffers = new List<byte[]>();

        const int BufferSize = 1024*8;

        int totalWrittenCount;
        int totalReadCount;

        bool isClosed;

        readonly object sync = new object();

        public event EventHandler Written;

        #region Required 'empty' overrides

        public override bool CanRead
        {
            get { return false; ; }
        }

        public override bool CanSeek
        {
            get { return false; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override void Flush()
        {
        }

        public override long Length
        {
            get { throw new NotSupportedException(); }
        }

        public override long Position
        {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        #endregion

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (isClosed)
                throw new ObjectDisposedException("OutputStream");

            if( buffer == null )
                throw new ArgumentNullException("buffer");
            if( offset<0 )
                throw new ArgumentOutOfRangeException("offset");
            if( count<=0 || offset+count>buffer.Length )
                throw new ArgumentOutOfRangeException("count");

            lock (sync)
            {
                int writtenCount = 0;

                int removedBufferCount = totalReadCount / BufferSize;
                int writtenWholeBufferCount = totalWrittenCount / BufferSize;

                int freeSpaceInLastBuffer = totalWrittenCount - writtenWholeBufferCount * BufferSize;

                int writeToFreeSpaceCount = Math.Min(count, freeSpaceInLastBuffer);
                if (writeToFreeSpaceCount > 0)
                {
                    Array.Copy(
                        buffer, offset,
                        buffers[buffers.Count - 1], BufferSize - freeSpaceInLastBuffer,
                        writeToFreeSpaceCount);

                    writtenCount += writeToFreeSpaceCount;
                    totalWrittenCount += writeToFreeSpaceCount;
                }

                while (writtenCount < count)
                {
                    buffers.Add(new byte[BufferSize]);

                    int writeCount = Math.Min(count - writtenCount, BufferSize);

                    Array.Copy(
                        buffer, offset + writtenCount,
                        buffers[buffers.Count - 1], 0,
                        writeCount);

                    writtenCount += writeCount;
                    totalWrittenCount += writeCount;
                }
            }

            EventHandler temp = this.Written;
            if (temp != null)
            {
                temp(this,EventArgs.Empty);
            }
        }

        internal int ReadToMemory(IntPtr memory, int count)
        {
            lock (sync)
            {
                int readCount = 0;
                int maxReadCount = totalWrittenCount - totalReadCount;

                
                int readWholeBufferCount = totalReadCount/BufferSize;

                int leftInFirstBuffer = totalReadCount - readWholeBufferCount * BufferSize;

                int readFromFirstBuffer = Math.Min(maxReadCount, leftInFirstBuffer);

                if (readFromFirstBuffer > 0)
                {
                    Marshal.Copy(
                        buffers[0], BufferSize - leftInFirstBuffer,
                        memory,
                        readFromFirstBuffer);

                    readCount += readFromFirstBuffer;
                    totalReadCount += readFromFirstBuffer;

                    if (readFromFirstBuffer == leftInFirstBuffer)
                    {
                        buffers.RemoveAt(0);
                    }
                }

                while (readCount < maxReadCount)
                {
                    int readFromBuffer = Math.Min(maxReadCount - readCount, BufferSize);

                    Marshal.Copy(
                        buffers[0], 0,
                        new IntPtr(memory.ToInt64() + readCount),
                        readFromBuffer);

                    readCount += readFromBuffer;
                    totalReadCount += readFromBuffer;

                    if (readFromBuffer == BufferSize)
                        buffers.RemoveAt(0);
                }

                return readCount;
            }
        }

        internal void SetClosed()
        {
            lock (sync)
            {
                isClosed = true;
            }
        }
    }
}
