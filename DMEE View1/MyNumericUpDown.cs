using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DMEEView1
{
    public partial class MyNumericUpDown : NumericUpDown
    {
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
        }

        public MyNumericUpDown()
        {
            InitializeComponent();
        }

        public override void UpButton()
        {
            base.UpButton();
            Value = (int)(Value / Increment) * Increment;
            Select(0, 0);
        }

        public override void DownButton()
        {
            decimal savedValue = Value;
            base.DownButton();
            if (savedValue % Increment != 0) Value = savedValue - Value % Increment;
            Select(0, 0);
        }
    }
}
