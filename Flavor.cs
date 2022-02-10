using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BDSP_Randomizer.GlobalData;

namespace BDSP_Randomizer
{
    /// <summary>
    ///  Responsible for ensuring an enjoyable experience for the user. Don't worry about it.
    /// </summary>
    public class Flavor
    {
        private Random rng = new();

        private readonly string[] verbs = new string[]
        {
            "Inspectin'", "Inspectigatin'", "Inspectimigatin'",
            "Perusin'", "Perusigatin'", "Perusulatin'",
            "Checkin'", "Checkin' out",
            "Examinin'", "Examigatin'",
            "Reviewin'",
            "Scannin'", "Doin' the scan on",
            "Scrutinizin'", "Scrutinatin'", "Scrutilatin'",
            "Porin' over",
            "Researchin'", "Researchigatin'",
            "Analyzin'", "Analysisin'",
            "Parsin'",
            "Delvin' into", "Doin' a delve into",
            "Explorin'", "Exploratin'",
            "Investigatin'", "Investimigatin'", "Investigaterin'",
            "Probin'",
            "Studyin'", "Studifyin'",
            "Categorizin'", "Categorygatin'",
            "Classifyin'", "Classificationifyin'",
            "Goin' over", "Goin' into",
            "Skimmin' through", "Skimmin' over",
            "Determinin'",
            "Separatin'", "Separationifyin'",
            "Configurin'", "Configuratin'",
            "Structurin'", "Structurifyin'",
            "Settin' up", "Setupin'",
            "Makin' ready", "Ready-makin'",
            "Gettin' ready", "Ready-gettin'",
            "Puttin' together",
            "Arrangin'", "Arrangementin'",
            "Assemblin'",
            "Workin' out", "Workin' on",
            "Preparin'", "Preparatin'", "Preparationin'",
            "Formulatin'",
            "Plannin'", "Plannin' out",
            "Doin'",
            "Sortin'", "Sortin' out", "Sortin' up",
            "Extractin'"
        };

        private readonly string[] articles = new string[]
        {
            "The", "Your", "Yer", "All the", "That", "All this", "Some"
        };

        private readonly string[] nouns = new string[]
        {
            "Stuff", "Data", "Info", "Input", "Delicious data", "Scrumptious info"
        };

        private readonly (string, string)[] thoughts = new (string, string)[]
        {
            ("You know what? I like ", "."),
            ("How about I put ", " everywhere?"),
            ("You know what? Screw ", ". Imma delete it."),
            ("How many places can I place ", " I wonder?"),
            ("Hmmm, now where should I place ", "?"),
            ("You don't happen to like ", ", do you?"),
            ("I wonder where I should place this ", "..."),
            ("Hmmm... ", "? Yeah, let's place one here. Why not?")
        };

        public string GetSubTask()
        {
            return verbs[rng.Next(verbs.Length)] + " " + articles[rng.Next(articles.Length)].ToLower() + " " + nouns[rng.Next(nouns.Length)].ToLower() + ".";
        }

        private string GetRandomName()
        {
            List<List<string>> strLists = new();
            strLists.Add(gameData.abilities.Select(o => o.GetName()).ToList());
            strLists.Add(gameData.dexEntries.Select(o => o.GetName()).ToList());
            strLists.Add(gameData.items.Where(o => o.IsPurchasable()).Select(o => o.GetName()).ToList());
            strLists.Add(gameData.moves.Where(o => o.isValid == 1).Select(o => o.GetName()).ToList());

            int listIdx = rng.Next(strLists.Count);
            return strLists[listIdx][rng.Next(strLists[listIdx].Count)];
        }

        public string GetThought()
        {
            int thoughtIdx = rng.Next(thoughts.Length);
            return thoughts[thoughtIdx].Item1 + GetRandomName() + thoughts[thoughtIdx].Item2;
        }
    }
}
