﻿using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Nest
{
	[JsonConverter(typeof(VerbatimDictionaryKeysJsonConverter))]
	public interface ICharFilters : IHasADictionary { }

	public class CharFilters : HasADictionary<string, ICharFilter>, ICharFilters
	{
		public CharFilters() : base() { }
		public CharFilters(IDictionary<string, ICharFilter> container) : base(container) { }
		public CharFilters(Dictionary<string, ICharFilter> container)
			: base(container.Select(kv => kv).ToDictionary(kv => kv.Key, kv => kv.Value))
		{ }

		public void Add(string name, ICharFilter analyzer) => BackingDictionary.Add(name, analyzer);
	}

	public class CharFiltersDescriptor : HasADictionary<string, ICharFilter>, ICharFilters
	{
		protected CharFiltersDescriptor Assign(string name, ICharFilter analyzer) =>
			Fluent.Assign<CharFiltersDescriptor, CharFiltersDescriptor>(this, (a) => BackingDictionary.Add(name, analyzer));

		public CharFiltersDescriptor Add(string name, ICharFilter analyzer) => Assign(name, analyzer);
		
		/// <summary>
		/// The pattern_replace char filter allows the use of a regex to manipulate the characters in a string before analysis. 
		/// </summary>
		public CharFiltersDescriptor PatternReplace(string name, Func<PatternReplaceCharFilterDescriptor, IPatternReplaceCharFilter> selector) =>
			Assign(name, selector?.Invoke(new PatternReplaceCharFilterDescriptor()));

		/// <summary>
		/// A char filter of type html_strip stripping out HTML elements from an analyzed text.
		/// </summary>
		public CharFiltersDescriptor HtmlStrip(string name, Func<HtmlStripCharFilterDescriptor, IHtmlStripCharFilter> selector = null) =>
			Assign(name, selector?.InvokeOrDefault(new HtmlStripCharFilterDescriptor()));

		/// <summary>
		/// A char filter of type mapping replacing characters of an analyzed text with given mapping.
		/// </summary>
		public CharFiltersDescriptor Mapping(string name, Func<MappingCharFilterDescriptor, IMappingCharFilter> selector) =>
			Assign(name, selector?.Invoke(new MappingCharFilterDescriptor()));

	}
}