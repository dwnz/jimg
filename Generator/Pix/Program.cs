using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Pix
{
  internal class Program
  {
    private static void Main(string[] args)
    {
      var image = new Bitmap("C:\\temp\\surf.jpg");

      Dictionary<string, JColor> cache = new Dictionary<string, JColor>();

      JIMG jImage = new JIMG
      {
        Height = image.Height,
        Width = image.Width
      };

      for (var y = 0; y < image.Height; y++)
      {
        for (var x = 0; x < image.Width; x++)
        {
          var pix = image.GetPixel(x, y);
          string rgb = string.Format("{0},{1},{2}", pix.R, pix.G, pix.B);

          if (!cache.ContainsKey(rgb))
          {
            cache.Add(rgb, new JColor
            {
              Coords = new List<Coord>(),
              RGB = rgb
            });
          }

          cache[rgb].Coords.Add(new Coord
          {
            X = x,
            Y = y
          });
        }
      }

      jImage.ColorCoords = cache.Select(x => x.Value).ToList();

      File.WriteAllText("C:\\temp\\surf.jimg", JsonConvert.SerializeObject(jImage));

      Console.WriteLine("Done");
      Console.ReadKey();
    }
  }

  internal class JIMG
  {
    public JIMG()
    {
      ColorCoords = new List<JColor>();
    }

    public long Width { get; set; }
    public long Height { get; set; }
    public List<JColor> ColorCoords { get; set; }
  }

  internal class JColor
  {
    public string RGB { get; set; }
    public List<Coord> Coords { get; set; }
  }

  internal class Coord
  {
    public long X { get; set; }
    public long Y { get; set; }
  }
}