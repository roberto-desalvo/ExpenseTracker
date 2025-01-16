using RDS.ExpenseTracker.Importer.Parsers.CustomExcelParser.Models;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;

namespace RDS.ExpenseTracker.Desktop.WPF.Helpers
{
    public static class ConfigHelper
    {
        private const string CustomExcelImporterConfigSection = "ImportSection";
        private const string CustomExcelSetupSection = "SetupSection";
        private const string SheetsToIgnoreConfigField = "SheetsToIgnore";
        private const string FilePathConfigField = "FilePath";
        private const string FirstAccountNameConfigField = "FirstAccountName";
        private const string SecondAccountNameConfigField = "SecondAccountName";
        private const string ThirdAccountNameConfigField = "ThirdAccountName";
        private const string TransactionDateIndexConfigField = "TransactionDateIndex";
        private const string TransactionDescriptionIndexConfigField = "TransactionDescriptionIndex";
        private const string TransactionOutflowIndexConfigField = "TransactionOutflowIndex";
        private const string TransactionInflowIndexConfigField = "TransactionInflowIndex";
        private const string TransactionAccountNameIndexConfigField = "TransactionAccountNameIndex";
        private const string TransferDateIndexConfigField = "TransferDateIndex";
        private const string TransferDescriptionIndexConfigField = "TransferDescriptionIndex";
        private const string TransferAmountIndexConfigField = "TransferAmountIndex";


        public static CustomExcelImporterConfiguration? GetImporterConfig(out string errorMessage)
        {
            errorMessage = string.Empty;

            if (ConfigurationManager.GetSection(CustomExcelImporterConfigSection) is not NameValueCollection values)
            {
                errorMessage = $"Transaction import config is not valid, please check it.";
                return null;
            }

            var config = new CustomExcelImporterConfiguration();

            config.SheetsToIgnore = values[SheetsToIgnoreConfigField]?.Split(';', StringSplitOptions.None).Select(x => x.Trim()).ToList() ?? [];

            var filePath = values[FilePathConfigField];
            if (filePath is null)
            {
                errorMessage = $"Config is not valid, please check field {FilePathConfigField}.";
                return null;
            }

            config.FilePath = filePath;

            var firstAccountName = values[FirstAccountNameConfigField];
            var secondAccountName = values[SecondAccountNameConfigField];
            var thirdAccountName = values[ThirdAccountNameConfigField];

            if (firstAccountName is null || secondAccountName is null || thirdAccountName is null)
            {
                errorMessage = $"Config is invalid, please check fields {FirstAccountNameConfigField}, {SecondAccountNameConfigField}, {ThirdAccountNameConfigField}";
                return null;
            }

            config.FirstAccountName = firstAccountName;
            config.SecondAccountName = secondAccountName;
            config.ThirdAccountName = thirdAccountName;

            if (!int.TryParse(values[TransactionDateIndexConfigField], out var transactionDateIndex))
            {
                errorMessage = $"Config is invalid, please check field {TransactionDateIndexConfigField}";
                return null;
            }
            config.TransactionDateIndex = transactionDateIndex;

            if (!int.TryParse(values[TransactionDescriptionIndexConfigField], out var transactionDescriptionIndex))
            {
                errorMessage = $"Config is invalid, please check field {TransactionDescriptionIndexConfigField}";
                return null;
            }
            config.TransactionDescriptionIndex = transactionDescriptionIndex;

            if (!int.TryParse(values[TransactionOutflowIndexConfigField], out var transactionOutflowIndex))
            {
                errorMessage = $"Config is invalid, please check field {TransactionOutflowIndexConfigField}";
                return null;
            }
            config.TransactionOutflowIndex = transactionOutflowIndex;

            if (!int.TryParse(values[TransactionInflowIndexConfigField], out var transactionInflowIndex))
            {
                errorMessage = $"Config is invalid, please check field {TransactionInflowIndexConfigField}";
                return null;
            }
            config.TransactionInflowIndex = transactionInflowIndex;

            if (!int.TryParse(values[TransactionAccountNameIndexConfigField], out var transactionAccountNameIndex))
            {
                errorMessage = $"Config is invalid, please check field {TransactionAccountNameIndexConfigField}";
                return null;
            }
            config.TransactionAccountNameIndex = transactionAccountNameIndex;

            if (!int.TryParse(values[TransferDateIndexConfigField], out var transferDateIndex))
            {
                errorMessage = $"Config is invalid, please check field {TransferDateIndexConfigField}";
                return null;
            }
            config.TransferDateIndex = transferDateIndex;

            if (!int.TryParse(values[TransferDescriptionIndexConfigField], out var transferDescriptionIndex))
            {
                errorMessage = $"Config is invalid, please check field {TransferDescriptionIndexConfigField}";
                return null;
            }
            config.TransferDescriptionIndex = transferDescriptionIndex;

            if (!int.TryParse(values[TransferAmountIndexConfigField], out var transferAmountIndex))
            {
                errorMessage = $"Config is invalid, please check field {TransferAmountIndexConfigField}";
                return null;
            }
            config.TransferAmountIndex = transferAmountIndex;            

            if (ConfigurationManager.GetSection(CustomExcelSetupSection) is not NameValueCollection setupValues)
            {
                errorMessage = $"Config is not valid, please check {CustomExcelSetupSection} section.";
                return null;
            }

            foreach (var key in setupValues.AllKeys)
            {
                if (key is null)
                {
                    continue;
                }

                if (!int.TryParse(setupValues[key], out int value))
                {
                    errorMessage = $"Config is not valid, please check setup value: {key}";
                    return null;
                };

                config.AccountInitialAmounts[key] = value;
            }

            return config;
        }
    }
}
