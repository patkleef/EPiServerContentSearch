using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Castle.DynamicProxy.Generators;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.Framework.DataAnnotations;
using EPiServer.ServiceLocation;
using EPiServer.Web.Mvc;
using EPiServerContentSearch.Models.Pages;

namespace EPiServerContentSearch.Controllers
{
    public class DefaultPageController : PageController<PageData>
    {
        public ActionResult Index(PageData currentPage)
        {
            var test = ServiceLocator.Current.GetInstance<IContentTypeRepository>();
            var test1 = test.List();
            /* Implementation of action. You can create your own view model class that you pass to the view or
             * you can pass the page type for simpler templates */

            return View(string.Format("~/Views/{0}/Index.cshtml", currentPage.GetOriginalType().Name), currentPage);
        }
    }
}