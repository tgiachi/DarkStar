using System;
using Avalonia.Media.Imaging;

namespace DarkStar.Client.Models;

public class PlayerSelectEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public CroppedBitmap Image { get; set; }

    public int Level { get; set; }

}
