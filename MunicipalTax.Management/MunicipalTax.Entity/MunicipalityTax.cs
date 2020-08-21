using MunicipalTax.Entity.DataHelper;
using System;

namespace MunicipalTax.Entity
{
    public class MunicipalityTax
    {
        /// <summary>
        /// Municipality Tax Id.
        /// </summary>
        public int MunicipalityTaxId { get; set; }

        /// <summary>
        /// Name of the municipality.
        /// </summary>
        public string MunicipalityName { get; set; }

        /// <summary>
        /// Tax schedule type.
        /// </summary>
        public TaxType TaxType { get; set; }

        /// <summary>
        /// Start date for the scheduled period.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// End date for the scheduled period.
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Tax rate.
        /// </summary>
        public double Tax { get; set; }
    }
}
