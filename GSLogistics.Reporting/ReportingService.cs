using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GSLogistics.Reporting
{
    public class ReportingService : IDisposable
    {
        public ReportingService()
        {

        }

       

        public byte[] RenderReport(object datasource, string reportName, string datasetName, string format, out string mimeType, IDictionary<string, string> parameters = null)
        {
            LocalReport localReport = new LocalReport();

            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = $"GSLogistics.Reporting.Reports.{reportName}";

            var resources = assembly.GetManifestResourceNames();

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                localReport.LoadReportDefinition(stream);
            }

            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = datasetName;
            reportDataSource.Value = datasource;
            localReport.DataSources.Add(reportDataSource);

            string reportType = format;
            string encoding;
            string fileNameExtension;
            string deviceInfo = this.GetDeviceInfo(format);
            Warning[] warnings;
            string[] streams;

            if (parameters != null && parameters.Any())
            {
                List<ReportParameter> reportParameters = new List<ReportParameter>();

                foreach (var p in parameters)
                {
                    reportParameters.Add(new ReportParameter(p.Key, p.Value));
                }

                localReport.SetParameters(reportParameters);
            }

            return localReport.Render(reportType, deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

        }

        private string GetDeviceInfo(string format)
        {
            string deviceInfo = null;
            switch (format.ToLower())
            {
                case "excel":
                    deviceInfo = "<DeviceInfo>" +
                               "  <OutputFormat>Excel</OutputFormat>" +
                               "  <PageWidth>8.5in</PageWidth>" +
                               "  <PageHeight>11in</PageHeight>" +
                               "  <MarginTop>0.5in</MarginTop>" +
                               "  <MarginLeft>0.5in</MarginLeft>" +
                               "  <MarginRight>0.5in</MarginRight>" +
                               "  <MarginBottom>0.5in</MarginBottom>" +
                               "</DeviceInfo>";
                    break;
                case "word":
                    deviceInfo =
                        "<DeviceInfo>" +
                               "  <OutputFormat>Word</OutputFormat>" +
                               "  <PageWidth>8.5in</PageWidth>" +
                               "  <PageHeight>11in</PageHeight>" +
                               "  <MarginTop>0.5in</MarginTop>" +
                               "  <MarginLeft>0.5in</MarginLeft>" +
                               "  <MarginRight>0.5in</MarginRight>" +
                               "  <MarginBottom>0.5in</MarginBottom>" +
                               "</DeviceInfo>";
                    break;
                case "pdf":
                    deviceInfo = "<DeviceInfo>" +
                             "  <OutputFormat>PDF</OutputFormat>" +
                             "  <PageWidth>8.5in</PageWidth>" +
                             "  <PageHeight>11in</PageHeight>" +
                             "  <MarginTop>0.5in</MarginTop>" +
                             "  <MarginLeft>0.1in</MarginLeft>" +
                             "  <MarginRight>0.1in</MarginRight>" +
                             "  <MarginBottom>0.5in</MarginBottom>" +
                             "</DeviceInfo>";
                    break;
                default:
                    deviceInfo = "<DeviceInfo>" +
                             "  <OutputFormat>Image</OutputFormat>" +
                             "  <PageWidth>8.5in</PageWidth>" +
                             "  <PageHeight>11in</PageHeight>" +
                             "  <MarginTop>0.5in</MarginTop>" +
                             "  <MarginLeft>0.1in</MarginLeft>" +
                             "  <MarginRight>0.1in</MarginRight>" +
                             "  <MarginBottom>0.5in</MarginBottom>" +
                             "</DeviceInfo>";
                    break;
            }
            return deviceInfo;
        }
        public string GetReportName(string reportName, string format)
        {
            switch (format.ToLower())
            {
                case "pdf":
                    return $"{reportName}.pdf";
                case "word":
                    return $"{reportName}.doc";
                case "excel":
                    return $"{reportName}.xls";
                default:
                    return $"{reportName}.pdf";


            }
        }

        #region IDisposable

        // Flag: Has Dispose already been called? 
        bool disposed = false;



        // Public implementation of Dispose pattern callable by consumers. 
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern. 
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                // Free any other managed objects here. 
                //
            }

            // Free any unmanaged objects here. 
            //
            disposed = true;
        }

        #endregion IDisposable
    }
}
