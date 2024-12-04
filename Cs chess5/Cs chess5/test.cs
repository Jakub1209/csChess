// using System;
//
// namespace Jakub_Szewczyk_71695_Szachy
// {
//     public class test
//     {
//         public static void Main(string[] args)
//         {
//             Console.Write("Make a move: ");
//             string move = Console.ReadLine();
//             int[] moveCharArray = new int[4];
//             int arrayIndex = 0;
//             
//             foreach (char character in move)
//             {
//                 int charToInt = character;
//                 moveCharArray[arrayIndex] = charToInt;
//                 arrayIndex++;
//             }
//             
//             for (int i = 0; i < moveCharArray.GetLength(0); i++)
//             {
//                 Console.WriteLine(moveCharArray[i]);
//             }
//         }
//     }
// }