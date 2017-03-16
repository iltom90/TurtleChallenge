using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurtleChallenge
{
    public class Settings
    {
        public Board boardSettings { get; set; }
        public Position startPosition { get; set; }
        public ExitPoint exitPoint { get; set; }
        public List<Mine> mines { get; set; }

        public void LoadSettings(string path)
        {
            Settings settings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(path));
            if (settings.boardSettings != null)
            {
                boardSettings = settings.boardSettings;
                if (settings.boardSettings.boardRows < 0 || settings.boardSettings.boardColumns < 0)
                    throw new Exception($"Impossible to load field setting (Rows: {settings.boardSettings.boardRows}, Columns: {settings.boardSettings.boardColumns})");
                if (settings.startPosition.posX > settings.boardSettings.boardColumns || settings.startPosition.posX < 0)
                    throw new Exception($"Impossible to load start position (X coordiante: {settings.startPosition.posX})");
                if (settings.startPosition.posY > settings.boardSettings.boardRows || settings.startPosition.posY < 0)
                    throw new Exception($"Impossible to load start position (Y coordiante: {settings.startPosition.posY})");
                if (settings.exitPoint.posX > settings.boardSettings.boardColumns || settings.exitPoint.posX < 0)
                    throw new Exception($"Impossible to load exit position (X coordiante: {settings.exitPoint.posX})");
                if (settings.exitPoint.posY > settings.boardSettings.boardRows || settings.exitPoint.posY < 0)
                    throw new Exception($"Impossible to load exit position (Y coordiante: {settings.exitPoint.posY})");
                startPosition = settings.startPosition;
                exitPoint = settings.exitPoint;
                mines = settings.mines;
            }
            else
            {
                throw new Exception($"Impossible to load settings, please check input file");
            }
        }
    }

    public class Board
    {
        public Int32 boardColumns { get; set; }
        public Int32 boardRows { get; set; }
    }

    public class Position
    {
        public Int32 posX { get; set; }
        public Int32 posY { get; set; }
        public string direction { get; set; }
    }

    public class ExitPoint
    {
        public Int32 posX { get; set; }
        public Int32 posY { get; set; }
    }

    public class Mine
    {
        public Int32 posX { get; set; }
        public Int32 posY { get; set; }
    }

    public enum turtleGame
    {
        Exit = 5,
        Mine = 8
    }
}
