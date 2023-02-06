using System;
using System.Collections.Generic;
using System.Linq;
namespace QuiddlerLibrary
{

    public class Deck : IDeck
    {
        
        internal Dictionary<string, int> TotalCountForEachCard;
        private int cardsPerPlayer;
        internal string topDiscard;
        internal Dictionary<string, int> CardPointValues;
        internal List<string> CardDeck;

        public Deck()
        {
            TotalCountForEachCard = new Dictionary<string, int>();
            CardPointValues = new Dictionary<string, int>();
            CardDeck = new List<string>();

            InitializeCardDeck();
        }
   
        public int CardCount => CardDeck.Count;

        public int CardsPerPlayer 
        {

            get
            {
                return cardsPerPlayer;
            }
            set
            {
                cardsPerPlayer = value;
            }
        
        }

        public string TopDiscard
        {
            get 
            {
                
                if(topDiscard == null)
                {
                    var random = new Random();
                    int index = random.Next(0,CardDeck.Count -1);
                    topDiscard = CardDeck[index];
                    CardDeck.RemoveAt(index);
                   
                }
                return topDiscard;


            } 
        }

        public string About => "Test Client for: Quiddler (TM) Library, © 2022 S.Gault, P.Tracey ";
        
        private void InitializeCardPointValues()
        {
            CardPointValues.Add("a", 2);
            CardPointValues.Add("e", 2);
            CardPointValues.Add("i", 2);
            CardPointValues.Add("o", 2);
            CardPointValues.Add("l", 3); 
            CardPointValues.Add("s", 3);
            CardPointValues.Add("t", 3);
            CardPointValues.Add("u", 4);
            CardPointValues.Add("y", 4);
            CardPointValues.Add("d", 5);
            CardPointValues.Add("m", 5);
            CardPointValues.Add("n", 5);
            CardPointValues.Add("r", 5);
            CardPointValues.Add("f", 6);
            CardPointValues.Add("g", 6);
            CardPointValues.Add("p", 6);
            CardPointValues.Add("h", 7);
            CardPointValues.Add("er", 7);
            CardPointValues.Add("in", 7);
            CardPointValues.Add("b", 8);
            CardPointValues.Add("c", 8);
            CardPointValues.Add("k", 8);
            CardPointValues.Add("qu", 9);
            CardPointValues.Add("th", 9);
            CardPointValues.Add("w", 10);
            CardPointValues.Add("cl", 10);
            CardPointValues.Add("v", 11);
            CardPointValues.Add("x", 12);
            CardPointValues.Add("j", 13);
            CardPointValues.Add("z", 14);
            CardPointValues.Add("q", 15);

        }
        private void InitializeNumCards()
        {
            TotalCountForEachCard.Clear();
            TotalCountForEachCard.Add("b", 2);
            TotalCountForEachCard.Add("c", 2);
            TotalCountForEachCard.Add("f", 2);
            TotalCountForEachCard.Add("h", 2);
            TotalCountForEachCard.Add("j", 2);
            TotalCountForEachCard.Add("k", 2);
            TotalCountForEachCard.Add("m", 2);
            TotalCountForEachCard.Add("p", 2);
            TotalCountForEachCard.Add("q", 2);
            TotalCountForEachCard.Add("v", 2);
            TotalCountForEachCard.Add("w", 2);
            TotalCountForEachCard.Add("x", 2);
            TotalCountForEachCard.Add("z", 2);
            TotalCountForEachCard.Add("cl", 2);
            TotalCountForEachCard.Add("er", 2);
            TotalCountForEachCard.Add("in", 2);
            TotalCountForEachCard.Add("qu", 2);
            TotalCountForEachCard.Add("th", 2);
            TotalCountForEachCard.Add("d", 4);
            TotalCountForEachCard.Add("g", 4);
            TotalCountForEachCard.Add("l", 4);
            TotalCountForEachCard.Add("s", 4);
            TotalCountForEachCard.Add("y", 4);
            TotalCountForEachCard.Add("n", 6);
            TotalCountForEachCard.Add("r", 6);
            TotalCountForEachCard.Add("t", 6);
            TotalCountForEachCard.Add("u", 6);
            TotalCountForEachCard.Add("i", 8);
            TotalCountForEachCard.Add("o", 8);
            TotalCountForEachCard.Add("a", 10);
            TotalCountForEachCard.Add("e", 12);
        }
        private void InitializeCardDeck()
        {
            InitializeCardPointValues();
            InitializeNumCards();
            foreach (var c in TotalCountForEachCard)
            {
                for (int i = 0; i < c.Value; i++)
                {
                    CardDeck.Add(c.Key);
                }
            }
           
            
        }
    
        public IPlayer NewPlayer()
        {

            Player p = new Player(this);
            for(int i = 0; i < CardsPerPlayer; i++)
            {
                p.DrawCard();
            }

            return p;

        }
        public override string ToString()
        {
            List<string> formatted = new List<string>();
            formatted = CardDeck;
            formatted.Sort();
            var grouped = formatted.GroupBy(i => i);
            int i = 1;
            string format = "";
           
            foreach (var grp in grouped)
            {
                format += string.Format("{0,6}",$"{grp.Key}({grp.Count()})");
                if (i % 10 == 0 && i != 0)
                    format += "\n";
                i++;
            }
            return format;
        }
    }
}
