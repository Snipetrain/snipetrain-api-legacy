﻿using System;
using System.Collections.Generic;
using System.Text;

namespace srcds_control_api.Utilities.ServerQuery
{
    public class ResponseParser
    {
        private readonly byte[] _bytes;

        public ResponseParser(byte[] bytes)
        {
            _bytes = bytes;
        }

        public byte GetByte()
        {
            return _bytes[CurrentPosition++];
        }

        public bool BytesLeft
        {
            get { return _bytes.Length - 1 >= CurrentPosition; }
        }

        public string GetStringToTermination()
        {
            var buffer = new List<byte>();
            for (; CurrentPosition < _bytes.Length; CurrentPosition++)
            {
                var b = _bytes[CurrentPosition];
                if (b != 0x00)
                {
                    buffer.Add(b);
                }
                else
                {
                    CurrentPosition++;
                    break;
                }
            }
            return Encoding.ASCII.GetString(buffer.ToArray());
        }

        public Int32 GetShort()
        {
            return BitConverter.ToUInt16(new[]
            {
                _bytes[CurrentPosition++],
                _bytes[CurrentPosition++]
            }, 0);
        }

        public string GetStringOfByte()
        {
            return Encoding.ASCII.GetString(new[] { _bytes[CurrentPosition++] });
        }

        public int CurrentPosition { get; set; }

    }
}