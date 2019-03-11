using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CyrillicTicker
{
    public static class FileConverter
    {
        public static string Encode(string inputFile, string outputFile = null)
        {
            var content = Open(inputFile);
            
            int limit = 5;
            for (int i = 10; i < content.Length; i+=5)
            {
                if (content[i] == 0 && --limit == 0)
                    break;
                
                char key = EncodingWindows1251.GetChars(content, i, 1)[0];
                if (!Map.ContainsKey(key))
                    continue;

                string value = Map[key];
                var valueBytes = EncodingWindows1251.GetBytes(value);

                Array.Copy(valueBytes, 0, content, i, valueBytes.Length);

                if (value.Length >= 4)
                    content[i - 1] = 0x02;
                else
                    content[i + 1] = 0x20;
            }

            if (outputFile == null)
                outputFile = DefaultOutputFile(inputFile);

            Save(outputFile, content);
            return outputFile;
        }

        private static byte[] Open(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException($"Не найден файл для обработки: {path}", path);

            var content = File.ReadAllBytes(path);
            if (content.Length <= 11)
                throw new ArgumentException($"Содержимое файла отсутствует: {path}", nameof(path));

            // ReSharper disable once LoopCanBeConvertedToQuery
            for (int i = 0; i < FileTypeSign.Length - 1; i++)
            {
                if (content[i] != FileTypeSign[i])
                    throw new ArgumentException($"Неверный тип файла для обработки: {path}", nameof(path));
            }

            return content;
        }

        private static void Save(string path, byte[] content)
        {
            File.WriteAllBytes(path, content);
        }

        private static string DefaultOutputFile(string inputFile)
        {
            string inputFolder = Path.GetDirectoryName(inputFile);
            if (string.IsNullOrEmpty(inputFolder))
                throw new NullReferenceException("Не удалось получить папку " + inputFile);

            string inputFileName = Path.GetFileNameWithoutExtension(inputFile);
            string extension = Path.GetExtension(inputFile);

            return Path.Combine(inputFolder, $"{inputFileName}_encoded{extension}");
        }
        
        #region Consts

        private static readonly Encoding EncodingWindows1251 = Encoding.GetEncoding("windows-1251");
        
        private static readonly byte[] FileTypeSign = Encoding.ASCII.GetBytes("SEL95");

        private static readonly Dictionary<char, string> Map = new Dictionary<char, string> {
            {'А', "A"},
            {'а', "a"},

            {'Б', "7501" },
            {'б', "7528" },
            
            {'В', "B"},
            {'в', "7502" },

            {'Г', "7535" },
            {'г', "7536" },

            {'Д', "7511" },
            {'д', "7514" },

            {'Е', "E"},
            {'е', "e"},
            
            {'Ё', "7503" },
            {'ё', "7534" },

            {'Ж', "7539" },
            {'ж', "7540" },
            
            {'З', "7541" },
            {'з', "7542" },

            {'И', "7504" },            
            {'и', "7506" },

            {'Й', "7505" },
            {'й', "7507" },

            {'К', "K"},
            {'к', "k"},

            {'Л', "7512" },
            {'л', "7515" },

            {'М', "7509" },
            {'м', "7510" },

            {'Н', "H"},
            {'н', "7508" },

            {'О', "O"},
            {'о', "o"},

            {'П', "7513" },
            {'п', "7516" },

            {'Р', "P"},
            {'р', "p"},

            {'С', "C"},
            {'с', "c"},

            {'Т', "T"},
            {'т', "7519" },

            {'У', "7526" },
            {'у', "7527" },

            {'Ф', "7547" },
            {'ф', "7548" },

            {'Х', "X"},
            {'х', "x"},

            {'Ц', "7520" },
            {'ц', "7521" },
            
            {'Ч', "7537" },
            {'ч', "7538" },
            
            {'Ш', "7522" },
            {'ш', "7524" },

            {'Щ', "7523" },
            {'щ', "7525" },

            {'ь', "7533" },

            {'Ы', "7530" },
            {'ы', "7532" },
            
            {'Ъ', "7529" },
            {'ъ', "7531" },
            
            {'Э', "7543" },
            {'э', "7544" },

            {'Ю', "7545" },
            {'ю', "7546" },

            {'Я', "7517" },
            {'я', "7518" }
        };
        
        #endregion
    }
}