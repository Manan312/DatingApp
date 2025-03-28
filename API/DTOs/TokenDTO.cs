using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class TokenDTO
    {
        public string? Status{get;set;}
        public string? Message{get;set;}
        public string? Token{get;set;}
    }
}