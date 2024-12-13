using System;

namespace Jakub_Szewczyk_71695_Szachy
{
    internal class Program
    {
        static string[,] _player1Coordinates = new string[19, 19]; // setting up global variables
        static string[,] _player2Coordinates = new string[19, 19];
        static string[,] _chessBoard = CreateChessBoard();
        static int _turnNumber = 0;

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

            while (gameRunning)
            {
                int playerNumber = _turnNumber % 2 + 1;

                
                // Console.Clear();
                Console.WriteLine($"It is player{playerNumber}'s turn! \n");
                PrintChessBoard(_chessBoard);
                MakeAmove(playerNumber);
                
                _turnNumber++;
            }
        }
        
        static void MakeAmove(int playerNumber)
        {
            while (true)
            {
                int[] moveInIntArray = GetMoveFromPlayer();
                string chosenPawn = _chessBoard[moveInIntArray[0], moveInIntArray[1]];
                string[,] currentPlayersCoordinates = playerNumber == 1 ? _player1Coordinates : _player2Coordinates;
                string[,] opponentCoordinates = playerNumber == 1 ? _player2Coordinates : _player1Coordinates;
                
                // if the pawn has a legal move, only then you can move it.
                
                if (PawnBelongsToPlayer(currentPlayersCoordinates, moveInIntArray) && 
                    PawnMoveIsLegal(moveInIntArray, chosenPawn))
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

        static bool PawnMoveIsLegal(int[] moveInIntArray, string pawn)
        {
            int rowDifference = Math.Abs(moveInIntArray[0] - moveInIntArray[2]);
            int columnDifference = Math.Abs(moveInIntArray[1] - moveInIntArray[3]);
            string[,] opponentCords = _turnNumber % 2 == 0 ? _player2Coordinates : _player1Coordinates;
            string[,] currentPLayersCords = _turnNumber % 2 == 0 ? _player1Coordinates : _player2Coordinates;
            string[,] possibleMoves = GenerateTableWithPossibleMoves(moveInIntArray[1], moveInIntArray[0], pawn);
            string[,] attacksAfterMove = GenerateTableWithPossibleMoves(moveInIntArray[3], moveInIntArray[2], pawn);
            int[] opponentKingPosition = new int[2];
            
            if (pawn == "P")
            {
                //checking if the pawn doesn't move backwards - for player1 the difference should be negative
                if ((_turnNumber % 2 == 0 && moveInIntArray[2] - moveInIntArray[0] < 0)
                    || (_turnNumber % 2 == 1 && moveInIntArray[2] - moveInIntArray[0] > 0))
                {
                    if (
                        rowDifference <= 4 //if the pawn wants tp move two tiles
                        && (moveInIntArray[0] == 14 ||
                            moveInIntArray[0] == 4) //and it's on the correct starting position
                        && columnDifference == 0 //and it doesn't want to move sideways
                    )
                        return true;
                    if (
                        rowDifference == 2
                        && (
                            (columnDifference == 0
                             && _player1Coordinates[moveInIntArray[2], moveInIntArray[3]] != "x"
                             && _player2Coordinates[moveInIntArray[2], moveInIntArray[3]] != "x")
                            ||
                            (columnDifference == 2
                             && opponentCords[moveInIntArray[2], moveInIntArray[3]] == "x"))
                    )
                        return true;
                }
                else
                {
                    Console.WriteLine("You cannot move backwards dumbass!");
                    return false;
                }
            }
            else if (pawn == "R")
            {
                //look for maxX
                for (int i = 2; i <= 16; i += 2)
                {
                    //generate maxX - right side of the board
                    if (moveInIntArray[1] + i <= 16)
                    {
                        //if the next position is occupied by an ally it means that the current position is the last one possible
                        if (currentPLayersCords[moveInIntArray[0], moveInIntArray[1] + i] == "x")
                        {
                            possibleMoves[moveInIntArray[0], moveInIntArray[1]] = "x";
                            break;
                        }

                        possibleMoves[moveInIntArray[0], moveInIntArray[1] + i] = "x";
                        //if the next position is occupied by an enemy it means that it is the last possible position
                        if (opponentCords[moveInIntArray[0], moveInIntArray[1] + i] == "x")
                        {
                            break;
                        }
                    }
                }

                //look for minX
                for (int i = 2; i <= 16; i += 2)
                {
                    //generate minX - left side of board
                    if (moveInIntArray[1] - i >= 2)
                    {
                        //if the next position is occupied by an ally it means that the current position is the last one possible
                        if (currentPLayersCords[moveInIntArray[0], moveInIntArray[1] - i] == "x")
                        {
                            possibleMoves[moveInIntArray[0], moveInIntArray[1]] = "x";
                            break;
                        }

                        possibleMoves[moveInIntArray[0], moveInIntArray[1] - i] = "x";
                        //if the next position is occupied by an enemy it means that it is the last possible position
                        if (opponentCords[moveInIntArray[0], moveInIntArray[1] - i] == "x")
                        {
                            break;
                        }
                    }
                }

                //look for maxY
                for (int i = 2; i <= 16; i += 2)
                {
                    //maxY - downside of board
                    if (moveInIntArray[0] + i <= 16)
                    {
                        //if the next position is occupied by an ally it means that the current position is the last one possible
                        if (currentPLayersCords[moveInIntArray[0] + i, moveInIntArray[1]] == "x")
                        {
                            possibleMoves[moveInIntArray[0], moveInIntArray[1]] = "x";
                            break;
                        }

                        possibleMoves[moveInIntArray[0] + i, moveInIntArray[1]] = "x";
                        //if the next position is occupied by an enemy it means that it is the last possible position
                        if (opponentCords[moveInIntArray[0] + i, moveInIntArray[1]] == "x")
                        {
                            break;
                        }
                    }
                }

                //look for minY
                for (int i = 2; i <= 16; i += 2)
                {
                    //minY - upside of board
                    if (moveInIntArray[0] - i >= 2)
                    {
                        //if the next position is occupied by an ally it means that the current position is the last one possible
                        if (currentPLayersCords[moveInIntArray[0] - i, moveInIntArray[1]] == "x")
                        {
                            possibleMoves[moveInIntArray[0], moveInIntArray[1]] = "x";
                            break;
                        }

                        possibleMoves[moveInIntArray[0] - i, moveInIntArray[1]] = "x";
                        //if the next position is occupied by an enemy it means that it is the last possible position
                        if (opponentCords[moveInIntArray[0] - i, moveInIntArray[1]] == "x")
                        {
                            break;
                        }
                    }
                }

                //for diagnostic purposes - print the possible moves table
                for (int row = 2; row <= 16; row += 2)
                {
                    for (int col = 2; col <= 16; col += 2)
                    {
                        if (possibleMoves[row, col] == "x") Console.Write("x");
                        else Console.Write("0");
                    }

                    Console.WriteLine();
                }

                //if the move is in the table with possible moves, then move
                if (possibleMoves[moveInIntArray[2], moveInIntArray[3]] == "x") return true;
                //else:
                Console.WriteLine("This move is out of bounds for your Rook! You cannot move there!");
                return false;
            }
            else if (pawn == "N")
            {
                //generate table possibleMoves with every legal move
                for (int row = moveInIntArray[1] - 4; row <= moveInIntArray[1] + 4; row++) //check the rows
                {
                    //if the rows are on the chessboard and there isn't an allied pawn there
                    if (row >= 2 && row <= 16
                                 && currentPLayersCords[moveInIntArray[2], moveInIntArray[3]] != "x")
                    {
                        //for the moves 2 places to the left
                        if (row == moveInIntArray[1] - 4)
                        {
                            if (moveInIntArray[0] <= 14) possibleMoves[row, moveInIntArray[0] + 2] = "x";
                            if (moveInIntArray[0] >= 4) possibleMoves[row, moveInIntArray[0] - 2] = "x";
                        }

                        //for the moves 1 place to the left
                        if (row == moveInIntArray[1] - 2)
                        {
                            if (moveInIntArray[0] <= 14) possibleMoves[row, moveInIntArray[0] + 4] = "x";
                            if (moveInIntArray[0] >= 4) possibleMoves[row, moveInIntArray[0] - 4] = "x";
                        }

                        //for the moves 2 places to the right
                        if (row == moveInIntArray[1] + 4)
                        {
                            if (moveInIntArray[0] <= 14) possibleMoves[row, moveInIntArray[0] + 2] = "x";
                            if (moveInIntArray[0] >= 4) possibleMoves[row, moveInIntArray[0] - 2] = "x";
                        }

                        //for the moves 1 place to the right
                        if (row == moveInIntArray[1] + 2)
                        {
                            if (moveInIntArray[0] <= 14) possibleMoves[row, moveInIntArray[0] + 4] = "x";
                            if (moveInIntArray[0] >= 4) possibleMoves[row, moveInIntArray[0] - 4] = "x";
                        }
                    }
                }

                //if the move is in the table with possible moves, then move
                if (possibleMoves[moveInIntArray[3], moveInIntArray[2]] == "x") return true;
                //else:
                Console.WriteLine("This move is out of bounds for your knight! You cannot move there!");
                return false;
            }
            else if (pawn == "B")
            {
                Console.WriteLine(
                    $"{moveInIntArray[0]}, {moveInIntArray[1]}, {moveInIntArray[2]}, {moveInIntArray[3]}");
                //generate legal moves for every diagonal
                //look for maxXandMaxY
                for (int i = 2; i <= 16; i += 2)
                {
                    //generate maxXandMaxY - right and downside of board
                    if (moveInIntArray[1] + i <= 16 && moveInIntArray[0] + i <= 16)
                    {
                        //if the next position is occupied by an ally it means that the current position is the last one possible
                        if (currentPLayersCords[moveInIntArray[0] + i, moveInIntArray[1] + i] == "x")
                        {
                            possibleMoves[moveInIntArray[0], moveInIntArray[1]] = "x";
                            break;
                        }

                        possibleMoves[moveInIntArray[0] + i, moveInIntArray[1] + i] = "x";
                        //if the next position is occupied by an enemy it means that it is the last possible position
                        if (opponentCords[moveInIntArray[0] + i, moveInIntArray[1] + i] == "x")
                        {
                            break;
                        }
                    }
                }

                //look for maxXandMinY
                for (int i = 2; i <= 16; i += 2)
                {
                    //generate maxXandMinY - right and upside of board
                    if (moveInIntArray[1] + i <= 16 && moveInIntArray[0] - i >= 2)
                    {
                        //if the next position is occupied by an ally it means that the current position is the last one possible
                        if (currentPLayersCords[moveInIntArray[0] - i, moveInIntArray[1] + i] == "x")
                        {
                            possibleMoves[moveInIntArray[0], moveInIntArray[1]] = "x";
                            break;
                        }

                        possibleMoves[moveInIntArray[0] - i, moveInIntArray[1] + i] = "x";
                        //if the next position is occupied by an enemy it means that it is the last possible position
                        if (opponentCords[moveInIntArray[0] - i, moveInIntArray[1] + i] == "x")
                        {
                            Console.WriteLine("found and enemy maxXandMinY");
                            break;
                        }
                    }
                }

                //look for minXandMaxY
                for (int i = 2; i <= 16; i += 2)
                {
                    //minXandMaxY - left and downside of board
                    if (moveInIntArray[1] - i >= 2 && moveInIntArray[0] + i <= 16)
                    {
                        //if the next position is occupied by an ally it means that the current position is the last one possible
                        if (currentPLayersCords[moveInIntArray[0] + i, moveInIntArray[1] - i] == "x")
                        {
                            possibleMoves[moveInIntArray[0], moveInIntArray[1]] = "x";
                            break;
                        }

                        possibleMoves[moveInIntArray[0] + i, moveInIntArray[1] - i] = "x";
                        //if the next position is occupied by an enemy it means that it is the last possible position
                        if (opponentCords[moveInIntArray[0] + i, moveInIntArray[1] - i] == "x")
                        {
                            break;
                        }
                    }
                }

                //look for minXandMinY
                for (int i = 2; i <= 16; i += 2)
                {
                    //mixXandMinY - left and upside of board
                    if (moveInIntArray[1] - i >= 2 && moveInIntArray[0] - i >= 2)
                    {
                        //if the next position is occupied by an ally it means that the current position is the last one possible
                        if (currentPLayersCords[moveInIntArray[0] - i, moveInIntArray[1] - i] == "x")
                        {
                            possibleMoves[moveInIntArray[0], moveInIntArray[1]] = "x";
                            break;
                        }

                        possibleMoves[moveInIntArray[0] - i, moveInIntArray[1] - i] = "x";
                        //if the next position is occupied by an enemy it means that it is the last possible position
                        if (opponentCords[moveInIntArray[0] - i, moveInIntArray[1] - i] == "x")
                        {
                            break;
                        }
                    }
                }

                //if the move is in the table with possible moves, then move
                if (possibleMoves[moveInIntArray[2], moveInIntArray[3]] == "x") return true;
                //else:
                Console.WriteLine("This move is out of bounds for your Bishop! You cannot move there!");
                return false;
            }
            else if (pawn == "Q")
            { 
                if (possibleMoves[moveInIntArray[2], moveInIntArray[3]] == "x") return true;
                //else:
                Console.WriteLine("This move is out of bounds for your Queen! You cannot move there!");
                return false;
            }
            else if (pawn == "K")
            {
                //check if the new x coordinate is in bounds of the board - right side
                if (moveInIntArray[1] + 2 <= 16)
                {
                    //make move right possible
                    possibleMoves[moveInIntArray[0], moveInIntArray[1] + 2] = "x";
                    //make move right and up possible
                    if (moveInIntArray[0] - 2 >= 2) possibleMoves[moveInIntArray[0] - 2, moveInIntArray[1] + 2] = "x";
                    //make move right and down possible
                    if (moveInIntArray[0] + 2 <= 16) possibleMoves[moveInIntArray[0] + 2, moveInIntArray[1] + 2] = "x";
                }

                //check if the new x coordinate is in bounds of the board - left side
                if (moveInIntArray[1] - 2 >= 2)
                {
                    //make move left possible
                    possibleMoves[moveInIntArray[0], moveInIntArray[1] - 2] = "x";
                    //make move left and up possible
                    if (moveInIntArray[0] - 2 >= 2) possibleMoves[moveInIntArray[0] - 2, moveInIntArray[1] - 2] = "x";
                    //make move left and down possible
                    if (moveInIntArray[0] + 2 <= 16) possibleMoves[moveInIntArray[0] + 2, moveInIntArray[1] - 2] = "x";
                }

                //make move up possible
                if (moveInIntArray[0] - 2 >= 2) possibleMoves[moveInIntArray[0] - 2, moveInIntArray[1]] = "x";
                //make move down possible
                if (moveInIntArray[0] + 2 <= 16) possibleMoves[moveInIntArray[0] + 2, moveInIntArray[1]] = "x";
                //check if there are allies in the legal fields - if yes, remove that field from the possibleMoves table
                for (int row = 2; row <= 16; row += 2)
                {
                    for (int col = 2; col <= 16; col += 2)
                    {
                        if (possibleMoves[row, col] == "x" && currentPLayersCords[row, col] == "x")
                            possibleMoves[row, col] = "";
                    }
                }

                if (possibleMoves[moveInIntArray[2], moveInIntArray[3]] == "x") return true;
                //else:
                Console.WriteLine("This move is out of bounds for your King! You cannot move there!");
            }

            //get the cords of the opponents King
            for (int row = 2; row <= 16; row += 2)
            {
                for (int col = 2; col <= 16; col += 2)
                {
                    if (_chessBoard[row, col] == "K" && opponentCords[row, col] == "x")
                    {
                        opponentKingPosition[0] = row;
                        opponentKingPosition[1] = col;
                        break;
                    }
                }
            }
            //check for a check
            if (attacksAfterMove[opponentKingPosition[0], opponentKingPosition[1]] == "x")
            {
                //TODO: mark the King in check
            }
            return false;
        }

        static string[,] GenerateTableWithPossibleMoves(int startingPosX, int startingPosY, string pawn)
        {
            string[,] possibleMoves = new string[19, 19];
            string[,] opponentCords = _turnNumber % 2 == 0 ? _player2Coordinates : _player1Coordinates;
            string[,] currentPlayersCords = _turnNumber % 2 == 0 ? _player1Coordinates : _player2Coordinates;
            
            if (pawn == "Q")
            {
                //generate all possible moves in horizontal and vertical lines
                possibleMoves = GenerateHorizontalAndVerticalMoves(startingPosX, startingPosY, currentPlayersCords, opponentCords);

                //generate legal moves for every diagonal
                //look for maxX and maxY
                for (int i = 2; i <= 16; i += 2)
                {
                    //generate maxXandMaxY - right and downside of board
                    if (startingPosX + i <= 16 && startingPosY + i <= 16)
                    {
                        //if the next position is occupied by an ally it means that the current position is the last one possible
                        if (currentPlayersCords[startingPosY + i, startingPosX + i] == "x")
                        {
                            possibleMoves[startingPosY, startingPosX] = "x";
                            break;
                        }

                        possibleMoves[startingPosY + i, startingPosX + i] = "x";
                        //if the next position is occupied by an enemy it means that it is the last possible position
                        if (opponentCords[startingPosY + i, startingPosX + i] == "x")
                        {
                            break;
                        }
                    }
                }

                //look for maxX and minY
                for (int i = 2; i <= 16; i += 2)
                {
                    //generate maxXandMinY - right and upside of board
                    if (startingPosX + i <= 16 && startingPosY - i >= 2)
                    {
                        //if the next position is occupied by an ally it means that the current position is the last one possible
                        if (currentPlayersCords[startingPosY - i, startingPosX + i] == "x")
                        {
                            possibleMoves[startingPosY, startingPosX] = "x";
                            break;
                        }

                        possibleMoves[startingPosY - i, startingPosX + i] = "x";
                        //if the next position is occupied by an enemy it means that it is the last possible position
                        if (opponentCords[startingPosY - i, startingPosX + i] == "x")
                        {
                            Console.WriteLine("found and enemy maxXandMinY");
                            break;
                        }
                    }
                }

                //look for minX and maxY
                for (int i = 2; i <= 16; i += 2)
                {
                    //minXandMaxY - left and downside of board
                    if (startingPosX - i >= 2 && startingPosY + i <= 16)
                    {
                        //if the next position is occupied by an ally it means that the current position is the last one possible
                        if (currentPlayersCords[startingPosY + i, startingPosX - i] == "x")
                        {
                            possibleMoves[startingPosY, startingPosX] = "x";
                            break;
                        }

                        possibleMoves[startingPosY + i, startingPosX - i] = "x";
                        //if the next position is occupied by an enemy it means that it is the last possible position
                        if (opponentCords[startingPosY + i, startingPosX - i] == "x")
                        {
                            break;
                        }
                    }
                }

                //look for minX and minY
                for (int i = 2; i <= 16; i += 2)
                {
                    //mixXandMinY - left and upside of board
                    if (startingPosX - i >= 2 && startingPosY - i >= 2)
                    {
                        //if the next position is occupied by an ally it means that the current position is the last one possible
                        if (currentPlayersCords[startingPosY - i, startingPosX - i] == "x")
                        {
                            possibleMoves[startingPosY, startingPosX] = "x";
                            break;
                        }

                        possibleMoves[startingPosY - i, startingPosX - i] = "x";
                        //if the next position is occupied by an enemy it means that it is the last possible position
                        if (opponentCords[startingPosY - i, startingPosX - i] == "x")
                        {
                            break;
                        }
                    }
                }
            }

            return possibleMoves;
        }

        static string[,] GenerateHorizontalAndVerticalMoves(
            int startingPosX, 
            int startingPosY, 
            string[,] currentPlayersCords, 
            string[,] opponentCords
            )
        {
            string[,] possibleMoves = new string[19, 19];
            
            //look for maxX
                for (int i = 2; i <= 16; i += 2)
                {
                    //generate maxX - right side of the board
                    if (startingPosX + i <= 16)
                    {
                        //if the next position is occupied by an ally it means that the current position is the last one possible
                        if (currentPlayersCords[startingPosY, startingPosX + i] == "x")
                        {
                            possibleMoves[startingPosY, startingPosX] = "x";
                            break;
                        }

                        possibleMoves[startingPosY, startingPosX + i] = "x";
                        //if the next position is occupied by an enemy it means that it is the last possible position
                        if (opponentCords[startingPosY, startingPosX + i] == "x")
                        {
                            break;
                        }
                    }
                }

                //look for minX
                for (int i = 2; i <= 16; i += 2)
                {
                    //generate minX - left side of board
                    if (startingPosX - i >= 2)
                    {
                        //if the next position is occupied by an ally it means that the current position is the last one possible
                        if (currentPlayersCords[startingPosY, startingPosX - i] == "x")
                        {
                            possibleMoves[startingPosY, startingPosX] = "x";
                            break;
                        }

                        possibleMoves[startingPosY, startingPosX - i] = "x";
                        //if the next position is occupied by an enemy it means that it is the last possible position
                        if (opponentCords[startingPosY, startingPosX - i] == "x")
                        {
                            break;
                        }
                    }
                }

                //look for maxY
                for (int i = 2; i <= 16; i += 2)
                {
                    //maxY - downside of board
                    if (startingPosY + i <= 16)
                    {
                        //if the next position is occupied by an ally it means that the current position is the last one possible
                        if (currentPlayersCords[startingPosY + i, startingPosX] == "x")
                        {
                            possibleMoves[startingPosY, startingPosX] = "x";
                            break;
                        }

                        possibleMoves[startingPosY + i, startingPosX] = "x";
                        //if the next position is occupied by an enemy it means that it is the last possible position
                        if (opponentCords[startingPosY + i, startingPosX] == "x")
                        {
                            break;
                        }
                    }
                }

                //look for minY
                for (int i = 2; i <= 16; i += 2)
                {
                    //minY - upside of board
                    if (startingPosY - i >= 2)
                    {
                        //if the next position is occupied by an ally it means that the current position is the last one possible
                        if (currentPlayersCords[startingPosY - i, startingPosX] == "x")
                        {
                            possibleMoves[startingPosY, startingPosX] = "x";
                            break;
                        }

                        possibleMoves[startingPosY - i, startingPosX] = "x";
                        //if the next position is occupied by an enemy it means that it is the last possible position
                        if (opponentCords[startingPosY - i, startingPosX] == "x")
                        {
                            break;
                        }
                    }
                }
                
            return possibleMoves;
        }
    }
}