using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDS.ExpenseTracker.Desktop.Controls
{
    public class CustomButton : Button
    {
        public CustomButton()
        {
            this.FlatStyle = FlatStyle.Flat;
            this.BackColor = System.Drawing.Color.Blue;
            this.ForeColor = System.Drawing.Color.White;
        }
    }
}
