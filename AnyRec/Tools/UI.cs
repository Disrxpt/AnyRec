using System;
using System.Collections.Generic;
using System.Text;

namespace AnyRec.Tools
{
    internal class UI
    {
        public static void WriteTitle()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(" █████╗ ███╗   ██╗██╗   ██╗██████╗ ███████╗ ██████╗\r\n██╔══██╗████╗  ██║╚██╗ ██╔╝██╔══██╗██╔════╝██╔════╝\r\n███████║██╔██╗ ██║ ╚████╔╝ ██████╔╝█████╗  ██║     \r\n██╔══██║██║╚██╗██║  ╚██╔╝  ██╔══██╗██╔══╝  ██║     \r\n██║  ██║██║ ╚████║   ██║   ██║  ██║███████╗╚██████╗\r\n╚═╝  ╚═╝╚═╝  ╚═══╝   ╚═╝   ╚═╝  ╚═╝╚══════╝ ╚═════╝");
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static string ShowList(List<string> strings, int pgsize = 10)
        {
            if (strings == null || strings.Count == 0)
                return "";

            int selected = 0;
            int pageSize = pgsize;

            Console.WriteLine();

            int startY = Console.CursorTop;

            int totalPages =
                (int)Math.Ceiling(strings.Count / (double)pageSize);

            while (true)
            {
                int pageStart =
                    (selected / pageSize) * pageSize;

                int pageEnd =
                    Math.Min(pageStart + pageSize, strings.Count);

                Console.SetCursorPosition(0, startY);

                for (int i = pageStart; i < pageEnd; i++)
                {
                    Console.ForegroundColor =
                        i == selected
                            ? ConsoleColor.White
                            : ConsoleColor.DarkGray;

                    string text = strings[i];

                    if (i == selected)
                    {
                        Console.WriteLine(
                            "> " +
                            text +
                            new string(
                                ' ',
                                Math.Max(
                                    0,
                                    Console.WindowWidth -
                                    text.Length -
                                    2
                                )
                            )
                        );
                    }
                    else
                    {
                        Console.WriteLine(
                            text +
                            new string(
                                ' ',
                                Math.Max(
                                    0,
                                    Console.WindowWidth -
                                    text.Length
                                )
                            )
                        );
                    }
                }

                for (int i = pageEnd; i < pageStart + pageSize; i++)
                {
                    Console.WriteLine(
                        new string(' ', Console.WindowWidth)
                    );
                }

                Console.ResetColor();

                if (totalPages > 1)
                {
                    Console.WriteLine(
                        $"\n({pageStart / pageSize + 1}/{totalPages})"
                    );
                }

                ConsoleKeyInfo keyinfo =
                    Console.ReadKey(true);

                char key =
                    char.ToLower(keyinfo.KeyChar);

                if (
                    keyinfo.Key == ConsoleKey.UpArrow ||
                    key == 'w'
                )
                {
                    selected--;

                    if (selected < 0)
                        selected = strings.Count - 1;
                }
                else if (
                    keyinfo.Key == ConsoleKey.DownArrow ||
                    key == 's'
                )
                {
                    selected++;

                    if (selected >= strings.Count)
                        selected = 0;
                }
                else if (
                    keyinfo.Key == ConsoleKey.LeftArrow ||
                    key == 'a'
                )
                {
                    if (totalPages > 1)
                    {
                        selected -= pageSize;

                        if (selected < 0)
                            selected = 0;
                    }
                }
                else if (
                    keyinfo.Key == ConsoleKey.RightArrow ||
                    key == 'd'
                )
                {
                    if (totalPages > 1)
                    {
                        selected += pageSize;

                        if (selected >= strings.Count)
                            selected = strings.Count - 1;
                    }
                }
                else if (keyinfo.Key == ConsoleKey.Enter)
                {
                    return strings[selected];
                }
            }
        }
    }
}
