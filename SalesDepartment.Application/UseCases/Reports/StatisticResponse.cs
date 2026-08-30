using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesDepartment.Application.UseCases.Reports
{
    public class StatisticResponse
    {
        public int CountOfAllHomes { get; set; }
        public int CountOfAllCustomers { get; set; }
        public int CountOfHomeWithoutContract { get; set; }
        public int CountOfHomeWithContract { get; set; }

        public int AllContracts { get; set; }
        public int AllContractsNumberInCurrentYear { get; set; }
        public int AllContractsInCurrentMonth { get; set; }
        public int AllContractsInCurrentWeek { get; set; }
        public int AllContractsInCurrentDay { get; set; }

        public decimal TotalAmountOfAllContracts { get; set; }
        public decimal TotalAmountOfAllContractsInCurrentYear { get; set; }
        public decimal TotalAmountOfAllContractsInCurrentMonth { get; set; }
        public decimal TotalAmountOfAllContractsInCurrentWeek { get; set; }
        public decimal TotalAmountOfAllContractsInCurrentDay { get; set; }

        public int CountOfAllPayments { get; set; }
        public int CountOfAllPaymentsInCurrentYear { get; set; }
        public int CountOfPaymentsInCurrentMonth { get; set; }
        public int CountOfPaymentsInLastSevenDays { get; set; }
        public int CountOfPaymentsInCurrentDay { get; set; }

        public decimal TotalAmountOfAllPayments { get; set; }
        public decimal TotalAmountOfAllPaymentsInCurrentYear { get; set; }
        public decimal TotalAmountOfAlPaymentsInCurrentMonth { get; set; }
        public decimal TotalAmountOfAllPaymentsInLastSevenDays { get; set; }
        public decimal TotalAmountOfAllPaymentsInCurrentDay { get; set; }

        public Dictionary<string, int> AllContractsByFounders { get; set; }
    }
}
