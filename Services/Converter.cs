using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StringTableEditorCS.Services
{
    public static class Converter
    {
        public static readonly Dictionary<byte, string> SpecialCharacters = new Dictionary<byte, string>
        {
            { 0x00, "¡" }, { 0x01, "¿" }, { 0x02, "Ä" }, { 0x03, "À" }, { 0x04, "Á" },
            { 0x05, "Â" }, { 0x06, "Ã" }, { 0x07, "Å" }, { 0x08, "Ç" }, { 0x09, "È" },
            { 0x0A, "É" }, { 0x0B, "Ê" }, { 0x0C, "Ë" }, { 0x0D, "Ì" }, { 0x0E, "Í" },
            { 0x0F, "Î" }, { 0x10, "Ï" }, { 0x11, "Ð" }, { 0x12, "Ñ" }, { 0x13, "Ò" },
            { 0x14, "Ó" }, { 0x15, "Ô" }, { 0x16, "Õ" }, { 0x17, "Ö" }, { 0x18, "Ø" },
            { 0x19, "Ù" }, { 0x1A, "Ú" }, { 0x1B, "Û" }, { 0x1C, "Ü" }, { 0x1D, "ß" },
            { 0x1E, "Þ" }, { 0x1F, "à" }, { 0x23, "á" }, { 0x24, "â" }, { 0x2A, "~" },
            { 0x2B, "♥" }, { 0x2F, "♪" }, { 0x3B, "🌢" }, { 0x5B, "ã" }, { 0x5C, "💢" },
            { 0x5D, "ä" }, { 0x5E, "å" }, { 0x60, "ç" }, { 0x7B, "è" }, { 0x7C, "é" },
            { 0x7D, "ê" }, { 0x7E, "ë" }, { 0x81, "ì" }, { 0x82, "í" }, { 0x83, "î" },
            { 0x84, "ï" }, { 0x85, "·" }, { 0x86, "ð" }, { 0x87, "ñ" }, { 0x88, "ò" },
            { 0x89, "ó" }, { 0x8A, "ô" }, { 0x8B, "õ" }, { 0x8C, "ö" }, { 0x8D, "ø" },
            { 0x8E, "ù" }, { 0x8F, "ú" }, { 0x90, "—" }, { 0x91, "û" }, { 0x92, "ü" },
            { 0x93, "ý" }, { 0x94, "ÿ" }, { 0x95, "þ" }, { 0x96, "Ý" }, { 0x97, "¦" },
            { 0x98, "§" }, { 0x99, "ª" }, { 0x9A, "º" }, { 0x9B, "‖" }, { 0x9C, "µ" },
            { 0x9D, "³" }, { 0x9E, "²" }, { 0x9F, "¹" }, { 0xA0, "¯" }, { 0xA1, "¬" },
            { 0xA2, "Æ" }, { 0xA3, "æ" }, { 0xA4, "‥" }, { 0xA5, "»" }, { 0xA6, "«" },
            { 0xA7, "☀" }, { 0xA8, "☁" }, { 0xA9, "☂" }, { 0xAA, "🌀" }, { 0xAB, "⛄" },
            { 0xAC, "⚞" }, { 0xAD, "⚟" }, { 0xAE, "／" }, { 0xAF, "∞" }, { 0xB0, "○" },
            { 0xB1, "╳" }, { 0xB2, "□" }, { 0xB3, "△" }, { 0xB4, "＋" }, { 0xB5, "⚡" },
            { 0xB6, "♂" }, { 0xB7, "♁" }, { 0xB8, "✿" }, { 0xB9, "★" }, { 0xBA, "💀" },
            { 0xBB, "😮" }, { 0xBC, "😊" }, { 0xBD, "😢" }, { 0xBE, "😠" }, { 0xBF, "😃" },
            { 0xC0, "×" }, { 0xC1, "÷" }, { 0xC2, "🔨" }, { 0xC3, "🎀" }, { 0xC4, "✉" },
            { 0xC5, "💰" }, { 0xC6, "🐾" }, { 0xC7, "🐶" }, { 0xC8, "🐱" }, { 0xC9, "🐰" },
            { 0xCA, "🐔" }, { 0xCB, "🐮" }, { 0xCC, "🐷" }, { 0xCD, "\n" }, { 0xCE, "🐟" },
            { 0xCF, "🐞" }, { 0xD0, ";" }, { 0xD1, "#" }, { 0xD2, "░" }, { 0xD3, "▓" },
            { 0xD4, "🔑" }, { 0xD5, "“" }, { 0xD6, "”" }, { 0xD7, "‘" }, { 0xD8, "’" },
            { 0xD9, "Œ" }, { 0xDA, "œ" }, { 0xDB, "☱" }, { 0xDC, "☲" }, { 0xDD, "☴" },
            { 0xDE, "\\" }
        };

        public static readonly Dictionary<string, byte> ReverseSpecialCharacters = SpecialCharacters.ToDictionary(x => x.Value, x => x.Key);

        public static string Decode(byte[] bytes)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                byte b = bytes[i];
                if (SpecialCharacters.TryGetValue(b, out var specialCharacter))
                {
                    sb.Append(specialCharacter);
                }
                else
                {
                    sb.Append((char)b);
                }
            }
            return sb.ToString();
        }

        public static byte[] Encode(string text)
        {
            List<byte> bytes = new List<byte>();
            for (int i = 0; i < text.Length; i++)
            {
                // Check for surrogate pairs (multi-char strings in our dictionary)
                bool found = false;
                foreach (var kvp in ReverseSpecialCharacters)
                {
                    if (text.Substring(i).StartsWith(kvp.Key))
                    {
                        bytes.Add(kvp.Value);
                        i += kvp.Key.Length - 1;
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    bytes.Add((byte)text[i]);
                }
            }
            return bytes.ToArray();
        }
    }
}
