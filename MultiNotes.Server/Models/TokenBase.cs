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

        private static bool TokenVerify(string token)             //checks if token exists and is valid, otherwise deletes token 
        {
            int retVal = TokenList.Count(g => g.GetString == token);

            if (retVal == 0)
                return false;
            else if (retVal == 1)
            {
                if(TokenList.Find(g => g.GetString == token).IsValid)
                    return true;
                else
                {
                    RemoveToken(token);
                    return false;
                }
                
            }     
            else
                throw new InvalidOperationException("Non-unique token exists!");
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
            if (VerifyToken(token) == true)
            {
                Token tmp=TokenList.Find(g => g.GetString == token);
                return tmp;
            }
            return null;
        }

        public static Token GetToken(Token token)
        {
            if (VerifyToken(token) == true)
            {
                Token tmp = TokenList.Find(g => g == token);
                return tmp;
            }
            return null;
        }


        public static Token GetUserToken(User user)
        {
            return TokenList.Find(g => g.User.Id == user.Id);
        }

        //todo: troche to usprawnic bo namieszane jest
        public static bool VerifyUserToken(User user)
        {
            var retVal = TokenList.Count(g => g.User.Id == user.Id);

            if (retVal == 0)
                return false;
            else if (retVal == 1)
                return TokenVerify(tokenList.Find(g => g.User.Id == user.Id).GetString);
            else
                throw new InvalidOperationException("Too much tokens for one user exists!");
        }

        public static void RemoveToken(string token)
        {
            TokenList.Remove(TokenList.Find(g=>g.GetString==token));
        }
    }
}