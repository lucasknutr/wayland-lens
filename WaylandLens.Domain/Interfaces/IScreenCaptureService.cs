using WaylandLens.Domain.Entities;

namespace WaylandLens.Domain.Interfaces;

public interface IScreenCaptureService
{
    Task<ImageCapture> CaptureScreenAsync();
}