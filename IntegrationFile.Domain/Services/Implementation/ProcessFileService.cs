using IntegrationFile.Domain.Services.Contracts;
using IntegrationFiles.Domain.Configuration;
using IntegrationFiles.Domain.Constants;
using IntegrationFiles.Domain.Entity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace IntegrationFile.Domain.Services.Implementation
{
    public class ProcessFileService : IProcessFileService
    {
        private readonly AppSettings _appSettings;
        private readonly string _pattern = "ç(?![a-zç])";

        public ProcessFileService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings?.Value ?? throw new ArgumentNullException(nameof(appSettings));
        }

        public string ReadFiles()
        {
            if (!Directory.Exists(_appSettings.ImportPath)) return Constants.DIRECTORY_NOT_FOUND;

            IList<string> values = new List<string>();

            string line = string.Empty;

            IList<string> lines = new List<string>();

            foreach (string file in Directory.GetFiles(_appSettings.ImportPath))
            {
                string fileName = Path.GetFileName(file);

                if (!Path.GetExtension(file).Equals(_appSettings.FileExtension)) continue;

                try
                {
                    using (StreamReader sr = new StreamReader(file, Encoding.UTF8))
                        while ((line = sr.ReadLine()) != null)
                        {
                            lines.Add(line);
                        }
                }
                catch (FileNotFoundException ex)
                {
                    return $"{ Constants.ERROR_PROCESS } | { ex.Message }";
                    throw;
                }
            }

            MoveFiles();

            return ProcessFile(lines);
        }

        private void MoveFiles()
        {
            foreach (string file in Directory.GetFiles(_appSettings.ImportPath))
            {
                var processedDirectory = $"{ _appSettings.ImportPath }{ Path.DirectorySeparatorChar }Processed";

                if (!Directory.Exists(processedDirectory))
                {
                    Directory.CreateDirectory(processedDirectory);
                }

                string fileName = Path.GetFileName(file);

                File.Move(file, $"{ processedDirectory}{ Path.DirectorySeparatorChar }{ fileName }");
            }
        }

        private string ProcessFile(IList<string> readLines)
        {
            if (!readLines.Any()) return Constants.NO_FILE_TO_PROCESS;

            var listSalesMan = new List<SalesMan>();
            var listCustomers = new List<Customer>();
            var listSales = new List<Sale>();

            readLines.ToList().ForEach(line =>
            {
                var splitedLine = Regex.Split(line, _pattern);

                if (splitedLine[0].Equals(Constants.SALESMAN))
                {
                    listSalesMan.Add(new
                        SalesMan(splitedLine[0], splitedLine[1], splitedLine[2], Convert.ToDecimal(splitedLine[3])));
                }

                if (splitedLine[0].Equals(Constants.CUSTOMER))
                {
                    listCustomers.Add(new
                        Customer(splitedLine[0], splitedLine[1], splitedLine[2], splitedLine[3]));
                }

                if (splitedLine[0].Equals(Constants.SALE))
                {
                    listSales.Add(new
                        Sale(splitedLine[0], splitedLine[1], splitedLine[2], splitedLine[3]));
                }
            });

            var result = new
            {
                QtyCustomer = $"Quantidade de clientes: { listCustomers.Count }",
                QtySalesMan = $"Quantidade de vendedores: { listSalesMan.Count }",
                High = $"Id da maior venda: { listSales.OrderByDescending(x => x.TotalAmount).FirstOrDefault().SaleId }",
                LowSale = $"O pior vendedor: { listSales.OrderBy(x => x.TotalAmount).FirstOrDefault().SalesManName }",
            };

            var response = JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });

            WriteOutFile(response);

            MoveFiles();

            return response;
        }

        private void WriteOutFile(string response)
        {
            string fileName = $"{ DateTime.Now.ToString("yyyyMMddHHmmss") }.done.dat ";

            using (StreamWriter sW = new StreamWriter($"{_appSettings.ExportPath }{ Path.DirectorySeparatorChar }{ fileName }", false, Encoding.UTF8))
                sW.Write(response);
        }
    }
}