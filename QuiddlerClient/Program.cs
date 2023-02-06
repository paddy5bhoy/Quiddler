using System;
using System.Collections.Generic;
using System.Reflection;
using QuiddlerLibrary;
using Word_Object_Library;

namespace QuiddlerClient
{
    public class Program
    {
        static void Main(string[] args)
        {
            IDeck deck = new Deck();
            bool loopFlag = true;
            bool check = true;
            bool discardResult;
            bool finalFlag = true;
            bool emptyCards = false;
            int numPlayers = 0;
            int numCardsPerPlayer = 0;
            int playerID;
            string response;
            string word;
            int points;
            bool innerLoopFlag;

            Console.WriteLine(deck.About);
            List<IPlayer> playerList = new List<IPlayer>();
            Console.WriteLine($"Deck initialized with the following {deck.CardCount} cards...");
            Console.WriteLine(deck.ToString());

            do
            {
                loopFlag = true;
                Console.Write("How many players are there? (1-8): ");
                response = Console.ReadLine();
                check = Int32.TryParse(response, out numPlayers);
                if (check)
                {
                    numPlayers = Int32.Parse(response);
                    if (Int32.Parse(response) >= 1 && Int32.Parse(response) <= 9)
                    {
                        numPlayers = Int32.Parse(response);
                        loopFlag = true;
                    }
                    else
                    {
                        Console.WriteLine("Error: Must be a number between 1 and 10");
                        loopFlag = false;
                    }
                }
                else
                {
                    Console.WriteLine("Error: Must be a number");
                    loopFlag = false;
                }


            } while (!loopFlag);

            do
            {
                loopFlag = true;
                Console.Write("\nHow many cards will be dealt to each player? (3-10): ");
                response = Console.ReadLine();
                check = Int32.TryParse(response, out numCardsPerPlayer);
                if (check)
                {
                    numCardsPerPlayer = Int32.Parse(response);
                    if (numCardsPerPlayer >= 3 && numCardsPerPlayer <= 10)
                    {
                        deck.CardsPerPlayer = numCardsPerPlayer;
                    }
                    else
                    {
                        Console.WriteLine("Error: Must be a number between 3 and 10");
                        loopFlag = false;
                    }
                }
                else
                {
                    Console.WriteLine("Error: Must be a number");
                    loopFlag = false;
                }

            } while (!loopFlag);
            for (int i = 0; i < numPlayers; i++)
                playerList.Add(deck.NewPlayer());

            Console.WriteLine($"Cards were dealt to {numPlayers} player(s)");
            Console.WriteLine($"The top card which was '{deck.TopDiscard}' was moved to the discard pile");

            //the main game
            do
            {
                playerID = 1;
                foreach (IPlayer player in playerList)
                {
                    Console.WriteLine("\n-----------------------------");
                    Console.WriteLine($"Player {playerID} ({player.TotalPoints} points)");
                    Console.WriteLine("-----------------------------");
                    Console.WriteLine("\n");
                    Console.WriteLine($"The deck now contains the following {deck.CardCount} cards");
                    Console.WriteLine(deck.ToString());

                    Console.WriteLine($"Your cards are {player.ToString()}");
                    loopFlag = true;
                    if (player.CardCount != 0)
                    {
                        do
                        {
                            Console.Write($"Do you want the top card in the discard pile which is '{deck.TopDiscard}' (y/n)");
                            response = Console.ReadLine();
                            response = response.ToLower();
                            if (response == "y")
                            {

                                player.PickupTopDiscard();
                                Console.WriteLine($"Your cards are {player.ToString()}");

                                loopFlag = true;
                            }
                            else if (response == "n")
                            {
                                string returned = player.DrawCard();
                                Console.WriteLine($"The dealer dealt you '{returned}' from the deck.");
                                Console.WriteLine($"The deck contains {deck.CardCount} cards.");
                                Console.WriteLine($"Your cards are {player.ToString()}");
                                loopFlag = true;
                            }
                            else
                            {
                                Console.WriteLine("Error: Invalid input. Must be 'y' or 'n'");
                                loopFlag = false;
                            }
                        } while (!loopFlag);

                        innerLoopFlag = true;
                        do
                        {
                            Console.WriteLine("Test a word for it's point value? (y/n)");
                            response = Console.ReadLine();
                            response = response.ToLower();
                            if (response == "y")
                            {
                                Console.Write($"Enter a word using {player.ToString()} leaving a space between cards: ");
                                word = Console.ReadLine();

                                points = player.TestWord(word);
                                if (points == 0)
                                {
                                    loopFlag = false;
                                    Console.WriteLine($"The word [ { word} ] is worth {points}  points.");
                                }
                                else
                                {
                                    Console.WriteLine($"The word [ {word} ] is worth {points}  points.");
                                    Console.WriteLine($"Do you want to play the word [ {word} ]? (y/n): ");
                                    response = Console.ReadLine();
                                    response = response.ToLower();

                                    if (response == "y")
                                    {
                                        player.PlayWord(word);
                                        Console.WriteLine($"Your cards are {player.ToString()} and you have {player.TotalPoints}  points.");
                                        do
                                        {
                                            if (player.CardCount != 0)
                                            {
                                                if (player.CardCount > 1)
                                                {
                                                    Console.Write($"Enter a card from your hand to drop on the discard pile: ");
                                                    response = Console.ReadLine();
                                                    discardResult = player.Discard(response);
                                                    if (discardResult)
                                                    {
                                                        if (player.CardCount == 0)
                                                        {
                                                            emptyCards = true;
                                                            Console.WriteLine($"Player {playerID} is Out of cards");
                                                            break;
                                                        }
                                                        Console.WriteLine($"Your cards are {player.ToString()}");
                                                        innerLoopFlag = true;
                                                    }
                                                    else
                                                    {
                                                        innerLoopFlag = false;
                                                        Console.WriteLine($"Error. '{response}' is not a card in the players hand");
                                                    }
                                                }
                                                else
                                                {
                                                    Console.WriteLine($"Dropped {player.ToString()} from player {playerID}'s hand ");
                                                    Console.WriteLine($"Player {playerID} is Out of cards");
                                                    emptyCards = true;
                                                    player.Discard("");
                                                    break;
                                                }
                                            }
                                            else
                                            {
                                                emptyCards = true;
                                                Console.WriteLine($"Player {playerID} is Out of cards");
                                                break;
                                            }
                                        } while (!innerLoopFlag);

                                        loopFlag = true;
                                    }
                                    else if (response == "n")
                                    {
                                        do
                                        {
                                            Console.Write($"Enter a card from your hand to drop on the discard pile: ");
                                            response = Console.ReadLine();
                                            discardResult = player.Discard(response);
                                            if (discardResult)
                                            {
                                                Console.WriteLine($"Your cards are {player.ToString()}");
                                                innerLoopFlag = true;
                                            }
                                            else
                                            {
                                                innerLoopFlag = false;
                                                Console.WriteLine($"Error. '{response}' is not a card in the players hand");
                                            }
                                        } while (!innerLoopFlag);

                                        loopFlag = true;
                                    }
                                    else
                                    {
                                        loopFlag = false;
                                        Console.WriteLine("Error: Invalid input. Must be 'y' or 'n' ");
                                    }
                                }
                            }

                            else if (response == "n")
                            {
                                do
                                {
                                    Console.Write("Enter a card from your hand to drop on the discard pile: ");
                                    response = Console.ReadLine();
                                    discardResult = player.Discard(response);
                                    if (discardResult)
                                    {
                                        Console.WriteLine($"Your cards are {player.ToString()}");
                                        innerLoopFlag = true;
                                    }
                                    else
                                    {
                                        innerLoopFlag = false;
                                        Console.WriteLine($"Error. '{response}' is not a card in the players hand");
                                    }
                                } while (!innerLoopFlag);

                                loopFlag = true;
                            }
                            else
                            {
                                loopFlag = false;
                                Console.WriteLine("Error: Invalid input. Must be 'y' or 'n' ");
                            }

                        } while (!loopFlag);

                        playerID++;
                    }
                    else
                    {
                        Console.WriteLine($"Player {playerID} is Out of cards");
                        loopFlag = true;
                        emptyCards = true;
                        break;
                    }
                }//end foreach
                do
                {
                    if (emptyCards == false)
                    {
                        Console.Write("Would you like each player to take another turn? (y/n): ");
                        string message = Console.ReadLine();
                        message = message.ToLower();
                        if (message == "y")
                        {
                            finalFlag = true;
                            loopFlag = false;
                        }
                        else if (message == "n")
                        {
                            finalFlag = true;
                            loopFlag = true;
                        }
                        else
                        {
                            finalFlag = false;
                            Console.WriteLine("Error: Invalid input. Must be 'y' or 'n'");
                        }
                    }
                    else
                    {
                        loopFlag = true;
                        break;
                    }
                } while (!finalFlag);

            } while (!loopFlag);


            Console.WriteLine("Retiring the game.");
            Console.WriteLine("\nThe Final Scores are...");
            Console.WriteLine("------------------------------------------");
            playerID = 1;
            foreach (IPlayer player in playerList)
            {
                Console.WriteLine($"Player {playerID++}: {player.TotalPoints} points"); // added player Id increment
            }
            WordObjectLibrary.Quit();
        }
    }
}
