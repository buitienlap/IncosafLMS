using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncoSafCMS.Core.Interface
{
    public interface ITechnicalDocumentRepository
    {
        void Add(TechnicalDocument item);
        void Edit(TechnicalDocument item);
        void Remove(string id);
        IEnumerable<TechnicalDocument> GetTechnicalDocuments();
        TechnicalDocument FindByID(string id);
    }
}
