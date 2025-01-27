using ImageCloude.Data;
using ImageCloude.Models;
using ImageCloude.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ImageCloude.Controllers
{
    public class ImageController : Controller
    {
        private readonly AppDbContext _context;
        private readonly CloudinaryService _cloudinaryService;

        public ImageController(AppDbContext context, CloudinaryService cloudinaryService)
        {
            _context = context;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<IActionResult> Index(string searchQuery)
        {
            var images = string.IsNullOrEmpty(searchQuery)
                ? await _context.Images.ToListAsync()
                : await _context.Images
                    .Where(i => i.ImageName.Contains(searchQuery) || i.ImageId.ToString() == searchQuery)
                    .ToListAsync();

            return View(images);
        }

        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file, string imageName)
        {
            if (file == null || string.IsNullOrEmpty(imageName))
                return BadRequest("File or image name cannot be null.");

            var uploadResult = await _cloudinaryService.UploadImageAsync(file);

            if (uploadResult == null)
                return BadRequest("Image upload failed.");

            var image = new ImageModel
            {
                ImageUrl = uploadResult.SecureUrl.AbsoluteUri,
                ImageName = imageName,
                PublicId = uploadResult.PublicId
            };

            _context.Images.Add(image);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var image = await _context.Images.FindAsync(id);
            if (image == null) return NotFound();

            await _cloudinaryService.DeleteImageAsync(image.PublicId);
            _context.Images.Remove(image);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var image = await _context.Images.FindAsync(id);
            if (image == null) return NotFound();

            return View(image);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ImageModel updatedImage)
        {
            var image = await _context.Images.FindAsync(updatedImage.ImageId);
            if (image == null) return NotFound();

            image.ImageName = updatedImage.ImageName;
            _context.Images.Update(image);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
