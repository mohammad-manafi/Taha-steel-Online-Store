using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace ServiceHost.Areas.Administration.Pages
{
    public class IndexModel : PageModel
    {
        public Chart DoughnutDataSet { get; set; }
        public List<Chart> BarLineDataSet { get; set; }

        public void OnGet()
        {
            BarLineDataSet = new List<Chart>
            {
                new Chart
                {
                    Label = "کنسول",
                    Data = new List<int> {100, 200, 250, 170, 250, 167, 345, 423, 169, 75, 218, 198},
                    BackgroundColor = new[] {"#ffcdb2"},
                    BorderColor = "#b5838d"
                },
                new Chart
                {
                    Label = "میز جلو مبلی استیل",
                    Data = new List<int> {200, 300, 350, 270, 100, 432, 234, 532, 478, 123, 490, 590},
                    BackgroundColor = new[] {"#ffc8dd"},
                    BorderColor = "#ffafcc"
                },
                new Chart
                {
                    Label = "میز آرایش",
                    Data = new List<int> {200, 300, 350, 270, 100, 432, 234, 532, 478, 123, 490, 590},
                    BackgroundColor = new[] {"#ffc8dd"},
                    BorderColor = "#ffafcc"
                },
                new Chart
                {
                    Label = "ویترین",
                    Data = new List<int> {200, 300, 350, 270, 100, 432, 234, 532, 478, 123, 490, 590},
                    BackgroundColor = new[] {"#ffc8dd"},
                    BorderColor = "#ffafcc"
                },
                new Chart
                {
                    Label = "تخت خواب",
                    Data = new List<int> {200, 300, 350, 270, 100, 432, 234, 532, 478, 123, 490, 590},
                    BackgroundColor = new[] {"#ffc8dd"},
                    BorderColor = "#ffafcc"
                },
                new Chart
                {
                    Label = "میز و صندلی",
                    Data = new List<int> {200, 300, 350, 270, 100, 432, 234, 532, 478, 123, 490, 590},
                    BackgroundColor = new[] {"#ffc8dd"},
                    BorderColor = "#ffafcc"
                },
                new Chart
                {
                    Label = "میز استیل عسلی",
                    Data = new List<int> {200, 300, 350, 270, 100, 432, 234, 532, 478, 123, 490, 590},
                    BackgroundColor = new[] {"#ffc8dd"},
                    BorderColor = "#ffafcc"
                },
                new Chart
                {
                    Label = "Total",
                    Data = new List<int> {200, 300, 350, 270, 100, 432, 234, 532, 478, 123, 490, 590},
                    BackgroundColor = new[] {"#0077b6"},
                    BorderColor = "#023e8a"
                },
            };
            DoughnutDataSet = new Chart
            {
                Label = "Apple",
                Data = new List<int> { 200, 300, 350, 270, 100, 432, 234, 532, 478, 123, 490, 590 },
                BorderColor = "#ffcdb2",
                BackgroundColor = new[] {"#b5838d", "#ffd166", "#7f4f24", "#ef233c", "#003049","#b5838d", "#ffd166", "#7f4f24", "#ef233c", "#003049", "#003049", "#b5838d"}
            };
        }
    }

    public class Chart
    {
        [JsonProperty(PropertyName = "label")] public string Label { get; set; }

        [JsonProperty(PropertyName = "data")] public List<int> Data { get; set; }

        [JsonProperty(PropertyName = "backgroundColor")]
        public string[] BackgroundColor { get; set; }

        [JsonProperty(PropertyName = "borderColor")]
        public string BorderColor { get; set; }
    }
}