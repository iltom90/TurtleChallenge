using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurtleChallenge
{
    class Program
    {
        static void Main(string[] args)
        {
        start:
            try
            {
                Console.Clear();
                Console.WriteLine("WELCOME ON TURTLE CHALLANGE!");
                Settings sett = new Settings();
                Moves moves = new Moves();
                if (args.Length >= 2)
                {
                    //Load settings and moves                                        
                    sett.LoadSettings(args[0]);
                    moves.LoadMoves(args[1]);                                       
                }
                else
                {
                    Console.WriteLine("Please insert path of settings file:");
                    sett.LoadSettings(Console.ReadLine());
                    Console.WriteLine("Please insert path of moves file:");
                    moves.LoadMoves(Console.ReadLine());
                }
                Console.Clear();
                StartGame(sett, moves);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Some errors occur: {ex.Message}");
            }
            Console.WriteLine("END GAME!");
            Console.WriteLine($"Press any key to restart or ESC to close");
            var key = Console.ReadKey();
            switch (key.Key)
            {
                case ConsoleKey.Escape:
                    Environment.Exit(0);
                    break;
                default:
                    goto start;
            }
        }

        public static void StartGame(Settings settings,Moves moves)
        {
            Int32[,] matrixBoard = SetScenario(settings);
            Position turtlePosition = settings.startPosition;
            Console.WriteLine("GAME STARTED!");
            int nSequence = 1;
            foreach(Move move in moves.moves)
            {
                MoveTurtle(move, matrixBoard,ref turtlePosition);
                Console.Write($"Sequence {nSequence} : ");
                if (!VerifyPosition(matrixBoard, turtlePosition))
                    break;
                nSequence++;
            }            
        }

        /// <summary>
        /// Function to verify if the turtle hit a mine, find exit or is out of filed
        /// </summary>
        /// <param name="matrixBoard"></param>
        /// <param name="turtlePosition"></param>
        /// <returns></returns>
        public static bool VerifyPosition(int[,] matrixBoard,Position turtlePosition)
        {
            try
            {
                if (matrixBoard[turtlePosition.posX, turtlePosition.posY] == (int)turtleGame.Exit)
                {
                    Console.WriteLine("Exit found!!!");
                }
                else if (matrixBoard[turtlePosition.posX, turtlePosition.posY] == (int)turtleGame.Mine)
                {
                    Console.WriteLine("Hit mine!");
                }
                else
                {
                    Console.WriteLine("Success!");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("The tourtle are exit from field!");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Function to move to position passed as parameter
        /// </summary>
        /// <param name="move"></param>
        /// <param name="matrixBoard"></param>
        /// <param name="turtlePosition"></param>
        /// <returns></returns>
        public static void MoveTurtle(Move move,Int32[,] matrixBoard, ref Position turtlePosition)
        {
            switch (move.move)
            {
                case "rotate":
                    switch(turtlePosition.direction)
                    {
                        case "north":
                            turtlePosition.direction = "est";
                            break;
                        case "est":
                            turtlePosition.direction = "south";
                            break;
                        case "south":
                            turtlePosition.direction = "west";
                            break;
                        case "west":
                            turtlePosition.direction = "north";
                            break;
                        default:
                            Console.WriteLine("Direction not recognised! check input files");
                            throw new Exception();
                            break;
                    }
                    break;
                case "forward":
                    switch (turtlePosition.direction)
                    {
                        case "north":
                            turtlePosition.posY -= 1;
                            break;
                        case "est":
                            turtlePosition.posX += 1;
                            break;
                        case "south":
                            turtlePosition.posY += 1;
                            break;
                        case "west":
                            turtlePosition.posX -= 1;
                            break;
                        default:
                            Console.WriteLine("Direction not recognised! check input files");
                            throw new Exception();
                            break;
                    }
                    break;
                case null:
                    break;
                default:
                    Console.WriteLine($"Move: {move.move} not recognised!");
                    break;
            }
        }

        /// <summary>
        /// Function for setup the field with mine,exit
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static Int32[,] SetScenario(Settings settings)
        {
            Int32[,] matrixBoard = new Int32[settings.boardSettings.boardColumns, settings.boardSettings.boardRows];
            if (settings.exitPoint.posX <= 0)
                throw new Exception("Impossible to set Exit x point at values less or equal then 0!");
            if (settings.exitPoint.posY <= 0)
                throw new Exception("Impossible to set Exit y point at values less or equal then 0!");
            matrixBoard[settings.exitPoint.posX -1, settings.exitPoint.posY -1] = (int)turtleGame.Exit;
            foreach(Mine mine in settings.mines)
            {
                try
                {
                    matrixBoard[mine.posX - 1, mine.posY - 1] = (int)turtleGame.Mine;
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"Impossible to apply mine at position: {mine.posX},{mine.posY}!");
                }                
            }
            return matrixBoard;
        }
    }
}
