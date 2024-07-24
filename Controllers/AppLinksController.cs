using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Data.Common;

namespace Controllers
{
	public class AppLinksController : Controller
	{
        private List<string>? UpdatedFavouriteCookiesValue { get; set; }

        public IActionResult Index()
        {
            ViewBag.Options = OptionsCheck();

            if (UpdatedFavouriteCookiesValue != null)
                ViewBag.Favourites = UpdatedFavouriteCookiesValue;
            else
                ViewBag.Favourites = GetFavourites();
            ViewBag.Title = "";

            return View("AppLinks", Models.AppLinksModel.GetAllAppLinks());
        }

        public IActionResult AppRestart(string poolName)
        {
            Models.AppLinksModel.AppRestart(poolName);
            return RedirectToAction("Index");
        }

        public IActionResult SearchResults(string SearchFor)
        {
            ViewBag.Options = OptionsCheck();
            if (SearchFor != "" && SearchFor != null)
            {
                var result = Models.AppLinksModel.GetAppLinksByText(SearchFor);
                if (result.Count == 0)
                    ViewBag.Title = "Žádna aplikace nebyla nalezena";
                else ViewBag.Title = $"Výsledky pro: {SearchFor}";

                return View("AppLinks", result);
            }
            else
            {
                ViewBag.Title = "Výsledky hledání: ";
                return Index();
            }
        }
        
        #region Cookie handling
        public IActionResult AddFavourite(string name)
        {
            var favourites = GetFavourites();
            if (!favourites.Contains(name))
            {
                favourites.Add(name);
                Response.Cookies.Delete("Favourites");
                Response.Cookies.Append("Favourites", string.Join(",", favourites), new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(360),
                });
                UpdatedFavouriteCookiesValue = favourites;
            }
            return RedirectToAction("Index");
        }

        public IActionResult RemoveFavourite(string name)
        {
            var favourites = GetFavourites();
            if (favourites.Contains(name))
            {
                favourites.Remove(name);
                Response.Cookies.Delete("Favourites");
                Response.Cookies.Append("Favourites", string.Join(",", favourites), new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(360),
                });
                UpdatedFavouriteCookiesValue = favourites;
            }
            return RedirectToAction("Index");
        }

        private List<string> GetFavourites()
        {
            List<string> result = new List<string>();

            try
            {
                if (Request.Cookies == null || Request.Cookies["Favourites"] == null)
                {
                    Response.Cookies.Append("Favourites", string.Join(",", result), new CookieOptions
                    {
                        Expires = DateTime.Now.AddDays(360),
                    });
                    return result;
                }
                else
                {
                    var cookieValue = Request.Cookies["Favourites"];
                    var values = cookieValue?.Split(",");
                    if (cookieValue.Length > 0)
                        foreach (var name in values)
                            result.Add(name);

                    return result;
                }
            }
            catch (Exception e)
            {
                throw new Exception("Failed to retrieve 'favourites' cookies. " + e.Message);
            }
        }

        private bool OptionsCheck()
        {
            if (Request.Cookies == null || Request.Cookies["MPSopt"] == null)
            {
                Response.Cookies.Append("MPSopt", "false", new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(30),
                });
                return false;
            }
            string cookieValue = Request.Cookies["MPSopt"];

            if (cookieValue == "false")
                return false;
            else return true;
        }
        #endregion
    }
}
