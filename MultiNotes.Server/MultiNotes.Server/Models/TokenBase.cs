using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MultiNotes.Core;

namespace MultiNotes.Server.Models
{
    //singleton
    static class TokenBase
    {
        private static List<Token> tokenList;

        private static List<Token> TokenList
        {
            get
            {
                if (tokenList == null)
                    tokenList = new List<Token>();
                return tokenList;
            }
        }

        public static Token AddNewToken(User user)
        {
            Token token = new Token(user);
            TokenList.Add(token);
            return token;
        }

        public static Token CheckToken(string id)
        {
            //todo: mozna rozdzielic reakcje na to ze token wygasl od tego ze go nie ma w ogole

            Token token = TokenList.Find(g => g.GetToken == id);

            if (token != null)
            {
                if(token.IsValid)
                {
                    return token;
                }
                else
                {
                    TokenList.Remove(token);
                    return null;
                } 
            }
            else
            {
                return null;
            }             
        }
    }
}