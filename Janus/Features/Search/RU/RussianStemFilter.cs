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
 * A filter that stems Russian words. The implementation was inspired by GermanStemFilter.
 * The input should be filtered by RussianLowerCaseFilter before passing it to RussianStemFilter ,
 * because RussianStemFilter only works  with lowercase part of any "russian" charset.
 *
 *
 * @version   $Id: RussianStemFilter.java 564236 2007-08-09 15:21:19Z gsingers $
 */
/**
 * Port from Java by Janus team
 */
namespace Lucene.Net.Analysis.Ru
{
	public sealed class RussianStemFilter : TokenFilter
	{
		/**
		 * The actual token in the input stream.
		*/
		private Token _token = null;
		private RussianStemmer _stemmer = null;

		public RussianStemFilter(TokenStream input, char[] charset) : base(input)
		{
			_stemmer = new RussianStemmer(charset);
		}

		/**
		 * @return  Returns the next token in the stream, or null at EOS
		 */
		public override Token Next()
		{
			if ((_token = input.Next()) == null)
			{
				return null;
			}
			var s = _stemmer.stem(_token.TermText());
			return
				!s.Equals(_token.TermText())
					? new Token(s, _token.StartOffset(), _token.EndOffset(), _token.Type())
					: _token;
		}

		/**
		 * Set a alternative/custom RussianStemmer for this filter.
		 */
		public void SetStemmer(RussianStemmer stemmer)
		{
			if (stemmer != null)
				_stemmer = stemmer;
		}
	}
}