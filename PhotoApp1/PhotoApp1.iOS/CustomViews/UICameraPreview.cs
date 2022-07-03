using AVFoundation;
using CoreVideo;
using Foundation;
using PhotoApp1.Models;
using System;
using System.Linq;
using UIKit;

namespace PhotoApp1.iOS.CustomViews
{
	public class UICameraPreview : UIView
	{
		private readonly CustomAVCapturePhotoCaptureDelegate _customAVCapturePhotoCaptureDelegate = new CustomAVCapturePhotoCaptureDelegate();
		AVCaptureVideoPreviewLayer previewLayer;
		CameraOptions cameraOptions;
		AVCapturePhotoOutput _photoOutput;

		public event EventHandler<EventArgs> Tapped;

		public AVCaptureSession CaptureSession { get; private set; }

		public bool IsPreviewing { get; set; }

		public UICameraPreview(CameraOptions options)
		{
			cameraOptions = options;
			IsPreviewing = false;
			Initialize();
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			if (previewLayer != null)
				previewLayer.Frame = Bounds;
		}

		public override void TouchesBegan(NSSet touches, UIEvent evt)
		{
			base.TouchesBegan(touches, evt);
			OnTapped();
		}

		protected virtual void OnTapped()
		{
			_photoOutput.CapturePhoto(CreatePhotoSettings(), _customAVCapturePhotoCaptureDelegate);

			//var eventHandler = Tapped;
			//if (eventHandler != null)
			//{
			//	eventHandler(this, new EventArgs());
			//}
		}

		private AVCapturePhotoSettings CreatePhotoSettings()
        {
			var settings = AVCapturePhotoSettings.Create();
			var firstAvailablePreviewPhotoPixelFormatTypes = settings.AvailablePreviewPhotoPixelFormatTypes.First();

			var keys = new[]
			{
				CVPixelBuffer.PixelFormatTypeKey
			};

			var objects = new NSObject[]
			{
				firstAvailablePreviewPhotoPixelFormatTypes,
			};

			var dicionary = new NSDictionary<NSString, NSObject>(keys, objects);
			settings.PreviewPhotoFormat = dicionary;

			return settings;
		}

		private void Initialize()
		{
			CaptureSession = new AVCaptureSession();
			previewLayer = new AVCaptureVideoPreviewLayer(CaptureSession)
			{
				Frame = Bounds,
				VideoGravity = AVLayerVideoGravity.ResizeAspect
			};

			var videoDevices = AVCaptureDevice.DevicesWithMediaType(AVMediaType.Video);
			var cameraPosition = (cameraOptions == CameraOptions.Front) ? AVCaptureDevicePosition.Front : AVCaptureDevicePosition.Back;
			var device = videoDevices.FirstOrDefault(d => d.Position == cameraPosition);

			if (device == null)
			{
				return;
			}

			NSError error;
			var input = new AVCaptureDeviceInput(device, out error);
			CaptureSession.AddInput(input);
			Layer.AddSublayer(previewLayer);
			CaptureSession.StartRunning();

			_photoOutput = new AVCapturePhotoOutput();
			CaptureSession.AddOutput(_photoOutput) ;

			IsPreviewing = true;
		}
	}
}