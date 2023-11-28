using System.Reflection;

namespace first_console_app;

class Game
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

    int rows = 4;
    int cols = 4;

    int[,] board;

    public Game(int rows, int cols) {
        this.rows = rows;
        this.cols = cols;
    }

    public void run() {
        initBoard();
        drawBoard();

        do {
            var pressedKey = Console.ReadKey();

            calcBoard(pressedKey.Key);
            spawnTile();
            drawBoard();
        } while(true);
    }

    void initBoard() {
        board = new int[rows, cols];
        for (int row = 0; row < rows; row++) {
            for (int col = 0; col < cols; col++) {
                board[row, col] = 0;
            }
        }

        spawnTile();
        spawnTile();
    }

    void spawnTile() {
        Random rnd = new Random();

        int row = rnd.Next(rows);
        int col = rnd.Next(cols);

        while (board[row, col] != 0) {
            row = rnd.Next(rows);
            col = rnd.Next(cols);
        }

        board[row, col] = generateNewTile();
    }

    void drawBoard() {
        int boardCharWidth = cols * 5 + 1;

        Console.Clear();

        for (int row = 0; row < rows; row++) {
            if (row == 0) {
                Console.WriteLine(new string('-', boardCharWidth));
            }

            for (int col = 0; col < cols; col++) {
                int value = board[row, col];

                if (col == 0) {
                    Console.Write($"|");
                    Console.BackgroundColor = (ConsoleColor)(value % 14);
                    Console.Write($"{value}".PadLeft(4));
                    Console.ResetColor();
                    Console.Write($"|");
                } else {
                    Console.BackgroundColor = (ConsoleColor)(value % 14);
                    Console.Write($"{value}".PadLeft(4));
                    Console.ResetColor();
                    Console.Write($"|");
                }
            }

            Console.WriteLine("\n" + new string('-', boardCharWidth));
        }
    }

    int generateNewTile() {
        Random rnd = new Random();
        int[] newTileValues = [2, 4];
        return newTileValues[rnd.Next(newTileValues.Length)];
    }

    void calcBoard(ConsoleKey dir) {
        switch (dir) {
            case ConsoleKey.RightArrow:
            case ConsoleKey.LeftArrow: {
                int startFrom = ConsoleKey.LeftArrow == dir ? 1 : cols - 2;
                int goDirection =  ConsoleKey.LeftArrow == dir ? +1 : -1;

                for (int row = 0; row < rows; row++) {
                    for (
                        int col = startFrom;
                        ConsoleKey.LeftArrow == dir ? col < cols : col >= 0 ;
                        col += goDirection
                    ) {
                        int value = board[row, col];

                        if (value != 0) {
                            bool previousMerge = false;
                            int prevPos = col - goDirection;

                            for (
                                int prev = prevPos;
                                ConsoleKey.LeftArrow == dir ? prev >= 0 : prev <= cols - 1;
                                prev -= goDirection
                            ) {
                                if (board[row, prev] == 0) {
                                    board[row, prev + goDirection] = 0;
                                    board[row, prev] = value;
                                } else {
                                    if (board[row, prev] == value && !previousMerge) {
                                        previousMerge = true;
                                        value = board[row, prev] + value;
                                        board[row, prev + goDirection] = 0;
                                        board[row, prev] = value;
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
                int startFrom = ConsoleKey.UpArrow == dir ? 1 : cols - 2;
                int goDirection =  ConsoleKey.UpArrow == dir ? +1 : -1;

                for (int col = 0; col < cols; col++) {
                    for (
                        int row = startFrom;
                        ConsoleKey.UpArrow == dir ? row < rows : row >= 0 ;
                        row += goDirection
                    ) {
                        int value = board[row, col];

                        if (value != 0) {
                            bool previousMerge = false;
                            int prevPos = row - goDirection;

                            for (
                                int prev = prevPos;
                                ConsoleKey.UpArrow == dir ? prev >= 0 : prev <= cols - 1;
                                prev -= goDirection
                            ) {
                                if (board[prev, col] == 0) {
                                    board[prev + goDirection, col] = 0;
                                    board[prev, col] = value;
                                } else {
                                    if (board[prev, col] == value && !previousMerge) {
                                        previousMerge = true;
                                        value = board[prev, col] + value;
                                        board[prev + goDirection, col] = 0;
                                        board[prev, col] = value;
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