using System.Collections.Generic;

namespace Budget_Lab
{
    public interface IBudgetRepo
    {
        List<Budget> GetAll();
    }
}
