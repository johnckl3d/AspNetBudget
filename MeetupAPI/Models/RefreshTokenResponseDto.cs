using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeetupAPI.Models
{
    public class RefreshTokenResponseDto
    {
        public string accessToken { get; set; }
        public string refreshToken { get; set; }


        public RefreshTokenResponseDto(string _accessToken, string _refreshToken)
        {
            accessToken = _accessToken;
            refreshToken = _refreshToken;
        }

    }
}

