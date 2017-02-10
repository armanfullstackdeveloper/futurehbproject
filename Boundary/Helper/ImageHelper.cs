using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using ImageResizer;

namespace Boundary.Helper
{
    public class ImageHelper
    {
        public static Bitmap ByteToImage(byte[] blob)
        {
            MemoryStream mStream = new MemoryStream();
            byte[] pData = blob;
            mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
            Bitmap bm = new Bitmap(mStream, false);
            mStream.Dispose();
            return bm;
        }

        public static byte[] ReadFile(string sPath)
        {
            if (string.IsNullOrEmpty(sPath))
                return null;
            byte[] data = null;
            FileInfo fInfo = new FileInfo(sPath);
            long numBytes = fInfo.Length;
            FileStream fStream = new FileStream(sPath, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fStream);
            data = br.ReadBytes((int)numBytes);
            return data;
        }

        public static void Resise(string path, int maxHeight, int maxWidth,string format)
        {
            ResizeSettings resizeSetting = new ResizeSettings
            {
                MaxHeight = maxHeight,
                MaxWidth = maxWidth,
                Format = format,
            };
            ImageBuilder.Current.Build(path, path, resizeSetting);
        }
    }
}