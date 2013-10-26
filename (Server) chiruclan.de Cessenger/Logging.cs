using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using DotNetLibrary;

namespace Server_chiruclande_Cessenger
{
    class Logging
    {
        private SimpleLogging log;
        private int current_day = DateTime.Now.DayOfYear;

        private string _basename, _prefix, _suffix;
        private bool _with_date;
        private TextWriter _disable_console_output = TextWriter.Null;
        private TextWriter _enable_console_output = Console.Out;

        public string basename
        {
            get { return _basename; }
            set { _basename = value; }
        }

        public string prefix
        {
            get { return _prefix; }
            set { _prefix = value; }
        }

        public string suffix
        {
            get { return suffix; }
            set { _suffix = value; }
        }

        public bool with_date
        {
            get { return _with_date; }
            set { _with_date = value; }
        }

        public Logging (string basename = "server", string prefix = null, string suffix = null, bool with_date = true)
        {
            try
            {
                if (with_date)
                    log = new SimpleLogging(string.Format("{0}{1}{2}_{3}-{4}-{5}.log", prefix, basename, suffix, DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year));
                else
                    log = new SimpleLogging(string.Format("{0}{1}{2}.log", prefix, basename, suffix));

                _basename = basename;
                _prefix = prefix;
                _suffix = suffix;
                _with_date = with_date;

            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nError:\n{0}", ex.Message);
                Console.ResetColor();
            }
        }

        public void cout(string text, params object[] values)
        {
            try
            {
                text = string.Format(text, values);
                string[] logtext = text.Split(new char[] { '\n' }, StringSplitOptions.None);

                if (_with_date)
                {
                    if (current_day != DateTime.Now.DayOfYear)
                        log = new SimpleLogging(string.Format("{0}{1}{2}_{3}-{4}-{5}.log", _prefix, _basename, _suffix, DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year));

                    for (int i = 0; i < logtext.Length; i++)
                        Console.WriteLine("[{0}:{1}:{2}] {3}", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, logtext[i]);
                }
                else
                {
                    for (int i = 0; i < logtext.Length; i++)
                        Console.WriteLine(logtext[i]);
                }

                for (int i = 0; i < logtext.Length; i++)
                    log.Log(logtext[i], _with_date);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nError:\n{0}", ex.Message);
                Console.ResetColor();
            }
        }

        public void cerr(string text, params object[] values)
        {
            try
            {
                text = string.Format(text, values);
                string[] logtext = text.Split(new char[] { '\n' }, StringSplitOptions.None);
                Console.ForegroundColor = ConsoleColor.Red;

                if (_with_date)
                {
                    if (current_day != DateTime.Now.DayOfYear)
                        log = new SimpleLogging(string.Format("{0}{1}{2}_{3}-{4}-{5}.log", _prefix, _basename, _suffix, DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year));

                    for (int i = 0; i < logtext.Length; i++)
                        Console.WriteLine("[{0}:{1}:{2}] >>>> Error: {3}", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, logtext[i]);
                }
                else
                {
                    for (int i = 0; i < logtext.Length; i++)
                        Console.WriteLine(">>>> Error: {0}", logtext[i]);
                }

                Console.ResetColor();
                log.Log(string.Format(">>>> Error: {0}", text), _with_date);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nError:\n{0}", ex.Message);
                Console.ResetColor();
            }
        }

        public void disable_console()
        {
            Console.SetOut(_disable_console_output);
        }

        public void enable_console()
        {
            Console.SetOut(_enable_console_output);
        }
    }
}
