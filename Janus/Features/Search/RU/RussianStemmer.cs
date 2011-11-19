/**
 * Licensed to the Apache Software Foundation (ASF) under one or more
 * contributor license agreements.  See the NOTICE file distributed with
 * this work for additional information regarding copyright ownership.
 * The ASF licenses this file to You under the Apache License, Version 2.0
 * (the "License"); you may not use this file except in compliance with
 * the License.  You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

/**
 * Russian stemming algorithm implementation (see http://snowball.sourceforge.net for detailed description).
 *
 *
 * @version $Id: RussianStemmer.java 564236 2007-08-09 15:21:19Z gsingers $
 */
/**
 * Port from Java by Janus team
 */
using System;
using System.Text;

namespace Lucene.Net.Analysis.Ru
{
	public class RussianStemmer
	{
		private char[] _charset;

		// positions of RV, R1 and R2 respectively
		private int RV, R2;

		// letters (currently unused letters are commented out)
		private const char A = (char)0;
		//private const char B = 1;
		private const char V = (char)2;
		private const char G = (char)3;
		//private const char D = 4;
		private const char E = (char)5;
		//private const char ZH = 6;
		//private const char Z = 7;
		private const char I = (char)8;
		private const char I_ = (char)9;
		//private const char K = 10;
		private const char L = (char)11;
		private const char M = (char)12;
		private const char N = (char)13;
		private const char O = (char)14;
		//private const char P = 15;
		//private const char R = 16;
		private const char S = (char)17;
		private const char T = (char)18;
		private const char U = (char)19;
		//private const char F = 20;
		private const char X = (char)21;
		//private const char TS = 22;
		//private const char CH = 23;
		private const char SH = (char)24;
		private const char SHCH = (char)25;
		//private final static char HARD = 26;
		private const char Y = (char)27;
		private const char SOFT = (char)28;
		private const char AE = (char)29;
		private const char IU = (char)30;
		private const char IA = (char)31;

		// stem definitions
		private static readonly char[] _vowels = { A, E, I, O, U, Y, AE, IU, IA };

		private static readonly char[][] _perfectiveGerundEndings1 = {
			new[]{ V },
			new[]{ V, SH, I },
			new[]{ V, SH, I, S, SOFT }
		};

		private static readonly char[][] _perfectiveGerund1Predessors = {
			new[]{ A },
			new[]{ IA }
		};

		private static readonly char[][] _perfectiveGerundEndings2 = { 
			new[]{ I, V },
			new[]{Y, V },
			new[]{I, V, SH, I },
			new[]{Y, V, SH, I },
			new[]{I, V, SH, I, S, SOFT },
			new[]{Y, V, SH, I, S, SOFT }
		};

		private static readonly char[][] _adjectiveEndings = {
			new[]{ E, E },
			new[]{ I, E },
			new[]{ Y, E },
			new[]{ O, E },
			new[]{ E, I_ },
			new[]{ I, I_ },
			new[]{ Y, I_ },
			new[]{ O, I_ },
			new[]{ E, M },
			new[]{ I, M },
			new[]{ Y, M },
			new[]{ O, M },
			new[]{ I, X },
			new[]{ Y, X },
			new[]{ U, IU },
			new[]{ IU, IU },
			new[]{ A, IA },
			new[]{ IA, IA },
			new[]{ O, IU },
			new[]{ E, IU },
			new[]{ I, M, I },
			new[]{ Y, M, I },
			new[]{ E, G, O },
			new[]{ O, G, O },
			new[]{ E, M, U },
			new[]{O, M, U }
		};

		private static readonly char[][] _participleEndings1 = {
			new[]{ SHCH },
			new[]{ E, M },
			new[]{ N, N },
			new[]{ V, SH },
			new[]{ IU, SHCH }
		};

		private static readonly char[][] _participleEndings2 = {
			new[]{ I, V, SH },
			new[]{ Y, V, SH },
			new[]{ U, IU, SHCH }
		};

		private static readonly char[][] _participle1Predessors = {
			new[]{ A },
			new[]{ IA }
		};

		private static readonly char[][] _reflexiveEndings = {
			new[]{ S, IA },
			new[]{ S, SOFT }
		};

		private static readonly char[][] _verbEndings1 = {
			new[]{ I_ },
			new[]{ L },
			new[]{ N },
			new[]{ L, O },
			new[]{ N, O },
			new[]{ E, T },
			new[]{ IU, T },
			new[]{ L, A },
			new[]{ N, A },
			new[]{ L, I },
			new[]{ E, M },
			new[]{ N, Y },
			new[]{ E, T, E },
			new[]{ I_, T, E },
			new[]{ T, SOFT },
			new[]{ E, SH, SOFT },
			new[]{ N, N, O }
		};

		private static readonly char[][] _verbEndings2 = {
			new[]{ IU },
			new[]{ U, IU },
			new[]{ E, N },
			new[]{ E, I_ },
			new[]{ IA, T },
			new[]{ U, I_ },
			new[]{ I, L },
			new[]{ Y, L },
			new[]{ I, M },
			new[]{ Y, M },
			new[]{ I, T },
			new[]{ Y, T },
			new[]{ I, L, A },
			new[]{ Y, L, A },
			new[]{ E, N, A },
			new[]{ I, T, E },
			new[]{ I, L, I },
			new[]{ Y, L, I },
			new[]{ I, L, O },
			new[]{ Y, L, O },
			new[]{ E, N, O },
			new[]{ U, E, T },
			new[]{ U, IU, T },
			new[]{ E, N, Y },
			new[]{ I, T, SOFT },
			new[]{ Y, T, SOFT },
			new[]{ I, SH, SOFT },
			new[]{ E, I_, T, E },
			new[]{ U, I_, T, E }
		};

		private static readonly char[][] _verb1Predessors = {
			new[]{ A },
			new[]{ IA }
		};

		private static readonly char[][] _nounEndings = {
			new[]{ A },
			new[]{ U },
			new[]{ I_ },
			new[]{ O },
			new[]{ U },
			new[]{ E },
			new[]{ Y },
			new[]{ I },
			new[]{ SOFT },
			new[]{ IA },
			new[]{ E, V },
			new[]{ O, V },
			new[]{ I, E },
			new[]{ SOFT, E },
			new[]{ IA, X },
			new[]{ I, IU },
			new[]{ E, I },
			new[]{ I, I },
			new[]{ E, I_ },
			new[]{ O, I_ },
			new[]{ E, M },
			new[]{ A, M },
			new[]{ O, M },
			new[]{ A, X },
			new[]{ SOFT, IU },
			new[]{ I, IA },
			new[]{ SOFT, IA },
			new[]{ I, I_ },
			new[]{ IA, M },
			new[]{ IA, M, I },
			new[]{ A, M, I },
			new[]{ I, E, I_ },
			new[]{ I, IA, M },
			new[]{ I, E, M },
			new[]{ I, IA, X },
			new[]{ I, IA, M, I }
		};

		private static readonly char[][] _superlativeEndings = {
			new[]{ E, I_, SH },
			new[]{ E, I_, SH, E }
		};

		private static readonly char[][] _derivationalEndings = {
			new[]{ O, S, T },
			new[]{ O, S, T, SOFT }
		};

		/**
		 * RussianStemmer constructor comment.
		*/
		public RussianStemmer()
		{}

		/**
		 * RussianStemmer constructor comment.
		*/
		public RussianStemmer(char[] charset)
		{
			_charset = charset;
		}

		/**
     * Adjectival ending is an adjective ending,
     * optionally preceded by participle ending.
     * Creation date: (17/03/2002 12:14:58 AM)
     * @param stemmingZone java.lang.StringBuffer
     */
		private bool Adjectival(StringBuilder stemmingZone)
		{
			// look for adjective ending in a stemming zone
			if (!FindAndRemoveEnding(stemmingZone, _adjectiveEndings))
				return false;
			// if adjective ending was found, try for participle ending.
			// we are just interested in the side effect of
			// FindAndRemoveEnding():
			if (!FindAndRemoveEnding(stemmingZone, _participleEndings1, _participle1Predessors))
				FindAndRemoveEnding(stemmingZone, _participleEndings2);
			return true;
		}

		/**
		 * Derivational endings
		 * Creation date: (17/03/2002 12:14:58 AM)
		 * @param stemmingZone java.lang.StringBuffer
		 */
		private void Derivational(StringBuilder stemmingZone)
		{
			var endingLength = FindEnding(stemmingZone, _derivationalEndings);
			if (endingLength == 0)
				// no derivational ending found
				return;
			// Ensure that the ending locates in R2
			if (R2 - RV <= stemmingZone.Length - endingLength)
				stemmingZone.Length = stemmingZone.Length - endingLength;
		}

		/**
		 * Finds ending among given ending class and returns the length of ending found(0, if not found).
		 * Creation date: (17/03/2002 8:18:34 PM)
		 */
		private int FindEnding(StringBuilder stemmingZone, int startIndex, char[][] theEndingClass)
		{
			for (var i = theEndingClass.Length - 1; i >= 0; i--)
			{
				var theEnding = theEndingClass[i];
				// check if the ending is bigger than stemming zone
				if (startIndex < theEnding.Length - 1)
					continue;
				var match = true;
				var stemmingIndex = startIndex;
				for (var j = theEnding.Length - 1; j >= 0; j--)
					if (stemmingZone[stemmingIndex--] != _charset[theEnding[j]])
					{
						match = false;
						break;
					}
				// check if ending was found
				if (match)
					return theEndingClass[i].Length; // cut ending
			}
			return 0;
		}

		private int FindEnding(StringBuilder stemmingZone, char[][] theEndingClass)
		{
			return FindEnding(stemmingZone, stemmingZone.Length - 1, theEndingClass);
		}

		/**
		 * Finds the ending among the given class of endings and removes it from stemming zone.
		 * Creation date: (17/03/2002 8:18:34 PM)
		 */
		private bool FindAndRemoveEnding(StringBuilder stemmingZone, char[][] theEndingClass)
		{
			var endingLength = FindEnding(stemmingZone, theEndingClass);
			if (endingLength == 0)
				// not found
				return false;
			stemmingZone.Length = stemmingZone.Length - endingLength;
			// cut the ending found
			return true;
		}

		/**
		 * Finds the ending among the given class of endings, then checks if this ending was
		 * preceded by any of given predessors, and if so, removes it from stemming zone.
		 * Creation date: (17/03/2002 8:18:34 PM)
		 */
		private bool FindAndRemoveEnding(StringBuilder stemmingZone,
			char[][] theEndingClass, char[][] thePredessors)
		{
			var endingLength = FindEnding(stemmingZone, theEndingClass);
			if (endingLength == 0)
				// not found
				return false;
			var predessorLength =
				FindEnding(stemmingZone,
					stemmingZone.Length - endingLength - 1,
					thePredessors);
			if (predessorLength == 0)
				return false;
			stemmingZone.Length = stemmingZone.Length - endingLength;
			// cut the ending found
			return true;
		}

		/**
		 * Marks positions of RV, R1 and R2 in a given word.
		 * Creation date: (16/03/2002 3:40:11 PM)
		 */
		private void markPositions(string word)
		{
			RV = 0;
			R2 = 0;
			var i = 0;
			// find RV
			while (word.Length > i && !IsVowel(word[i]))
				i++;
			if (word.Length - 1 < ++i)
				return; // RV zone is empty
			RV = i;
			// find R1
			while (word.Length > i && IsVowel(word[i]))
				i++;
			if (word.Length - 1 < ++i)
				return; // R1 zone is empty
			// find R2
			while (word.Length > i && !IsVowel(word[i]))
				i++;
			if (word.Length - 1 < ++i)
				return; // R2 zone is empty
			while (word.Length > i && IsVowel(word[i]))
				i++;
			if (word.Length - 1 < ++i)
				return; // R2 zone is empty
			R2 = i;
		}

		/**
		 * Checks if character is a vowel..
		 * Creation date: (16/03/2002 10:47:03 PM)
		 * @return boolean
		 * @param letter char
		 */
		private bool IsVowel(char letter)
		{
			for (var i = 0; i < _vowels.Length; i++)
				if (letter == _charset[_vowels[i]])
					return true;
			return false;
		}

		/**
		 * Noun endings.
		 * Creation date: (17/03/2002 12:14:58 AM)
		 * @param stemmingZone java.lang.StringBuffer
		 */
		private void Noun(StringBuilder stemmingZone)
		{
			FindAndRemoveEnding(stemmingZone, _nounEndings);
		}

		/**
		 * Perfective gerund endings.
		 * Creation date: (17/03/2002 12:14:58 AM)
		 * @param stemmingZone java.lang.StringBuffer
		 */
		private bool PerfectiveGerund(StringBuilder stemmingZone)
		{
			return FindAndRemoveEnding(
				stemmingZone,
				_perfectiveGerundEndings1,
				_perfectiveGerund1Predessors)
					|| FindAndRemoveEnding(stemmingZone, _perfectiveGerundEndings2);
		}

		/**
		 * Reflexive endings.
		 * Creation date: (17/03/2002 12:14:58 AM)
		 * @param stemmingZone java.lang.StringBuffer
		 */
		private void Reflexive(StringBuilder stemmingZone)
		{
			FindAndRemoveEnding(stemmingZone, _reflexiveEndings);
		}

		/**
		 * Insert the method's description here.
		 * Creation date: (17/03/2002 12:14:58 AM)
		 * @param stemmingZone java.lang.StringBuffer
		 */
		private void RemoveI(StringBuilder stemmingZone)
		{
			if (stemmingZone.Length > 0
					&& stemmingZone[stemmingZone.Length - 1] == _charset[I])
				stemmingZone.Length = stemmingZone.Length - 1;
		}

		/**
		 * Insert the method's description here.
		 * Creation date: (17/03/2002 12:14:58 AM)
		 * @param stemmingZone java.lang.StringBuffer
		 */
		private void RemoveSoft(StringBuilder stemmingZone)
		{
			if (stemmingZone.Length > 0
					&& stemmingZone[stemmingZone.Length - 1] == _charset[SOFT])
				stemmingZone.Length = stemmingZone.Length - 1;
		}

		/**
		 * Insert the method's description here.
		 * Creation date: (16/03/2002 10:58:42 PM)
		 * @param newCharset char[]
		 */
		public void SetCharset(char[] newCharset)
		{
			_charset = newCharset;
		}

		/**
		 * Finds the stem for given Russian word.
		 * Creation date: (16/03/2002 3:36:48 PM)
		 * @return java.lang.String
		 * @param input java.lang.String
		 */
		public String stem(string input)
		{
			markPositions(input);
			if (RV == 0)
				return input; //RV wasn't detected, nothing to stem
			var stemmingZone = new StringBuilder(input.Substring(RV));
			// stemming goes on in RV
			// Step 1
			if (!PerfectiveGerund(stemmingZone))
			{
				Reflexive(stemmingZone);
				// we are just interested in the flow that gets
				// created by logical expression: apply Adjectival(); if that fails,
				// apply Verb() etc
				if (Adjectival(stemmingZone))
					if (Verb(stemmingZone))
						Noun(stemmingZone);
			}
			// Step 2
			RemoveI(stemmingZone);
			// Step 3
			Derivational(stemmingZone);
			// Step 4
			Superlative(stemmingZone);
			undoubleN(stemmingZone);
			RemoveSoft(stemmingZone);
			// return result
			return input.Substring(0, RV) + stemmingZone;
		}

		/**
		 * Superlative endings.
		 * Creation date: (17/03/2002 12:14:58 AM)
		 * @param stemmingZone java.lang.StringBuffer
		 */
		private void Superlative(StringBuilder stemmingZone)
		{
			FindAndRemoveEnding(stemmingZone, _superlativeEndings);
		}

		/**
		 * Undoubles N.
		 * Creation date: (17/03/2002 12:14:58 AM)
		 * @param stemmingZone java.lang.StringBuffer
		 */
		private void undoubleN(StringBuilder stemmingZone)
		{
			char[][] doubleN = {new[]{ N, N }};
			if (FindEnding(stemmingZone, doubleN) != 0)
				stemmingZone.Length = stemmingZone.Length - 1;
		}

		/**
		 * Verb endings.
		 * Creation date: (17/03/2002 12:14:58 AM)
		 * @param stemmingZone java.lang.StringBuffer
		 */
		private bool Verb(StringBuilder stemmingZone)
		{
			return FindAndRemoveEnding(
				stemmingZone,
				_verbEndings1,
				_verb1Predessors)
					|| FindAndRemoveEnding(stemmingZone, _verbEndings2);
		}

		/**
		 * Static method for stemming with different charsets
		 */
		public static String stem(String theWord, char[] charset)
		{
			var stemmer = new RussianStemmer();
			stemmer.SetCharset(charset);
			return stemmer.stem(theWord);
		}
	}
	
}