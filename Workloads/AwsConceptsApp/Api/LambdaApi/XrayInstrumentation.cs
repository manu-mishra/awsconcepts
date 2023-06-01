using Amazon.XRay.Recorder.Core;
using Application.Interfaces;

namespace LambdaApi
{
    public class XrayInstrumentation : IApplicationLogger
    {
        public void AddAnnotation(string key, object value)
        {
            AWSXRayRecorder.Instance.AddAnnotation(key, value);
        }

        public void AddMetadata(string key, object value)
        {
            AWSXRayRecorder.Instance.AddMetadata(key, value);
        }

        public void AddException(Exception ex)
        {
            AWSXRayRecorder.Instance.AddException(ex);
            AWSXRayRecorder.Instance.MarkError();
        }
    }
}
