using System.Collections.Generic;

namespace BasicApi.Service
{
    public interface ISqliteCardManager
    {
        IEnumerable<Card> GetAll();
        Card Post(Card card);
        Card Put(Card card);
        void Delete(int id);
    }
}