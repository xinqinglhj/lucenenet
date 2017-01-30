﻿using Lucene.Net.Analysis.TokenAttributes;
using Lucene.Net.Analysis.Util;
using Lucene.Net.Support;
using Lucene.Net.Util;
using System.Collections.Generic;
using System.Linq;

namespace Lucene.Net.Analysis.Core
{
    /*
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

    /// <summary>
    /// Removes stop words from a token stream.
    /// 
    /// <a name="version"/>
    /// <para>You must specify the required <seealso cref="LuceneVersion"/>
    /// compatibility when creating StopFilter:
    /// <ul>
    ///   <li> As of 3.1, StopFilter correctly handles Unicode 4.0
    ///         supplementary characters in stopwords and position
    ///         increments are preserved
    /// </ul>
    /// </para>
    /// </summary>
    public sealed class StopFilter : FilteringTokenFilter
    {

        private readonly CharArraySet stopWords;
        private readonly ICharTermAttribute termAtt;

        /// <summary>
        /// Constructs a filter which removes words from the input TokenStream that are
        /// named in the Set.
        /// </summary>
        /// <param name="matchVersion">
        ///          Lucene version to enable correct Unicode 4.0 behavior in the stop
        ///          set if Version > 3.0.  See <a href="#version">above</a> for details. </param>
        /// <param name="in">
        ///          Input stream </param>
        /// <param name="stopWords">
        ///          A <seealso cref="CharArraySet"/> representing the stopwords. </param>
        /// <seealso cref= #makeStopSet(Version, java.lang.String...) </seealso>
        public StopFilter(LuceneVersion matchVersion, TokenStream @in, CharArraySet stopWords)
            : base(matchVersion, @in)
        {
            termAtt = AddAttribute<ICharTermAttribute>();
            this.stopWords = stopWords;
        }

        /// <summary>
        /// Builds a Set from an array of stop words,
        /// appropriate for passing into the StopFilter constructor.
        /// This permits this stopWords construction to be cached once when
        /// an Analyzer is constructed.
        /// </summary>
        /// <param name="matchVersion"> Lucene version to enable correct Unicode 4.0 behavior in the returned set if Version > 3.0 </param>
        /// <param name="stopWords"> An array of stopwords </param>
        /// <seealso cref= #makeStopSet(Version, java.lang.String[], boolean) passing false to ignoreCase </seealso>
        public static CharArraySet MakeStopSet(LuceneVersion matchVersion, params string[] stopWords)
        {
            return MakeStopSet(matchVersion, stopWords, false);
        }

        /// <summary>
        /// Builds a Set from an array of stop words,
        /// appropriate for passing into the StopFilter constructor.
        /// This permits this stopWords construction to be cached once when
        /// an Analyzer is constructed.
        /// </summary>
        /// <param name="matchVersion"> Lucene version to enable correct Unicode 4.0 behavior in the returned set if Version > 3.0 </param>
        /// <param name="stopWords"> A List of Strings or char[] or any other toString()-able list representing the stopwords </param>
        /// <returns> A Set (<seealso cref="CharArraySet"/>) containing the words </returns>
        /// <seealso cref= #makeStopSet(Version, java.lang.String[], boolean) passing false to ignoreCase </seealso>
        public static CharArraySet MakeStopSet<T1>(LuceneVersion matchVersion, IList<T1> stopWords)
        {
            return MakeStopSet(matchVersion, stopWords, false);
        }

        /// <summary>
        /// Creates a stopword set from the given stopword array.
        /// </summary>
        /// <param name="matchVersion"> Lucene version to enable correct Unicode 4.0 behavior in the returned set if Version > 3.0 </param>
        /// <param name="stopWords"> An array of stopwords </param>
        /// <param name="ignoreCase"> If true, all words are lower cased first. </param>
        /// <returns> a Set containing the words </returns>
        public static CharArraySet MakeStopSet(LuceneVersion matchVersion, string[] stopWords, bool ignoreCase)
        {
            CharArraySet stopSet = new CharArraySet(matchVersion, stopWords.Length, ignoreCase);
            stopSet.UnionWith(stopWords);
            return stopSet;
        }

        /// <summary>
        /// Creates a stopword set from the given stopword list. </summary>
        /// <param name="matchVersion"> Lucene version to enable correct Unicode 4.0 behavior in the returned set if Version > 3.0 </param>
        /// <param name="stopWords"> A List of Strings or char[] or any other toString()-able list representing the stopwords </param>
        /// <param name="ignoreCase"> if true, all words are lower cased first </param>
        /// <returns> A Set (<seealso cref="CharArraySet"/>) containing the words </returns>
        public static CharArraySet MakeStopSet<T1>(LuceneVersion matchVersion, IList<T1> stopWords, bool ignoreCase)
        {
            var stopSet = new CharArraySet(matchVersion, stopWords.Count, ignoreCase);
            stopSet.UnionWith(stopWords);
            return stopSet;
        }

        /// <summary>
        /// Returns the next input Token whose term() is not a stop word.
        /// </summary>
        protected internal override bool Accept()
        {
            return !stopWords.Contains(termAtt.Buffer, 0, termAtt.Length);
        }
    }
}