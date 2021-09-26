using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasicApi.Service
{
    public class CardFactoryService : ICardFactoryService
    {
        private ISqliteCardManager _sqliteCardManager;
        public CardFactoryService(IOptions<MyConfiguration> myConfiguration)
        {
            _sqliteCardManager = new SqliteCardManager(myConfiguration.Value.DbName);
        }

        public IEnumerable<Card> GetAll()
        {
            return _sqliteCardManager.GetAll();
        }

        public Card Post(Card card)
        {
            return _sqliteCardManager.Post(card);
        }

        public Card Put(Card card)
        {
            return _sqliteCardManager.Put(card);

        }

        public void Delete(int id)
        {
            _sqliteCardManager.Delete(id);
        }
    }
}
