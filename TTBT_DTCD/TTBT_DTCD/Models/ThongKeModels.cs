using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TTBT_DTCD.Models
{
    public class ThongKeChartItem
    {
        [JsonPropertyName("placeId")]
        public int PlaceId { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("soLuotTrongNuoc")]
        public long SoLuotTrongNuoc { get; set; }

        [JsonPropertyName("soLuotQuocTe")]
        public long SoLuotQuocTe { get; set; }
    }

    public class ThongKeGridItem
    {
        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; } = string.Empty;

        [JsonPropertyName("placeId")]
        public int PlaceId { get; set; }

        [JsonPropertyName("soLuongTrongNuoc")]
        public long SoLuongTrongNuoc { get; set; }

        [JsonPropertyName("soLuongQuocTe")]
        public long SoLuongQuocTe { get; set; }

        [JsonPropertyName("customerTypeId")]
        public int CustomerTypeId { get; set; }
    }

    public class ThongKeResponse
    {
        [JsonPropertyName("chart")]
        public List<ThongKeChartItem> Chart { get; set; } = new();

        [JsonPropertyName("grid")]
        public List<ThongKeGridItem> Grid { get; set; } = new();
    }
}
