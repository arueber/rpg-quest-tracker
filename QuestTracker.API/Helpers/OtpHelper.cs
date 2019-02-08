using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using QuestTracker.API.Services;
using OtpNet;

namespace QuestTracker.API.Helpers
{
    public static class OtpHelper
    {
        private const string OTP_HEADER = "X-OTP";

        public static bool HasValidTotp(this HttpRequestMessage request, string key)
        {
            if (request.Headers.Contains(OTP_HEADER))
            {
                string otp = request.Headers.GetValues(OTP_HEADER).First();

                // We need to check the passcode against the past, current, and future passcodes
                if (!string.IsNullOrWhiteSpace(otp))
                {
                    var base32Bytes = Base32Encoding.ToBytes(key);
                    var timeWindowUsed = Int64.MinValue;
                    var totp = new Totp(base32Bytes);

                    var headeroffset = request.Headers.Date;
                    if (headeroffset.HasValue)
                    {
                        DateTime correctTime = headeroffset.Value.DateTime;
                        var correction = new TimeCorrection(correctTime);
                        totp = new Totp(base32Bytes, timeCorrection: correction);
                    }

                    if (totp.VerifyTotp(otp, out timeWindowUsed, VerificationWindow.RfcSpecifiedNetworkDelay))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static string GenerateSharedPrivateKey()
        {
            var key = KeyGeneration.GenerateRandomKey(20);
            var base32String = Base32Encoding.ToString(key);

            return base32String;
        }
    }
}