using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuiddlerLibrary
{
    public interface IDeck
    {
        public IPlayer NewPlayer();
        public string ToString();
        public string About { get; }
        public int CardCount { get; }
        public int CardsPerPlayer { get; set; }
        public string TopDiscard { get; }
    }
}
