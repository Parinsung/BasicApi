using System.Collections.Generic;

namespace BasicApi.Service
{
    public interface ICardFactoryService
    {
        void Delete(int id);
        IEnumerable<Card> GetAll();
        Card Post(Card card);
        Card Put(Card card);
    }
}