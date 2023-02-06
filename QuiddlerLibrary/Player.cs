using System;
using System.Collections.Generic;
using System.Linq;
using Word_Object_Library;

namespace QuiddlerLibrary 
{

    public class Player : IPlayer
    {
        public int NumPoints;
        private List<string> cards = new List<string>();
        internal Deck Dealer;

        public int CardCount { get { return cards.Count; } }

        public int TotalPoints
        {
            get { return NumPoints; }
        }

        internal Player(Deck d)
        {
            //deck object = d
            //that's how you can use it for drawing cards since it'll use the same deck instance
            Dealer = d;
            NumPoints = 0;  
            
        }
        public bool Discard(string card)
        {
            if (card != "")
            {
                if (cards.Contains(card))
                {
                    Dealer.topDiscard = card;
                    cards.Remove(card);//make card on top of discard pile
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                cards.Remove(cards.First());
                return true;
            }            
        }

        public string DrawCard()
        {
            if(Dealer.CardCount == 0)
            {
                throw new InvalidOperationException();
            }
            else
            {
                var random = new Random();
                int index = random.Next(0,Dealer.CardDeck.Count -1);
                cards.Add(Dealer.CardDeck[index]);
                Dealer.CardDeck.RemoveAt(index);
                return cards.ElementAt(cards.Count - 1); 
            }           
        }

        public string PickupTopDiscard()//returns the value of the card as a string
        {
            cards.Add(Dealer.TopDiscard);
            return Dealer.TopDiscard;          
        }

        public int PlayWord(string candidate)
        {
            string temp ="";
            if(TestWord(candidate) > 0 )
            {
                //need to remove the letters/cards(To discard pile) if played
                NumPoints += TestWord(candidate);
                    for (int x = 0; x < candidate.Count(); x++)
                    {
                      if (candidate[x].ToString() != " ")
                      {
                        temp += candidate[x].ToString();
                      }
                      else
                      {
                        Discard(temp);
                        temp = "";
                      }
                    }
                Discard(temp);

                return TestWord(candidate);
            }
            throw new NotImplementedException();
        }

        public int TestWord(string candidate)
        {

            List<string> cardList = new List<string>();
            string card = "";
            int pointsPerCard = 0;
            int totalPoints = 0;

            string word = String.Concat(candidate.Where(x => !Char.IsWhiteSpace(x)));

            if (CardCount != 0 && WordObjectLibrary.CheckSpelling(word))
            {
                for(int x = 0; x<=candidate.Count()-1;x++)
                {                    
                    if(candidate[x].ToString() != " ")
                    {
                        card += candidate[x].ToString();
                    }else
                    {
                        cardList.Add(card);
                        card = " ";                        
                    }                        
                }
                cardList.Add(card);

                foreach (string s in cardList)
                {
                    if (Dealer.CardPointValues.ContainsKey(s.Trim()) && cards.Contains(s.Trim()))//Checks if letter is in dicitionary / no spaces  between letters
                    {
                        if (candidate.Contains(s))// gets points
                        {
                            candidate.Remove(candidate.IndexOf(s)); //Remove so it doesn't check the contain method on that card if there is doubles  
                            pointsPerCard = Dealer.CardPointValues[s.Trim()];
                            totalPoints += pointsPerCard;
                        }
                        else
                            return 0;
                    }
                    else
                        return 0;
                }
                return totalPoints;
            }
            else            
                return 0;            
        }

        public override string ToString()
        {
            string letters= "";
            letters += "[";
            foreach (string s in cards){letters += " " + s;}
            letters += " ]";
            return letters;
        }
    }
}
