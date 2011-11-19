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
 * Normalizes token text to lower case, analyzing given ("russian") charset.
 *
 *
 * @version $Id: RussianLowerCaseFilter.java 564236 2007-08-09 15:21:19Z gsingers $
 */
/**
 * Port from Java by Janus team
 */
namespace Lucene.Net.Analysis.Ru
{
	public sealed class RussianLowerCaseFilter : TokenFilter
	{
		private readonly char[] _charset;

		public RussianLowerCaseFilter(TokenStream input, char[] charset) : base(input)
		{
			_charset = charset;
		}

		public override Token Next()
		{
			var t = input.Next();
			if (t == null)
				return null;

			var txt = t.TermText();
			var chArray = txt.ToCharArray();
			for (var i = 0; i < chArray.Length; i++)
				chArray[i] = RussianCharsets.ToLowerCase(chArray[i], _charset);

			var newTxt = new string(chArray);
			// create new token
			var newToken = new Token(newTxt, t.StartOffset(), t.EndOffset());
			return newToken;
		}
	}
}
