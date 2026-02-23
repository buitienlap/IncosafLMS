using System.Collections.Generic;
using System.Runtime.Serialization;

namespace IncosafCMS.Core.DomainModels
{
    public sealed class Image : BaseEntity
    {
        public Image()
        {
            //Products = new HashSet<Product>();
        }

        public string Path { get; set; }

    }
}