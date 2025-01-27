namespace ImageCloude.Models
{
    public class ImageModel
    {
        public int ImageId { get; set; } // Typo corrected to ImageId
        public string ImageUrl { get; set; }
        public string ImageName { get; set; }
        public string PublicId { get; set; } // Required for Cloudinary image management
    }
}
