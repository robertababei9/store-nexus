//using IronPdf;

using Domain.Dto;
using System.Globalization;

namespace Common.Helpers
{
    public static class HtmlToPdfHelper
    {
        public static byte[] GetPdfFromHtmlByInvoiceData(InvoiceFormDto invoiceData)
        {

            string htmlFilePath = "InvoiceTemplateDefault.html";
            string htmlContent = File.ReadAllText(htmlFilePath);

            string rowItemTemplate = @"<div style='display: flex; border-top: 1px solid rgb(75 78 84 / 15%); padding-top: 18px; margin-bottom: 18px;' >
                    <div style='width: 60%; display: flex; flex-direction: column; align-items: start; justify-content: center;'>
                        <p style='text-align: left; color: black; font-size: 14px; font-weight: 600; margin-bottom: 6px;' >{{Item.Name}}</p>
                        <p style='text-align: left; font-size: 14px;'>{{Item.Description}}</p>
                    </div>
                    <p style='width: 10%; display: flex; align-items: center;'>{{Item.Qty}}</p>
                    <p style='width: 15%; display: flex; align-items: center;'>{{Item.Price}}</p>
                    <p style='width: 15%; text-align: right; display: flex; justify-content: end; align-items: center;'>{{Item.Total}}</p>
                </div>";

            htmlContent = htmlContent.Replace("{{BillFrom.From}}", invoiceData.BillFrom.From);
            htmlContent = htmlContent.Replace("{{BillFrom.Address}}", invoiceData.BillFrom.Address);
            htmlContent = htmlContent.Replace("{{BillFrom.Email}}", invoiceData.BillFrom.Email);

            htmlContent = htmlContent.Replace("{{BillTo.To}}", invoiceData.BillTo.To);
            htmlContent = htmlContent.Replace("{{BillTo.Address}}", invoiceData.BillTo.Address);
            htmlContent = htmlContent.Replace("{{BillTo.Email}}", invoiceData.BillTo.Email);

            htmlContent = htmlContent.Replace("{{InvoiceNo}}", invoiceData.InvoiceNo);
            htmlContent = htmlContent.Replace("{{InvoiceDate}}", invoiceData.CreatedDate);
            htmlContent = htmlContent.Replace("{{DueDate}}", invoiceData.DueDate);
            htmlContent = htmlContent.Replace("{{Total}}", invoiceData.Total.ToString("C", new CultureInfo("en-US")));


            string itemsHtml = "";
            foreach( var item in invoiceData.Items )
            {
                var rowTemplate = rowItemTemplate.Replace("{{Item.Name}}", item.Name);
                rowTemplate = rowTemplate.Replace("{{Item.Description}}", item.Description);
                rowTemplate = rowTemplate.Replace("{{Item.Qty}}", item.Qty.ToString());
                rowTemplate = rowTemplate.Replace("{{Item.Price}}", item.Price.ToString("C", new CultureInfo("en-US")));
                rowTemplate = rowTemplate.Replace("{{Item.Total}}", (item.Qty * item.Price).ToString("C", new CultureInfo("en-US")));
                itemsHtml += rowTemplate;
            }

            htmlContent = htmlContent.Replace("{{ITEMS}}", itemsHtml);
            htmlContent = htmlContent.Replace("{{Subtotal}}", invoiceData.Subtotal.ToString("C", new CultureInfo("en-US")));
            htmlContent = htmlContent.Replace("{{Tax}}", invoiceData.TaxSubtotal.ToString("C", new CultureInfo("en-US")));
            htmlContent = htmlContent.Replace("{{Discount}}", invoiceData.DiscountSubtotal.ToString("C", new CultureInfo("en-US")));

            htmlContent = htmlContent.Replace("{{Notes}}", invoiceData.Notes);


            var renderer = new ChromePdfRenderer();
            var pdf = renderer.RenderHtmlAsPdf(htmlContent);


            var pdfResult = pdf.SaveAs("test.pdf");

            return pdfResult.BinaryData;
        }
    }
}
