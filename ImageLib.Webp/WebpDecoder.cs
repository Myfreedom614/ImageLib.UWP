﻿// ===============================================================================
// WebpDecoder.cs
// ImageLib for UWP
// ===============================================================================
// Copyright (c) 陈仁松. 
// All rights reserved.
// ===============================================================================

using ImageLib.IO;
using System;
using System.Threading.Tasks;
using System.Threading;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Xaml.Media;
using System.Runtime.InteropServices.WindowsRuntime;
using WebpLib;
using Windows.UI.Xaml.Media.Imaging;

namespace ImageLib.Webp
{
    public class WebpDecoder : IImageDecoder
    {

        public int HeaderSize
        {
            get
            {
                return 12;
            }
        }

        public void Dispose()
        {
            //empty
        }

        public async Task<ExtendImageSource> InitializeAsync(CoreDispatcher dispatcher, IRandomAccessStream streamSource,
             CancellationTokenSource cancellationTokenSource)
        {
            byte[] bytes = new byte[streamSource.Size];
            await streamSource.ReadAsync(bytes.AsBuffer(), (uint)streamSource.Size, InputStreamOptions.None).AsTask(cancellationTokenSource.Token);
            WriteableBitmap writeableBitmap = WebpCodec.Decode(bytes);
            ExtendImageSource imageSource = new ExtendImageSource();
            if (writeableBitmap != null)
            {
                imageSource.PixelWidth = writeableBitmap.PixelWidth;
                imageSource.PixelHeight = writeableBitmap.PixelHeight;
                imageSource.ImageSource = writeableBitmap;
            }
            return imageSource;
        }


        public bool IsSupportedFileFormat(byte[] header)
        {
            return header != null && header.Length >= 12
                && header[0] == 'R' && header[1] == 'I' && header[2] == 'F' && header[3] == 'F'
                && header[8] == 'W' && header[9] == 'E' && header[10] == 'B' && header[11] == 'P';
        }

        public ImageSource RecreateSurfaces()
        {
            return null;
        }

        public void Start()
        {
            //empty
        }

        public void Stop()
        {
            //empty
        }
    }
}
