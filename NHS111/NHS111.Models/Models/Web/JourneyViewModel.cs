﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json;
using NHS111.Models.Models.Domain;
using NHS111.Models.Models.Web.Enums;
using NHS111.Models.Models.Web.FromExternalServices;

namespace NHS111.Models.Models.Web
{
    public class JourneyViewModel
    {
        public Guid UserId { get; set; }
        public string PathwayId { get; set; }
        public string PathwayNo { get; set; }
        public string PathwayTitle { get; set; }
        public string Id { get; set; }

        private string _title;
        public string Title
        {
            get { return string.IsNullOrEmpty(_title) ? "" : _title; }
            set { _title = value; }
        }
        public string TitleWithoutBullets { get; set; }
        public List<string> Bullets { get; set; }

        public List<Answer> Answers { get; set; }
        public string SelectedAnswer { get; set; }
        public NodeType NodeType { get; set; }

        public string JourneyJson { get; set; }

        public UserInfo UserInfo { get; set; }

        public string PreviousTitle { get; set; }
        public string PreviousStateJson { get; set; }
        public string QuestionNo { get; set; }
        public string SymptomDiscriminator { get; set; }
        public IDictionary<string, string> State { get; set; }
        public string StateJson { get; set; }



        public JourneyViewModel()
        {
            Answers = new List<Answer>();
            JourneyJson = JsonConvert.SerializeObject(new Journey());
            Bullets = new List<string>();
            State = new Dictionary<string, string>();
        }

        public List<Answer> OrderedAnswers()
        {
            return Answers.OrderBy(x=>x.Order).ToList();
        }
    }
}