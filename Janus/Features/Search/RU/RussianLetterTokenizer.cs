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
 * A RussianLetterTokenizer is a tokenizer that extends LetterTokenizer by additionally looking up letters
 * in a given "russian charset". The problem with LeterTokenizer is that it uses Character.isLetter() method,
 * which doesn't know how to detect letters in encodings like CP1252 and KOI8
 * (well-known problems with 0xD7 and 0xF7 chars)
 *
 *
 * @version $Id: RussianLetterTokenizer.java 564236 2007-08-09 15:21:19Z gsingers $
 */
/**
 * Port from Java by Janus team
 */
using System.IO;

namespace Lucene.Net.Analysis.Ru
{
	public class RussianLetterTokenizer : CharTokenizer
	{
		/** Construct a new LetterTokenizer. */
		private readonly char[] _charset;

		public RussianLetterTokenizer(TextReader input, char[] charset)
			: base(input)
		{
			_charset = charset;
		}

		/**
		 * Collects only characters which satisfy
		 * {@link Character#isLetter(char)}.
		 */
		protected override bool IsTokenChar(char c)
		{
			if (char.IsLetter(c))
				return true;
			for (var i = 0; i < _charset.Length; i++)
				if (c == _charset[i])
					return true;
			return false;
		}
	}
}