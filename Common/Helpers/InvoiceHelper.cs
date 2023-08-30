using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Helpers
{
    public static class InvoiceHelper
    {

        public static string GenerateInvoiceNumber(int invoiceNo)
        {
            DateTime currentDate = DateTime.Now;
            StringBuilder invoiceNumber = new StringBuilder();

            invoiceNumber.Append("#");

            // Add year, month, and day to the invoice number
            invoiceNumber.Append(currentDate.Year.ToString("D2")); // 2-digit year
            invoiceNumber.Append(currentDate.Month.ToString("D2")); // 2-digit month
            invoiceNumber.Append(currentDate.Day.ToString("D2"));   // 2-digit day

            invoiceNumber.Append('-');
            invoiceNumber.Append(invoiceNo);

            return invoiceNumber.ToString();
        }
    }
}
