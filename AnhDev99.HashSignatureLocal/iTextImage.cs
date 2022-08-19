namespace AnhDev99.HashSignatureLocal
{
	using iText.IO.Exceptions;
	using iTextSharp.text;
	using iTextSharp.text.error_messages;
	using iTextSharp.text.io;
	using iTextSharp.text.pdf;
	using iTextSharp.text.pdf.codec;
	using System;

	public class iTextImage
    {
		public static Image GetInstance(byte[] imgb, bool recoverFromImageError)
		{
			RandomAccessSourceFactory randomAccessSourceFactory = new RandomAccessSourceFactory();
			int num = imgb[0];
			int num2 = imgb[1];
			int num3 = imgb[2];
			int num4 = imgb[3];
			if (num == 71 && num2 == 73 && num3 == 70)
			{
				GifImage gifImage = new GifImage(imgb);
				return gifImage.GetImage(1);
			}
			if (num == 255 && num2 == 216)
			{
				return new Jpeg(imgb);
			}
			if (num == 0 && num2 == 0 && num3 == 0 && num4 == 12)
			{
				return new Jpeg2000(imgb);
			}
			if (num == 255 && num2 == 79 && num3 == 255 && num4 == 81)
			{
				return new Jpeg2000(imgb);
			}
			if (num == PngImage.PNGID[0] && num2 == PngImage.PNGID[1] && num3 == PngImage.PNGID[2] && num4 == PngImage.PNGID[3])
			{
				return PngImage.GetImage(imgb);
			}
			if (num == 215 && num2 == 205)
			{
				return new ImgWMF(imgb);
			}
			if (num == 66 && num2 == 77)
			{
				return BmpImage.GetImage(imgb);
			}
			if ((num == 77 && num2 == 77 && num3 == 0 && num4 == 42) || (num == 73 && num2 == 73 && num3 == 42 && num4 == 0))
			{
				RandomAccessFileOrArray randomAccessFileOrArray = null;
				try
				{
					randomAccessFileOrArray = new RandomAccessFileOrArray(randomAccessSourceFactory.CreateSource(imgb));
					Image tiffImage = TiffImage.GetTiffImage(randomAccessFileOrArray, 1);
					if (tiffImage.OriginalData == null)
					{
						tiffImage.OriginalData = imgb;
					}
					return tiffImage;
				}
				catch (Exception ex)
				{
					if (recoverFromImageError)
					{
						Image tiffImage2 = TiffImage.GetTiffImage(randomAccessFileOrArray, recoverFromImageError, 1);
						if (tiffImage2.OriginalData == null)
						{
							tiffImage2.OriginalData = imgb;
						}
						return tiffImage2;
					}
					throw ex;
				}
				finally
				{
					randomAccessFileOrArray?.Close();
				}
			}
			if (num == 151 && num2 == 74 && num3 == 66 && num4 == 50)
			{
				int num5 = imgb[4];
				int num6 = imgb[5];
				int num7 = imgb[6];
				int num8 = imgb[7];
				if (num5 == 13 && num6 == 10 && num7 == 26 && num8 == 10)
				{
					RandomAccessFileOrArray randomAccessFileOrArray2 = null;
					try
					{
						randomAccessFileOrArray2 = new RandomAccessFileOrArray(randomAccessSourceFactory.CreateSource(imgb));
						Image jbig2Image = JBIG2Image.GetJbig2Image(randomAccessFileOrArray2, 1);
						if (jbig2Image.OriginalData == null)
						{
							jbig2Image.OriginalData = imgb;
						}
						return jbig2Image;
					}
					finally
					{
						randomAccessFileOrArray2?.Close();
					}
				}
			}
			throw new IOException(MessageLocalization.GetComposedMessage("the.byte.array.is.not.a.recognized.imageformat"));
		}
	}
}
