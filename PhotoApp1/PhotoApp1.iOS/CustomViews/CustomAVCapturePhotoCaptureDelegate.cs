using AVFoundation;
using Foundation;
using System;

namespace PhotoApp1.iOS.CustomViews
{
	public class CustomAVCapturePhotoCaptureDelegate : AVCapturePhotoCaptureDelegate
	{
		public Action<byte[]> PhotoCaptured;

		public override void DidFinishProcessingPhoto(AVCapturePhotoOutput output, AVCapturePhoto photo, NSError error)
		{
			//We can return via event byte array to use in viewmodel

			InMemoryStorage.Photos.Add(photo.FileDataRepresentation.ToArray());
		}
	}
}