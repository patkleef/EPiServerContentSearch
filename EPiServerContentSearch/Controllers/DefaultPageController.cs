using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Castle.DynamicProxy.Generators;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAccess;
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
            var test = ServiceLocator.Current.GetInstance<IContentRepository>();
            var children = test.GetChildren<NewsItemPage>(new ContentReference(6));

            /*foreach (var item in children)
            {
                var writeable = (NewsItemPage)item.CreateWritableClone();
                //writeable.NewsDate = RandomDate();
                //writeable.Category = GetRandomCategories();

                //DataFactory.Instance.Save(writeable, SaveAction.Publish);
            }*/

            /* Implementation of action. You can create your own view model class that you pass to the view or
             * you can pass the page type for simpler templates */

            return View(string.Format("~/Views/{0}/Index.cshtml", currentPage.GetOriginalType().Name), currentPage);
        }

        private DateTime RandomDate()
        {
            Random rnd = new Random();
            int month = rnd.Next(1, 13);

            return new DateTime(2015, month, 1);
        }

        private CategoryList GetRandomCategories()
        {
            var cat = Category.Find("news type");
            Random rnd = new Random();
            int number = rnd.Next(1, 4);

            var tst = cat.Categories[number - 1];

            var d = new CategoryList();
            d.Add(tst.ID);

            return d;
        }
    }
}