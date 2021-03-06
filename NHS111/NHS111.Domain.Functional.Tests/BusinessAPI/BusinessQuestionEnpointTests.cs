﻿using System;
using System.ComponentModel;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using NHS111.Utils.Helpers;
using NUnit.Framework;

namespace NHS111.Domain.Functional.Tests
{
    [TestFixture]
    public class BusinessQuestionEndpointTests
    {
        private string _BusinessdomainApiDomain =
            "https://microsoft-apiapp40f6723d48db47ed8f4d3ff1-integration.azurewebsites.net/";

        private string _testQuestionId = "PW1346.1000";
        private string _testPathwayId = "P275";
        private string _testPathwayId2 = "P908";
        private string _testPathwayId3 = "P1786";
        private string _testPathwayId4 = "PW752";
        private string _testPathwayNo = "PW1401";
        private string _expectedNodeId = "PW752.200";
        

        private RestfulHelper _restfulHelper = new RestfulHelper();

        /// <summary>

        // Question Controller tests
        [Test]
        public async void TestGetQuestion_returns_valid_nodeId_response()
        {
            var getQuestionEndpoint = "node/{0}/next_node/{1}/answer/yes?state=%7B%22PATIENT_AGE%22%3A%2222%22%7D";
            var result = await _restfulHelper.GetAsync(String.Format(_BusinessdomainApiDomain + getQuestionEndpoint, _testPathwayId2, _expectedNodeId));


            Assert.IsNotNull(result);
           
            //these check the right fields are returned
            AssertValidResponseSchema(result, ResponseSchemaType.Question);

            Assert.IsTrue(result.Contains("\"id\":\"PW758.0"));
           
        }

        [Test]
        public async void TestGetQuestion_returns_valid_node_fields()
        {
            var getQuestionEndpoint = "node/{0}/answers/{1}";
            var result = await _restfulHelper.GetAsync(String.Format(_BusinessdomainApiDomain + getQuestionEndpoint, _testPathwayId, _testQuestionId));

            //this checks a responce is returned
            Assert.IsNotNull(result);

            //these check the right fields are returned
            AssertValidResponseSchema(result, ResponseSchemaType.Answers);

            //this next one checks the right question has returned
            Assert.IsTrue(result.Contains("\"title\":\"Yes\",\"symptomDiscriminator\":\"4028\",\"order\":1"));
         }
         

        [Test]
        public async void TestGetQuestion_returns_valid_node_question()
        {
            var getQuestionEndpoint = "node/{0}/question/{1}";
            var result = await _restfulHelper.GetAsync(String.Format(_BusinessdomainApiDomain + getQuestionEndpoint, _testPathwayId, _testQuestionId));

            //this checks a responce is returned
            Assert.IsNotNull(result);

            //these check the right fields are returned
            AssertValidResponseSchema(result, ResponseSchemaType.Question);

            //this next one checks the right answers have returned
            Assert.IsTrue(result.Contains("\"title\":\"Yes"));
            Assert.IsTrue(result.Contains("\"title\":\"Not sure"));
            Assert.IsTrue(result.Contains("\"title\":\"No"));
        }


        [Test]
        public async void TestGetQuestion_returns_valid_node_first_question()
        {
            var getQuestionEndpoint = "node/{0}/questions/first/?state=%7B%22PATIENT_AGE%22%3A%2222%22%7D";
            var result = await _restfulHelper.GetAsync(String.Format(_BusinessdomainApiDomain + getQuestionEndpoint, _testPathwayId2));

            //this checks a responce is returned
            Assert.IsNotNull(result);

            //these check the right fields are returned
            AssertValidResponseSchema(result, ResponseSchemaType.Question);

            //this next one checks the right question has returned
            Assert.IsTrue(result.Contains("\"questionNo\":\"Tx1715"));
        }
        [Test]
        public async void TestGetQuestion_returns_valid_node_first_jtbs()
        {
            var getQuestionEndpoint = "node/{0}/jtbs_first";
            var result = await _restfulHelper.GetAsync(String.Format(_BusinessdomainApiDomain + getQuestionEndpoint, _testPathwayId3));

            //this checks an empty responce is returned as JTBS questions not implemented yet
            Assert.IsNotNull(result);
                        Assert.IsTrue(result.Contains("[]"));

        }

        // Care Advice Controller tests
        [Test]
        public async void TestGetQuestion_returns_valid_care_advice_AdultAge()
        {
            var getQuestionEndpoint = "pathways/care-advice/43/Female?markers=Cx220179";
            var result = await _restfulHelper.GetAsync(String.Format(_BusinessdomainApiDomain + getQuestionEndpoint));

            //this checks a responce is returned
            Assert.IsNotNull(result);

            //these check the right fields are returned
            Assert.IsTrue(result.Contains("\"id"));
            Assert.IsTrue(result.Contains("\"title"));
            Assert.IsTrue(result.Contains("\"excludeTitle"));
            Assert.IsTrue(result.Contains("\"items"));

            //these check the wrong fields are not returned
            AssertValidResponseSchema(result, ResponseSchemaType.Answer);

            //this next one checks the right question has returned
            Assert.IsTrue(result.Contains("\"id\":\"Cx220179-Adult-Female"));
            Assert.IsTrue(result.Contains("\"title\":\"Needlestick injury"));
        }

        [Test]
        public async void TestGetQuestion_returns_valid_care_advice_ToddlerAge()
        {
            var getQuestionEndpoint = "pathways/care-advice/1/Female?markers=Cx220179";
            var result = await _restfulHelper.GetAsync(String.Format(_BusinessdomainApiDomain + getQuestionEndpoint));

            //this checks a responce is returned
            Assert.IsNotNull(result);

            //these check the right fields are returned
            Assert.IsTrue(result.Contains("\"id"));
            Assert.IsTrue(result.Contains("\"title"));
            Assert.IsTrue(result.Contains("\"excludeTitle"));
            Assert.IsTrue(result.Contains("\"items"));

            //these check the wrong fields are not returned
            AssertValidResponseSchema(result, ResponseSchemaType.Answer);

            //this next one checks the right question has returned
            Assert.IsFalse(result.Contains("\"id\":\"Cx220179-Adult-Female"));
            Assert.IsTrue(result.Contains("\"id\":\"Cx220179-Toddler-Female"));
            Assert.IsTrue(result.Contains("\"title\":\"Needlestick injury"));
        }

     

        // Pathways Controller tests

        [Test]
        public async void TestGetQuestion_returns_valid_Pathway_ID()
        {
            var getQuestionEndpoint = "pathway/{0}";
            var result = await _restfulHelper.GetAsync(String.Format(_BusinessdomainApiDomain + getQuestionEndpoint, _testPathwayId));

            //this checks a responce is returned
            Assert.IsNotNull(result);

            //these check the right fields are returned
            AssertValidResponseSchema(result, ResponseSchemaType.Pathway);

            //this next one checks the right question has returned
            Assert.IsTrue(result.Contains("\"title\":\"Headache"));
            Assert.IsTrue(result.Contains("\"id\":\"P275"));
            Assert.IsTrue(result.Contains("\"gender\":\"Female"));
            Assert.IsFalse(result.Contains("\"title\":\"Head, Facial or Neck Injury, Blunt"));
            Assert.IsFalse(result.Contains("\"title\":\"Abdominal Pain"));
        }
        [Test]
        public async void TestGetQuestion_returns_valid_Pathway_Numbers()
        {
            var getQuestionEndpoint = "pathway/{0}/Female/16";
            var result = await _restfulHelper.GetAsync(String.Format(_BusinessdomainApiDomain + getQuestionEndpoint, _testPathwayId4));

            //this checks a responce is returned
            Assert.IsNotNull(result);

            //these check the right fields are returned
            AssertValidResponseSchema(result, ResponseSchemaType.Pathway);

            //this next one checks the right question has returned
            Assert.IsTrue(result.Contains("\"title\":\"Headache"));
            Assert.IsTrue(result.Contains("\"id\":\"P908"));
            Assert.IsTrue(result.Contains("\"gender\":\"Female"));
            Assert.IsTrue(result.Contains("\"pathwayNo\":\"PW752"));

        }
        [Test]
        public async void TestGetQuestion_returns_valid_Pathway_Numbers_InvalidAge1()
        {
            var getQuestionEndpoint = "pathway/{0}/Female/1";
            var result = await _restfulHelper.GetAsync(String.Format(_BusinessdomainApiDomain + getQuestionEndpoint, _testPathwayId4));

            //this checks a responce is returned
            Assert.IsTrue(result.Contains("null"));


        }
        [Test]
        public async void TestGetQuestion_returns_valid_Pathway_Numbers_InvalidAge200()
        {
            var getQuestionEndpoint = "pathway/{0}/Female/200";
            var result = await _restfulHelper.GetAsync(String.Format(_BusinessdomainApiDomain + getQuestionEndpoint, _testPathwayId4));

            //this checks a responce is returned
            Assert.IsTrue(result.Contains("null"));


        }
        [Test]
        public async void TestGetQuestion_returns_valid_Pathway_Numbers_InvalidAge15()
        {
            var getQuestionEndpoint = "pathway/{0}/Female/15";
            var result = await _restfulHelper.GetAsync(String.Format(_BusinessdomainApiDomain + getQuestionEndpoint, _testPathwayId4));

            //this checks a responce is returned
            Assert.IsTrue(result.Contains("null"));


        }
        [Test]
        public async void TestGetQuestion_returns_valid_Pathway_Numbers_InvalidGender()
        {
            var getQuestionEndpoint = "pathway/{0}/Male/16";
            var result = await _restfulHelper.GetAsync(String.Format(_BusinessdomainApiDomain + getQuestionEndpoint, _testPathwayId4));

            //this checks a responce is returned
            Assert.IsTrue(result.Contains("null"));


        }
      
        [Test]
        public async void TestGetQuestion_returns_valid_Pathway_Symptom_Group()
        {
            var getQuestionEndpoint = "pathway/symptomGroup/{0}";
            var result = await _restfulHelper.GetAsync(String.Format(_BusinessdomainApiDomain + getQuestionEndpoint, _testPathwayNo));

            //this checks a responce is returned
            Assert.IsNotNull(result);

            //these check the right fields are returned
            Assert.IsTrue(result.Contains("1110"));

            //this checks only the SD code returns
            Assert.AreEqual("", result.Replace("1110", ""));

        }
        [Test]
        public async void TestGetQuestion_returns_valid_Pathway()
        {
            var getQuestionEndpoint = "pathway";
            var result = await _restfulHelper.GetAsync(String.Format(_BusinessdomainApiDomain + getQuestionEndpoint));

            //this checks a responce is returned
            Assert.IsNotNull(result);

            //these check the right fields are returned
            AssertValidResponseSchema(result, ResponseSchemaType.Pathway);

           //this next one checks the right question has returned
            Assert.IsTrue(result.Contains("\"title\":\"Headache"));
            Assert.IsTrue(result.Contains("\"title\":\"Head, Facial or Neck Injury, Blunt"));
            Assert.IsTrue(result.Contains("\"id\":\"P130"));
            Assert.IsTrue(result.Contains("\"id\":\"P275"));
            Assert.IsTrue(result.Contains("\"gender\":\"Female"));
            Assert.IsTrue(result.Contains("\"gender\":\"Male"));
            Assert.IsTrue(result.Contains("\"pathwayNo\":\"PW684"));
            Assert.IsTrue(result.Contains("\"pathwayNo\":\"PW1401"));

        }
        
        [Test]
        //pathway_suggest/{name}
        public async void TestGetQuestion_returns_expected_Pathways_beginning_with()
        {
            var getQuestionEndpoint = "pathway_suggest/Head";
            var result = await _restfulHelper.GetAsync(String.Format(_BusinessdomainApiDomain + getQuestionEndpoint));

            //this checks a responce is returned
            Assert.IsNotNull(result);

            //this next one checks the right question has returned
            Assert.IsTrue(result.Contains("\"pathwayNumbers\":[\"PW753\",\"PW756\",\"PW752\",\"PW755\",\"PW754\",\"PW754\"],\"group\":\"Headache\"},{\"pathwayNumbers\":[\"PW1401\",\"PW1401\",\"PW686\",\"PW686\",\"PW684\",\"PW684\",\"PW684\",\"PW684\"],\"group\":\"Head, Facial or Neck Injury, Blunt"));
        }


        public static HttpRequestMessage CreateHTTPRequest(string requestContent)
        {
            return new HttpRequestMessage
            {
                Content = new StringContent("\"" + requestContent + "\"", Encoding.UTF8, "application/json")
            };
        }


        public enum ResponseSchemaType
        {
            Pathway,
            Question,
            Answer,
            Answers
        }

        private static void AssertValidResponseSchema(string result, ResponseSchemaType schemaType)
        {
            switch (schemaType)
            {
                case ResponseSchemaType.Pathway:
                    AssertValidPathwayResponseSchema(result);
                    break;
                case ResponseSchemaType.Answers:
                    AssertValidAnswersResponseSchema(result);
                    break;
                case ResponseSchemaType.Question:
                    AssertValidQuestionResponseSchema(result);
                    break;
                case ResponseSchemaType.Answer:
                    AssertValidAnswerResponseSchema(result);
                    break;
                default:
                    throw new InvalidEnumArgumentException("ResponseSchemaType of " + schemaType.ToString() +
                                                       "is unsupported");
            }
            
        }



        private static void AssertValidAnswerResponseSchema(string result)
        {
            Assert.IsFalse(result.Contains("\"Question"));
            Assert.IsFalse(result.Contains("\"group"));
            Assert.IsFalse(result.Contains("\"topic"));
            Assert.IsFalse(result.Contains("\"questionNo"));
            Assert.IsFalse(result.Contains("\"jtbs"));
            Assert.IsFalse(result.Contains("\"jtbsText"));
            Assert.IsFalse(result.Contains("\"Answers"));
            Assert.IsFalse(result.Contains("\"Labels"));
        }

        private static void AssertValidAnswersResponseSchema(string result)
        {
            Assert.IsTrue(result.Contains("\"title"));
            Assert.IsTrue(result.Contains("\"symptomDiscriminator"));
            Assert.IsTrue(result.Contains("\"order"));

        }
        private static void AssertValidPathwayResponseSchema(string result)
        {
            Assert.IsTrue(result.Contains("\"id"));
            Assert.IsTrue(result.Contains("\"title"));
            Assert.IsTrue(result.Contains("\"pathwayNo"));
            Assert.IsTrue(result.Contains("\"gender"));
            Assert.IsTrue(result.Contains("\"minimumAgeInclusive"));
            Assert.IsTrue(result.Contains("\"maximumAgeExclusive"));
            Assert.IsTrue(result.Contains("\"module"));
            Assert.IsTrue(result.Contains("\"symptomGroup"));
            Assert.IsTrue(result.Contains("\"group"));
        }

        private static void AssertValidQuestionResponseSchema(string result)
        {
            
            Assert.IsTrue(result.Contains("\"Question"));
            Assert.IsTrue(result.Contains("\"group"));
            Assert.IsTrue(result.Contains("\"order"));
            Assert.IsTrue(result.Contains("\"topic"));
            Assert.IsTrue(result.Contains("\"id"));
            Assert.IsTrue(result.Contains("\"questionNo"));
            Assert.IsTrue(result.Contains("\"title"));
            Assert.IsTrue(result.Contains("\"jtbs"));
            Assert.IsTrue(result.Contains("\"jtbsText"));
            Assert.IsTrue(result.Contains("\"Answers"));
            Assert.IsTrue(result.Contains("\"symptomDiscriminator"));
            Assert.IsTrue(result.Contains("\"Labels"));
            Assert.IsTrue(result.Contains("\"State"));
        }

    }
}
