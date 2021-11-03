using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using user_bff.Helpers;
using user_bff.Models;

namespace user_bff.Services
{
    public interface ICardTokenService
    {
        /// <summary>
        /// Get Card token by user id
        /// </summary>
        /// <param name="CardTokenId">user id</param>
        /// <returns>Card token entity</returns>
        CardToken GetById(long CardTokenId);

        /// <summary>
        /// Get Card token by user id
        /// </summary>
        /// <param name="userId">user id</param>
        /// <returns>Card token entity</returns>
        List<CardToken> GetCardTokens(Guid userId);

        /// <summary>
        /// Create new card token in system
        /// </summary>
        /// <param name="CardToken">card token</param>
        /// <returns>Card token</returns>
        CardToken Create(CardToken CardToken);

        /// <summary>
        /// update card token in system
        /// </summary>
        /// <param name="id">card token id</param>
        /// <param name="CardToken">card token</param>
        /// <returns>Card token</returns>
        CardToken Update(int id, CardToken CardToken);

        /// <summary>
        /// delete card token from system
        /// </summary>
        /// <param name="id">card token id</param>
        /// <returns></returns>
        void Remove(long id);
    }

    public class CardTokenService : ICardTokenService
    {
        private DBContext _context;

        public CardTokenService(DBContext context, IStripeService stripeService)
        {
            _context = context;
        }

        ///<inheritdoc/>
        public CardToken Create(CardToken CardToken)
        {
            try {
                _context.CardToken.Add(CardToken);
                _context.SaveChanges();

                return CardToken;
            }
            catch (AppException ex) {
                throw new AppException(ex.Message);
            }
        }

        ///<inheritdoc/>
        public CardToken GetById(long CardTokenId)
        {
            try {
                return _context.CardToken.FirstOrDefault(x => x.CardTokenId == CardTokenId);
            }
            catch (AppException ex)
            {
                throw new AppException(ex.Message);
            }
        }

        public CardToken GetSavedCard(Guid userId)
        {
            throw new NotImplementedException();
        }

        ///<inheritdoc/>
        public void Remove(long id)
        {
            try {
                var CardToken = _context.CardToken.Find(id);

                if (CardToken == null)
                    throw new AppException("Token not exists.");
                _context.Remove(CardToken);
                _context.SaveChanges();
            }
            catch (AppException ex)
            {
                throw new AppException(ex.Message);
            }
        }

        ///<inheritdoc/>
        public CardToken Update(int id, CardToken CardToken)
        {
            try {
                var saveCardToken = _context.CardToken.FirstOrDefault(x => x.CardTokenId == id);

                if (saveCardToken == null)
                    throw new AppException("Card token not found.");

                _context.Entry(saveCardToken).State = EntityState.Detached;

                saveCardToken.CustomerId = CardToken.CustomerId;

                // update card token information
                _context.CardToken.Update(saveCardToken);
                _context.SaveChanges();

                //_context.Dispose();
                return saveCardToken;
            }
            catch (AppException ex)
            {
                throw new AppException(ex.Message);
            }
        }

        public List<CardToken> GetCardTokens(Guid userId)
        {
            try {
                return _context.CardToken.Where(x => x.CognitoUserId == userId).ToList();
            }
            catch (AppException ex)
            {
                throw new AppException(ex.Message);
            }
        }
    }
}
