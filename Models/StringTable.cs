using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace StringTableEditorCS.Models
{
    public class StringTable
    {
        public List<StringTableEntry> Entries { get; private set; } = new List<StringTableEntry>();

        public void LoadTableFromFiles(string tablePath, string dataPath)
        {
            Entries.Clear();

            List<int> endingsTable = new List<int>();

            using (FileStream tableStream = File.OpenRead(tablePath))
            using (BinaryReader tableReader = new BinaryReader(tableStream))
            {
                while (tableStream.Position < tableStream.Length)
                {
                    byte[] bytes = tableReader.ReadBytes(4);
                    if (bytes.Length < 4) break;
                    
                    // Big Endian conversion
                    Array.Reverse(bytes);
                    int endingPos = BitConverter.ToInt32(bytes, 0);

                    if (endingPos < 0)
                    {
                        throw new Exception($"Negative ending position: {endingPos}");
                    }
                    endingsTable.Add(endingPos);
                }
            }

            using (FileStream dataStream = File.OpenRead(dataPath))
            using (BinaryReader dataReader = new BinaryReader(dataStream))
            {
                int pos = 0;
                for (int i = 0; i < endingsTable.Count; i++)
                {
                    int endingPos = endingsTable[i];

                    if (pos > 0 && endingPos == 0)
                    {
                        // Zero values are used for empty entries at the end
                        break;
                    }
                    else if (endingPos < pos)
                    {
                        throw new Exception($"File string positions not in ascending order ({endingPos} < {pos})");
                    }

                    int length = endingPos - pos;
                    byte[] byteArray = dataReader.ReadBytes(length);

                    Entries.Add(new StringTableEntry(i, byteArray));

                    pos = endingPos;
                }
            }
        }

        public void SaveTableToFiles(string tablePath, string dataPath)
        {
            using (FileStream tableStream = File.Create(tablePath))
            using (BinaryWriter tableWriter = new BinaryWriter(tableStream))
            using (FileStream dataStream = File.Create(dataPath))
            using (BinaryWriter dataWriter = new BinaryWriter(dataStream))
            {
                int pos = 0;
                foreach (var entry in Entries)
                {
                    byte[] rawBytes = entry.Encode();
                    dataWriter.Write(rawBytes);

                    pos += rawBytes.Length;

                    byte[] offsetBytes = BitConverter.GetBytes(pos);
                    Array.Reverse(offsetBytes); // Big Endian
                    tableWriter.Write(offsetBytes);
                }
                
                // Optional: Fill remaining slots with 0 if needed, 
                // but for a basic version we'll just save what we have.
            }
        }
    }
}
