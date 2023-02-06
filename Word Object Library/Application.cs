using System;
using Microsoft.Office.Interop.Word;

namespace Word_Object_Library
{
    public class WordObjectLibrary
    {
        public static Application checker = new Application();
        public static bool CheckSpelling(string word)
        {
            return checker.CheckSpelling(word.ToLower());
        }
        public static void Quit()
        {
            checker.Quit();
        }
        static void Main(string[] args)
        {
        }
    }
    
    }