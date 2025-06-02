using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;
using Task.Models;
using Task.Repositories;

namespace Task.RazorHelpers
{
    public static class PizzaHelper
    {
        public static async Task<List<int>> GetAvailableSizes(IPizzaRepository pizzaRepository) {
            var availableSizes = await pizzaRepository.GetAvaiableSizes();
            var sizeValues = availableSizes.Select(s => s.Value).ToList();
            return sizeValues;
        }
        public static async Task<List<string>> GetAvailableTypes(IPizzaRepository pizzaRepository)
        {
            var availableTypes = await pizzaRepository.GetAvaiableDoughTypes();
            var typeNames = availableTypes.Select(t => t.Name).ToList();
            return typeNames;
        }
        public static (int activeSize, string activeType) GetActiveSelectors(this IHtmlHelper html, PizzaModel model, List<int> sizeValues, List<string> typeNames)
        {
            var activeSize = model.Sizes.Contains(sizeValues[0])
                ? sizeValues[0]
                : model.Sizes.FirstOrDefault(size => sizeValues.Contains(size));

            var activeType = model.Types?.Contains(typeNames[0]) == true
                ? typeNames[0]
                : model.Types?.FirstOrDefault(type => typeNames.Contains(type));

            return (activeSize, activeType);
        }

        public static IHtmlContent RenderSelectors<T>
        (
            this IHtmlHelper htmlHelper,
            List<T> referenceArray,
            List<T> selectorData,
            T selectedSelector,
            bool isSizeSelector
        )
        {
            var resultHtml = new StringBuilder();
            foreach (var item in referenceArray)
            {
                if (selectorData.Contains(item))
                {
                    var activeClass = item.Equals(selectedSelector) ? "active" : "";
                    resultHtml.Append($"<span class=\"size {activeClass}\">{item} {(isSizeSelector ? "см" : "")}</span>");
                }
                else
                {
                    resultHtml.Append($"<span class=\"size disable\">{item} {(isSizeSelector ? "см" : "")}</span>");
                }
            }
            return new HtmlString(resultHtml.ToString());
        }

        public static List<string> GetAvailableImages(this IHtmlHelper htmlHelper)
        {
            var imageDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "pizzas");
            var imageFiles = Directory.GetFiles(imageDir)
                                      .Select(path => "/images/pizzas/" + Path.GetFileName(path))
                                      .ToList();
            return imageFiles;
        }
    }
}