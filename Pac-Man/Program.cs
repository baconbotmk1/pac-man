using System;
using System.Collections.Generic;
using System.Threading;

namespace Pac_Man
{
    class Program
    {
        static List<char> previousBrick = new List<char>() { ' ', ' ', ' ', ' ' };
        static List<char> ghosts = new List<char>();

        static void Main(string[] args)
        {
            Console.SetWindowSize(55, 35);
            List<char> board = GenerateBoard();
            int playerPos = 23 * 55 + 27;
            int playerMoves = 0, playerLives = 1;

            do
            {
                Console.Clear();
                PrintBoard(board, playerLives);
                List<char> possibleDirrections = PossibleDirrections(playerPos, board);
                if (board.Contains('P'))
                {
                    PrintPossibleDirrections(possibleDirrections);
                    string playerInput = PlayerInput(possibleDirrections);
                    #region Moving Player
                    board[playerPos] = ' ';
                    switch (playerInput)
                    {
                        case "▲":
                            playerPos = playerPos - 55;
                            playerMoves++;
                            break;
                        case "▼":
                            playerPos = playerPos + 55;
                            playerMoves++;
                            break;
                        case "◄":
                            if (playerPos == 14 * 55)
                                playerPos = playerPos + 54;
                            else
                                playerPos = playerPos - 2;
                            playerMoves++;
                            break;
                        case "►":
                            if (playerPos == 15 * 55 - 1)
                                playerPos = playerPos - 54;
                            else
                                playerPos = playerPos + 2;
                            playerMoves++;
                            break;
                        default:
                            break;
                    }
                    #region Corrects Starting Movement
                    if (playerMoves == 1 && playerInput == "►")
                        playerPos--;
                    else if (playerMoves == 1 && playerInput == "◄")
                        playerPos++;
                    #endregion
                    board[playerPos] = 'P';
                    #endregion
                    if (board[1292] == 'P')
                        board[1292] = ' ';
                    List<char> ghostMoves = GhostMovement(board, playerMoves);
                }
                else if (playerLives > 1)
                {
                    Console.Clear();
                    playerLives--;
                    PrintBoard(board, playerLives);

                    #region Resets ghostpos
                    board[board.IndexOf('G')] = ' ';
                    board[739] = 'G';
                    board[board.IndexOf('B')] = ' ';
                    board[855] = 'B';
                    board[board.IndexOf('R')] = ' ';
                    board[849] = 'R';
                    board[board.IndexOf('M')] = ' ';
                    board[745] = 'M';
                    #endregion
                    #region Resets playerpos
                    board[1292] = 'P';
                    #endregion
                    playerMoves = 0;
                    playerPos = 23 * 55 + 27;
                    for (int i = 0; i < 4; i++)
                    {
                        previousBrick[i] = ' ';
                    }
                    ghosts.Clear();
                    for (int i = 0; i < 3; i++)
                    {
                        Console.Write("{0}", 3 - i);
                        Thread.Sleep(250);
                        Console.Write(".");
                        Thread.Sleep(250);
                        Console.Write(".");
                        Thread.Sleep(250);
                        Console.Write(".");
                        Thread.Sleep(250);
                    }

                }
                else
                {
                    Console.Clear();
                    --playerLives;
                    PrintBoard(board, playerLives);
                }


            } while (board.Contains('*') && playerLives != 0);
            #region Game Ending Results
            if (!board.Contains('*'))
            {
                Console.Clear();
                PrintBoard(board, playerLives);
                Console.WriteLine("\n\n\t\tCongratulations! You've won");
            }
            else if (board.Contains('*'))
                Console.WriteLine("\t\t\tGAME OVER!");
            else
                Console.WriteLine("An error has accured!");
            #endregion


        }
        static List<char> PossibleDirrections(int playerPos, List<char> board)
        {
            List<char> possibleDirrections = new List<char>();
            if (board[playerPos - 55] == ' ' || board[playerPos - 55] == '*')//Up
                possibleDirrections.Add('▲');
            else
                possibleDirrections.Add(' ');

            if (board[playerPos - 2] == ' ' || board[playerPos - 2] == '*' || playerPos == 14 * 55)//Left
                possibleDirrections.Add('◄');
            else
                possibleDirrections.Add(' ');

            if (board[playerPos + 55] == ' ' || board[playerPos + 55] == '*') //Down
                possibleDirrections.Add('▼');
            else
                possibleDirrections.Add(' ');

            if (board[playerPos + 2] == ' ' || board[playerPos + 2] == '*' || playerPos == 15 * 55 - 1) //right
                possibleDirrections.Add('►');
            else
                possibleDirrections.Add(' ');

            return possibleDirrections;
        }
        static void PrintPossibleDirrections(List<char> possibleDirrections)
        {
            Console.WriteLine();
            Console.WriteLine("\t\t\t╔═════╗", Console.ForegroundColor = ConsoleColor.DarkBlue);
            Console.Write("\t\t\t║  ", Console.ForegroundColor = ConsoleColor.DarkBlue);
            Console.ResetColor();
            Console.Write(possibleDirrections[0]); //Displays "up"
            Console.WriteLine("  ║", Console.ForegroundColor = ConsoleColor.DarkBlue);
            Console.Write("\t\t\t║", Console.ForegroundColor = ConsoleColor.DarkBlue);
            Console.ResetColor();
            Console.Write(possibleDirrections[1] + " " + possibleDirrections[2] + " " + possibleDirrections[3]); //Displays "left" "Down" "Right"
            Console.Write("║", Console.ForegroundColor = ConsoleColor.DarkBlue);
            Console.ResetColor();
        }
        static void PrintBoard(List<char> board, int playerLives)
        {
            int j = 0;
            foreach (var item in board)
            {
                #region Sets color for "type"
                switch (item)
                {
                    #region Points
                    case '*':
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                    #endregion
                    #region Pac-Man
                    case 'P':
                        Console.BackgroundColor = ConsoleColor.DarkYellow;
                        Console.ForegroundColor = ConsoleColor.Black;
                        break;
                    #endregion
                    #region Borders
                    case '╔':
                    case '╗':
                    case '╚':
                    case '╝':
                    case '╠':
                    case '╣':
                    case '╦':
                    case '╩':
                    case '╬':
                    case '═':
                    case '║':
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                        break;
                    #endregion
                    #region Ghosts
                    case 'B':
                        Console.BackgroundColor = ConsoleColor.Cyan;
                        Console.ForegroundColor = ConsoleColor.Black;
                        break;
                    case 'R':
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.ForegroundColor = ConsoleColor.Black;
                        break;
                    case 'G':
                        Console.BackgroundColor = ConsoleColor.Green;
                        Console.ForegroundColor = ConsoleColor.Black;
                        break;
                    case 'M':
                        Console.BackgroundColor = ConsoleColor.Magenta;
                        Console.ForegroundColor = ConsoleColor.Black;
                        break;
                    #endregion
                    default:
                        Console.ResetColor();
                        break;
                }
                Console.Write(item);
                #endregion
                Console.ResetColor();
                #region Set board length
                j++;
                if (j == 55)
                {
                    j = 0;
                    Console.WriteLine();
                }
                #endregion

            }

            #region Lives display
            Console.Write("Lives:");
            for (int i = 0; i < playerLives; i++)
            {
                Console.Write(" ");
                Console.BackgroundColor = ConsoleColor.DarkYellow;
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(" ");
                Console.ResetColor();
            }
            #endregion
        }
        static List<char> GenerateBoard()
        {
            #region Board to list<char>
            string boardString = "" +
                "╔═════════════════════════╗ ╔═════════════════════════╗" +
                "║ * * * * * * * * * * * * ║ ║ * * * * * * * * * * * * ║" +
                "║ * ╔═════╗ * ╔═══════╗ * ║ ║ * ╔═══════╗ * ╔═════╗ * ║" +
                "║ * ║     ║ * ║       ║ * ║ ║ * ║       ║ * ║     ║ * ║" +
                "║ * ╚═════╝ * ╚═══════╝ * ╚═╝ * ╚═══════╝ * ╚═════╝ * ║" +
                "║ * * * * * * * * * * * * * * * * * * * * * * * * * * ║" +
                "║ * ╔═════╗ * ╔═╗ * ╔═════════════╗ * ╔═╗ * ╔═════╗ * ║" +
                "║ * ╚═════╝ * ║ ║ * ╚═════╗ ╔═════╝ * ║ ║ * ╚═════╝ * ║" +
                "║ * * * * * * ║ ║ * * * * ║ ║ * * * * ║ ║ * * * * * * ║" +
                "╚═════════╗ * ║ ╚═════╗ * ║ ║ * ╔═════╝ ║ * ╔═════════╝" +
                "          ║ * ║ ╔═════╝   ╚═╝   ╚═════╗ ║ * ║          " +
                "          ║ * ║ ║                     ║ ║ * ║          " +
                "          ║ * ║ ║   ╔════-----════╗   ║ ║ * ║          " +
                "══════════╝ * ╚═╝   ║   G     M   ║   ╚═╝ * ╚══════════" +
                "            *       ║             ║       *            " +
                "══════════╗ * ╔═╗   ║   R     B   ║   ╔═╗ * ╔══════════" +
                "          ║ * ║ ║   ╚═════════════╝   ║ ║ * ║          " +
                "          ║ * ║ ║                     ║ ║ * ║          " +
                "          ║ * ║ ║   ╔═════════════╗   ║ ║ * ║          " +
                "╔═════════╝ * ╚═╝   ╚═════╗ ╔═════╝   ╚═╝ * ╚═════════╗" +
                "║ * * * * * * * * * * * * ║ ║ * * * * * * * * * * * * ║" +
                "║ * ╔═════╗ * ╔═══════╗ * ║ ║ * ╔═══════╗ * ╔═════╗ * ║" +
                "║ * ╚═══╗ ║ * ╚═══════╝ * ╚═╝ * ╚═══════╝ * ║ ╔═══╝ * ║" +
                "║ * * * ║ ║ * * * * * * *  P  * * * * * * * ║ ║ * * * ║" +
                "╚═══╗ * ║ ║ * ╔═╗ * ╔═════════════╗ * ╔═╗ * ║ ║ * ╔═══╝" +
                "╔═══╝ * ╚═╝ * ║ ║ * ╚═════╗ ╔═════╝ * ║ ║ * ╚═╝ * ╚═══╗" +
                "║ * * * * * * ║ ║ * * * * ║ ║ * * * * ║ ║ * * * * * * ║" +
                "║ * ╔═════════╝ ╚═════╗ * ║ ║ * ╔═════╝ ╚═════════╗ * ║" +
                "║ * ╚═════════════════╝ * ╚═╝ * ╚═════════════════╝ * ║" +
                "║ * * * * * * * * * * * * * * * * * * * * * * * * * * ║" +
                "╚═════════════════════════════════════════════════════╝";
            char[] boardSplitted = boardString.ToCharArray();
            List<char> board = new List<char>();
            board.AddRange(boardSplitted);
            #endregion
            return board;
        }
        static string PlayerInput(List<char> possibleDirrections)
        {
            while (true)
            {
                #region Pressed key checker
                ConsoleKeyInfo keyInfo = Console.ReadKey();
                if (keyInfo.Key == ConsoleKey.UpArrow && possibleDirrections.Contains('▲'))
                {
                    return "▲";
                }
                else if (keyInfo.Key == ConsoleKey.DownArrow && possibleDirrections.Contains('▼'))
                {
                    return "▼";
                }
                else if (keyInfo.Key == ConsoleKey.LeftArrow && possibleDirrections.Contains('◄'))
                {
                    return "◄";
                }
                else if (keyInfo.Key == ConsoleKey.RightArrow && possibleDirrections.Contains('►'))
                {
                    return "►";
                }
                #endregion
            }
        }
        static List<char> GhostMovement(List<char> board, int totalMoves)
        {
            List<char> ghostMoves = new List<char>();
            #region Adding ghosts according to total player moves
            if (totalMoves == 1)
            {
                ghosts.Add('G');
                board[board.IndexOf('G')] = ' ';
                #region Checks for empty spawning platform
                if (board[633] == ' ')
                    board[633] = 'G';
                else if (board[631] == ' ')
                    board[631] = 'G';
                #endregion

            }
            if (totalMoves == 25)
            {
                ghosts.Add('M');
                board[board.IndexOf('M')] = ' ';
                #region Checks for empty spawning platform
                if (board[633] == ' ')
                    board[633] = 'M';
                else if (board[631] == ' ')
                    board[631] = 'M';
                else if (board[635] == ' ')
                    board[635] = 'M';
                #endregion
            }
            if (totalMoves == 50)
            {
                ghosts.Add('R');
                board[board.IndexOf('R')] = ' ';
                #region Checks for empty spawning platform
                if (board[633] == ' ')
                    board[633] = 'R';
                else if (board[631] == ' ')
                    board[631] = 'R';
                else if (board[635] == ' ')
                    board[635] = 'R';
                else if (board[629] == ' ')
                    board[629] = 'R';
                #endregion
            }
            if (totalMoves == 75)
            {
                ghosts.Add('B');
                board[board.IndexOf('B')] = ' ';
                #region Checks for empty spawning platform
                if (board[633] == ' ')
                    board[633] = 'B';
                else if (board[631] == ' ')
                    board[631] = 'B';
                else if (board[635] == ' ')
                    board[635] = 'B';
                else if (board[629] == ' ')
                    board[629] = 'B';
                else if (board[637] == ' ')
                    board[637] = 'B';
                #endregion
            }
            #endregion

            foreach (var item in ghosts)
            {
                int ghostPos = board.IndexOf(item);
                List<char> possibleGhostDirrections = PossibleGhostDirrections(ghostPos, board);
                Random range = new Random();
                int ghostMove;
                while (true)
                {
                    ghostMove = range.Next(0, possibleGhostDirrections.Count);
                    if (possibleGhostDirrections[ghostMove] == '◄')
                    {
                        board[ghostPos] = previousBrick[ghosts.IndexOf(item)];
                        if (ghostPos == 14 * 55)
                            ghostPos = ghostPos + 54;
                        else
                            ghostPos = ghostPos - 2;
                        break;
                    }
                    else if (possibleGhostDirrections[ghostMove] == '▼')
                    {
                        board[ghostPos] = previousBrick[ghosts.IndexOf(item)];
                        ghostPos = ghostPos + 55;
                        break;
                    }
                    else if (possibleGhostDirrections[ghostMove] == '►')
                    {
                        board[ghostPos] = previousBrick[ghosts.IndexOf(item)];
                        if (ghostPos == 15 * 55 - 1)
                            ghostPos = ghostPos - 54;
                        else
                            ghostPos = ghostPos + 2;
                        break;
                    }
                    else if (possibleGhostDirrections[ghostMove] == '▲')
                    {
                        board[ghostPos] = previousBrick[ghosts.IndexOf(item)];
                        ghostPos = ghostPos - 55;

                        break;
                    }
                }
                previousBrick[ghosts.IndexOf(item)] = board[ghostPos];
                board[ghostPos] = item;
            }

            return ghostMoves;
        }
        static List<char> PossibleGhostDirrections(int ghostPos, List<char> board)
        {
            List<char> possibleGhostDirrections = new List<char>();
            if (board[ghostPos - 55] == ' ' || board[ghostPos - 55] == '*' || board[ghostPos - 55] == 'P')//Up
                possibleGhostDirrections.Add('▲');
            else
                possibleGhostDirrections.Add(' ');

            if (board[ghostPos - 2] == ' ' || board[ghostPos - 2] == '*' || board[ghostPos - 2] == 'P' || ghostPos == 14 * 55)//Left
                possibleGhostDirrections.Add('◄');
            else
                possibleGhostDirrections.Add(' ');

            if (board[ghostPos + 55] == ' ' || board[ghostPos + 55] == '*' || board[ghostPos + 55] == 'P') //Down
                possibleGhostDirrections.Add('▼');
            else
                possibleGhostDirrections.Add(' ');

            if (board[ghostPos + 2] == ' ' || board[ghostPos + 2] == '*' || board[ghostPos + 2] == 'P' || ghostPos == 15 * 55 - 1) //right
                possibleGhostDirrections.Add('►');
            else
                possibleGhostDirrections.Add(' ');

            return possibleGhostDirrections;
        }
    }
}