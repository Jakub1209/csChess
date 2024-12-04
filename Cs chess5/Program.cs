using System;
using System.Threading.Channels;

namespace Jakub_Szewczyk_71695_Szachy
{
    internal class Program
    {
        static string[,] _player1Coordinates = new string[19, 19];
        static string[,] _player2Coordinates = new string[19, 19];
        static string[,] _chessBoard = CreateChessBoard();

        public static void Main(string[] args)
        {
            GameSetup();
            GameRunning();
        }

        static void PrintChessBoard(string[,] chessBoard)
        {
            for (int row = 0; row < chessBoard.GetLength(0); row++)
            {
                for (int col = 0; col < chessBoard.GetLength(1); col++)
                {
                    if (row % 2 == 1) //add the horizontal lines color
                        Console.ForegroundColor = ConsoleColor.Green;


                    else if (row % 2 == 0 && col % 2 == 1) //add the vertical lines color
                        Console.ForegroundColor = ConsoleColor.Green;

                    if (_player1Coordinates[row, col] == "x")
                        Console.ForegroundColor = ConsoleColor.Red;
                    else if (_player2Coordinates[row, col] == "x")
                        Console.ForegroundColor = ConsoleColor.Blue;

                    Console.Write(chessBoard[row, col]);
                    Console.ResetColor();
                }

                Console.Write("\n");
            }

            Console.Write("\n");
        }

        static string[,] CreateChessBoard()
        {
            string[,] chessBoard = new string[19, 19];

            string[] numbers = { "1", "2", "3", "4", "5", "6", "7", "8" };
            int numberCounter = 7;
            string[] letters = { "a", "b", "c", "d", "e", "f", "g", "h" };
            int letterCounter = 0;

            for (int row = 0; row < chessBoard.GetLength(0); row++) //create rows
            {
                for (int col = 0; col < chessBoard.GetLength(1); col++) //create columns
                {
                    if ((row == 0 || row == 18) && (col == 0 || col == 18)) //add blank spaces in corners
                        chessBoard[row, col] = " ";

                    else if (row % 2 == 1) //add the horizontal lines
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        chessBoard[row, col] = "--";
                        Console.ResetColor();
                    }

                    else if (row % 2 == 0 && col % 2 == 1) //add the vertical lines
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        chessBoard[row, col] = " | ";
                        Console.ResetColor();
                    }
                    else if (row % 2 == 0 && col % 2 == 0 && row > 1 && row < 17 && col > 1 &&
                             col < 17) //add the spaces in empty slots
                        chessBoard[row, col] = " ";

                    else if ((row == 0 || row == 18) && !(col == 0 || col == 18)) //add the letters in rows
                    {
                        if (letterCounter > 7)
                            letterCounter = 0;


                        chessBoard[row, col] = letters[letterCounter];
                        letterCounter++;
                    }

                    else if ((col == 0 || col == 18) && (row > 1 && row < 17) && row % 2 == 0) //add numbers in columns
                        chessBoard[row, col] = numbers[numberCounter];


                }

                if ((row > 1 && row < 17) && row % 2 == 0) //decrease the numberCounter
                    numberCounter--;
            }

            return chessBoard;
        }

        static void PutPiecesOnChessBoard(string[,] chessBoard)
        {
            string[] pieces = { "R", "N", "B", "Q", "K", "B", "N", "R", "P" };
            int pieceCounter = 0;

            for (int row = 0; row < chessBoard.GetLength(0); row++)
            {
                for (int col = 0; col < chessBoard.GetLength(1); col++)
                {
                    if ((row == 2 || row == 16) && col % 2 == 0 && col > 0 && col < 18)
                    {
                        chessBoard[row, col] = pieces[pieceCounter];
                        pieceCounter++;

                        string[,] result = row == 2 ? _player2Coordinates : _player1Coordinates;
                        result[row, col] = "x";
                    }

                    if ((row == 4 || row == 14) && col % 2 == 0 && col > 0 && col < 18)
                    {
                        chessBoard[row, col] = pieces[8];

                        string[,] result = row == 4 ? _player2Coordinates : _player1Coordinates;
                        result[row, col] = "x";
                    }
                }

                pieceCounter = 0;
            }
        }

        static void GameSetup()
        {
            PutPiecesOnChessBoard(_chessBoard);

            Console.WriteLine("Welcome to Jakub's fabulous game!");
            Console.WriteLine("Players, this is your batlefield: \n");
            PrintChessBoard(_chessBoard);
            Console.WriteLine("May the best one win!");
            Console.WriteLine("Press 'Enter' to continue...");
            Console.ReadLine();
        }

        static void GameRunning()
        {
            bool gameRunning = true;
            int turnNumber = 0;

            while (gameRunning)
            {
                int playerNumber = turnNumber % 2 + 1;

                Console.Clear();
                Console.WriteLine($"It is player{playerNumber}'s turn! \n");
                PrintChessBoard(_chessBoard);
                MakeAmove(playerNumber);
                
                turnNumber++;
            }
        }

        static void MakeAmove(int playerNumber)
        {
            int[] moveInIntArray = GetMoveFromPlayer();
            string chosenPawn = _chessBoard[moveInIntArray[0], moveInIntArray[1]];
            string[,] currentPlayersCoordinates = playerNumber == 1 ? _player1Coordinates : _player2Coordinates;

            if (!PawnBelongsToPlayer(currentPlayersCoordinates, moveInIntArray))
            {
                Console.WriteLine("You cannot make this move!");
                MakeAmove(playerNumber);   
            }
            
            currentPlayersCoordinates[moveInIntArray[0], moveInIntArray[1]] = " ";
            currentPlayersCoordinates[moveInIntArray[2], moveInIntArray[3]] = "x";
            
            _chessBoard[moveInIntArray[0], moveInIntArray[1]] = " ";
            _chessBoard[moveInIntArray[2], moveInIntArray[3]] = chosenPawn;
            return;
        }

        static int[] ConvertMoveToIntArray(string move)
        {
            int[] moveInInt = new int[4];
            int count = 0;
            int index = 1;

            foreach (char c in move)
            {
                int cInt = c; //rows
                char result = c > 56 ? 'a' : '1';
                cInt -= result;
                if (count % 2 == 0)
                {
                    moveInInt[index] = 2 * cInt + 2;
                    index += 2;
                }

                count++;
            }

            count = 0;
            index = 0;

            foreach (char c in move) //columns
            {
                int cInt = c;
                char result = c > 56 ? 'a' : '1';
                cInt -= result;
                if (count % 2 == 1)
                {
                    moveInInt[index] = _chessBoard.GetLength(1) - 2 * cInt - 3;
                    index += 2;
                }

                count++;
            }

            return moveInInt;
        }

        static int[] GetMoveFromPlayer()
        {
            Console.Write("Make a move: ");
            string move = Console.ReadLine();

            if (!MoveIsLegal(move))
                return GetMoveFromPlayer();
            
            return ConvertMoveToIntArray(move);
        }

        static bool MoveIsLegal(string move)
        {
            return MoveIsOnChessboard(move);
        }

        static bool MoveIsOnChessboard(string move)
        {
            int arrayIndex = 0;

            if (move.Length > 4)
            {
                Console.WriteLine($"Incorrect move! '{move}' has too many symbols!\n" +
                                  $"To make a move, use this formula:\n" +
                                  $"columnFrom + rowFrom + columnTo + rowTo, example" +
                                  $": a2a4");
                return false;
            }

            foreach (char character in move)
            {
                int charToInt = character;
                
                if ((arrayIndex % 2 == 0 && (charToInt < 97 || charToInt > 104)) ||
                    (arrayIndex % 2 == 1 && (charToInt < 49 || charToInt > 56)))
                {
                    Console.Write("Incorrect move! ");
                    if (arrayIndex % 2 == 0)
                        Console.WriteLine($"'{character}' should be a letter in range: (a-h).");
                    else if (arrayIndex % 2 == 1)
                        Console.WriteLine($"'{character}' should be a digit in range: (1-8).");
                    return false;
                }
                arrayIndex++;
            }
            return true;
        }

        static bool PawnBelongsToPlayer(string[,] playerCords, int[] moveInIntArray)
        {
            return playerCords[moveInIntArray[0], moveInIntArray[1]] == "x";
        }
    }
}