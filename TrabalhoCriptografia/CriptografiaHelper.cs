using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.RegularExpressions;

namespace TrabalhoCriptografia
{
    public class CriptografiaHelper
    {
        private static readonly Dictionary<int, char> intToCharDictionary = new Dictionary<int, char>()
        {
            {0, 'a' },
            {1, 'b' },
            {2, 'c' },
            {3, 'd' },
            {4, 'e' },
            {5, 'f' },
            {6, 'g' },
            {7, 'h' },
            {8, 'i' },
            {9, 'j' },
            {10, 'k' },
            {11, 'l' },
            {12, 'm' },
            {13, 'n' },
            {14, 'o' },
            {15, 'p' },
        };

        private static readonly Dictionary<char, int> charToIntDictionary = intToCharDictionary.ToDictionary((i) => i.Value, (i) => i.Key);

        public string Criptografar(string texto, string key)
        {

            var bits = ConvertStringToBitArray(texto);

            var bitsEmbaralhados = EmbaralharUsandoKey(bits, key);

            var charArray = Regex.Split(new string(bitsEmbaralhados.ToArray()), "(.{4})")
                .Where(quatroBitsString => !string.IsNullOrEmpty(quatroBitsString))
                .Select(binaryString => intToCharDictionary[BinaryStringToInt(binaryString)]).ToArray();

            return new string(charArray);
        }

        public string Descriptografar(string texto, string key)
        {
            var byteArray = texto.ToCharArray()
                .Select(caractere => IntTo4bitsString(charToIntDictionary[caractere]));

            var byteString = string.Join("", byteArray);

            var bitsDesembaralhados = DesembaralharUsandoKey(byteString.ToCharArray().ToList(), key);

            var stringOriginal = ConvertBinaryToString(bitsDesembaralhados);

            return stringOriginal;
        }


        private static int BinaryStringToInt(string binaryString)
        {
            return Convert.ToInt32(binaryString.ToString(), 2);
        }

        private static string IntTo4bitsString(int @int)
        {
            return Convert.ToString(@int, 2).PadLeft(4, '0');
        }

        private List<char> ConvertStringToBitArray(string text)
        {
            var bits = Encoding.UTF8.GetBytes(text)
                .SelectMany(@byte => Convert.ToString(@byte, 2).PadLeft(8, '0').ToCharArray());

            return bits.ToList();
        }

        private string ConvertBinaryToString(string binary)
        {
            return Encoding.UTF8.GetString(StringToByteArray(binary));

        }

        private byte[] StringToByteArray(string text)
        {
            var array = Regex.Split(text, "(.{8})");
            return Regex.Split(text, "(.{8})") // this is the consequence of running them all together.
           .Where(binary => !string.IsNullOrEmpty(binary)) // keeps the matches; drops empty parts 
           .Select(binary => Convert.ToByte(binary, 2))
           .ToArray();
        }

        private List<char> EmbaralharUsandoKey(List<char> bits, string key)
        {
            for (int i = 0; i < bits.Count; i++)
            {
                int movementLength = key[i % key.Length];

                int bitPosition = movementLength % bits.Count;

                char temp = bits[bitPosition];
                bits[bitPosition] = bits[i];
                bits[i] = temp;
            }


            return bits;
        }

        private string DesembaralharUsandoKey(List<char> bits, string key)
        {
            for (int i = bits.Count - 1; i >= 0; i--)
            {
                int movementLength = key[i % key.Length];

                int bitPosition = movementLength % bits.Count;

                char temp = bits[bitPosition];
                bits[bitPosition] = bits[i];
                bits[i] = temp;
            }
            return string.Join("", bits);
        }
    }
}
