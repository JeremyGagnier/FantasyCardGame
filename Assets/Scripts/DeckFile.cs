using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class DeckFile
{
    private static string DECK_DIRECTORY = "decks/";
    private static string _decksPath = null;
    private static string decksPath
    {
        get
        {
            if (_decksPath == null)
            {
                _decksPath = FileManager.appDataPath + DECK_DIRECTORY;
                if (!Directory.Exists(_decksPath))
                {
                    Directory.CreateDirectory(_decksPath);
                }
            }
            return _decksPath;
        }
    }

    public readonly Hero hero;
    public readonly List<Card> cards = new List<Card>();
    public DeckFile(string deckName)
    {
        try
        {
            StreamReader deckFile = new StreamReader(decksPath);
            string heroName = deckFile.ReadLine();
            hero = Hero.byName[heroName];

            while (!deckFile.EndOfStream)
            {
                string cardName = deckFile.ReadLine();
                cards.Add(Card.byName[cardName]);
            }

            if (cards.Count != GameRules.DECK_SIZE)
            {
                Debug.LogError(string.Format(
                    "Deck '{0}' has {1} cards when it should have {2}.",
                    deckName,
                    cards.Count.ToString(),
                    GameRules.DECK_SIZE.ToString()));
            }
        }
        catch (FileNotFoundException)
        {
            Debug.LogError("Deck file '" + deckName + "' not found.");
        }
    }
}
