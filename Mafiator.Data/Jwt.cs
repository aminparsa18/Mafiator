using System;

namespace Mafiator.Data
{
    public class Jwt
    {
        public string Secret { get; set; }
        public TimeSpan TokenLifeTime{get; set; }
    }
}