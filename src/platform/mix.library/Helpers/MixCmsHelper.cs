using ClosedXML.Excel;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Text.RegularExpressions;
namespace Mix.Lib.Helpers
{
    public class MixCmsHelper
    {
        public static bool CheckStaticFileRequest(string path)
        {
            return !string.IsNullOrEmpty(path) && !string.IsNullOrEmpty(Path.GetExtension(path));
        }

        public static ExpressionMethod ParseExpressionMethod(MixCompareOperator? compareOperator)
        {
            switch (compareOperator)
            {
                case MixCompareOperator.Equal:
                    return ExpressionMethod.Equal;
                case MixCompareOperator.Like:
                    return ExpressionMethod.Like;
                case MixCompareOperator.NotEqual:
                    return ExpressionMethod.NotEqual;
                case MixCompareOperator.Contain:
                    return ExpressionMethod.In;
                case MixCompareOperator.NotInRange:
                    return ExpressionMethod.NotIn;
                case MixCompareOperator.NotContain:
                    return ExpressionMethod.NotEqual;
                case MixCompareOperator.InRange:
                    return ExpressionMethod.In;
                case MixCompareOperator.GreaterThanOrEqual:
                    return ExpressionMethod.GreaterThanOrEqual;
                case MixCompareOperator.GreaterThan:
                    return ExpressionMethod.GreaterThan;
                case MixCompareOperator.LessThanOrEqual:
                    return ExpressionMethod.LessThanOrEqual;
                case MixCompareOperator.LessThan:
                    return ExpressionMethod.LessThan;
                default:
                    return ExpressionMethod.Equal;
            }
        }
        public static object ParseSearchKeyword(MixCompareOperator? searchMethod, object keyword)
        {
            if (keyword == null)
            {
                return keyword;
            }
            return searchMethod switch
            {
                MixCompareOperator.Like => $"%{keyword}%",
                MixCompareOperator.InRange => keyword.ToString().Split(',', StringSplitOptions.TrimEntries),
                MixCompareOperator.NotInRange => keyword.ToString().Split(',', StringSplitOptions.TrimEntries),
                _ => keyword
            };
        }

        public static WebApplicationBuilder CreateWebApplicationBuilder(string[] args)
        {
            var mixContentFolder = new DirectoryInfo($"{Environment.CurrentDirectory}/{MixFolders.MixContentSharedFolder}");

            // Clone Settings from shared folder
            if (!mixContentFolder.Exists)
            {
                MixHelper.CopyFolder($"{Environment.CurrentDirectory}/{MixFolders.DefaultMixContentFolder}", $"{Environment.CurrentDirectory}/{MixFolders.MixContentFolder}");
                Console.WriteLine("Clone Settings from shared folder completed.");
            }
            var builder = WebApplication.CreateBuilder(args);
            builder.WebHost.UseContentRoot(Directory.GetCurrentDirectory());

            builder.Configuration
                       .AddJsonFile("appsettings.json",true, true)
                       .AddJsonFile($"{Environment.CurrentDirectory}/{MixAppConfigFilePaths.Shared}/appconfigs/global.json",true, true)
                       .AddJsonFile($"{Environment.CurrentDirectory}/{MixAppConfigFilePaths.Shared}/appconfigs/ocelot.json",true, true)
                       .AddJsonFile($"{Environment.CurrentDirectory}/{MixAppConfigFilePaths.Shared}/appconfigs/storage.json",true, true)
                       .AddJsonFile($"{Environment.CurrentDirectory}/{MixAppConfigFilePaths.Shared}/appconfigs/azure.json",true, true)
                       .AddJsonFile($"{Environment.CurrentDirectory}/{MixAppConfigFilePaths.Shared}/appconfigs/queue.json",true, true)
                       .AddJsonFile($"{Environment.CurrentDirectory}/{MixAppConfigFilePaths.Shared}/appconfigs/mix_heart.json",true, true)
                       .AddJsonFile($"{Environment.CurrentDirectory}/{MixAppConfigFilePaths.Shared}/appconfigs/authentication.json",true, true)
                       .AddJsonFile($"{Environment.CurrentDirectory}/{MixAppConfigFilePaths.Shared}/appconfigs/google_credential.json",true, true)
                       .AddJsonFile($"{Environment.CurrentDirectory}/{MixAppConfigFilePaths.Shared}/appconfigs/google_firebase.json",true, true)
                       .AddJsonFile($"{Environment.CurrentDirectory}/{MixAppConfigFilePaths.Shared}/appconfigs/smtp.json",true, true)
                       .AddJsonFile($"{Environment.CurrentDirectory}/{MixAppConfigFilePaths.Shared}/appconfigs/payments.json",true, true)
                       .AddJsonFile($"{Environment.CurrentDirectory}/{MixAppConfigFilePaths.Shared}/appconfigs/redis.json",true, true)
                       .AddJsonFile($"{Environment.CurrentDirectory}/{MixAppConfigFilePaths.Shared}/appconfigs/log.json",true, true)
                       .AddJsonFile($"{Environment.CurrentDirectory}/{MixAppConfigFilePaths.Shared}/appconfigs/rate_limit.json",true, true)
                       .AddEnvironmentVariables();
            return builder;
        }
        public static IHostBuilder CreateHostBuilder<TStartup>(string[] args) where TStartup : class
        {
            var mixContentFolder = new DirectoryInfo(MixFolders.MixContentFolder);

            // Clone Settings from shared folder
            if (!mixContentFolder.Exists)
            {
                MixHelper.CopyFolder(MixFolders.DefaultMixContentFolder, MixFolders.MixContentFolder);
                Console.WriteLine("Clone Settings from shared folder completed.");
            }
            return Host.CreateDefaultBuilder(args)
            .UseContentRoot(Directory.GetCurrentDirectory())
               .ConfigureAppConfiguration((hostingContext, config) =>
               {
                   config
                       .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                       .AddJsonFile("appsettings.json",true, true)
                       .AddJsonFile($"{Environment.CurrentDirectory}/{MixAppConfigFilePaths.Shared}/appconfigs/global.json",true, true)
                       .AddJsonFile($"{Environment.CurrentDirectory}/{MixAppConfigFilePaths.Shared}/appconfigs/azure.json",true, true)
                       .AddJsonFile($"{Environment.CurrentDirectory}/{MixAppConfigFilePaths.Shared}/appconfigs/ocelot.json",true, true)
                       .AddJsonFile($"{Environment.CurrentDirectory}/{MixAppConfigFilePaths.Shared}/appconfigs/storage.json",true, true)
                       .AddJsonFile($"{Environment.CurrentDirectory}/{MixAppConfigFilePaths.Shared}/appconfigs/queue.json",true, true)
                       .AddJsonFile($"{Environment.CurrentDirectory}/{MixAppConfigFilePaths.Shared}/appconfigs/mix_heart.json",true, true)
                       .AddJsonFile($"{Environment.CurrentDirectory}/{MixAppConfigFilePaths.Shared}/appconfigs/authentication.json",true, true)
                       .AddJsonFile($"{Environment.CurrentDirectory}/{MixAppConfigFilePaths.Shared}/appconfigs/google_credential.json",true, true)
                       .AddJsonFile($"{Environment.CurrentDirectory}/{MixAppConfigFilePaths.Shared}/appconfigs/google_firebase.json",true, true)
                       .AddJsonFile($"{Environment.CurrentDirectory}/{MixAppConfigFilePaths.Shared}/appconfigs/smtp.json",true, true)
                       .AddJsonFile($"{Environment.CurrentDirectory}/{MixAppConfigFilePaths.Shared}/appconfigs/payments.json",true, true)
                       .AddJsonFile($"{Environment.CurrentDirectory}/{MixAppConfigFilePaths.Shared}/appconfigs/redis.json",true, true)
                       .AddJsonFile($"{Environment.CurrentDirectory}/{MixAppConfigFilePaths.Shared}/appconfigs/log.json",true, true)
                       .AddJsonFile($"{Environment.CurrentDirectory}/{MixAppConfigFilePaths.Shared}/appconfigs/rate_limit.json",true, true)
                       .AddEnvironmentVariables();
               })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                    .UseStartup<TStartup>();
                });
        }

        public static string FormatPrice(double? price, string oldPrice = "0")
        {
            string strPrice = price?.ToString();
            if (string.IsNullOrEmpty(strPrice))
            {
                return "0";
            }
            string s1 = strPrice.Replace(",", string.Empty);
            if (CheckIsPrice(s1))
            {
                Regex rgx = new Regex("(\\d+)(\\d{3})");
                while (rgx.IsMatch(s1))
                {
                    s1 = rgx.Replace(s1, "$1" + "," + "$2");
                }
                return s1;
            }
            return oldPrice;
        }
        public static bool CheckIsPrice(string number)
        {
            if (number == null)
            {
                return false;
            }
            number = number.Replace(",", "");
            return double.TryParse(number, out _);
        }

        public static FileModel ExportJObjectToExcel(List<JObject> lstData, string sheetName, string folderPath, string fileName, List<string> headers = null)
        {
            try
            {
                var repositoryResponse = new FileModel()
                {
                    FileFolder = folderPath,
                    Filename = fileName,
                    Extension = ".xlsx"
                };
                if (lstData.Count > 0)
                {
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add(sheetName);
                        var currentRow = 1;
                        var currentCol = 1;
                        string filename = repositoryResponse.Filename + repositoryResponse.Extension;
                        if (headers == null)
                        {
                            IEnumerable<JProperty> enumerable = lstData[0].Properties();
                            foreach (JProperty item in enumerable)
                            {
                                worksheet.Cell(currentRow, currentCol).Value = item.Name;
                                currentCol++;
                            }
                        }
                        else
                        {
                            foreach (string header in headers)
                            {
                                worksheet.Cell(currentRow, currentCol).Value = header;
                                currentCol++;
                            }
                        }

                        foreach (JObject lstDatum in lstData)
                        {

                            currentRow++;
                            currentCol = 1;
                            foreach (JProperty item2 in lstDatum.Properties())
                            {
                                if (lstDatum.TryGetValue(item2.Name, StringComparison.OrdinalIgnoreCase, out var token))
                                {
                                    worksheet.Cell(currentRow, currentCol).Value = token.ToString();
                                }
                                currentCol++;
                            }
                        }

                        using (var stream = new MemoryStream())
                        {
                            workbook.SaveAs(stream);
                            var content = stream.ToArray();
                            MixFileHelper.SaveFileBytes(folderPath, filename, content);
                            return repositoryResponse;
                        }
                    }
                }

                throw new MixException(MixErrorStatus.Badrequest, "Can not export data of empty list");
            }
            catch (Exception ex)
            {
                throw new MixException(MixErrorStatus.ServerError, ex);
            }
        }

        public static List<JObject> LoadExcelFileData(IFormFile file)
        {
            //create a list to hold all the values
            List<JObject> excelData = new List<JObject>();

            //create a new Excel package in a memorystream
            using (var stream = file.OpenReadStream())
            using (XLWorkbook excelPackage = new XLWorkbook(stream))
            {
                //loop all worksheets
                foreach (var worksheet in excelPackage.Worksheets)
                {
                    bool FirstRow = true;
                    //Range for reading the cells based on the last cell used.  
                    string readRange = "1:1";
                    List<string> columnNames = new();
                    foreach (IXLRow row in worksheet.RowsUsed())
                    {
                        //If Reading the First Row (used) then add them as column name  
                        if (FirstRow)
                        {
                            readRange = string.Format("{0}:{1}", 1, row.LastCellUsed().Address.ColumnNumber);
                            foreach (IXLCell cell in row.Cells(readRange))
                            {
                                columnNames.Add(cell.Value.ToString());
                            }
                            FirstRow = false;
                        }
                        else
                        {
                            JObject obj = new JObject();
                            int colIndex = 0;
                            foreach (IXLCell cell in row.Cells(readRange))
                            {
                                obj.Add(new JProperty(columnNames[colIndex], cell.Value.ToString()));
                                colIndex++;
                            }
                            excelData.Add(obj);
                        }
                    }
                }
                return excelData;
            }
        }
    }
}
