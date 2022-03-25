using Structr.Abstractions.Attributes;

namespace Structr.Samples.Abstractions.Flowers
{
    public enum FlowerId
    {
        [BindProperty("Description", "Gazania description")]
        [BindProperty("Color", FlowerColor.Yellow)]
        [BindProperty("DateCreated", null)]
        Gazania,

        [BindProperty("Description", "Windflower description")]
        [BindProperty("Color", FlowerColor.Red)]
        [BindProperty("DateCreated", "2020-04-20")]
        Windflower,

        [BindProperty("Description", "Masterwort description")]
        [BindProperty("Color", FlowerColor.White)]
        [BindProperty("DateCreated", null)]
        Masterwort,

        [BindProperty("Description", "Hellebore description")]
        [BindProperty("Color", FlowerColor.White)]
        [BindProperty("DateCreated", "2020-04-20")]
        Hellebore
    }
}
