using System;
using System.Linq;

namespace MonkeySay.TextHandler
{
    public class RandomWordGenerator
    {

        // Settings
        const int FirstSyllableHasStartConsonant = 60; // % 1st syllables which start with a consonant
        const int SyllableHasStartConsonant = 50; // % syllables which start with a consonant
        const int SyllableHasEndConsonant = 70; // % syllables which end with a consonant
        const int StartConsonantComplex = 40; // % end consonants which are complex
        const int EndConsonantComplex = 50; // % end consonants which are complex
        const int FinalConsonantsModified = 30; // % modifiable end consonants which are modified
        const int VowelComplex = 30; // % vowels which are complex

        // Used to determine the syllable count randomly. Higher numbers mean a syllable is more likely to be chosen.
        // Index 0 is 1 syllable, index 1 is 2 syllables, etc.
        static readonly int[] SyllableWeights = {
			2, // 1 syllable words
			5, // 2 syllable words
			3  // 3 syllable words
		};

        static readonly int CombinedSyllableWeights;

        static RandomWordGenerator()
        {
            CombinedSyllableWeights = 0;
            foreach (var weight in SyllableWeights)
            {
                CombinedSyllableWeights += weight;
            }
        }

        static readonly char[] Vowels = {
			'a', 'e', 'i', 'o', 'u', 'y'
		};

        static readonly string[] ComplexVowels = {
			"ai", "au",
			"ea", "ee",
			"ie",
			"oo", "oa", "oi", "ou",
			"ua"					
		};

        static readonly string[] SimpleConsonants = {
			"b", "c", "d", "g", "l", "m", "n", "p", "s", "t", "w", "z", "v"
		};

        // complex consonant sounds
        static readonly string[] StartConsonants = {
			"bl", "br",
			"ch", "cl", "cr",
			"dr",
			"f", "fl", "fr",
			"gl", "gr",
			"h",
			"j",
			"k", "kl", "kn", "kr",
			"pe", "ph", "pl", "pr",
			"qu",
			"rh",
			"sc", "sh", "sk", "sl", "sm", "sn", "sp", "st", "str",
			"th", "tr", "tw",
			"v",
			"wh", "wr",
			"y"
		};

        static readonly string[] EndConsonants = {
			"ch", "ck",
			"dge",
			"fe", "ff", "ft",
			"ght", //"gh"
			"ke",
			"ld", "ll",
			"nd", "ng", "nk", "nt",
			"mg", "mp",
			"re", "rp", "rt",
			"sh", "sk", "sp", "ss", "st",
			"tch", "th",
			"x"
		};

        // consonants which can be modified (softening the consonant or changing the vowel) by adding an "e" to the end
        static readonly char[] EndConsonantsModifiable = {
			'b', 'd', 'g', 'l', 'm', 'n', 'r', 't',
		};

        // consonants which must be modified if they occur at the end of a word
        static readonly char[] EndConsonantsMustBeModified = {
			'c', 'l', 'v', 's'
		};

        // consonants where another consonant never follows mid-word
        static readonly char[] ConsonantNeverFollows = {
		   'b'
		};

        static readonly Random Rand = new Random();


        // simple test - run through and
        public static void Test()
        {
            for (var i = 0; i < 30; i++)
            {
                Console.WriteLine(Word());
            }
        }

        public static string Word()
        {
            // determine no of syllables
            var syllables = 0;
            var selector = Rand.Next(CombinedSyllableWeights - 1);
            for (var i = 0; i < SyllableWeights.Length; i++)
            {
                var weight = SyllableWeights[i];
                if (selector < weight)
                {
                    syllables = i + 1;
                    break;
                }
                selector -= weight;
            }
            return Word(syllables, true);
        }

        public static string Word(int syllables)
        {
            return Word(syllables, true);
        }

        public static string Word(int syllables, bool filter)
        {
            if (syllables < 1) throw new ArgumentException("Word must have at least 1 syllable.");
            var word = "";
            string lastSyllable = null;
            for (var i = 0; i < syllables; i++)
            {
                var makeItShort = i > 0 && syllables > 2;
                var last = i == syllables - 1;
                var syllable = Syllable(lastSyllable, makeItShort, last);

                // single syllable words must be more than one character
                while (syllables == 1 && syllable.Length == 1)
                {
                    syllable = Syllable(lastSyllable, makeItShort, last);
                }

                //if(i > 0) word += "-";
                word += syllable;
                lastSyllable = syllable;
            }

            if (filter)
            {
                // check for filtered words
                if (Filtered.Any(badWord => word.Contains(badWord)))
                {
                    return Word(syllables, true);
                }
            }
            return word;

        }

        static string Syllable(string previous, bool makeItShort, bool isLast)
        {
            var isFirst = previous == null;
            var lastSyllableSimple = previous != null && previous.Length < 3;
            var lastSyllableComplexEnd = false;
            var lastLetterIsNeverFollowedByConsonant = false;

            if (previous != null)
            {
                var count = 0;
                for (var i = previous.Length - 1; i > 0; i--)
                {
                    var lookAt = previous[i];
                    if (!Vowels.Contains(lookAt)) count++;
                    else break;
                }
                lastSyllableComplexEnd = count > 1;

                if (ConsonantNeverFollows.Contains(previous[previous.Length - 1])) lastLetterIsNeverFollowedByConsonant = true;
            }

            var startConsonant = isFirst ?
                Rand.Next(100) < FirstSyllableHasStartConsonant : Rand.Next(100) < SyllableHasStartConsonant;

            startConsonant = startConsonant && (isFirst || lastLetterIsNeverFollowedByConsonant);

            // always start with consonant if previous syllable ends in vowel
            if (!startConsonant && previous != null && !lastLetterIsNeverFollowedByConsonant)
            {
                var lastChar = previous[previous.Length - 1];
                if (Vowels.Contains(lastChar)) startConsonant = true;
            }

            var endConsonant = !(makeItShort && startConsonant) && Rand.Next(100) < SyllableHasEndConsonant;

            // always end in consonant if the last syllable was short
            if (lastSyllableSimple && !endConsonant)
            {
                endConsonant = true;
            }

            var syllable = "";

            string start = null;
            var complexStartConsonant = false;
            if (startConsonant)
            {
                complexStartConsonant = !lastSyllableComplexEnd && Rand.Next(100) < StartConsonantComplex;
                start = complexStartConsonant ? StartConsonants.Random() : SimpleConsonants.Random();
                syllable += start;
            }

            var complexVowel = startConsonant && !complexStartConsonant && Rand.Next(100) < VowelComplex;
            if (complexVowel)
                syllable += ComplexVowels.Random();
            else
                syllable += Vowels.Random().ToString();

            if (!endConsonant) return syllable;
            var complex = Rand.Next(100) < EndConsonantComplex;
            var startEndsWithR = start != null && start.EndsWith("r");
            string end;
            do
            {
                end = complex ? EndConsonants.Random() : SimpleConsonants.Random();
            } while (startEndsWithR && end.StartsWith("r"));

            var modify = isLast && (EndConsonantsMustBeModified.Contains(end[end.Length - 1]) || (
                (end == "ch" || EndConsonantsModifiable.Contains(end[end.Length - 1])) && Rand.Next(100) < FinalConsonantsModified));
            if (modify)
                end += "e";

            syllable += end;

            return syllable;
        }


        // Don't output any of these, in the unlikely event they get generated!
        static readonly string[] Filtered = {
			"allah",
			"anal",
			"anus",
			"arse",
			"ass",
			"bitch",
			"bloody",
			"bollock",
			"boob",
			"cock",
			"cunt",
			"crap",
			"cum",
			"damn",
			"dick",
			"fag",
			"fart",
			"fuck",
			"gimp",
			"hell",
			"homo",
			"nigga",
			"nigger",
			"penis",
			"piss",
			"poo",
			"pussy",
			"rape",
			"rapist",
			"sex",
			"shit",
			"slut",
			"spastic",
			"tits",
			"twat",
			"vag",
			"vagina",
			"vomit",
			"wank",
			"whore"
		};
    }

    static class RandomWordExtensions
    {
        static readonly Random Rand = new Random();
        public static string Random(this string[] source)
        {
            return source[Rand.Next(source.Length - 1)];
        }

        public static char Random(this char[] source)
        {
            return source[Rand.Next(source.Length - 1)];
        }
    }
}
