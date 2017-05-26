using System;
using System.Collections.Generic;
using System.Linq;
using MultiNotes.Model;

namespace MultiNotes.Server.Services
{
    //singleton
    internal static class TokenBase
    {
        private static List<Token> _tokenList;
        private static List<Token> TokenList => _tokenList ?? (_tokenList = new List<Token>());

        public static Token AddNewToken(User user)
        {
            var token = new Token(user);
            TokenList.Add(token);
            return token;
        }

        private static bool TokenVerify(string token)      //checks if token exists and is valid, otherwise deletes token 
        {
            var retVal = TokenList.Count(g => g.GetString == token);

            switch (retVal)
            {
                case 0:
                    return false;
                case 1:
                    if(TokenList.Find(g => g.GetString == token).IsValid)
                        return true;
                    else
                    {
                        RemoveToken(token);
                        return false;
                    }
                default:
                    throw new InvalidOperationException("Non-unique token exists!");
            }
        }

        public static bool VerifyToken(string token)
        {
            return TokenVerify(token);
        }

        public static bool VerifyToken(Token token)
        {
            return TokenVerify(token.GetString);
        }

        public static Token GetToken(string token)
        {
            return VerifyToken(token) == true ? TokenList.Find(g => g.GetString == token) : null;
        }

        public static Token GetToken(Token token)
        {
            return VerifyToken(token) == true ? TokenList.Find(g => g == token) : null;
        }


        public static Token GetUserToken(User user)
        {
            return TokenList.Find(g => g.User.Id == user.Id);
        }

        public static bool VerifyUserToken(User user)
        {
            var retVal = TokenList.Count(g => g.User.Id == user.Id);

            switch (retVal)
            {
                case 0:
                    return false;
                case 1:
                    return TokenVerify(_tokenList.Find(g => g.User.Id == user.Id).GetString);
                default:
                    throw new InvalidOperationException("Too much tokens for one user exists!");
            }
        }

        public static void RemoveToken(string token)
        {
            TokenList.Remove(TokenList.Find(g=>g.GetString==token));
        }
    }
}