namespace WaylandLens.Domain.Entities;

public class ImageCapture
{
   public required byte[] Data { get; set; }
   public int Width { get; set; }
   public int Height { get; set; }
}