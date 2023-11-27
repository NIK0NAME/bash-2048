using System.Reflection;

namespace first_console_app;

class Program
{

    enum CellColor
    {
        DarkGray = 0,
        Gray = 2,
        DarkYellow = 4,
        Yellow = 8,
        DarkRed = 16,
        Red = 32,
    }

    static int rows = 4;
    static int cols = 4;

    static int[][] board = [
        [0, 0, 0, 0],
        [0, 0, 0, 0],
        [0, 0, 0, 0],
        [0, 0, 0, 0],
    ];

    static void Main(string[] args)
    {
        Program.initBoard();
        Program.drawBoard();

        do {
            var pressedKey = Console.ReadKey();

            Program.calcBoard(pressedKey.Key);
            Program.spawnTile();
            Program.drawBoard();
        } while(true);
    }

    static void initBoard() {
        for (int row = 0; row < Program.rows; row++) {
            for (int col = 0; col < Program.cols; col++) {
                Program.board[row][col] = 0;
            }
        }

        spawnTile();
        spawnTile();
    }

    static void spawnTile() {
        Random rnd = new Random();

        int row = rnd.Next(Program.board.Length);
        int col = rnd.Next(Program.board[0].Length);

        while (Program.board[row][col] != 0) {
            row = rnd.Next(Program.board.Length);
            col = rnd.Next(Program.board[0].Length);
        }

        Program.board[row][col] = Program.generateNewTile();
    }

    static void drawBoard() {
        ConsoleColor currentBackground = Console.BackgroundColor;
        ConsoleColor currentForeground = Console.ForegroundColor;
        int boardCharWidth = 17;

        Console.Clear();

        for (int row = 0; row < Program.rows; row++) {
            if (row == 0) {
                Console.WriteLine(new string('-', boardCharWidth));
            }

            for (int col = 0; col < Program.cols; col++) {
                int value = Program.board[row][col];

                if (col == 0) {
                    Console.Write($"|");
                    Console.BackgroundColor = (ConsoleColor)(value % 14);
                    Console.Write($" {value} ");
                    Console.ResetColor();
                    Console.Write($"|");
                } else {
                    Console.BackgroundColor = (ConsoleColor)(value % 14);
                    Console.Write($" {value} ");
                    Console.ResetColor();
                    Console.Write($"|");
                }
            }

            Console.WriteLine("\n" + new string('-', boardCharWidth));
        }
    }

    static int generateNewTile() {
        Random rnd = new Random();
        int[] newTileValues = [2, 4];
        return newTileValues[rnd.Next(newTileValues.Length)];
    }

    static void calcBoard(ConsoleKey dir) {
        switch (dir) {
            case ConsoleKey.RightArrow:
            case ConsoleKey.LeftArrow: {
                int startFrom = ConsoleKey.LeftArrow == dir ? 1 : Program.cols - 2;
                int goTo = ConsoleKey.LeftArrow == dir ?  Program.cols : 0;
                int goDirection =  ConsoleKey.LeftArrow == dir ? +1 : -1;

                for (int row = 0; row < Program.rows; row++) {
                    for (
                        int col = startFrom;
                        ConsoleKey.LeftArrow == dir ? col < Program.cols : col >= 0 ;
                        col += goDirection
                    ) {
                        int value = Program.board[row][col];

                        if (value != 0) {
                            bool previousMerge = false;
                            int prevPos = col - goDirection;
                            // Program.board[row][col] = 0;

                            for (
                                int prev = prevPos;
                                ConsoleKey.LeftArrow == dir ? prev >= 0 : prev <= Program.cols - 1;
                                prev -= goDirection
                            ) {
                                if (Program.board[row][prev] == 0) {
                                    Program.board[row][prev + goDirection] = 0;
                                    Program.board[row][prev] = value;
                                } else {
                                    if (Program.board[row][prev] == value && !previousMerge) {
                                        previousMerge = true;
                                        value = Program.board[row][prev] + value;
                                        Program.board[row][prev + goDirection] = 0;
                                        Program.board[row][prev] = value;
                                    } else {
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            } break;
            case ConsoleKey.UpArrow:
            case ConsoleKey.DownArrow: {
                int startFrom = ConsoleKey.UpArrow == dir ? 1 : Program.cols - 2;
                int goTo = ConsoleKey.UpArrow == dir ?  Program.cols : 0;
                int goDirection =  ConsoleKey.UpArrow == dir ? +1 : -1;

                for (int col = 0; col < Program.rows; col++) {
                    for (
                        int row = startFrom;
                        ConsoleKey.UpArrow == dir ? row < Program.cols : row >= 0 ;
                        row += goDirection
                    ) {
                        int value = Program.board[row][col];

                        if (value != 0) {
                            bool previousMerge = false;
                            int prevPos = col - goDirection;
                            // Program.board[row][col] = 0;

                            for (
                                int prev = prevPos;
                                ConsoleKey.UpArrow == dir ? prev >= 0 : prev <= Program.cols - 1;
                                prev -= goDirection
                            ) {
                                if (Program.board[prev][col] == 0) {
                                    Program.board[prev + goDirection][col] = 0;
                                    Program.board[prev][col] = value;
                                } else {
                                    if (Program.board[prev][col] == value && !previousMerge) {
                                        previousMerge = true;
                                        value = Program.board[prev][col] + value;
                                        Program.board[prev + goDirection][col] = 0;
                                        Program.board[prev][col] = value;
                                    } else {
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            } break;
            default: break;
        }
    }
}
