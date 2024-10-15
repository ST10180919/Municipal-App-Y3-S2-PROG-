using Municipal_App.Stores;
using Municipal_App.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;

namespace Municipal_App.Services
{
    public enum RecommendationTermType 
    {
        Search,
        Category,
        Date
    }

    internal class RecommendationService
    {
        public Dictionary<RecommendationTermType, Dictionary<string, int>> RecommendationTerms { get; private set; }

        private int TopOccurence = 0;

        public RecommendationService()
        {
            this.RecommendationTerms = new Dictionary<RecommendationTermType, Dictionary<string, int>>();
        }

        public void AddTerm(RecommendationTermType termType, string termValue)
        {
            // If the term type has already been added
            if (RecommendationTerms.ContainsKey(termType))
            {
                // If this term value has already been added to the term type
                if (RecommendationTerms[termType].ContainsKey(termValue))
                {
                    // Increment the occurrences of this term by one
                    RecommendationTerms[termType][termValue] += 1;
                }
                else
                {
                    // Add the new termValue with the occurrences set to 1
                    RecommendationTerms[termType].Add(termValue, 1);
                }
            }
            else
            {
                // Add the new term type
                RecommendationTerms.Add(termType, new Dictionary<string, int>());
                // Add the new term value
                RecommendationTerms[termType].Add(termValue, 1);
            }
        }

        public bool GetRecommendation(string title, string category, string date)
        {
            var occurrences = 0;

            // getting occurences of title 
            if (RecommendationTerms.ContainsKey(RecommendationTermType.Search))
            {
                // Filter RecommendationTerms[RecommendationTermType.Search] for items containing part of the title
                var searchOccurrences = RecommendationTerms[RecommendationTermType.Search]
                    .Where(pair => title.IndexOf(pair.Key, StringComparison.OrdinalIgnoreCase) >= 0)
                    .Sum(pair => pair.Value);
                occurrences += searchOccurrences;
            }

            // getting occurences of category
            if (RecommendationTerms.ContainsKey(RecommendationTermType.Category))
            {
                if (RecommendationTerms[RecommendationTermType.Category].ContainsKey(category))
                {
                    occurrences += RecommendationTerms[RecommendationTermType.Category][category];
                }
            }

            // getting occurences of date
            if (RecommendationTerms.ContainsKey(RecommendationTermType.Date))
            {
                // Attempt to parse date
                if (DateTime.TryParse(date, out DateTime parsedDate))
                {
                    DateTime now = DateTime.Now;
                    DateType dateType;

                    // Determine which DateType the date falls into
                    if (parsedDate.Date == now.Date)
                    {
                        dateType = DateType.Today;
                    }
                    else if (parsedDate >= now && parsedDate <= now.AddDays(7))
                    {
                        dateType = DateType.ThisWeek;
                    }
                    else if (parsedDate.Year == now.Year && parsedDate.Month == now.Month)
                    {
                        dateType = DateType.ThisMonth;
                    }
                    else
                    {
                        dateType = DateType.AnyTime;
                    }

                    // Determining how many times the user has searched for this date type
                    if (RecommendationTerms[RecommendationTermType.Date].ContainsKey(dateType.ToString()))
                    {
                        occurrences += RecommendationTerms[RecommendationTermType.Date][dateType.ToString()];
                    }
                }
            }

            // update the top occurence
            if (occurrences > TopOccurence)
            {
                TopOccurence = occurrences;
            }

            var isRecommended = occurrences >= 2 && occurrences >= TopOccurence;

            // Return true if the occurences is greater than 2 and is the TopOccurence
            return isRecommended;
        }
    }
}
