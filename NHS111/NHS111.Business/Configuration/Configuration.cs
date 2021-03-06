﻿using System.Collections.Generic;
using System.Configuration;
using System.IO;
using Newtonsoft.Json;

namespace NHS111.Business.Configuration
{
    public class Configuration : IConfiguration
    {
        public string GetDomainApiMonitorHealthUrl()
        {
            return GetDomainApiUrl("DomainApiMonitorHealthUrl");
        }

        public string GetDomainApiQuestionUrl(string questionId)
        {
            return GetDomainApiUrl("DomainApiQuestionUrl").
                Replace("{questionId}", questionId);
        }

        public string GetDomainApiAnswersForQuestionUrl(string questionId)
        {
            return GetDomainApiUrl("DomainApiAnswersForQuestionUrl").
                Replace("{questionId}", questionId);
        }

        public string GetDomainApiNextQuestionUrl(string questionId)
        {
            return GetDomainApiUrl("DomainApiNextQuestionUrl").
                Replace("{questionId}", questionId);
        }

        public string GetDomainApiFirstQuestionUrl(string pathwayId)
        {
            return GetDomainApiUrl("DomainApiFirstQuestionUrl").
                Replace("{pathwayId}", pathwayId);
        }

        public string GetDomainApiJustToBeSafeQuestionsFirstUrl(string pathwayId)
        {
            return GetDomainApiUrl("DomainApiJustToBeSafeQuestionsFirstUrl").
                Replace("{pathwayId}", pathwayId);
        }

        public string GetDomainApiJustToBeSafeQuestionsNextUrl(string pathwayId, IEnumerable<string> answeredQuestionIds, bool multipleChoice, string selectedQuestionId)
        {
            return GetDomainApiUrl("DomainApiJustToBeSafeQuestionsNextUrl").
                Replace("{pathwayId}", pathwayId).
                Replace("{answeredQuestionIds}", string.Join(",", answeredQuestionIds)).
                Replace("{multipleChoice}", multipleChoice.ToString()).
                Replace("{selectedQuestionId}", selectedQuestionId);
        }

        public string GetDomainApiPathwaysUrl(bool grouped)
        {
            return GetDomainApiUrl("DomainApiPathwaysUrl").
                Replace("{grouped}", grouped.ToString());
        }

        public string GetDomainApiPathwayUrl(string pathwayId)
        {
            return GetDomainApiUrl("DomainApiPathwayUrl").
                Replace("{pathwayId}", pathwayId);
        }

        public string GetDomainApiIdentifiedPathwayUrl(string pathwayNumbers, string gender, int age)
        {
            return GetDomainApiUrl("DomainApiIdentifiedPathwayUrl").
                Replace("{pathwayNumbers}",pathwayNumbers).
                Replace("{gender}", gender).
                Replace("{age}", age.ToString());
        }

        public string GetDomainApiPathwaySymptomGroup(string pathwayNumbers)
        {
            return GetDomainApiUrl("DomainApiPathwaySymptomGroup").
                Replace("{pathwayNumbers}", pathwayNumbers);
        }

        public string GetDomainApiCareAdviceUrl(int age, string gender, IEnumerable<string> markers)
        {
            return GetDomainApiUrl("DomainApiCareAdviceUrl").
                Replace("{age}", age.ToString()).
                Replace("{gender}", gender).
                Replace("{markers}", string.Join(",", markers));
        }

        private static string GetDomainApiUrl(string key)
        {
            return string.Format("{0}{1}", ConfigurationManager.AppSettings["DomainApiBaseUrl"], ConfigurationManager.AppSettings[key]).Replace("&amp;", "&");
        }

        public string GetRedisUrl()
        {
            return ConfigurationManager.AppSettings["RedisUrl"];
        }

    }

    public interface IConfiguration
    {
        string GetDomainApiMonitorHealthUrl();

        /* Questions */
        string GetDomainApiQuestionUrl(string questionId);
        string GetDomainApiAnswersForQuestionUrl(string questionId);
        string GetDomainApiNextQuestionUrl(string questionId);
        string GetDomainApiFirstQuestionUrl(string pathwayId);
        string GetDomainApiJustToBeSafeQuestionsFirstUrl(string pathwayId);
        string GetDomainApiJustToBeSafeQuestionsNextUrl(string pathwayId, IEnumerable<string> answeredQuestionIds, bool multipleChoice, string selectedQuestionId);

        /* Pathways */
        string GetDomainApiPathwaysUrl(bool grouped);
        string GetDomainApiPathwayUrl(string pathwayId);
        string GetDomainApiIdentifiedPathwayUrl(string pathwayNumbers, string gender, int age);
        string GetDomainApiPathwaySymptomGroup(string pathwayNumbers);

        /* Care Advice */
        string GetDomainApiCareAdviceUrl(int age, string gender, IEnumerable<string> markers);
        string GetRedisUrl();
    }
}