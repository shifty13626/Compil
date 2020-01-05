using System;
using System.IO;
using System.Reflection.Emit;
using System.Text.RegularExpressions;

namespace Compil.Preprocessor {
    public class SimplePreprocessor {
        public string Process(string code) {
            code = Include(code);
            code = Define(code);
            return code;
        }

        private string Include(string code) {
            string regx = @"(#include<([a-zA-Z.]+)>)";
            Regex r = new Regex(regx, RegexOptions.Multiline);
            var matches = r.Matches(code);
            
            foreach (Match match in matches) {
                string include = match.Groups[1].ToString();
                string lib = match.Groups[2].ToString();

                if (lib == "stdlib") {
                    string text = File.ReadAllText(@"std/stdlib.c");  
                    code = code.Replace(include, text);
                } else if (lib == "stdio") {
                    string text = File.ReadAllText(@"std/stdio.c");  
                    code = code.Replace(include, text);
                }

            }
            
            return code;
        }

        private string Define(string code) {
            string regx = @"(#define ([a-zA-Z0-9]+) ([0-9]+))";
            Regex r = new Regex(regx, RegexOptions.Multiline);
            var matches = r.Matches(code);
            
            foreach (Match match in matches) {
                string define = match.Groups[1].ToString();
                string key = match.Groups[2].ToString();
                string val = match.Groups[3].ToString();
                code = code.Replace(define, "");
                code = code.Replace(key, val);
            }
            
            return code;
        }
    }
}
