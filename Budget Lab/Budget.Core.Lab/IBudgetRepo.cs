using System.Collections.Generic;

namespace Budget.Core.Lab
{
    public interface IBudgetRepo
    {
        List<Budget> GetAll();
    }
}