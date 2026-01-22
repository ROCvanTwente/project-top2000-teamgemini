using Microsoft.AspNetCore.Mvc;

namespace TemplateJwtProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImageProxyController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public ImageProxyController()
        {
            _httpClient = new HttpClient();
        }

        [HttpGet("proxy")]
        public async Task<IActionResult> ProxyImage([FromQuery] string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return BadRequest("URL parameter is required");
            }

            try
            {
                // Validate that it's a proper URL
                if (!Uri.TryCreate(url, UriKind.Absolute, out var uri))
                {
                    return BadRequest("Invalid URL");
                }

                // Fetch the image from the external source
                var response = await _httpClient.GetAsync(uri);
                
                if (!response.IsSuccessStatusCode)
                {
                    return StatusCode((int)response.StatusCode, "Failed to fetch image");
                }

                var contentType = response.Content.Headers.ContentType?.MediaType ?? "image/png";
                var imageBytes = await response.Content.ReadAsByteArrayAsync();

                // Return with proper CORS headers
                Response.Headers.Add("Cache-Control", "public, max-age=86400"); // Cache for 24 hours
                Response.Headers.Add("Access-Control-Allow-Origin", "*");
                return File(imageBytes, contentType);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching image: {ex.Message}");
            }
        }
    }
}
