using System;
using System.Collections.Generic;
using System.Text;
using StringTableEditorCS.Services;

namespace StringTableEditorCS.Models
{
    public class StringTableEntry
    {
        public int Id { get; set; }
        public byte[] RawBytes { get; set; }
        public string Content { get; set; }

        public StringTableEntry(int id, byte[] rawBytes)
        {
            Id = id;
            RawBytes = rawBytes;
            Content = DecodeMessage(rawBytes);
        }

        public override string ToString()
        {
            return $"{Id}: {Content}";
        }

        private string DecodeMessage(byte[] bytes)
        {
            StringBuilder result = new StringBuilder();
            List<byte> plainBytes = new List<byte>();

            for (int i = 0; i < bytes.Length; i++)
            {
                byte b = bytes[i];

                // 0x7F or 0x80 are processor codes
                if ((b == 0x7F || b == 0x80) && i + 1 < bytes.Length)
                {
                    // Flush plain bytes
                    if (plainBytes.Count > 0)
                    {
                        result.Append(Converter.Decode(plainBytes.ToArray()));
                        plainBytes.Clear();
                    }

                    byte code = bytes[i + 1];
                    int size = GetProcessorSize(code);

                    if (i + size <= bytes.Length)
                    {
                        byte[] procData = new byte[size];
                        Array.Copy(bytes, i, procData, 0, size);
                        result.Append(DecodeProcessor(procData));
                        i += size - 1;
                    }
                    else
                    {
                        // Fallback if not enough bytes
                        plainBytes.Add(b);
                    }
                }
                else
                {
                    plainBytes.Add(b);
                }
            }

            if (plainBytes.Count > 0)
            {
                result.Append(Converter.Decode(plainBytes.ToArray()));
            }

            return result.ToString();
        }

        private int GetProcessorSize(byte code)
        {
            // Based on ProcessorDefinitions.kt
            switch (code)
            {
                case 0x03: return 3; // PAUSE
                case 0x05: return 5; // COLOR_LINE
                case 0x51: return 3; // SET_COLOR_CHAR
                case 0x53: return 3; // SET_LINE_OFFSET
                case 0x54: return 3; // SET_LINE_TYPE
                case 0x55: return 3; // SET_CHAR_SCALE
                case 0x5B: return 3; // SET_LINE_SCALE
                // Add more as needed, default is 2 for placeholders
                default: return 2;
            }
        }

        private string DecodeProcessor(byte[] data)
        {
            byte code = data[1];
            string name = GetProcessorName(code);
            
            if (data.Length == 2)
            {
                return $"{{{{{name}}}}}";
            }
            else
            {
                string hexData = BitConverter.ToString(data, 2).Replace("-", "");
                return $"{{{{{name}:{hexData}}}}}";
            }
        }

        private string GetProcessorName(byte code)
        {
            // Simplified mapping
            switch (code)
            {
                case 0x00: return "LAST";
                case 0x01: return "CONTINUE";
                case 0x02: return "CLEAR";
                case 0x03: return "PAUSE";
                case 0x04: return "BUTTON";
                case 0x05: return "COLOR_LINE";
                case 0x1B: return "PLAYER_NAME";
                case 0xCD: return "NEWLINE"; // This is actually handled by Converter but just in case
                default: return $"CODE_{code:X2}";
            }
        }

        public byte[] Encode()
        {
            // This is complex because of tags. For a simple version, we'll just encode the text.
            // Full implementation would need a regex to parse {{TAG:DATA}}
            return Converter.Encode(Content);
        }
    }
}
