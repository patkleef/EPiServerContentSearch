﻿using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.SpecializedProperties;

namespace EPiServerContentSearch.Models.Pages
{
    [ContentType(DisplayName = "News item", GUID = "3226ead5-eaa6-45d9-99bd-50491dbbbd6e", Description = "")]
    public class NewsItemPage : PageData
    {
        [Display(
            Name = "News date",
            Description = "News date",
            GroupName = SystemTabNames.Content,
            Order = 10)]
        public virtual DateTime NewsDate { get; set; }
         
    }
}