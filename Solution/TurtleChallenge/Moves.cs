using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurtleChallenge
{
    public class Moves
    {
       public List<Move> moves { get; set; }

        public void LoadMoves(string path)
        {
            moves = JsonConvert.DeserializeObject<Moves>(File.ReadAllText(path)).moves;
            if (moves == null)
                throw new Exception("Impossible to load moves, please check input file");
        }
    }

    public class Move
    {
        public string move { get; set; }
    }
}
