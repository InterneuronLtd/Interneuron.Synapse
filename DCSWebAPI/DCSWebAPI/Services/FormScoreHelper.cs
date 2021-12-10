//Interneuron Synapse

//Copyright(C) 2021  Interneuron CIC

//This program is free software: you can redistribute it and/or modify
//it under the terms of the GNU General Public License as published by
//the Free Software Foundation, either version 3 of the License, or
//(at your option) any later version.

//This program is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.

//See the
//GNU General Public License for more details.

//You should have received a copy of the GNU General Public License
//along with this program.If not, see<http://www.gnu.org/licenses/>.


﻿using DCSWebAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DCSWebAPI.Services
{
    public class FormScoreHelper
    {
        public static string CalculateASAScore(List<FormResponse> formResponses)
        {
            Dictionary<string, int> counters = new Dictionary<string, int>();
            counters.Add("r", 0);
            counters.Add("a", 0);
            counters.Add("y", 0);
            counters.Add("g", 0);

            string reviewType = string.Empty;
            string asaScore = string.Empty;
            string description = string.Empty;

            if (formResponses == null || formResponses.Count < 1) // cannot calculate score if a response is missing
                return JsonConvert.SerializeObject(new { result = new { reviewtype = reviewType, asascore = asaScore, description = "Form response is null. Score Calculation aborted." } });

            var asaJsonDefinition = new { asa = new[] { new { field_id = "", fieldlabeltext = "", scores = new[] { new { optionval = "", score = "" } } } } };
            string asaScoreDefinition = System.IO.File.ReadAllText(@".\JSONStore\ASAScoreDefinition.json");
            var scoresDefinition = JsonConvert.DeserializeAnonymousType(asaScoreDefinition, asaJsonDefinition);

            var scoringModelDefinition = new { scoringmodel = new[] { new { reviewtype = "", asascore = "", description = "", rule = new[] { new { score = "", opr = "", value = 1 } } } } };
            string scoringModelJson = System.IO.File.ReadAllText(@".\JSONStore\ASAScoringModel.json");
            var scoringModel = JsonConvert.DeserializeAnonymousType(scoringModelJson, scoringModelDefinition);

            foreach (var item in scoresDefinition.asa)
            {
                //find answer for this field from responses
                var response = formResponses.Where(x => x.FieldId == item.field_id);

                if (response == null || response.Count() < 1) // cannot calculate score if a response is missing
                    return JsonConvert.SerializeObject(new { result = new { reviewtype = reviewType, asascore = asaScore, description = item.field_id + "is not answered. Score Calculation aborted." } });


                foreach (var score in item.scores)
                {
                    if (response.First().ResponseValue.Trim().ToLower().Equals(score.optionval.Trim().ToLower()))
                        counters[score.score.ToLower()]++;
                }
            }

            bool reviewFound = false;

            foreach (var model in scoringModel.scoringmodel)
            {
                foreach (var rule in model.rule)
                {
                    if (rule.opr == ">=")
                    {
                        if (counters[rule.score.ToLower().Trim()] >= rule.value)
                            reviewFound = true;
                    }
                    else if (rule.opr == "=")
                    {
                        if (counters[rule.score.ToLower().Trim()] == rule.value)
                            reviewFound = true;
                    }
                    else if (rule.opr == ">")
                    {
                        if (counters[rule.score.ToLower().Trim()] > rule.value)
                            reviewFound = true;
                    }
                }
                if (reviewFound)
                {
                    reviewType = model.reviewtype;
                    asaScore = model.asascore;
                    description = model.description;
                    break;
                }
            }

            if (!reviewFound)
            {
                description = "Review not found for scores : ";
                foreach (var item in counters)
                {
                    description += string.Format("{0} = {1}", item.Key, item.Value);
                }
            }

            return JsonConvert.SerializeObject(new { result = new { reviewtype = reviewType, asascore = asaScore, desc = description } });
        }
    }
}