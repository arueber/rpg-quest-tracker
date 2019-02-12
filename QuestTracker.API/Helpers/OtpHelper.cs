using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using Base32;
using Microsoft.AspNet.Identity;
using QuestTracker.API.Services;
using OtpSharp;

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
                    long timeStepMatched = 0;

                    var totp = new Totp(Base32Encoder.Decode(key));

                    var headeroffset = request.Headers.Date;
                    if (headeroffset.HasValue)
                    {
                        DateTime correctTime = headeroffset.Value.DateTime;
                        var correction = new TimeCorrection(correctTime);
                        totp = new Totp(Base32Encoder.Decode(key), timeCorrection: correction);
                    }

                    bool verified = totp.VerifyTotp(otp, out timeStepMatched,
                       VerificationWindow.RfcSpecifiedNetworkDelay);

                    if (verified)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static string GenerateSharedPrivateKey()
        {
            byte[] secretKey = KeyGeneration.GenerateRandomKey(20);
            var base32String = Base32Encoder.Encode(secretKey);

            return base32String;
        }
    }
}