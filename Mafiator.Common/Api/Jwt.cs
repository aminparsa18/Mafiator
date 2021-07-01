using System;

namespace Mafiator.Common.Api
{
    public class Jwt
    {
        public string Secret { get; set; }
        public TimeSpan TokenLifeTime{get; set; }

    }
}