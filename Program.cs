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
            case ConsoleKey.LeftArrow: {
                for (int row = 0; row < Program.rows; row++) {
                    for (int col = 1; col < Program.cols; col++) {
                        int value = Program.board[row][col];
                        int valuesPos = col;
                        if (value != 0) {
                            Program.board[row][col] = 0;
                            while(valuesPos > 0) {
                                valuesPos--;
                                int nextValue = Program.board[row][valuesPos];
                                if (nextValue == 0 || nextValue == value) {
                                    value = nextValue + value;
                                }
                            }

                            Program.board[row][valuesPos] = value;
                        }
                    }
                }
            } break;
            case ConsoleKey.RightArrow: break;
            case ConsoleKey.UpArrow: break;
            case ConsoleKey.DownArrow: break;
            default: break;
        }
    }
}
