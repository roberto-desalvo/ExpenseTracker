﻿using RDS.ExpenseTracker.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDS.ExpenseTracker.Importer.Parsers.Abstractions
{
    public interface ITransactionDataParser
    {
        IEnumerable<Transaction> ParseTransactions();
    }
}
