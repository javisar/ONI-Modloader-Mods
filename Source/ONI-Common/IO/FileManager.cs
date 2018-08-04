namespace ONI_Common.IO
{
    using System.IO;

    using UnityEngine;

    public static class FileManager
    {
        public static Sprite LoadSpriteFromFile(string path, int width, int height)
        {
            byte[] bytes = File.ReadAllBytes(path);
            Texture2D texture =
				new Texture2D(width, height, TextureFormat.RGB24, false) { filterMode = FilterMode.Trilinear };
			//Debug.Log(" === SIZE "+bytes.Length+" === ");
			// texture.LoadImage(bytes);
			ImageConversion.LoadImage(texture, bytes);

			return Sprite.Create(texture, new Rect(0, 0, width, height), new Vector2(0.5f, 0.0f), 1.0f);
        }
    }
}