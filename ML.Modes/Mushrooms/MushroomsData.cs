
using Microsoft.ML.Data;

namespace ML.Modes.Mushrooms
{
    public class MushroomsData
    {
        [ColumnName("class"), LoadColumn(0)]
        public string Class { get; set; }


        [ColumnName("cap-shape"), LoadColumn(1)]
        public string Cap_shape { get; set; }


        [ColumnName("cap-surface"), LoadColumn(2)]
        public string Cap_surface { get; set; }


        [ColumnName("cap-color"), LoadColumn(3)]
        public string Cap_color { get; set; }


        [ColumnName("bruises"), LoadColumn(4)]
        public string Bruises { get; set; }


        [ColumnName("odor"), LoadColumn(5)]
        public string Odor { get; set; }


        [ColumnName("gill-attachment"), LoadColumn(6)]
        public string Gill_attachment { get; set; }


        [ColumnName("gill-spacing"), LoadColumn(7)]
        public string Gill_spacing { get; set; }


        [ColumnName("gill-size"), LoadColumn(8)]
        public string Gill_size { get; set; }


        [ColumnName("gill-color"), LoadColumn(9)]
        public string Gill_color { get; set; }


        [ColumnName("stalk-shape"), LoadColumn(10)]
        public string Stalk_shape { get; set; }


        [ColumnName("stalk-root"), LoadColumn(11)]
        public string Stalk_root { get; set; }


        [ColumnName("stalk-surface-above-ring"), LoadColumn(12)]
        public string Stalk_surface_above_ring { get; set; }


        [ColumnName("stalk-surface-below-ring"), LoadColumn(13)]
        public string Stalk_surface_below_ring { get; set; }


        [ColumnName("stalk-color-above-ring"), LoadColumn(14)]
        public string Stalk_color_above_ring { get; set; }


        [ColumnName("stalk-color-below-ring"), LoadColumn(15)]
        public string Stalk_color_below_ring { get; set; }


        [ColumnName("veil-type"), LoadColumn(16)]
        public string Veil_type { get; set; }


        [ColumnName("veil-color"), LoadColumn(17)]
        public string Veil_color { get; set; }


        [ColumnName("ring-number"), LoadColumn(18)]
        public string Ring_number { get; set; }


        [ColumnName("ring-type"), LoadColumn(19)]
        public string Ring_type { get; set; }


        [ColumnName("spore-print-color"), LoadColumn(20)]
        public string Spore_print_color { get; set; }


        [ColumnName("population"), LoadColumn(21)]
        public string Population { get; set; }


        [ColumnName("habitat"), LoadColumn(22)]
        public string Habitat { get; set; }


    }
}
