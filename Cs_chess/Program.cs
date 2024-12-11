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
            string[,] currentPLayersCords = turnNumber % 2 == 0 ? player1Cords : player2Cords;
            string[,] possibleMoves = new string[19, 19];
            int[] maxPositions = new int[4];
            if (pawn == "P")
            {
                //checking if the pawn doesn't move backwards - for player1 the difference should be negative
                if ((turnNumber % 2 == 0 && moveInIntArray[2] - moveInIntArray[0] < 0)
                    || (turnNumber % 2 == 1 && moveInIntArray[2] - moveInIntArray[0] > 0))
                {
                    if (
                        rowDifference <= 4 //if the pawn wants tp move two tiles
                        && (moveInIntArray[0] == 14 || moveInIntArray[0] == 4) //and it's on the correct starting position
                        && columnDifference == 0 //and it doesn't want to move sideways
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
                else
                {
                    Console.WriteLine("You cannot move backwards dumbass!");
                    return false;
                }
            }
            else if (pawn == "R")
            {
                //generate table possibleMoves with every legal move
                for (int row = 2; row < 17; row += 2) //create rows
                {
                    for (int col = 2; col < 17; col += 2) //create columns
                    {
                        //fill in all empty slots as 0
                        possibleMoves[row, col] = "0";
                        
                        //fill in the column in which the piece is sitting
                        if (moveInIntArray[1] == col)
                        {
                            possibleMoves[row, col] = "x";
                        }
                        // fill in the row in which the piece currently is
                         if (moveInIntArray[0] == row)
                        {
                            possibleMoves[row, col] = "x";
                        }

                        // mark the enemy as E
                        if (opponentCords[row, col] == "x")
                        {
                            possibleMoves[row, col] = "E";
                        }
                        // mark the ally as A
                        if (currentPLayersCords[row, col] == "x")
                        {
                            possibleMoves[row, col] = "A";
                        }
                        // mark the piece as an inaccessible field - 0
                        if (moveInIntArray[0] == row && moveInIntArray[1] == col)
                        {
                            possibleMoves[row, col] = "0";
                        }
                    }
                }
                //look for the max position in every direction
                //generate minX
                for (int i = moveInIntArray[1]; i > 2; i -= 2)
                {
                    if (possibleMoves[moveInIntArray[0], i] == "E")
                    {
                        //if the piece sees an enemy, the last field to attack can only be the enemy
                        maxPositions[0] = i;
                        break;
                    }
                    if (possibleMoves[moveInIntArray[0], i] == "A")
                    {
                        //if the piece sees an ally, the last legal field is one to the right
                        maxPositions[0] = i + 2;
                        break;
                    }
                }
                //if there are no enemies or allies, max boundary on the left side is 2
                if (maxPositions[0] == 0) maxPositions[0] = 2;
                //generate maxX
                for (int i = moveInIntArray[1]; i < 17; i += 2)
                {
                    if (possibleMoves[moveInIntArray[0], i] == "E")
                    {
                        //if the piece sees an enemy, the last field to attack can only be the enemy
                        maxPositions[1] = i;
                        break;
                    }
                    if (possibleMoves[moveInIntArray[0], i] == "A")
                    {
                        //if the piece sees an ally, the last legal field is one to the right
                        maxPositions[1] = i - 2;
                        break;
                    }
                }
                //if there are no enemies or allies, max boundary on the right side is 16
                if (maxPositions[1] == 0) maxPositions[1] = 16;
                //generate minY
                for (int i = moveInIntArray[0]; i > 2; i -= 2)
                {
                    if (possibleMoves[i, moveInIntArray[1]] == "E")
                    {
                        //if the piece sees an enemy, the last field to attack can only be the enemy
                        maxPositions[2] = i;
                        break;
                    }
                    if (possibleMoves[i, moveInIntArray[1]] == "A")
                    {
                        //if the piece sees an ally, the last legal field is one to the right
                        maxPositions[2] = i + 2;
                        break;
                    }
                }
                //if there are no enemies or allies, max boundary on the upside is 2
                if (maxPositions[2] == 0) maxPositions[2] = 2;
                //generate maxY
                for (int i = moveInIntArray[0]; i < 17; i += 2)
                {
                    if (possibleMoves[i, moveInIntArray[1]] == "E")
                    {
                        //if the piece sees an enemy, the last field to attack can only be the enemy
                        maxPositions[3] = i;
                        break;
                    }
                    if (possibleMoves[i, moveInIntArray[1]] == "A")
                    {
                        //if the piece sees an ally, the last legal field is one to the right
                        maxPositions[3] = i - 2;
                        break;
                    }
                }
                //if there are no enemies or allies, max boundary on the downside is 16
                if (maxPositions[3] == 0) maxPositions[3] = 16;
                //if the move is in the bounds of the max coordinates, it means you can move the piece
                if (moveInIntArray[2] >= maxPositions[2] //if the row number is greater than or equal minY
                    && moveInIntArray[2] <= maxPositions[3] //and it's smaller or equal to maxY
                    && moveInIntArray[3] >= maxPositions[0] //and the column number is greater than or equal minX
                    && moveInIntArray[3] <= maxPositions[1]) //and it's smaller or equal to maxX
                    return true;
                else
                {
                    Console.WriteLine("This move is out of bounds! You cannot move your rook there!");
                    return false;
                }
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
                //TODO: check if the chosen field is occupied by an ally or by an enemy for the bishop
                Console.WriteLine($"{moveInIntArray[0]}, {moveInIntArray[1]}, {moveInIntArray[2]}, {moveInIntArray[3]}");
                //generate legal moves for every diagonal
                for (int i = 2; i <= 16; i += 2)
                {
                    //generate maxXandMaxY - right and downside of board
                    if (moveInIntArray[1] + i <= 16 && moveInIntArray[0] + i <= 16) possibleMoves[moveInIntArray[0] + i, moveInIntArray[1] + i] = "x";
                    //generate maxXandMinY - right and upside of board
                    if (moveInIntArray[1] + i <= 16 && moveInIntArray[0] - i >= 2) possibleMoves[moveInIntArray[0] - i, moveInIntArray[1] + i] = "x";
                }
                for (int i = 16; i >= 2; i -= 2)
                {
                    //mixXandMaxY - left and downside of board
                    if (moveInIntArray[1] - i >= 2 && moveInIntArray[0] + i <= 16) possibleMoves[moveInIntArray[0] + i, moveInIntArray[1] - i] = "x";
                    //mixXandMinY - left and upside of board
                    if (moveInIntArray[1] - i >= 2 && moveInIntArray[0] - i >= 2) possibleMoves[moveInIntArray[0] - i, moveInIntArray[1] - i] = "x";
                }
                //if the move is in the table with possible moves, then move
                if (possibleMoves[moveInIntArray[2], moveInIntArray[3]] == "x") return true;
                //else:
                Console.WriteLine("This move is out of bounds for your Bishop! You cannot move there!");
                return false;
            }
            else if (pawn == "Q")
            {
                //TODO: check if the chosen field is occupied by an ally or by an enemy for the queen
                //generate all possible moves in the x and y axis
                for (int row = 2; row < 17; row += 2) //create rows
                {
                    for (int col = 2; col < 17; col += 2) //create columns
                    {
                        //fill in all empty slots as 0
                        possibleMoves[row, col] = "0";
                        
                        //fill in the column in which the piece is sitting
                        if (moveInIntArray[1] == col)
                        {
                            possibleMoves[row, col] = "x";
                        }
                        // fill in the row in which the piece currently is
                        if (moveInIntArray[0] == row)
                        {
                            possibleMoves[row, col] = "x";
                        }

                        // mark the enemy as E
                        if (opponentCords[row, col] == "x")
                        {
                            possibleMoves[row, col] = "E";
                        }
                        // mark the ally as A
                        if (currentPLayersCords[row, col] == "x")
                        {
                            possibleMoves[row, col] = "A";
                        }
                        // mark the piece as an inaccessible field - 0
                        if (moveInIntArray[0] == row && moveInIntArray[1] == col)
                        {
                            possibleMoves[row, col] = "0";
                        }
                    }
                }
                //generate legal moves for every diagonal
                //diagonal right up and down
                for (int i = 2; i <= 16; i += 2)
                {
                    //generate maxXandMaxY - right and downside of board
                    if (moveInIntArray[1] + i <= 16 && moveInIntArray[0] + i <= 16) possibleMoves[moveInIntArray[0] + i, moveInIntArray[1] + i] = "x";
                    //generate maxXandMinY - right and upside of board
                    if (moveInIntArray[1] + i <= 16 && moveInIntArray[0] - i >= 2) possibleMoves[moveInIntArray[0] - i, moveInIntArray[1] + i] = "x";
                }
                //diagonal left up and down
                for (int i = 16; i >= 2; i -= 2)
                {
                    //mixXandMaxY - left and downside of board
                    if (moveInIntArray[1] - i >= 2 && moveInIntArray[0] + i <= 16) possibleMoves[moveInIntArray[0] + i, moveInIntArray[1] - i] = "x";
                    //mixXandMinY - left and upside of board
                    if (moveInIntArray[1] - i >= 2 && moveInIntArray[0] - i >= 2) possibleMoves[moveInIntArray[0] - i, moveInIntArray[1] - i] = "x";
                }
                //if the move is in the table with possible moves, then move
                if (possibleMoves[moveInIntArray[2], moveInIntArray[3]] == "x") return true;
                //else:
                Console.WriteLine("This move is out of bounds for your Queen! You cannot move there!");
                return false;
            }
            return false;
        }
        
    }
}