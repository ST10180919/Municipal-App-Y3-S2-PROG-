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
    //---------------------------------------------------------------------------------
    /// <summary>
    /// Defines the types of recommendation terms that the system can track and evaluate.
    /// These include Search terms, Category terms, and Date terms, used to provide event recommendations.
    /// </summary>
    public enum RecommendationTermType 
    {
        Search,
        Category,
        Date
    }

    //---------------------------------------------------------------------------------
    /// <summary>
    /// This class provides functionality for tracking and recommending events based on 
    /// certain criteria, such as search terms, categories, and dates. It keeps a record of 
    /// occurrences of these terms and uses them to calculate recommendations.
    /// </summary>
    internal class RecommendationService
    {
        //-----------------------------------------------------------------------------
        /// <summary>
        /// A dictionary that tracks the occurrences of terms for each RecommendationTermType.
        /// The keys represent the term type, and the values are dictionaries that map term values to their occurrence counts.
        /// </summary>
        public Dictionary<RecommendationTermType, Dictionary<string, int>> RecommendationTerms { get; private set; }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Stores the highest number of occurrences for any term.
        /// </summary>
        private int TopOccurence = 0;

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the RecommendationService class.
        /// The constructor creates the dictionary used to store recommendation terms.
        /// </summary>
        public RecommendationService()
        {
            this.RecommendationTerms = new Dictionary<RecommendationTermType, Dictionary<string, int>>();
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Adds a term to the recommendation system. If the term type and value already exist, the occurrence count is incremented.
        /// Otherwise, a new entry is created for the term type and value.
        /// </summary>
        /// <param name="termType">The type of the term (Search, Category, Date).</param>
        /// <param name="termValue">The value of the term (e.g., the search query, category name, or date).</param>
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

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Evaluates whether an event should be recommended based on its title, category, and date.
        /// The method checks for term matches in the stored recommendation terms and calculates the total occurrences.
        /// An event is recommended if its occurrence count is above 2 and is greater than or equal to the current top occurrence.
        /// </summary>
        /// <param name="title">The title of the event.</param>
        /// <param name="category">The category of the event.</param>
        /// <param name="date">The date of the event.</param>
        /// <returns>Returns true if the event should be recommended, false otherwise.</returns>
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

            // Return true if the occurences is greater than 2 and is the TopOccurence
            return occurrences >= 2 && occurrences >= TopOccurence;
        }
    }
}
//---------------------------------------EOF-------------------------------------------