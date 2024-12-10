using System;

namespace Jakub_Szewczyk_71695_Szachy
{
    internal class Program
    {
        static string[,] _player1Coordinates = new string[19, 19]; // setting up global variables
        static string[,] _player2Coordinates = new string[19, 19];
        static string[,] _chessBoard = CreateChessBoard();

        public static void Main(string[] args)
        {
            GameSetup();
            GameRunning();
        }

        static void PrintChessBoard(string[,] chessBoard)
        {
            for (int row = 0; row < chessBoard.GetLength(0); row++) //for every row in the chessboard
            {
                for (int col = 0; col < chessBoard.GetLength(1); col++) //for every column in the chessboard
                {
                    if (row % 2 == 1) //add the horizontal lines color
                        Console.ForegroundColor = ConsoleColor.Green;
                    else if (row % 2 == 0 && col % 2 == 1) //add the vertical lines color
                        Console.ForegroundColor = ConsoleColor.Green;
                    if (_player1Coordinates[row, col] == "x") //color in player1's pawns in red
                        Console.ForegroundColor = ConsoleColor.Red;
                    else if (_player2Coordinates[row, col] == "x") //color in player2's pawns in blue
                        Console.ForegroundColor = ConsoleColor.Blue;

                    Console.Write(chessBoard[row, col]); //print out the chessboard cell by cell, starting from UPPER LEFT!!!
                    Console.ResetColor(); //reset the color so that the numbers and letters print out in white
                }

                Console.WriteLine(); //begin a new row
            }

            Console.WriteLine(); //leave a blank row after printing out the chessboard
        }

        static string[,] CreateChessBoard()
        {
            string[,] chessBoard = new string[19, 19]; 
            /*
            declare the chessboard as 20 by 20 chars long, because:
            chars in columns 1 and 20 are occupied by numbers,
            chars in rows 1 and 20 are occupied by letters,
            in every other odd rows or columns, there is a building block - "-" or "|",
            in every even row and column, there is either a pawn, or a blank space left.
            */
            string[] numbers = { "1", "2", "3", "4", "5", "6", "7", "8 " };
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
                        chessBoard[row, col] = "--";

                    else if (row % 2 == 0 && col % 2 == 1) //add the vertical lines
                        chessBoard[row, col] = " | ";
                    
                    else if (
                        row % 2 == 0 
                        && col % 2 == 0 
                        && row > 1 
                        && row < 17 
                        && col > 1 
                        && col < 17
                        ) //add the spaces in empty slots
                        chessBoard[row, col] = " ";

                    else if (
                        (row == 0 || row == 18) 
                        && 
                        !(col == 0 || col == 18)) //add the letters in rows
                    {
                        if (letterCounter > 7) //if the letterCounter goes over 7, we have to restart the value
                                               //in order to print the letters once again
                            letterCounter = 0;
                        
                        chessBoard[row, col] = letters[letterCounter];
                        letterCounter++;
                    }

                    else if (
                        (col == 0 || col == 18) 
                        && row > 1 
                        && row < 17
                        && row % 2 == 0
                        ) //add numbers in columns
                        chessBoard[row, col] = numbers[numberCounter];
                }
                if (
                    row > 1 
                    && row < 17
                    && row % 2 == 0
                    ) //decrease the numberCounter for every number printed
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

                
                // Console.Clear();
                Console.WriteLine($"It is player{playerNumber}'s turn! \n");
                PrintChessBoard(_chessBoard);
                MakeAmove(playerNumber, turnNumber);
                
                turnNumber++;
            }
        }
        
        static void MakeAmove(int playerNumber, int turnNumber)
        {
            while (true)
            {
                int[] moveInIntArray = GetMoveFromPlayer();
                string chosenPawn = _chessBoard[moveInIntArray[0], moveInIntArray[1]];
                string[,] currentPlayersCoordinates = playerNumber == 1 ? _player1Coordinates : _player2Coordinates;
                string[,] opponentCoordinates = playerNumber == 1 ? _player2Coordinates : _player1Coordinates;
                
                // if the pawn has a legal move, only then you can move it.
                
                if (PawnBelongsToPlayer(currentPlayersCoordinates, moveInIntArray) && 
                    PawnMoveIsLegal(moveInIntArray, chosenPawn, _player1Coordinates, _player2Coordinates, turnNumber))
                {
                    currentPlayersCoordinates[moveInIntArray[0], moveInIntArray[1]] = " ";
                    currentPlayersCoordinates[moveInIntArray[2], moveInIntArray[3]] = "x";

                    if (currentPlayersCoordinates[moveInIntArray[2], moveInIntArray[3]]
                        == opponentCoordinates[moveInIntArray[2], moveInIntArray[3]])
                    {
                        opponentCoordinates[moveInIntArray[2], moveInIntArray[3]] = " ";
                    }

                    Console.WriteLine($"{moveInIntArray[0]} {moveInIntArray[1]} {moveInIntArray[2]} {moveInIntArray[3]}");
            
                    _chessBoard[moveInIntArray[0], moveInIntArray[1]] = " ";
                    _chessBoard[moveInIntArray[2], moveInIntArray[3]] = chosenPawn;
                    
                    break;   
                }
                
                Console.WriteLine("You cannot make this move!");
            }
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
            while (true)
            {
                Console.Write("Make a move: ");
                string move = Console.ReadLine();

                if (MoveIsLegal(move))
                    return ConvertMoveToIntArray(move);
            }
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

        static bool PawnMoveIsLegal(
            int[] moveInIntArray, 
            string pawn, 
            string[,] player1Cords, 
            string[,] player2Cords, 
            int turnNumber
            )
        {
            int rowDifference = Math.Abs(moveInIntArray[0] - moveInIntArray[2]);
            int columnDifference = Math.Abs(moveInIntArray[1] - moveInIntArray[3]);
            string[,] opponentCords = turnNumber % 2 == 0 ? player2Cords : player1Cords;
            string[,] possibleMoves = new string[19, 19];
            if (pawn == "P")
            {
                if (
                    rowDifference <= 4 
                    && (moveInIntArray[0] == 14 || moveInIntArray[0] == 4) 
                    && columnDifference == 0
                    )
                    return true;
                if (
                    rowDifference == 2 
                    && (
                        (columnDifference == 0
                         && player1Cords[moveInIntArray[2], moveInIntArray[3]] != "x" 
                         && player2Cords[moveInIntArray[2], moveInIntArray[3]] != "x") 
                        || 
                        (columnDifference == 2 
                         && opponentCords[moveInIntArray[2], moveInIntArray[3]] == "x"))
                    )
                    return true;
            }
            else if (pawn == "R")
            {
                //generate table with every legal move
                for (int row = 0; row < 19; row++) //create rows
                {
                    for (int col = 0; col < 19; col++) //create columns
                    {
                        //fill in all empty slots as 0
                        possibleMoves[row, col] = "0";
                        
                        //fill in the column in which the piece is sitting
                        if (moveInIntArray[1] == col)
                        {
                            possibleMoves[row, col] = "x";
                        }
                        //fill in the row in which it currently is
                        if (moveInIntArray[0] == row)
                        {
                            possibleMoves[row, col] = "x";
                        }
                        
                        Console.Write(possibleMoves[row, col]);
                    }
                    Console.WriteLine();
                }

                return true;
            }
            else if (pawn == "N")
            {
                return true;
            }
            else if (pawn == "B")
            {
                return true;
            }
            else if (pawn == "Q")
            {
                return true;
            }
            return false;
        }
        
    }
}