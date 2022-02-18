using Structr.Abstractions.Attributes;

namespace Structr.Samples.Abstractions.Enum
{
    public enum EFlower
    {
        [BindProperty("Description", "Gazania description")]
        [BindProperty("Color", EFlowerColor.Yellow)]
        [BindProperty("DateCreated", null)]
        Gazania,

        [BindProperty("Description", "Windflower description")]
        [BindProperty("Color", EFlowerColor.Red)]
        [BindProperty("DateCreated", "2020-04-20")]
        Windflower,

        [BindProperty("Description", "Masterwort description")]
        [BindProperty("Color", EFlowerColor.White)]
        [BindProperty("DateCreated", null)]
        Masterwort,

        [BindProperty("Description", "Hellebore description")]
        [BindProperty("Color", EFlowerColor.White)]
        [BindProperty("DateCreated", "2020-04-20")]
        Hellebore
    }
}
