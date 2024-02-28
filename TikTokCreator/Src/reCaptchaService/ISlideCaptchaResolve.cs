using System.Drawing;

namespace reCaptchaService
{
    public interface ISlideCaptchaResolve    
    {
        Point Resolve(string param, string NameOrIndex, Bitmap deviceScreen);
    }
}
