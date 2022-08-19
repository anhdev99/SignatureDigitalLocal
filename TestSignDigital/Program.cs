﻿using AnhDev99.HashSignatureLocal;
using System;

namespace TestSignDigital
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var data = new PdfHashSinger();

            int d1 = 157;
            int d2 = 842 - (201 + 149);
            int b1 = 148 + 264;
            int b2 = 792 - 214;
            data.AddSignatureComment(new PdfSignatureComment()
            {
                Type = (int)PdfSignatureComment.Types.IMAGE,
                Page =  1,
                Rectangle = $"{157},{d2},{264},{149}",
                X = d1,
                Y = d2,
            });

            int d11 = 133;
            int d22 = 792 - (154 + 30);
            data.AddSignatureComment(new PdfSignatureComment()
            {
                Type = (int)PdfSignatureComment.Types.TEXT,
                Text = "vy dep trai",
                Page =  1,
                Rectangle = "220,100,220,450",
                X = d11,
                Y = d22,
                FontSize = 17,
            });
            data.Sign(@"D:\4a.pdf", @"D:\4acopy.pdf");
        }
    }
}