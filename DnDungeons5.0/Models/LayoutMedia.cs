using System.ComponentModel.DataAnnotations;

namespace DnDungeons.Models
{
    public class LayoutMedia
    {
        // key
        public int LayoutID { get; set; }
        public int MediaID { get; set; }

        // properties
        [StringLength(50, ErrorMessage = "Label cannot be longer than 50 characters.")]
        public string MediaLabel { get; set; }

        // navigation properties
        public Layout Layout { get; set; }
        public Media Media { get; set; }
    }
}
